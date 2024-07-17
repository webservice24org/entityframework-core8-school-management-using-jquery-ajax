using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MohiuddinCoreMasterDetailCrud.Models;
using MohiuddinCoreMasterDetailCrud.Models.ViewModels;
using System.Reflection;

namespace MohiuddinCoreMasterDetailCrud.Controllers
{
    public class StudentsController : Controller
    {
        private readonly MohiuddinCoreMasterDetailsContext _db;
        private readonly IWebHostEnvironment _webHost;

        public StudentsController(MohiuddinCoreMasterDetailsContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetStudents()
        {
            try
            {
                var students = await _db.Students
                    .Select(s => new StudentViewModel
                    {
                        StudentId = s.StudentId,
                        StudentName = s.StudentName,
                        MobileNo = s.Mobile,
                        Dob = s.Dob,
                        ImageUrl = s.ImageUrl,
                    }).ToListAsync();

                return Json(new { success = true, data = students });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetStudentDetails(int id)
        {
            try
            {
                var studentDetails = await _db.Students
                    .Where(s => s.StudentId == id)
                    .Include(s => s.Enrollments)
                        .ThenInclude(e => e.Course)
                            .ThenInclude(c => c.Modules)
                    .Include(s => s.StudentDetails)
                    .Select(s => new
                    {
                        s.StudentId,
                        s.StudentName,
                        s.Mobile,
                        s.ImageUrl,
                        Enrollments = s.Enrollments.Select(e => new
                        {
                            e.CourseId,
                            e.Course.CourseName,
                            Modules = e.Course.Modules.Select(m => new
                            {
                                m.ModuleId,
                                m.ModuleName,
                                m.Duration
                            }).ToList()
                        }).ToList(),
                        StudentDetails = new
                        {
                            s.StudentDetails.PresentAddress,
                            s.StudentDetails.PermanentAddress,
                            s.StudentDetails.GuardianName,
                            s.StudentDetails.RelationWithGuardian,
                            s.StudentDetails.GuardianMobile
                        }
                    }).FirstOrDefaultAsync();

                if (studentDetails == null)
                {
                    return Json(new { success = false, message = "Student not found." });
                }

                return Json(new { success = true, data = studentDetails });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        public JsonResult GetCourses()
        {
            List<SelectListItem> course = (from cor in _db.Courses
                                           select new SelectListItem
                                           {
                                               Value = cor.CourseId.ToString(),
                                               Text = cor.CourseName
                                           }).ToList();
            return Json(course);
        }


        [HttpPost]
        public async Task<JsonResult> InsertStudent(StudentViewModel studentViewModel)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                string imageUrl = GetUploadedFileName(studentViewModel);
                if (imageUrl != null)
                {
                    var student = new Student
                    {
                        StudentName = studentViewModel.StudentName,
                        Dob = studentViewModel.Dob,
                        Mobile = studentViewModel.MobileNo,
                        ImageUrl = imageUrl
                    };

                    _db.Students.Add(student);
                    await _db.SaveChangesAsync();

                    var studentDetails = new StudentDetails
                    {
                        StudentId = student.StudentId,
                        PresentAddress = studentViewModel.PresentAddress,
                        PermanentAddress = studentViewModel.PermanentAddress,
                        GuardianName = studentViewModel.GuardianName,
                        RelationWithGuardian = studentViewModel.RelationWithGuardian,
                        GuardianMobile = studentViewModel.GuardianMobile
                    };

                    _db.StudentDetails.Add(studentDetails);
                    await _db.SaveChangesAsync();

                    var enrollment = new Enrollment
                    {
                        StudentId = student.StudentId,
                        CourseId = studentViewModel.CourseId
                    };

                    _db.Enrollments.Add(enrollment);
                    await _db.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return Json(new { success = true, message = "Student and related data added successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save profile image." });
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> EditStudent(int id)
        {
            try
            {
                var student = await _db.Students
                    .Include(s => s.StudentDetails)
                    .Include(s => s.Enrollments)
                        .ThenInclude(e => e.Course)
                    .Where(s => s.StudentId == id)
                    .Select(s => new
                    {
                        s.StudentId,
                        s.StudentName,
                        s.Dob,
                        s.Mobile,
                        s.ImageUrl,
                        s.StudentDetails.PresentAddress,
                        s.StudentDetails.PermanentAddress,
                        s.StudentDetails.GuardianName,
                        s.StudentDetails.RelationWithGuardian,
                        s.StudentDetails.GuardianMobile,
                        s.Enrollments.FirstOrDefault().CourseId  
                    })
                    .FirstOrDefaultAsync();

                if (student != null)
                {
                    return Json(new { success = true, data = student });
                }
                else
                {
                    return Json(new { success = false, message = "Student not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStudent(StudentViewModel studentViewModel)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var existingStudent = await _db.Students
                    .Include(s => s.StudentDetails)
                    .FirstOrDefaultAsync(s => s.StudentId == studentViewModel.StudentId);

                if (existingStudent == null)
                {
                    return Json(new { success = false, message = "Student not found." });
                }

                string imageUrl = GetUploadedFileName(studentViewModel);
                if (imageUrl != null)
                {
                    if (!string.IsNullOrEmpty(existingStudent.ImageUrl))
                    {
                        DeleteImageIfExists(existingStudent.ImageUrl);
                    }

                    existingStudent.ImageUrl = imageUrl;
                }

                existingStudent.StudentName = studentViewModel.StudentName;
                existingStudent.Dob = studentViewModel.Dob;
                existingStudent.Mobile = studentViewModel.MobileNo;

                if (existingStudent.StudentDetails != null)
                {
                    existingStudent.StudentDetails.PresentAddress = studentViewModel.PresentAddress;
                    existingStudent.StudentDetails.PermanentAddress = studentViewModel.PermanentAddress;
                    existingStudent.StudentDetails.GuardianName = studentViewModel.GuardianName;
                    existingStudent.StudentDetails.RelationWithGuardian = studentViewModel.RelationWithGuardian;
                    existingStudent.StudentDetails.GuardianMobile = studentViewModel.GuardianMobile;
                }
                else
                {
                    
                    var studentDetails = new StudentDetails
                    {
                        StudentId = existingStudent.StudentId,
                        PresentAddress = studentViewModel.PresentAddress,
                        PermanentAddress = studentViewModel.PermanentAddress,
                        GuardianName = studentViewModel.GuardianName,
                        RelationWithGuardian = studentViewModel.RelationWithGuardian,
                        GuardianMobile = studentViewModel.GuardianMobile
                    };
                    _db.StudentDetails.Add(studentDetails);
                }

                
                var enrollment = await _db.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentViewModel.StudentId);
                if (enrollment != null)
                {
                    enrollment.CourseId = studentViewModel.CourseId;
                }
                else
                {
                    enrollment = new Enrollment
                    {
                        StudentId = existingStudent.StudentId,
                        CourseId = studentViewModel.CourseId
                    };
                    _db.Enrollments.Add(enrollment);
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true, message = "Student updated successfully." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _db.Students
                    .Include(s => s.Enrollments)
                    .Include(s => s.StudentDetails)
                    .FirstOrDefaultAsync(s => s.StudentId == id);

                if (student == null)
                {
                    return Json(new { success = false, message = "Student not found." });
                }
                _db.Enrollments.RemoveRange(student.Enrollments);

                _db.StudentDetails.Remove(student.StudentDetails);

                if (!string.IsNullOrEmpty(student.ImageUrl))
                {
                    DeleteImageIfExists(student.ImageUrl); 
                }

                _db.Students.Remove(student);

                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Student and associated data deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        private void DeleteImageIfExists(string imageUrl)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), _webHost.WebRootPath, "Images", imageUrl);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        private string GetUploadedFileName(StudentViewModel student)
        {
            string uniqueFileName = null;

            if (student.ProfileFile != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + student.ProfileFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    student.ProfileFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


    }
}
