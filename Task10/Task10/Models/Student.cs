using System.ComponentModel.DataAnnotations.Schema;
using Task10.Models.Base;

namespace Task10.Models
{
    internal class Student : Person
    {
        public int? GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group? Group { get; set; }
    }
}
