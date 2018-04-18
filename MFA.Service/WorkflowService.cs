using MFA.Entities.Configurations;
using MFA.Entities.Constants;
using MFA.Entities.LogicModels;
using MFA.IService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFA.Service
{
    public class WorkFlowService : IWorkFlowService
    {
        private readonly IOptions<AppConfiguration> _appSettings;
        private readonly IOptions<CalendarConfiguration> _calendarSettings;
        private readonly ILogger _logger;

        public WorkFlowService(IOptions<AppConfiguration> appSettings, IOptions<CalendarConfiguration> calendarSettings, ILoggerFactory logger)
        {
            _appSettings = appSettings;
            _calendarSettings = calendarSettings;
            _logger = logger.CreateLogger(nameof(WorkFlowService));
        }

        #region WorkFlow Execution
        public string InitiateWorkflow(string faceName, List<CalendarEventResponse> calendarEvents)
        {
            try
            {
                return ExecuteWorkFlow(faceName, calendarEvents);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error => Inner :{e.InnerException} => Message :{e.Message}");
                throw;
            }
        }

        private string ExecuteWorkFlow(string faceName, List<CalendarEventResponse> calendarEvents)
        {
            var textResponse = new StringBuilder();
            var inititalTextResponse = faceName != ApplicationConstants.NoOneIdentified ? TextReponse.WelcomeText(_appSettings.Value.CompanyName, faceName, "David") : TextReponse.NonRecorgnizedWelcomeText(_appSettings.Value.CompanyName);
            textResponse.Append(inititalTextResponse.TrimEnd());
            if (calendarEvents.Count > 0)
            {
                foreach (var calendarEvent in calendarEvents)
                {
                    foreach (var attendee in calendarEvent.Attendees)
                    {
                        if (!string.IsNullOrEmpty(attendee) && (attendee.Contains(faceName) || calendarEvent.Summary.Contains(faceName)))
                        {
                            textResponse.Append(" ");
                            textResponse.Append(TextReponse.MeetingFoundText(calendarEvent.WhenTime, _calendarSettings.Value.BeachRoomName));
                            break;
                        }
                    }
                }
            }
            return textResponse.ToString();
        }
        #endregion
    }
}
