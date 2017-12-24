using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyChefDemo.Entities
{
    public class User : IEntityBase
    {
        public User()
        {
            UserRoles = new List<UserRole>();
            UserRestaurants = new List<UserRestaurant>();
            ContactsLists = new List<Contacts>();

        }
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }

        public string IPAddress { get; set; }
        public string LastLogin { get; set; }
        public DateTime DateCreated { get; set; }
//        public int Restaurant_ID { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserRestaurant> UserRestaurants { get; set; }

        public virtual ICollection<Contacts> ContactsLists { get; set; }

    }
}
