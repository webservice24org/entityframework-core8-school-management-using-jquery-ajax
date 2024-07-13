using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MohiuddinCoreMasterDetailCrud.Models;
using MohiuddinCoreMasterDetailCrud.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MohiuddinCoreMasterDetailCrud.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly MohiuddinCoreMasterDetailsContext _context;

        public DepartmentsController(MohiuddinCoreMasterDetailsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var department = new DepartmentViewModel
            {
                
            };

            return View(department);
        }

        [HttpGet]
        public async Task<JsonResult> GetDepartments()
        {
            var departments = await _context.Departments
                .Include(d => d.Administrator)
                .Select(d => new
                {
                    d.DepartmentID,
                    d.DepartmentName,
                    d.Budget,
                    d.StartDate,
                    d.InstructorID,
                    InstructorName = d.Administrator.FirstName + " " + d.Administrator.LastName
                }).ToListAsync();

            return Json(departments);
        }


        //[HttpGet]
        //public async Task<JsonResult> GetDepartment(int id)
        //{
        //    var department = await _context.Departments
        //        .Include(d => d.Administrator)
        //        .FirstOrDefaultAsync(d => d.DepartmentID == id);

        //    if (department == null)
        //    {
        //        return Json(new { success = false, message = "Department not found" });
        //    }

        //    var departmentViewModel = new DepartmentViewModel
        //    {
        //        DepartmentID = department.DepartmentID,
        //        DepartmentName = department.DepartmentName,
        //        Budget = department.Budget,
        //        StartDate = department.StartDate,
        //        InstructorID = department.InstructorID,
        //        InstructorName = department.Administrator.FirstName + " " + department.Administrator.LastName
        //    };

        //    return Json(new { success = true, data = departmentViewModel });
        //}

        [HttpPost]
        public JsonResult CreateDepartment([FromForm] DepartmentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid data." });
                }

                var department = new Department
                {
                    DepartmentName = model.DepartmentName,
                    Budget = model.Budget,
                    StartDate = model.StartDate,
                    InstructorID = model.InstructorID
                };

                _context.Departments.Add(department);
                _context.SaveChanges();

                return Json(new { success = true, message = "Department created successfully" });
            }
            catch (DbUpdateException ex)
            {
                var detailedError = GetFullErrorMessage(ex);
                return Json(new { success = false, message = detailedError });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string GetFullErrorMessage(DbUpdateException exception)
        {
            var message = exception.Message;
            if (exception.InnerException != null)
            {
                message += " Inner Exception: " + exception.InnerException.Message;
            }
            return message;
        }




        [HttpPut]
        public async Task<JsonResult> UpdateDepartment([FromBody] DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = await _context.Departments.FindAsync(model.DepartmentID);

                if (department == null)
                {
                    return Json(new { success = false, message = "Department not found" });
                }

                department.DepartmentName = model.DepartmentName;
                department.Budget = model.Budget;
                department.StartDate = model.StartDate;
                department.InstructorID = model.InstructorID;

                _context.Departments.Update(department);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Department updated successfully" });
            }

            return Json(new { success = false, message = "Invalid model state" });
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return Json(new { success = false, message = "Department not found" });
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Department deleted successfully" });
        }

        [HttpGet]
        public JsonResult GetInstructors()
        {
            var instructors = _context.Instructors
                .Select(i => new
                {
                    InstructorID = i.InstructorID,
                    Administrator = i.FirstName + " " + i.LastName
                })
                .ToList();
            return Json(instructors);
        }


    }
}
