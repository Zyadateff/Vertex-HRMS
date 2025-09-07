
namespace VertexHRMS.DAL.Entities
{
    public class HolidayCalendar
    {
        public int HolidayCalendarID { get; private set; }
        public string Name { get; private set; }
        public ICollection<Holiday> Holidays { get; private set; } = new List<Holiday>();
    }
}
