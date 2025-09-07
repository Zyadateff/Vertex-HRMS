
namespace VertexHRMS.DAL.Entities
{
    public class ExitClearance
    {
        public int ClearanceID { get; private set; }
        public int ResignationID { get; private set; }
        public Resignation Resignation { get; private set; }
        public bool HRCleared { get; private set; }
        public bool ITCleared { get; private set; }
        public bool FinanceCleared { get; private set; }
        public decimal FinalSettlementAmt { get; private set; }
        public string HRClearedByUserId { get; private set; }
        public ApplicationUser HRClearedByUser { get; private set; }
        public string ITClearedByUserId { get; private set; }
        public ApplicationUser ITClearedByUser { get; private set; }
        public string FinanceClearedByUserId { get; private set; }
        public ApplicationUser FinanceClearedByUser { get; private set; }
    }
}
