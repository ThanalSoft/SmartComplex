using System;

namespace ThanalSoft.SmartComplex.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableColumnAttribute : Attribute
    {
        public TableColumnAttribute(string pColumnHeader)
        {
            ColumnHeader = pColumnHeader;
            IDColumn = false;
            HiddenColumn = false;
        }

        public string ColumnHeader { get; set; }

        public bool IDColumn { get; set; }

        public bool HiddenColumn { get; set; }

        public string EmptyValue { get; set; }

    }
}