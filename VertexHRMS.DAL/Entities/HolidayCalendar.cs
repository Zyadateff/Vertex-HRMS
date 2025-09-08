
namespace VertexHRMS.DAL.Entities
{
    public class HolidayCalendar
    {
        public HolidayCalendar()
        {

        }
        public int HolidayCalendarId { get; private set; }
        public string Name { get; private set; }
        public ICollection<Holiday> Holidays { get; private set; } = new List<Holiday>();
    }
}
