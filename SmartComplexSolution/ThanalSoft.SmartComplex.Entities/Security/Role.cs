using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ThanalSoft.SmartComplex.Entities.Security
{
    public class Role : IdentityRole<Int64, UserRole>
    {
        public Role()
        {
            
        }

        public Role(string pName)
        {
            Name = pName;
        }
    }
}