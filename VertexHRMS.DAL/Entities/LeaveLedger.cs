namespace VertexHRMS.DAL.Entities
{
    public class LeaveLedger
    {
        public LeaveLedger()
        {

        }

        public LeaveLedger( int employeeId, Employee employee, int leaveTypeId, LeaveType leaveType, string txnType, decimal quantity, DateTime effectiveDate)
        {
            EmployeeId = employeeId;
            Employee = employee;
            LeaveTypeId = leaveTypeId;
            LeaveType = leaveType;

        public LeaveLedger(int employeeId, int leaveTypeId, string txnType, decimal quantity, DateTime effectiveDate)
        {
            EmployeeId = employeeId;
            LeaveTypeId = leaveTypeId;

            TxnType = txnType;
            Quantity = quantity;
            EffectiveDate = effectiveDate;
        }
        public int LeaveLedgerId { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public int LeaveTypeId { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public string TxnType { get; private set; }
        public decimal Quantity { get; private set; }
        public DateTime EffectiveDate { get; private set; }
    }
}
