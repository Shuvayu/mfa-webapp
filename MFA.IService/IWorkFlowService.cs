using MFA.Entities.LogicModels;
using System.Collections.Generic;

namespace MFA.IService
{
    public interface IWorkFlowService
    {
        /// <summary>
        /// Gets the formated text corrosponding to the workflow
        /// </summary>
        /// <param name="faceName"></param>
        /// <param name="calendarEvents"></param>
        /// <returns></returns>
        string InitiateWorkflow(string faceName, List<CalendarEventResponse> calendarEvents);
    }
}
