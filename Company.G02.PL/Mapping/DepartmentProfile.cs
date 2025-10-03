using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;

namespace Company.G02.PL.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<Department, CreateDepartmentDto>();
        }
    }

}
