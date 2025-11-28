using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.MappingProfile
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap()
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId,
                           opt => opt.MapFrom(src =>
                               src.Department != null ? src.Department.Id : src.DepartmentId));
        }
    }
}
