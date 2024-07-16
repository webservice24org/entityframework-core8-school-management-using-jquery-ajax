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
        public JsonResult GetStudents()
        {
            var students = _db.Students.ToList();

            return Json(students);
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
        public JsonResult InsertStudent(StudentViewModel studentViewModel)
        {
            using var transaction = _db.Database.BeginTransaction();
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
                        ImageUrl = imageUrl,
                        CourseId = studentViewModel.CourseId
                    };

                    _db.Students.Add(student);
                    _db.SaveChanges();

                    

                    transaction.Commit();
                    return Json(new { success = true, message = "Student and modules added successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save profile image." });
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Json(new { success = false, message = "Error: " + ex.Message });
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
