using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using System.Data.Entity.ModelConfiguration;
using EasyChefDemo.Entities;


namespace EasyChefDemo.Data.Configurations
{

    public class UserConfiguration : EntityBaseConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(u => u.Username).IsRequired().HasMaxLength(100);
            Property(u => u.Email).IsRequired().HasMaxLength(200);
            Property(u => u.HashedPassword).IsRequired().HasMaxLength(200);
            Property(u => u.Salt).IsRequired().HasMaxLength(200);
            Property(u => u.IsLocked).IsRequired();
            Property(u => u.DateCreated);
            Property(u => u.IPAddress).IsOptional().HasMaxLength(100);
            Property(u => u.LastLogin).IsOptional().HasMaxLength(100);
            //Property(u => u.Restaurant_ID).IsOptional();
            //Relationships
            HasMany(u => u.UserRestaurants).WithRequired().HasForeignKey(ur => ur.UserId);

            HasMany(u => u.ContactsLists).WithOptional().HasForeignKey(cl => cl.UserId);

        }
    }
}
