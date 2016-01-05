using System;

namespace ThanalSoft.SmartComplex.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public bool ClickableRow { get; set; }

        public TableAttribute(bool pClickableRow)
        {
            ClickableRow = pClickableRow;
        }
    }
}