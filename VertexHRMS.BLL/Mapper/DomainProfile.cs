using AutoMapper;
using VertexHRMS.BLL.ModelVM.AttendanceRecords;
using VertexHRMS.BLL.ModelVM.Payroll;
using VertexHRMS.BLL.ModelVM.ViewModels;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<PayrollRun, GetRunVM>().ReverseMap();
            CreateMap<Payroll, GetPayrollVM>().ReverseMap();
            CreateMap<AttendanceRecord, AttendanceRecordsVM>()
            .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee != null
                    ? (src.Employee.FirstName ?? "") + " " + (src.Employee.LastName ?? "")
                    : ""))
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Employee != null && src.Employee.Department != null
                    ? src.Employee.Department.DepartmentName
                    : ""))
            .ForMember(dest => dest.Position,
                opt => opt.MapFrom(src => src.Employee != null && src.Employee.Position != null
                    ? src.Employee.Position.PositionName
                    : ""));
            CreateMap<AttendanceRecordsVM, AttendanceRecord>()
            .ForMember(dest => dest.AttendanceRecordId, opt => opt.Ignore()) 
            .ForMember(dest => dest.Employee, opt => opt.Ignore()) 
            .ForMember(dest => dest.CheckIn, opt => opt.MapFrom(src => src.CheckIn == default ? DateTime.Now : src.CheckIn))
            .ForMember(dest => dest.CheckOut, opt => opt.MapFrom(src => src.CheckOut))
            .ForMember(dest => dest.WorkHours, opt => opt.MapFrom(src => src.WorkHours))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.AttendanceDate, opt => opt.MapFrom(src => src.CheckIn.Date));

            // Employee mappings
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.PositionName))

                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.ImagePath) ? "/images/default-avatar.png" : src.ImagePath));
        }
    }
    
}
