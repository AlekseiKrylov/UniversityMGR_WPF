using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniversityMGR_WPF.Models.Base;

namespace UniversityMGR_WPF.Models
{
    internal class Course : NamedEntity
    {
        [MaxLength(1000)]
        public string? Description { get; set; }

        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
