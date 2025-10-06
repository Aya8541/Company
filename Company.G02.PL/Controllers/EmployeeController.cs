using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        //ASK CLR Create Object of EmployeeRepository
        public EmployeeController(  IUnitOfWork unitOfWork
                                  //IEmployeeRepository EmployeeRepository
                                  //,IDepartmentRepository departmentRepository
                                  , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = EmployeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;

        }
        [HttpGet] //Get: /Employee/Index
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            
            if (string.IsNullOrEmpty(SearchInput))
            {
                  employees =await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                  employees =await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);

            }
            //Dictionary : 3 Property ways to pass Extra data from controller(Action) to view
            //1.ViewData : pass Extra data from controller(Action) to view
            //ViewData["Message"] = "Hello from ViewData";


            ////2.ViewBag  : pass Extra data from controller(Action) to view
            // ViewBag.Message = "Hello from ViewBag"; 

            //3.TempData
            return View(employees);
        }

        [HttpGet] //Get: /Employee/Create
        public IActionResult Create()
        {
            //var departments=_departmentRepository.GetAll();
            //ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) //Server side validation
            {
                //Manual Mapping
                //var employee = new Employee()
                //{
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreateAt = model.CreateAt,
                //    HiringDate = model.HiringDate,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    Phone = model.Phone,    
                //    Salary = model.Salary,
                //    DepartmentId = model.DepartmentId,//....
                //};
                if(model.Image is not null)
                {
                  model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee =_mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count =await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created !! ";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest("Invalid Id"); //400
            }
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee with Id : {id} is not Found" }); //404
            }
            return View(viewName, employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //var departments = _departmentRepository.GetAll();
            //ViewData["departments"] = departments;
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null)
            {
                return NotFound(new { statusCode = 404, message = $"Employee with Id : {id} is not Found" }); //404
            }
            //var employeeDto = new CreateEmployeeDto()
            //{
            //    Name = employee.Name,
            //    Address = employee.Address,
            //    Age = employee.Age,
            //    CreateAt = employee.CreateAt,
            //    HiringDate = employee.HiringDate,
            //    Email = employee.Email,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary,
            //    DepartmentId = employee.DepartmentId,//....

            //};
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        {

            if (ModelState.IsValid) //Server side validation
            {
                //if (id != model.Id)
                //{
                //    return BadRequest();
                //}
                //var employee = new Employee()
                //{
                //    Id = id,
                //    Name = model.EmpName,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreateAt = model.CreateAt,
                //    HiringDate = model.HiringDate,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    DepartmentId = model.DepartmentId,//....

                //};

                var existingEmp =await _unitOfWork.EmployeeRepository.GetAsync(id);
                if (existingEmp == null)
                    return NotFound();
                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }
                _mapper.Map(model, existingEmp); // Copy values from DTO to existing entity

                _unitOfWork.EmployeeRepository.Update(existingEmp);
                var count =await _unitOfWork.CompleteAsync();
                
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task <IActionResult >Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete([FromRoute] int id, Employee model)
        //{

        //    if (ModelState.IsValid) //Server side validation
        //    {
        //        if (id != model.Id)
        //        {
        //            return BadRequest();
        //        }
        //        var count = _employeeRepository.Delete(model);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(model);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee =await _unitOfWork.EmployeeRepository.GetAsync(id); // Get method in repo
            if (employee == null) return NotFound();

            _unitOfWork.EmployeeRepository.Delete(employee);
            var count = await _unitOfWork.CompleteAsync();

            if (count > 0)
            {
                if (employee.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employee.ImageName, "images");
                }
                return RedirectToAction(nameof(Index));

            }

            return View(employee); // لو Delete فشل
        }

    }
}
