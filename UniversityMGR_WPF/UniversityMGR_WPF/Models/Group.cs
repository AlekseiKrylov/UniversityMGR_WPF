using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversityMGR_WPF.Models.Base;

namespace UniversityMGR_WPF.Models
{
    internal class Group : NamedEntity
    {
        [Required]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
