using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyChefDemo.Web.Models
{
    public class UserRoleViewModel
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual RoleViewModel Role { get; set; }
    }
}