using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Task10.Models.Base;

namespace Task10.Models
{
    internal class Course : NamedEntity
    {
        [MaxLength(1000)]
        public string? Description { get; set; }

        public virtual ICollection<Group>? Groups { get; set; }
    }
}
