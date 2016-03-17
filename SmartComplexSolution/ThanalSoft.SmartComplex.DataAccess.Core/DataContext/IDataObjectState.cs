using System.ComponentModel.DataAnnotations.Schema;

namespace ThanalSoft.SmartComplex.DataAccess.Core.DataContext
{
    public interface IDataObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }

    public enum ObjectState
    {
        Unchanged,
        Added,
        Modified,
        Deleted
    }
}