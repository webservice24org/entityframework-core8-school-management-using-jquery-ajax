using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MohiuddinCoreMasterDetailCrud.Models;
using MohiuddinCoreMasterDetailCrud.Models.ViewModels;
using System.Reflection;

namespace MohiuddinCoreMasterDetailCrud.Controllers
{
    //public class StudentsController : Controller
    //{
    //    private readonly MohiuddinCoreMasterDetailsContext _db;
    //    private readonly IWebHostEnvironment _webHost;

    //    public StudentsController(MohiuddinCoreMasterDetailsContext db, IWebHostEnvironment webHost)
    //    {
    //        _db = db;
    //        _webHost = webHost;
    //    }
    //    public IActionResult Index()
    //    {
    //        var studentViewModel = new StudentViewModel
    //        {
    //            Modules = new List<ModuleViewModel>()
    //        };

    //        return View(studentViewModel);
    //    }


    //    [HttpGet]
    //    public JsonResult GetStudents()
    //    {
    //        var students = _db.Students.ToList();

    //        return Json(students);
    //    }

    //    [HttpGet]
    //    public JsonResult GetStudentDetails(int studentId)
    //    {
    //        var student = _db.Students
    //            .Include(s => s.Course)
    //            .Include(s => s.Modules)
    //            .FirstOrDefault(s => s.StudentId == studentId);

    //        if (student == null)
    //        {
    //            return Json(new { success = false, message = "Student not found." });
    //        }

    //        var studentDetails = new
    //        {
    //            studentId = student.StudentId,
    //            studentName = student.StudentName,
    //            dob = student.Dob,
    //            mobileNo = student.Mobile,
    //            imageUrl = student.ImageUrl,
    //            isEnrolled = student.IsEnroll,
    //            course = new
    //            {
    //                courseId = student.Course.CourseId,
    //                courseName = student.Course.CourseName
    //            },
    //            modules = student.Modules.Select(m => new
    //            {
    //                moduleId = m.ModuleId,
    //                moduleName = m.ModuleName,
    //                duration = m.Duration
    //            }).ToList()
    //        };

    //        return Json(new { success = true, data = studentDetails });
    //    }

    //    public JsonResult GetCourses()
    //    {
    //        List<SelectListItem> course = (from cor in _db.Courses
    //                                       select new SelectListItem
    //                                       {
    //                                           Value = cor.CourseId.ToString(),
    //                                           Text = cor.CourseName
    //                                       }).ToList();
    //        return Json(course);
    //    }


    //    [HttpPost]
    //    public JsonResult InsertStudent(StudentViewModel studentViewModel)
    //    {
    //        using var transaction = _db.Database.BeginTransaction();
    //        try
    //        {
    //            string imageUrl = GetUploadedFileName(studentViewModel);
    //            if (imageUrl != null)
    //            {
    //                var student = new Student
    //                {
    //                    StudentName = studentViewModel.StudentName,
    //                    Dob = studentViewModel.Dob,
    //                    Mobile = studentViewModel.MobileNo,
    //                    ImageUrl = imageUrl,
    //                    IsEnroll = studentViewModel.IsEnrolled,
    //                    CourseId = studentViewModel.CourseId
    //                };

    //                _db.Students.Add(student);
    //                _db.SaveChanges();

    //                if (studentViewModel.Modules != null && studentViewModel.Modules.Count > 0)
    //                {
    //                    foreach (var item in studentViewModel.Modules)
    //                    {
    //                        var module = new MohiuddinCoreMasterDetailCrud.Models.Module
    //                        {
    //                            StudentId = student.StudentId, 
    //                            Duration = item.Duration,
    //                            ModuleName = item.ModuleName
    //                        };
    //                        _db.Modules.Add(module);
    //                    }
    //                    _db.SaveChanges();
    //                }

    //                transaction.Commit();
    //                return Json(new { success = true, message = "Student and modules added successfully." });
    //            }
    //            else
    //            {
    //                return Json(new { success = false, message = "Failed to save profile image." });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            transaction.Rollback();
    //            return Json(new { success = false, message = "Error: " + ex.Message });
    //        }
    //    }


    //    [HttpGet]
    //    public JsonResult StudentEdit(int studentId)
    //    {
    //        try
    //        {
    //            var student = _db.Students
    //                .Include(s => s.Modules)
    //                .Include(s => s.Course)
    //                .FirstOrDefault(s => s.StudentId == studentId);

    //            if (student == null)
    //            {
    //                return Json(new { success = false, message = "Student not found." });
    //            }

    //            var studentViewModel = new StudentViewModel
    //            {
    //                StudentId = student.StudentId,
    //                StudentName = student.StudentName,
    //                Dob = student.Dob,
    //                CourseId = student.CourseId,
    //                MobileNo = student.Mobile,
    //                IsEnrolled = student.IsEnroll,
    //                ImageUrl = student.ImageUrl,
    //                Modules = student.Modules.Select(m => new ModuleViewModel
    //                {
    //                    ModuleId = m.ModuleId,
    //                    ModuleName = m.ModuleName,
    //                    Duration = m.Duration
    //                }).ToList()
    //            };

    //            return Json(new { success = true, data = studentViewModel });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { success = false, message = "Error fetching student data: " + ex.Message });
    //        }
    //    }


    //[HttpPost]
    //public JsonResult UpdateStudent(StudentViewModel studentViewModel)
    //{
    //    using var transaction = _db.Database.BeginTransaction();
    //    try
    //    {
    //        var student = _db.Students
    //            .Include(s => s.Modules)
    //            .FirstOrDefault(s => s.StudentId == studentViewModel.StudentId);

    //        if (student == null)
    //        {
    //            return Json(new { success = false, message = "Student not found." });
    //        }

    //        student.StudentName = studentViewModel.StudentName;
    //        student.Dob = studentViewModel.Dob;
    //        student.CourseId = studentViewModel.CourseId;
    //        student.Mobile = studentViewModel.MobileNo;
    //        student.IsEnroll = studentViewModel.IsEnrolled;

    //        if (studentViewModel.ProfileFile != null)
    //        {

    //            string newFileName = GetUploadedFileName(studentViewModel);
    //            if (!string.IsNullOrEmpty(student.ImageUrl))
    //            {
    //                string oldImagePath = Path.Combine(_webHost.WebRootPath, "Images", student.ImageUrl);
    //                if (System.IO.File.Exists(oldImagePath))
    //                {
    //                    System.IO.File.Delete(oldImagePath);
    //                }
    //            }
    //            student.ImageUrl = newFileName;
    //        }

    //        var existingModules = student.Modules.ToList();

    //        foreach (var existingModule in existingModules)
    //        {
    //            if (!studentViewModel.Modules.Any(m => m.ModuleId == existingModule.ModuleId))
    //            {
    //                _db.Modules.Remove(existingModule);
    //            }
    //        }
    //        foreach (var moduleViewModel in studentViewModel.Modules)
    //        {
    //            if (!string.IsNullOrWhiteSpace(moduleViewModel.ModuleName) && moduleViewModel.Duration > 0)
    //            {
    //                var existingModule = existingModules.FirstOrDefault(m => m.ModuleId == moduleViewModel.ModuleId);
    //                if (existingModule != null)
    //                {
    //                    existingModule.ModuleName = moduleViewModel.ModuleName;
    //                    existingModule.Duration = moduleViewModel.Duration;
    //                }
    //                else
    //                {
    //                    student.Modules.Add(new MohiuddinCoreMasterDetailCrud.Models.Module
    //                    {
    //                        ModuleName = moduleViewModel.ModuleName,
    //                        Duration = moduleViewModel.Duration
    //                    });
    //                }
    //            }
    //        }

    //        _db.SaveChanges();
    //        transaction.Commit();

    //        return Json(new { success = true, message = "Student updated successfully." });
    //    }
    //    catch (Exception ex)
    //    {
    //        transaction.Rollback();
    //        return Json(new { success = false, message = "Error updating student: " + ex.Message + " - Inner Exception: " + ex.InnerException?.Message });
    //    }
    //}

    //    private string GetUploadedFileName(StudentViewModel student)
    //    {
    //        string uniqueFileName = null;

    //        if (student.ProfileFile != null)
    //        {
    //            string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Images");
    //            uniqueFileName = Guid.NewGuid().ToString() + "_" + student.ProfileFile.FileName;
    //            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
    //            using (var fileStream = new FileStream(filePath, FileMode.Create))
    //            {
    //                student.ProfileFile.CopyTo(fileStream);
    //            }
    //        }
    //        return uniqueFileName;
    //    }

    //    [HttpPost]
    //    public JsonResult DeleteStudent(int studentId)
    //    {
    //        try
    //        {

    //            var student = _db.Students
    //                .Include(s => s.Modules)
    //                .FirstOrDefault(s => s.StudentId == studentId);

    //            if (student == null)
    //            {
    //                return Json(new { success = false, message = "Student not found." });
    //            }

    //            _db.Modules.RemoveRange(student.Modules);

    //            if (!string.IsNullOrEmpty(student.ImageUrl))
    //            {
    //                string imagePath = Path.Combine(_webHost.WebRootPath, "Images", student.ImageUrl);
    //                if (System.IO.File.Exists(imagePath))
    //                {
    //                    System.IO.File.Delete(imagePath);
    //                }
    //            }

    //            _db.Students.Remove(student);
    //            _db.SaveChanges();

    //            return Json(new { success = true, message = "Student and related modules deleted successfully." });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { success = false, message = "Error deleting student: " + ex.Message });
    //        }
    //    }



    //}
}
