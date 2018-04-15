namespace MFA.Entities.LogicModels
{
    public class TextReponse
    {
        public static string WelcomeText(string CientName, string ContatName) => $@"Hi {CientName}, welcome to Formation Technologies. 
                                                                                     I have informed {ContatName}, of your arrival.
                                                                                     Please have a seat till they arrive.";

        public static string NonRecorgnizedWelcomeText() => $@"Hi there, welcome to Formation Technologies. 
                                                                                     I have informed the reception, of your arrival.
                                                                                     Please have a seat till they arrive.";
    }
}
