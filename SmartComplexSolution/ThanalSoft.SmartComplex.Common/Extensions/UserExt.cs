using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Common.Extensions
{
    public static class UserExt
    {
        public static string UserFullName(this LoginUser pUser)
        {
            return pUser.FirstName + (string.IsNullOrEmpty(pUser.LastName)
                                                    ? ""
                                                    : " " + pUser.LastName);
        }
    }
}