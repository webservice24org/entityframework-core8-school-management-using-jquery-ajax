using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MohiuddinCoreMasterDetailCrud.Models;
using MohiuddinCoreMasterDetailCrud.Models.ViewModels;

namespace MohiuddinCoreMasterDetailCrud.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly MohiuddinCoreMasterDetailsContext _context;
        private readonly IWebHostEnvironment _webHost;

        public InstructorsController(MohiuddinCoreMasterDetailsContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> InsertInstructor(InstructorViewModel instructor)
        {
            if (ModelState.IsValid)
            {
                var newInstructor = new Instructor
                {
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    JoinDate = instructor.JoinDate,
                    Mobile = instructor.Mobile
                };

                _context.Instructors.Add(newInstructor);
                await _context.SaveChangesAsync();

                var instructorDetails = new InstructorDetails
                {
                    InstructorID = newInstructor.InstructorID,
                    PresentAddress = instructor.PresentAddress,
                    PermanentAddress = instructor.PermanentAddress,
                    Dob = instructor.Dob ?? DateTime.MinValue, // Default to min value if null
                    Salary = instructor.Salary ?? 0, // Default to 0 if null
                    InstructorPicture = instructor.InstructorProfile != null ? GetUploadedFileName(instructor) : null
                };

                _context.InstructorDetails.Add(instructorDetails);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(instructor.OfficeAssignment?.Location))
                {
                    var officeAssignment = new OfficeAssignment
                    {
                        InstructorID = newInstructor.InstructorID,
                        Location = instructor.OfficeAssignment.Location
                    };

                    _context.OfficeAssignments.Add(officeAssignment);
                    await _context.SaveChangesAsync();
                }

                if (instructor.SelectedCourseIDs != null && instructor.SelectedCourseIDs.Any())
                {
                    foreach (var courseId in instructor.SelectedCourseIDs)
                    {
                        var courseInstructor = new CourseInstructor
                        {
                            CourseId = courseId,
                            InstructorID = newInstructor.InstructorID
                        };

                        _context.CourseInstructor.Add(courseInstructor);
                    }
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, message = "Instructor inserted successfully" });
            }

            return Json(new { success = false, message = "Invalid model state" });
        }


        private string GetUploadedFileName(InstructorViewModel instructorpic)
        {
            string uniqueFileName = null;

            if (instructorpic.InstructorProfile != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + instructorpic.InstructorProfile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    instructorpic.InstructorProfile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        public JsonResult GetAllCourses()
        {
            var courses = _context.Courses
                .Include(c => c.Department) 
                .Select(c => new {
                    c.CourseId,
                    c.CourseName
                })
                .ToList();

            return Json(courses);
        }

    }
}
