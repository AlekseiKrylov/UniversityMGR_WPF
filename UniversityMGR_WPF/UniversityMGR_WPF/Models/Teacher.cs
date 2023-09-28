using System.Collections.Generic;
using UniversityMGR_WPF.Models.Base;

namespace UniversityMGR_WPF.Models
{
    internal class Teacher : Person
    {
        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
