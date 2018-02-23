namespace AARP.WebApi.Models
{
    public abstract class ApiCandidate
    {
        public abstract string Id { get; set; }
        public abstract string FirstName { get; set; }
        public abstract string LastName { get; set; }
        public abstract string Email { get; set; }
        public abstract int OpeningID { set; get; }
        public abstract int StageID { set; get; }
        public abstract string StageName { get; set; }
        public abstract string State { get; set; }
        public string GetFullName()
        {
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                return FirstName + " " + LastName;

            if (!string.IsNullOrEmpty(FirstName))
                return FirstName;

            if (!string.IsNullOrEmpty(LastName))
                return LastName;

            return "Anonymous";
        }
    }
}
