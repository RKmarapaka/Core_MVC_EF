using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core_MVC_EF.Models;
using Core_MVC_EF.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core_MVC_EF.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly AppDbContext context;

        public EmployeeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment, AppDbContext context)
        {
           _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        // Populate Department values to DropDownList
        private IEnumerable<SelectListItem> GetDepartments()
        {
            return context.Departments
                .Select(s => new SelectListItem
                {
                    Value=s.DepartmentId.ToString(),
                    Text=s.DepartmentName
                }).ToList();
        }

        [HttpGet]
        public ViewResult Create()
        {
            ViewBag.DeptListName = GetDepartments();

            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            
            if (ModelState.IsValid)
            {

                string uniqueFileName = ProcessUploadFile(model);

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email=model.Email,
                    City=model.City,
                    DepartmentId=model.DepartmentId,
                    PhotoPath= uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
           
            Employee employee = _employeeRepository.GetEmployee(id);
            ViewBag.DeptListName = GetDepartments();
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                City = employee.City,
                DepartmentId = employee.DepartmentId,
                ExistingPhotoPath = employee.PhotoPath,
             
     
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.City = model.City;
                employee.DepartmentId = model.DepartmentId;
                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath !=null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadFile(model);
                }
                _employeeRepository.Update(employee);

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            Employee employee = _employeeRepository.GetEmployee(id.Value);

            EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel 
            { 
                Employee =employee,
                PageTitle="Employee Details"
            };

            return View(employeeDetailsViewModel);
        }
        
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = _employeeRepository.GetEmployee(id);

            EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel
            {
                Employee = employee,
                PageTitle = "Employee Delete"
            };

            if (employee == null)
            {
                return NotFound();
            }
            return View(employeeDetailsViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _employeeRepository.Delete(id);

            return RedirectToAction("Index");
        }
        private string ProcessUploadFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string UploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(UploadsFolder, uniqueFileName);
                using(var fileStream=new FileStream(filePath, FileMode.Create))
                model.Photo.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}