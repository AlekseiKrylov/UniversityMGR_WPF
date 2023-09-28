using System.ComponentModel.DataAnnotations.Schema;
using UniversityMGR_WPF.Models.Base;

namespace UniversityMGR_WPF.Models
{
    internal class Student : Person
    {
        public int? GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group? Group { get; set; }
    }
}
