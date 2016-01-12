namespace ThanalSoft.SmartComplex.Web.Security
{
    public class SmartComplexPrincipalSerializeModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserIdentity { get; set; }
        public string[] Roles { get; set; }
    }
}