using System.ComponentModel.DataAnnotations;

namespace UniversityMGR_WPF.Models.Base
{
    internal abstract class NamedEntity : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
