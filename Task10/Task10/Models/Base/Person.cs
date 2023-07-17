using System.ComponentModel.DataAnnotations;

namespace Task10.Models.Base
{
    internal abstract class Person : NamedEntity
    {
        [MaxLength(50)]
        public string? Surname { get; set; }

        public virtual string FullName => $"{Name} {Surname}";
    }
}
