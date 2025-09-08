
using System.ComponentModel.DataAnnotations.Schema;

namespace VertexHRMS.DAL.Entities
{
    public class ExitClearance
    {
        public ExitClearance()
        {

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
