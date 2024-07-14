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
using Newtonsoft.Json;

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
        [HttpGet]
        public async Task<JsonResult> GetInstructors()
        {
            var instructors = await _context.Instructors
                .Join(
                    _context.InstructorDetails,
                    instructor => instructor.InstructorID,
                    details => details.InstructorID,
                    (instructor, details) => new
                    {
                        instructor.InstructorID,
                        InstructorName = instructor.FirstName + " " + instructor.LastName,
                        instructor.JoinDate,
                        details.InstructorPicture
                    }
                )
                .ToListAsync();

            return Json(instructors);
        }

        [HttpGet]
        public async Task<JsonResult> GetInstructorDetails(int id)
        {
            try
            {
                var instructorDetails = await _context.Instructors
                    .Where(i => i.InstructorID == id)
                    .Include(i => i.InstructorDetails)  // Include InstructorDetails
                    .Include(i => i.OfficeAssignment)  // Include OfficeAssignment
                    .Select(i => new
                    {
                        Instructor = i,
                        InstructorDetails = i.InstructorDetails,
                        OfficeAssignment = i.OfficeAssignment,
                        Courses = _context.CourseInstructor
                            .Where(ci => ci.InstructorID == i.InstructorID)
                            .Select(ci => new
                            {
                                Course = _context.Courses
                                    .Include(c => c.Department) // Include Department
                                    .FirstOrDefault(c => c.CourseId == ci.CourseId)
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (instructorDetails == null)
                {
                    return Json(new { success = false, message = "Instructor not found" });
                }

                var instructorViewModel = new
                {
                    instructor = new
                    {
                        id = instructorDetails.Instructor.InstructorID,
                        firstName = instructorDetails.Instructor.FirstName,
                        lastName = instructorDetails.Instructor.LastName,
                        joinDate = instructorDetails.Instructor.JoinDate,
                        mobile = instructorDetails.Instructor.Mobile
                        // Add more properties as needed
                    },
                    instructorDetails = new
                    {
                        presentAddress = instructorDetails.InstructorDetails?.PresentAddress,
                        permanentAddress = instructorDetails.InstructorDetails?.PermanentAddress,
                        dob = instructorDetails.InstructorDetails?.Dob,
                        salary = instructorDetails.InstructorDetails?.Salary,
                        instructorPicture = instructorDetails.InstructorDetails?.InstructorPicture
                        // Add more details from InstructorDetails model
                    },
                    officeAssignment = new
                    {
                        location = instructorDetails.OfficeAssignment?.Location
                        // Add more properties from OfficeAssignment model
                    },
                    courses = instructorDetails.Courses.Select(c => new
                    {
                        courseId = c.Course.CourseId,
                        courseName = c.Course.CourseName,
                        departmentName = c.Course.Department.DepartmentName // Include DepartmentName
                                                                            // Add more course properties as needed
                    }).ToList()
                };

                return Json(new { success = true, instructorDetails = instructorViewModel });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error retrieving instructor details: {ex.Message}" });
            }
        }



        [HttpPost]
        public async Task<JsonResult> InsertInstructor([FromForm] InstructorViewModel instructor)
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
                Dob = instructor.Dob ?? DateTime.MinValue,
                Salary = instructor.Salary ?? 0,
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
