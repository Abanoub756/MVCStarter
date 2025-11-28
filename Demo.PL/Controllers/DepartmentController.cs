using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            // ask clr to crreate object from class inplementing interface IDepartmentRepository
            // using dependency injection
        }

        #region Actions
        // BaseUrl/Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)// server side validation
            {
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                int result = await _unitOfWork.CompleteAsync();
                // TempData => dictionary Key Value pair
                // transfer data from current request to next request
                if (result > 0)
                    TempData["Message"] = "Department Created Successfully";
                else
                    TempData["Message"] = "Faild to Create Department";
                
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // BaseUrl/Department/Details

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id == null)
            {
                return BadRequest();
            }
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }
            return View(ViewName, department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return BadRequest();
            //}
            //var department = _unitOfWork.GetById(id.Value);
            //if (department == null)
            //{
            //    return NotFound();
            //}
            //return View(department);

            return await Details(id, "Edit");

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department, [FromRoute] int id)
        {
            if (ModelState.IsValid)// server side validation
            {
                try
                {
                     _unitOfWork.DepartmentRepository.Update(department);
                     await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Department department = null;
            try
            {
                department =await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
                _unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(department); 
        }



        #endregion

    }
}
