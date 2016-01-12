using System;
using System.Security.Principal;

namespace ThanalSoft.SmartComplex.Web.Security
{
    public interface ISmartComplexPrincipal : IPrincipal
    {
        Int64 UserId { get; set; }
        string UserName { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string UserIdentity { get; set; }
        string[] Roles { get; set; }
    }
}