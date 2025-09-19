
namespace VertexHRMS.DAL.Entities
{
    public class WorkSchedule
    {
        public WorkSchedule()
        {

        }
        public WorkSchedule(string name, TimeSpan startTime, TimeSpan endTime)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }
        public int WorkScheduleId { get; private set; }
        public string Name { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
    }
}
