
namespace VertexHRMS.DAL.Entities
{
    public class WorkSchedule
    {
        public int WorkScheduleID { get; private set; }
        public string Name { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
    }
}
