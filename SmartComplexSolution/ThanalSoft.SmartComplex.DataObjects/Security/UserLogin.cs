using System;
using System.Runtime.Serialization;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ThanalSoft.SmartComplex.DataObjects.Security
{
    [DataContract]
    public class UserLogin : IdentityUserLogin<Int64>
    {
    }
}