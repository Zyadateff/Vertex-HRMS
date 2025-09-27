namespace VertexHRMS.BLL.Service.Implementation
{
    public class AttendanceRecordsService : IAttendanceRecordsService
    {
        private readonly IAttendanceRecordsRepo attendanceRecordsRepo;
        private readonly IMapper mapper;
        public AttendanceRecordsService(IAttendanceRecordsRepo attendanceRecordsRepo, IMapper mapper)
        {
            this.attendanceRecordsRepo = attendanceRecordsRepo;
            this.mapper = mapper;
        }
        public async Task AddAttendanceRecordAsync(AttendanceRecordsVM attendanceRecord)
        {
            var record = mapper.Map<AttendanceRecord>(attendanceRecord);
            await attendanceRecordsRepo.AddAttendanceRecordAsync(record);
        }

        public async Task<AttendanceRecordsVM?> CheckoutAsync(int employeeId)
        {
            var result = await attendanceRecordsRepo.CheckoutAsync(employeeId);
            var vm = mapper.Map<AttendanceRecordsVM?>(result);
            return vm;
        }

        public async Task<List<AttendanceRecordsVM>?> GetAttendanceRecordByNameAsync(string firstname, string? lastname = null)
        {
            var result = await attendanceRecordsRepo.GetAttendanceRecordByNameAsync(firstname, lastname);
            var vm = mapper.Map<List<AttendanceRecordsVM>?>(result);
            return vm;
        }

        public async Task<List<AttendanceRecordsVM>> GetAttendanceRecordsFilteredAsync(string? departmentName = null, string? positionName = null, DateTime? date = null, string? status = null)
        {
            var result = await attendanceRecordsRepo.GetAttendanceRecordsFilteredAsync(departmentName, positionName, date, status);
            var vm = mapper.Map<List<AttendanceRecordsVM>>(result);
            
            return vm;
        }
    }
}
