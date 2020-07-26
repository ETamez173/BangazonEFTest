using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BangazonEFTest.Data;
using BangazonEFTest.Models;
using BangazonEFTest.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace BangazonEFTest.Controllers
{
    public class EmployeesController : Controller
    {

        // this is called Dependancy Injection (DI) about the 32 min point on the 042020 Entity Framework video
        // _context represent the Database and we now have access to it 
        // DI makes things very testable and flexible
        // Our controllers have a have reference to our database via the context field
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: EmployeesController
        public ActionResult Index()
        {
            // This queries the Employee table. Using the `Include` method will join the related entities
            var employees = _context.Employee
                .Include(e => e.Computer)
                .Include(e => e.Department)
                .ToList();

            return View(employees);
        }

        // GET: EmployeesController/Details/5
        public ActionResult Details(int id)
        {
            //var employee = _context.Employee;
                //.Include(e => e.Computer)
                //.Include(e => e.Department);

            return View();
        }

        // GET: EmployeesController/Create
        public async Task<ActionResult> Create()
        {

            // Entity Framework will always return data from the DB in the form of data models
            // If data models are not the type object you want for your view then you can use the "SELECT" method
            // Example - we dont want a list of Computer and Dept Objects here. We want a list of 
            // SelectListItems.
            // See EFTest_intro_p3 around 34min pnt

            var allComputers = await _context.Computer
                .Select(d => new SelectListItem() { Text = d.Model, Value = d.Id.ToString() })
                .ToListAsync();
            var allDepartments = await _context.Department
                .Select(d => new SelectListItem() { Text = d.Name, Value = d.Id.ToString() })
                .ToListAsync();

            var viewModel = new EmployeeCreateViewModel();

            viewModel.DepartmentOptions = allDepartments;
            viewModel.ComputerOptions = allComputers;


            return View(viewModel);
        }

        // POST: EmployeesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmployeeCreateViewModel employeeViewModel)
        {
            try
            {
                // Everything we do in EF has to do with Datamodels
                // we have to create a datamodel from a viewmodel (see 1.07 min point on 042020 EF video)
                // EFTest_intro_p3 around 51 min

                var employee = new Employee
                {
                    FirstName = employeeViewModel.FirstName,
                    LastName = employeeViewModel.LastName,
                    Email = employeeViewModel.Email,
                    DepartmentId = employeeViewModel.DepartmentId,
                    ComputerId = employeeViewModel.ComputerId

                };

                _context.Employee.Add(employee);
                // savechanges helps with performance as it opens one connection to DB and commit all of the changes
                // we have made up until that point to the DB
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            var allComputers = await _context.Computer
                 .Select(c => new SelectListItem() { Text = c.Model, Value = c.Id.ToString() })
                 .ToListAsync();

            var allDepartments = await _context.Department
                .Select(d => new SelectListItem() { Text = d.Name, Value = d.Id.ToString() })
                .ToListAsync();


            var employee = _context.Employee.FirstOrDefault(e => e.Id == id);

            var viewModel = new EmployeeCreateViewModel()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                ComputerId = employee.ComputerId,
                ComputerOptions = allComputers,
                DepartmentOptions = allDepartments
            };



            return View(viewModel);
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EmployeeCreateViewModel employeeViewModel)
        {
            try
            {
                // TODO: Add update logic here

                // Listen to Adam on EFTest_Intro_p3 around 1hr6min
                // he explains that this is the reverse of before viewmodel employee to datamodel employee
                var employee = new Employee()

                {
                    Id = id,
                    FirstName = employeeViewModel.FirstName,
                    LastName = employeeViewModel.LastName,
                    Email = employeeViewModel.Email,
                    ComputerId = employeeViewModel.ComputerId,
                    DepartmentId = employeeViewModel.DepartmentId,

                };

                _context.Employee.Update(employee);
                await _context.SaveChangesAsync();
                // promises in JS where for fetch calls
                // the burger example


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: EmployeesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
