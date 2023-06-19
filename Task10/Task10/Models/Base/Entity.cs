using System.ComponentModel.DataAnnotations;
using Task10.Models.Base.Interfaces;

namespace Task10.Models.Base
{
    internal abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
