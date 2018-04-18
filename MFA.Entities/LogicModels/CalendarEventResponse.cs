using System;
using System.Collections.Generic;
using System.Linq;

namespace MFA.Entities.LogicModels
{
    public class CalendarEventResponse
    {
        public string Summary { get; set; }
        public string When { get; set; }
        public string WhenTime
        {
            get
            {
                if (!string.IsNullOrEmpty(When))
                {
                    return Convert.ToDateTime(When).ToString("hh:mm tt");
                }
                return string.Empty;
            }
        }
        public List<string> Attendees { get; set; } = new List<string>();

        public CalendarEventResponse(string summary, string when, IEnumerable<string> attendies)
        {
            Summary = summary;
            When = when;
            Attendees = attendies.ToList();
        }
    }
}
