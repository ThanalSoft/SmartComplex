using System;
using System.Runtime.Serialization;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ThanalSoft.SmartComplex.Entities.Security
{
    [DataContract]
    public class UserClaim : IdentityUserClaim<Int64>
    {
    }
}