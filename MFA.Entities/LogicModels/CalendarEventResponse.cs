using System.Collections.Generic;
using System.Linq;

namespace MFA.Entities.LogicModels
{
    public class CalendarEventResponse
    {
        public string Summary { get; set; }
        public string When { get; set; }
        public List<string> Attendees { get; set; } = new List<string>();

        public CalendarEventResponse(string summary, string when, IEnumerable<string> attendies)
        {
            Summary = summary;
            When = when;
            Attendees = attendies.ToList();
        }
    }
}
