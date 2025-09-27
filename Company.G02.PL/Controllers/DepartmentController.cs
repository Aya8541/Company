using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        //ASK CLR Create Object of DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;

        }
        [HttpGet] //Get: /Department/Index
        public IActionResult Index()
        {
            //DepartmentRepository _departmentRepository = new DepartmentRepository();
            var departments = _departmentRepository.GetAll();

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

              var count =  _departmentRepository.Add(department);
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
            var department = _departmentRepository.Get(id.Value);

            if (department is null)
            {
                return NotFound(new {statusCode = 404 , message = $"Department with Id : {id} is not Found"}); //404
            }
            return View(viewName ,department);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
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

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {

            if (ModelState.IsValid) //Server side validation
            {
                if (id != department.Id)
                {
                    return BadRequest();
                }
                var count = _departmentRepository.Update(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }

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
        public IActionResult Delete([FromRoute] int id, Department department)
        {

            if (ModelState.IsValid) //Server side validation
            {
                if (id != department.Id)
                {
                    return BadRequest();
                }
                var count = _departmentRepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }


    }
}
