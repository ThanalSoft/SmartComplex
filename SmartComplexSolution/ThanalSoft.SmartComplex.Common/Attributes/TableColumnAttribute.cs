using System;

namespace ThanalSoft.SmartComplex.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableColumnAttribute : Attribute
    {
        public TableColumnAttribute(string pColumnHeader)
        {
            ColumnHeader = pColumnHeader;
            IdColumn = false;
            HiddenColumn = false;
        }

        public TableColumnAttribute(string pColumnHeader, bool pIdColumn, bool pHiddenColumn)
        {
            ColumnHeader = pColumnHeader;
            IdColumn = pIdColumn;
            HiddenColumn = pHiddenColumn;
        }

        public string ColumnHeader { get; set; }

        public bool IdColumn { get; set; }

        public bool HiddenColumn { get; set; }

    }
}