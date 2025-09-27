using AutoMapper;
using VertexHRMS.BLL.ModelVM;
using VertexHRMS.BLL.ModelVM.AttendanceRecords;
using VertexHRMS.BLL.ModelVM.Department;
using VertexHRMS.BLL.ModelVM.Employees;
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

            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.PositionName))

                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.ImagePath) ? "/images/default-avatar.jpg" : src.ImagePath));
            CreateMap<Employee, ProfileVM>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.PositionName))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null
                                                      ? $"{src.Manager.FirstName} {src.Manager.LastName}"
                                                      : null))
            .ForMember(dest => dest.TasksCount, opt => opt.MapFrom(src => src.Tasks.Count))
            .ForMember(dest => dest.ProjectsCount, opt => opt.MapFrom(src => src.Tasks
                                                                    .Select(t => t.ProjectId)
                                                                    .Distinct()
                                                                    .Count()));
            CreateMap<ProfileVM, Employee>()
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore()) // عشان ما يبوظش الـ PK
            .ForMember(dest => dest.Department, opt => opt.Ignore()) // علاقات navigation
            .ForMember(dest => dest.Position, opt => opt.Ignore())
            .ForMember(dest => dest.Manager, opt => opt.Ignore())
            .ForMember(dest => dest.DirectReports, opt => opt.Ignore())
            .ForMember(dest => dest.Tasks, opt => opt.Ignore())
            .ForMember(dest => dest.Trainings, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveRequests, opt => opt.Ignore())
            .ForMember(dest => dest.OvertimeRequests, opt => opt.Ignore())
            .ForMember(dest => dest.Resignations, opt => opt.Ignore())
            .ForMember(dest => dest.AttendanceRecords, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveEntitlements, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveLedgerEntries, opt => opt.Ignore())
            .ForMember(dest => dest.Payrolls, opt => opt.Ignore())
            .ForMember(dest => dest.DepartmentId, opt => opt.Ignore()) 
            .ForMember(dest => dest.PositionID, opt => opt.Ignore())   
            .ForMember(dest => dest.ManagerId, opt => opt.Ignore())
            .ForMember(dest => dest.ImagePath, opt => opt.Ignore());

            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
            CreateMap<Employee, GetAllUserByDepartmentIdVM>().ReverseMap();
            CreateMap<Department, GetDepartmentCardsVM>().ReverseMap();

        }
    }
    
}
