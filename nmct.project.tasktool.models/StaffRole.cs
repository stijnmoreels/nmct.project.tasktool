using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models
{
    public class StaffRole
    {
       
        [Key]
        public String NameStaff { get; set; }

        public String FullName { get; set; }

        public String TrelloUsername { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        [NotMapped]
        public Boolean Changed { get; set; }

    }
}
