
namespace VertexHRMS.DAL.Entities
{
    public class HolidayCalendar
    {
        public HolidayCalendar()
        {

        }
        public HolidayCalendar(string name)
        {
            Name = name;
        }

        public int HolidayCalendarId { get; private set; }
        public string Name { get; private set; }
        public ICollection<Holiday> Holidays { get; private set; } = new List<Holiday>();
    }
}
