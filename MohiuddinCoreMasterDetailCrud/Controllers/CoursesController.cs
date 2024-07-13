using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MohiuddinCoreMasterDetailCrud.Models;
using MohiuddinCoreMasterDetailCrud.Models.ViewModels;

namespace MohiuddinCoreMasterDetailCrud.Controllers
{
    public class CoursesController : Controller
    {
        private readonly MohiuddinCoreMasterDetailsContext _context;

        public CoursesController(MohiuddinCoreMasterDetailsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
public JsonResult Insert([FromForm] CourseViewModel courseViewModel)
{
    if (ModelState.IsValid)
    {
        var course = new Course
        {
            CourseName = courseViewModel.CourseName,
            DepartmentID = courseViewModel.DepartmentID
        };

        _context.Add(course);
        _context.SaveChanges();
        return Json(new { success = true });
    }

    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
}


        [HttpGet]
        public JsonResult EditCourse(int id)
        {
            var course = _context.Courses
                .Include(c => c.Department)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null)
            {
                return Json(new { success = false, message = "Course not found" });
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    course.CourseId,
                    course.CourseName,
                    DepartmentID = course.DepartmentID, // Corrected property name to match CourseViewModel
                    DepartmentName = course.Department.DepartmentName
                }
            });
        }


        [HttpPost]
        public JsonResult UpdateCourse([FromForm] CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                var existingCourse = _context.Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
                if (existingCourse != null)
                {
                    existingCourse.CourseName = course.CourseName;
                    existingCourse.DepartmentID = course.DepartmentID; 
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    
                    return Json(new { success = false, message = "Course not found" });
                }
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }


        [HttpPost]
        public JsonResult DeleteCourse(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                return Json(new { success = false, message = "Course not found" });
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return Json(new { success = true });
        }


        public JsonResult GetCourses()
        {
            var courses = _context.Courses
                .Include(c => c.Department)  // Include Department for joining
                .Select(c => new {
                    c.CourseId,
                    c.CourseName,
                    DepartmentName = c.Department.DepartmentName // Include DepartmentName
                })
                .ToList();

            return Json(courses);
        }

        [HttpGet]
        public JsonResult GetDepartments()
        {
            var departments = _context.Departments.Select(d => new { d.DepartmentID, d.DepartmentName }).ToList();
            return Json(departments);
        }

    }
}
