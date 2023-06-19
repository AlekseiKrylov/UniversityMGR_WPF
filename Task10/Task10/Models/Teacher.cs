using System.Collections.Generic;
using Task10.Models.Base;

namespace Task10.Models
{
    internal class Teacher : Person
    {
        public virtual ICollection<Group>? Groups { get; set; }
    }
}
