using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.BLL.ModelVM.LeaveApprovalVM;
using VertexHRMS.BLL.ModelVM.LeaveEntitlmentVM;
using VertexHRMS.BLL.ModelVM.LeaveLedger;
using VertexHRMS.BLL.ModelVM.LeaveLedgerVM;
using VertexHRMS.BLL.ModelVM.LeaveRequestVM;
using AddVM = VertexHRMS.BLL.ModelVM.LeaveRequestVM.AddVM;
using UpdateVM = VertexHRMS.BLL.ModelVM.LeaveRequestVM.UpdateVM;

namespace VertexHRMS.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<ModelVM.LeaveApprovalVM.AddVM, LeaveApproval>().ReverseMap();
            CreateMap<GetByApproverVM, LeaveApproval>().ReverseMap();
            CreateMap<ModelVM.LeaveApprovalVM.GetByIdVM, LeaveApproval>().ReverseMap();
            CreateMap<GetByRequestVM, LeaveApproval>().ReverseMap();
            CreateMap<ModelVM.LeaveApprovalVM.UpdateVM, LeaveApproval>().ReverseMap();
            CreateMap<ModelVM.LeaveEntitlmentVM.AddVM, LeaveEntitlement>().ReverseMap();
            CreateMap<GetAllForEmployeeVM, LeaveEntitlement>().ReverseMap();
            CreateMap<ModelVM.LeaveEntitlmentVM.GetByEmployeeAndTypeVM, LeaveEntitlement>().ReverseMap();
            CreateMap<ModelVM.LeaveEntitlmentVM.GetByEmployeeVM, LeaveEntitlement>().ReverseMap();
            CreateMap<ModelVM.LeaveEntitlmentVM.GetByIdVM, LeaveEntitlement>().ReverseMap();
            CreateMap<GetEntitlementVM, LeaveEntitlement>().ReverseMap();
            CreateMap<ModelVM.LeaveEntitlmentVM.UpdateVM, LeaveEntitlement>().ReverseMap();
            CreateMap<ModelVM.LeaveLedgerVM.AddVM, LeaveLedger>().ReverseMap();
            CreateMap<ModelVM.LeaveLedger.GetByEmployeeAndTypeVM, LeaveLedger>().ReverseMap();
            CreateMap<ModelVM.LeaveLedgerVM.GetByEmployeeVM, LeaveLedger>().ReverseMap();
            CreateMap<GetByLeaveTypeVM, LeaveLedger>().ReverseMap();
            CreateMap<ModelVM.LeaveLedgerVM.UpdateVM, LeaveLedger>().ReverseMap();
            CreateMap<AddVM, LeaveRequest>().ReverseMap();
            CreateMap<ModelVM.LeaveRequestVM.GetByEmployeeVM, LeaveRequest>().ReverseMap();
            CreateMap<GetPendingRequestsVM, LeaveRequest>().ReverseMap();
            CreateMap<GetRequestsByIdVM, LeaveRequest>().ReverseMap();
            CreateMap<UpdateVM, LeaveRequest>().ReverseMap();
        }
    }
}
