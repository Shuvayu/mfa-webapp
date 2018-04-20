using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using MFA.Entities.Configurations;
using MFA.Entities.LogicModels;
using MFA.IService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MFA.Service
{
    public class GoogleCalendarService : ICalendarService
    {
        private readonly static string[] _scopes = { CalendarService.Scope.CalendarReadonly };
        private readonly CalendarService _googleCalendarService;
        private readonly IOptions<CalendarConfiguration> _calendarSettings;
        private readonly IOptions<AppConfiguration> _appSettings;
        private readonly ILogger _logger;
        private GoogleCredential _credential;

        public GoogleCalendarService(IOptions<CalendarConfiguration> calendarSettings, IOptions<AppConfiguration> appSettings, ILoggerFactory logger)
        {
            _calendarSettings = calendarSettings;
            _appSettings = appSettings;
            GenerateCredentials();
            _logger = logger.CreateLogger(nameof(GoogleCalendarService));
            _googleCalendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credential,
                ApplicationName = _appSettings.Value.ApplicationName,
            });

        }

        #region Authentication Workflow
        private void GenerateCredentials()
        {
            try
            {
                ReadCrendentialFile();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error => {e.Message} - {e.StackTrace}");
            }

        }

        private void ReadCrendentialFile()
        {
            using (var stream = new FileStream(_calendarSettings.Value.ServiceAccountCredentialFilePath, FileMode.Open, FileAccess.Read))
            {
                _credential = GoogleCredential.FromStream(stream)
                                .CreateScoped(_scopes);
            }
        }
        #endregion

        #region Get Calendar Events
        public List<CalendarEventResponse> GetCalendarEvents()
        {
            var calendarResponse = new List<CalendarEventResponse>();
            try
            {
                GetEventsFromCalendar(calendarResponse, _calendarSettings.Value.BeachRoomCalendarId);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error => {e.Message} - {e.StackTrace}");
            }
            return calendarResponse;
        }

        private void GetEventsFromCalendar(List<CalendarEventResponse> calendarResponse, string calendarId)
        {
            var request = _googleCalendarService.Events.List(calendarId);
            request.TimeMin = DateTime.Now;
            request.TimeMax = DateTime.Now.AddHours(12);
            request.TimeZone = "Australia/Sydney";
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            var events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    var when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    calendarResponse.Add(new CalendarEventResponse(eventItem.Summary, when, eventItem.Attendees.Select(x => x.DisplayName)));
                }
            }
        }
        #endregion
    }
}
