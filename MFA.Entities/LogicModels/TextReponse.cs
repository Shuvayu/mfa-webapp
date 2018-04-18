namespace MFA.Entities.LogicModels
{
    public class TextReponse
    {
        public static string WelcomeText(string companyName, string cientName, string contatName) => $@"Hi {cientName}, welcome to {companyName}. 
                                                                                     I have informed {contatName} of your arrival.
                                                                                     Please have a seat till he arrives.";

        public static string NonRecorgnizedWelcomeText(string companyName) => $@"Hi there, welcome to {companyName}. 
                                                                                     I have informed the reception of your arrival.
                                                                                     Please have a seat till he arrives.";

        public static string MeetingFoundText(string meetingTimings, string room) => $@"There is a meeting booked for you at {meetingTimings} in {room}.";
    }
}
