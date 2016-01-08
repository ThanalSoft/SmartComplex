using System;

namespace ThanalSoft.SmartComplex.Common.Exceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException(string pName) : base($"{pName} with same name already exists!")
        {
            
        }
    }
}