using System.ComponentModel.DataAnnotations;

namespace Task10.Models.Base
{
    internal abstract class NamedEntity : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
