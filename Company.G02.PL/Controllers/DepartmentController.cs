using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace Company.G02.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;



        //ASK CLR Create Object of DepartmentRepository
        public DepartmentController( IUnitOfWork unitOfWork
            //IDepartmentRepository departmentRepository
            , IMapper mapper)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        [HttpGet] //Get: /Department/Index
        public IActionResult Index()
        {
            //DepartmentRepository _departmentRepository = new DepartmentRepository();
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            return View(departments);
        }
        [HttpGet] //Get: /Department/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] 
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) //Server side validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };

              _unitOfWork.DepartmentRepository.Add(department);
                var count = _unitOfWork.Complete();

                if (count > 0 )
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id ,string viewName= "Details")
        {
            if (id is null)
            {
                return BadRequest("Invalid Id"); //400
            }
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);

            if (department is null)
            {
                return NotFound(new {statusCode = 404 , message = $"Department with Id : {id} is not Found"}); //404
            }
            return View(viewName ,department);
        }


        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    //if (id is null)
        //    //{
        //    //    return BadRequest("Invalid Id"); //400
        //    //}
        //    //var department = _departmentRepository.Get(id.Value);

        //    //if (department is null)
        //    //{
        //    //    return NotFound(new { statusCode = 404, message = $"Department with Id : {id} is not Found" }); //404
        //    //}

        //    //return View(department);

        //    return Details(id, "Edit");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, Department department)
        //{

        //    if (ModelState.IsValid) //Server side validation
        //    {
        //        if (id != department.Id)
        //        {
        //            return BadRequest();
        //        }
        //        var count = _departmentRepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(department);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model)
        //{

        //    if (ModelState.IsValid) //Server side validation
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Code = model.Code,
        //            Name = model.Name,
        //            CreateAt = model.CreateAt,
        //        };
        //        var count = _departmentRepository.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(model);
        //}


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null)

            //{
            //    return BadRequest("Invalid Id"); //400
            //}
            //var department = _departmentRepository.Get(id.Value);

            //if (department is null)
            //{
            //    return NotFound(new { statusCode = 404, message = $"Department with Id : {id} is not Found" }); //404
            //}
            //return View(department);
            return Details(id, "Delete");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var department = _unitOfWork.DepartmentRepository.Get(id); // Get method in repo
            if (department == null) return NotFound();

           _unitOfWork.DepartmentRepository.Delete(department);
            var count = _unitOfWork.Complete();

            if (count > 0)
                return RedirectToAction(nameof(Index));

            return View(department); // لو Delete فشل
        }




        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest("Invalid Id");

            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department == null)
                return NotFound();

            var dto = _mapper.Map<CreateDepartmentDto>(department);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {

                var existingDept = _unitOfWork.DepartmentRepository.Get(id);
                if (existingDept == null)
                    return NotFound();

                _mapper.Map(model, existingDept); // Copy values from DTO to existing entity
              _unitOfWork.DepartmentRepository.Update(existingDept);
                var count = _unitOfWork.Complete();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
              return View(model);

        }

    }
}
