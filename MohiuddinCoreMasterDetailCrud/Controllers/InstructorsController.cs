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
                    .Include(i => i.InstructorDetails) 
                    .Include(i => i.OfficeAssignment)  
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
                                    .Include(c => c.Department) 
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
                    },
                    instructorDetails = new
                    {
                        presentAddress = instructorDetails.InstructorDetails?.PresentAddress,
                        permanentAddress = instructorDetails.InstructorDetails?.PermanentAddress,
                        dob = instructorDetails.InstructorDetails?.Dob,
                        salary = instructorDetails.InstructorDetails?.Salary,
                        instructorPicture = instructorDetails.InstructorDetails?.InstructorPicture
                    },
                    officeAssignment = new
                    {
                        location = instructorDetails.OfficeAssignment?.Location
                    },
                    courses = instructorDetails.Courses.Select(c => new
                    {
                        courseId = c.Course.CourseId,
                        courseName = c.Course.CourseName,
                        departmentName = c.Course.Department.DepartmentName 
                                                                            
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

        [HttpGet]
        public async Task<JsonResult> EditInstructor(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.InstructorDetails)
                .Include(i => i.CourseInstructor)
                    .ThenInclude(ci => ci.Course)
                .Include(i => i.OfficeAssignment)  
                .Where(i => i.InstructorID == id)
                .Select(i => new
                {
                    i.InstructorID,
                    i.FirstName,
                    i.LastName,
                    i.JoinDate,
                    i.Mobile,
                    InstructorDetailsID = i.InstructorDetails.Id,
                    i.InstructorDetails.PresentAddress,
                    i.InstructorDetails.PermanentAddress,
                    i.InstructorDetails.Dob,
                    i.InstructorDetails.Salary,
                    i.InstructorDetails.InstructorPicture,
                    SelectedCourseIDs = i.CourseInstructor.Select(ci => ci.CourseId).ToList(),
                    OfficeLocation = i.OfficeAssignment.Location  
                })
                .FirstOrDefaultAsync();

            if (instructor == null)
            {
                return Json(new { success = false, message = "Instructor not found" });
            }

            return Json(new { success = true, data = instructor });
        }


        [HttpPost]
        public async Task<JsonResult> UpdateInstructor([FromForm] InstructorViewModel instructor)
        {
            var existingInstructor = await _context.Instructors
                .Include(i => i.InstructorDetails)
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseInstructor)
                .FirstOrDefaultAsync(i => i.InstructorID == instructor.InstructorID);

            if (existingInstructor == null)
            {
                return Json(new { success = false, message = "Instructor not found" });
            }

            existingInstructor.FirstName = instructor.FirstName;
            existingInstructor.LastName = instructor.LastName;
            existingInstructor.JoinDate = instructor.JoinDate;
            existingInstructor.Mobile = instructor.Mobile;

            if (existingInstructor.InstructorDetails == null)
            {
                existingInstructor.InstructorDetails = new InstructorDetails
                {
                    InstructorID = existingInstructor.InstructorID,
                    PresentAddress = instructor.PresentAddress,
                    PermanentAddress = instructor.PermanentAddress,
                    Dob = instructor.Dob ?? DateTime.MinValue,
                    Salary = instructor.Salary ?? 0,
                    InstructorPicture = instructor.InstructorProfile != null ? GetUploadedFileName(instructor) : null
                };
                _context.InstructorDetails.Add(existingInstructor.InstructorDetails);
            }
            else
            {
                existingInstructor.InstructorDetails.PresentAddress = instructor.PresentAddress;
                existingInstructor.InstructorDetails.PermanentAddress = instructor.PermanentAddress;
                existingInstructor.InstructorDetails.Dob = instructor.Dob ?? DateTime.MinValue;
                existingInstructor.InstructorDetails.Salary = instructor.Salary ?? 0;

                if (instructor.InstructorProfile != null)
                {
                    if (!string.IsNullOrEmpty(existingInstructor.InstructorDetails.InstructorPicture))
                    {
                        var oldImagePath = Path.Combine(_webHost.WebRootPath, "Images", existingInstructor.InstructorDetails.InstructorPicture);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    existingInstructor.InstructorDetails.InstructorPicture = GetUploadedFileName(instructor);
                }
            }

            if (!string.IsNullOrWhiteSpace(instructor.OfficeAssignment?.Location))
            {
                if (existingInstructor.OfficeAssignment == null)
                {
                    existingInstructor.OfficeAssignment = new OfficeAssignment
                    {
                        InstructorID = existingInstructor.InstructorID,
                        Location = instructor.OfficeAssignment.Location
                    };
                    _context.OfficeAssignments.Add(existingInstructor.OfficeAssignment);
                }
                else
                {
                    existingInstructor.OfficeAssignment.Location = instructor.OfficeAssignment.Location;
                }
            }

            var existingCourseAssignments = _context.CourseInstructor.Where(ci => ci.InstructorID == existingInstructor.InstructorID);
            _context.CourseInstructor.RemoveRange(existingCourseAssignments);

            if (instructor.SelectedCourseIDs != null && instructor.SelectedCourseIDs.Any())
            {
                foreach (var courseId in instructor.SelectedCourseIDs)
                {
                    var courseInstructor = new CourseInstructor
                    {
                        CourseId = courseId,
                        InstructorID = existingInstructor.InstructorID
                    };
                    _context.CourseInstructor.Add(courseInstructor);
                }
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Instructor updated successfully" });
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

        [HttpPost]
        public async Task<JsonResult> DeleteInstructor(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.InstructorDetails)
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseInstructor)
                .FirstOrDefaultAsync(i => i.InstructorID == id);

            if (instructor == null)
            {
                return Json(new { success = false, message = "Instructor not found" });
            }

            if (instructor.InstructorDetails != null)
            {
                if (!string.IsNullOrEmpty(instructor.InstructorDetails.InstructorPicture))
                {
                    var imagePath = Path.Combine(_webHost.WebRootPath, "Images", instructor.InstructorDetails.InstructorPicture);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.InstructorDetails.Remove(instructor.InstructorDetails);
            }

            if (instructor.OfficeAssignment != null)
            {
                _context.OfficeAssignments.Remove(instructor.OfficeAssignment);
            }

            var courseAssignments = _context.CourseInstructor.Where(ci => ci.InstructorID == id);
            _context.CourseInstructor.RemoveRange(courseAssignments);

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Instructor deleted successfully" });
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
