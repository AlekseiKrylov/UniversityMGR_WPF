using System.ComponentModel.DataAnnotations;

namespace UniversityMGR_WPF.Models.Base
{
    internal abstract class Person : NamedEntity
    {
        [MaxLength(50)]
        public string? Surname { get; set; }

        public virtual string FullName => $"{Name} {Surname}";
    }
}
