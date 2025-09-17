
namespace VertexHRMS.DAL.Entities
{
    public class Holiday
    {
        public Holiday()
        {

        }
        public Holiday(int holidayCalendarId, DateTime holidayDate, string name)
        {
            HolidayCalendarId = holidayCalendarId;
            HolidayDate = holidayDate;
            Name = name;
        }
        public int HolidayId { get; private set; }
        public int HolidayCalendarId { get; private set; }
        public HolidayCalendar HolidayCalendar { get; private set; }
        public DateTime HolidayDate { get; private set; }
        public string Name { get; private set; }
    }
}
