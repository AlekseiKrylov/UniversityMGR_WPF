using System.ComponentModel.DataAnnotations;
using UniversityMGR_WPF.Models.Base.Interfaces;

namespace UniversityMGR_WPF.Models.Base
{
    internal abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
