using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
using FreeSql.DataAnnotations;

namespace FFPark.Entity
{
    [Table(Name = "BaseUserRole")]
    public class BaseUserRole : BaseEntity
    {

        public int UserId { get; set; }


        public int RoleId { get; set; }
    }
}
