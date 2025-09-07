
namespace VertexHRMS.DAL.Entities
{
    public class Holiday
    {
        public int HolidayID { get; private set; }
        public int HolidayCalendarID { get; private set; }
        public HolidayCalendar HolidayCalendar { get; private set; }
        public DateTime HolidayDate { get; private set; }
        public string Name { get; private set; }
    }
}
