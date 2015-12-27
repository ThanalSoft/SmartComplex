using System;

namespace ThanalSoft.SmartComplex.Common.Exceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException() : base("{0} with same name already exists!")
        {
            
        }
    }
}