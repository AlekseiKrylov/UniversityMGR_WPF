using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task10.Models.Base;

namespace Task10.Models
{
    internal class Teacher : Person
    {
        public virtual ICollection<Group>? Groups { get; set; }
    }
}
