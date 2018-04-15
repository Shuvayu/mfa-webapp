using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using MFA.Entities.Configurations;
using MFA.IService;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace MFA.Service
{
    public class GoogleCalendarService : ICalendarService
    {
        private readonly string _applicationName = "MFAWebApp";
        private readonly string _serviceAccountCredentialFilePath = "client_secret.json";
        private readonly static string[] _scopes = { CalendarService.Scope.CalendarReadonly };
        private readonly CalendarService _googleCalendarService;
        private readonly IOptions<CalendarConfiguration> _calendarSettings;
        private GoogleCredential _credential;

        public GoogleCalendarService(IOptions<CalendarConfiguration> calendarSettings)
        {
            _calendarSettings = calendarSettings;
            GenerateCredentials();
            _googleCalendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credential,
                ApplicationName = _applicationName,
            });

        }

        private void GenerateCredentials()
        {
            try
            {
                ReadCrendentialFile();
            }
            catch (Exception e)
            {

                throw;
            }

        }

        private void ReadCrendentialFile()
        {
            using (var stream = new FileStream(_serviceAccountCredentialFilePath, FileMode.Open, FileAccess.Read))
            {
                _credential = GoogleCredential.FromStream(stream)
                                .CreateScoped(_scopes);
            }
        }

        public string GetCalendarEvents()
        {
            try
            {
                var eventList = string.Empty;
                var request = _googleCalendarService.Events.List("primary");
                //var request = _googleCalendarService.Events.List(_calendarSettings.Value.LoungeRoomCalendarId);
                request.TimeMin = DateTime.Now;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                var events = request.Execute();
                var list = _googleCalendarService.CalendarList.List().Execute();
                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        var when = eventItem.Start.DateTime.ToString();
                        if (String.IsNullOrEmpty(when))
                        {
                            when = eventItem.Start.Date;
                        }
                        eventList = $"{eventItem.Summary} - ({when})";
                        eventList = Environment.NewLine;
                    }
                    return eventList;
                }
                else
                {
                    return "No upcoming events found.";
                }
            }
            catch (Exception e)
            {

                throw;
            }

        }

    }
}
