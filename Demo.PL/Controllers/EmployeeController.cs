using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            // ask clr for object from class that implements IEmployeeRepository
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            #region ViewData info
            //1- viewData => dictionary object to pass data from controller to view 
            //key value pair 
            // transfer data from controller (Action) to view 
            //.net framework 3.5

            //2- viewBag => dynamic property [based on dynamic keyword] to pass data from controller to view
            // .net framework 4.0
            #endregion

            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(SearchValue))
            {
                employees =await _unitOfWork.EmployeeRepository.GetAllAsync();

            }
            else
            {
                employees =await _unitOfWork.EmployeeRepository.GetEMployeeByName(SearchValue);
            }

            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            ViewData["Message"] = "Employees List from ViewDate";


            ViewBag.Message = "Message from ViewBag";
            return View(MappedEmployees);

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = (await _unitOfWork.DepartmentRepository.GetAllAsync()).ToList();
            ViewBag.Department = new SelectList(departments, "Id", "Name");
            return View(new EmployeeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeView)
        {
            if (ModelState.IsValid)
            {
                #region Manual mapping
                // Manual Mapping
                //var mappedEmployee = new Employee()
                //{
                //    Name = employeeView.Name,
                //    Age = employeeView.Age,
                //    Address = employeeView.Address,
                //    Email = employeeView.Email,
                //    Phone = employeeView.Phone,

                //};
                #endregion

                // Auto Mapping
                employeeView.ImageName = DocumentSettings.UploadFile(employeeView.Image, "Images");
                var mappedEmployee = _mapper.Map<Employee>(employeeView);

                await _unitOfWork.EmployeeRepository.AddAsync(mappedEmployee);
                // Update in database
                // Delete from database
                // Insert in database
                // Save in database
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Department = new SelectList(await _unitOfWork.DepartmentRepository.GetAllAsync(), "Id", "Name");
            return View(employeeView);
        }

        [HttpGet]

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName)
        {
            if (id == null)
                return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

            if (employee == null)
                return NotFound();

            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            // ✔ If the view is Edit, load dropdown BEFORE returning view
            if (ViewName == "Edit")
            {
                ViewBag.Department = new SelectList(
                    await _unitOfWork.DepartmentRepository.GetAllAsync(),
                    "Id",
                    "Name"
                );
            }

            return View(ViewName, mappedEmployee);
        }


        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeView)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Only upload a new image if the user selected one
                    if (employeeView.Image != null)
                    {
                        employeeView.ImageName = DocumentSettings.UploadFile(employeeView.Image, "Images");
                    }

                    // Now map to Employee
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeView);

                    // Update the entity
                    _unitOfWork.EmployeeRepository.Update(mappedEmployee);

                    // Save to database
                    await _unitOfWork.CompleteAsync();

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            ViewBag.Department = new SelectList(await _unitOfWork.DepartmentRepository.GetAllAsync(), "Id", "Name");
            return View(employeeView);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeView, [FromRoute] int id)
        {
            if (id != employeeView.Id)
            {
                return BadRequest();
            }
            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeView);
                _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                var result =await _unitOfWork.CompleteAsync();
                if (result > 0 && employeeView.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employeeView.ImageName, "Images");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(employeeView);
        }



    }
}
