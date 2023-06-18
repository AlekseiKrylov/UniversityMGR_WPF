using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task10.Models.Base;

namespace Task10.Models
{
    internal class Student : Person
    {
        public int? GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group? Group { get; set; }
    }
}
