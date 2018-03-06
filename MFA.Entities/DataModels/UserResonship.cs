namespace MFA.Entities.DataModels
{
    class UserResonship
    {
        public int Id { get; set; }
        public int RelatingUserID { get; set; }
        public int RelatedUserID { get; set; }
        public string Type { get; set; }
    }
}
