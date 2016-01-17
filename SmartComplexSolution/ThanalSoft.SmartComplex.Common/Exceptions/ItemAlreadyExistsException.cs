using System;

namespace ThanalSoft.SmartComplex.Common.Exceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException(string pName, string pItemName) : base($"{pItemName} with same name '{pName}' already exists!")
        {
            
        }
    }
}