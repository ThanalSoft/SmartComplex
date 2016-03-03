using System;
using System.Runtime.Serialization;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ThanalSoft.SmartComplex.Entities.Security
{
    [DataContract]
    public class UserRole : IdentityUserRole<Int64>
    {
        
    }
}