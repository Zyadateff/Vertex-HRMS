using AutoMapper;
using VertexHRMS.BLL.ModelVM.ViewModels;
using VertexHRMS.BLL.ModelVM.Models;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Department mappings
            CreateMap<Department, DepartmentViewModel>();

            // Position mappings
            CreateMap<Position, PositionViewModel>();

            // Employee mappings
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.PositionName))
               
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.ImagePath) ? "/images/default-avatar.png" : src.ImagePath));
        }
    }
}
