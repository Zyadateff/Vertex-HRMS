
using System.ComponentModel.DataAnnotations.Schema;

namespace VertexHRMS.DAL.Entities
{
    public class ExitClearance
    {
        public ExitClearance()
        {

        }
        public ExitClearance(int resignationId, bool hrCleared, bool itCleared, bool financeCleared, decimal finalSettlementAmt, string hrClearedByUserId, string itClearedByUserId, string financeClearedByUserId)
        {
            ResignationId = resignationId;
            HRCleared = hrCleared;
            ITCleared = itCleared;
            FinanceCleared = financeCleared;
            FinalSettlementAmt = finalSettlementAmt;
            HRClearedByUserId = hrClearedByUserId;
            ITClearedByUserId = itClearedByUserId;
            FinanceClearedByUserId = financeClearedByUserId;
        }
        public int ExitClearanceId { get; private set; }
        public int ResignationId { get; private set; }
        public Resignation Resignation { get; private set; }
        public bool HRCleared { get; private set; }
        public bool ITCleared { get; private set; }
        public bool FinanceCleared { get; private set; }
        public decimal FinalSettlementAmt { get; private set; }
        public string HRClearedByUserId { get; private set; }
        [ForeignKey("HRClearedByUserId")]
        public ApplicationUser HRClearedByUser { get; private set; }
        public string ITClearedByUserId { get; private set; }
        public ApplicationUser ITClearedByUser { get; private set; }
        public string FinanceClearedByUserId { get; private set; }
        public ApplicationUser FinanceClearedByUser { get; private set; }
    }
}
