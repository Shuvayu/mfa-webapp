using MFA.Entities.LogicModels;
using System.Collections.Generic;

namespace MFA.IService
{
    public interface ICalendarService
    {
        /// <summary>
        /// Get calendar events for today
        /// </summary>
        List<CalendarEventResponse> GetCalendarEvents();
    }
}
