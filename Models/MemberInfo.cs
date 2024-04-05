namespace JokesWebApp.Models
{
    public class MemberInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WslName { get; set; }
        public string WslId { get; set; }
        public string FsName { get; set; }
        public string FsId { get; set; }

        public MemberInfo(string firstName, string lastName, string wslName, string wslId, string fsName, string fsId)
        {
            FirstName = firstName;
            LastName = lastName;
            WslName = wslName;
            WslId = wslId;
            FsName = fsName;
            FsId = fsId;
        }
    }
}
