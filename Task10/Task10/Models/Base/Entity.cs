using System.ComponentModel.DataAnnotations;

namespace Task10.Models.Base
{
    internal abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
