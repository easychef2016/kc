using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyChefDemo.Entities;
using EasyChefDemo.Services.Utilities;

namespace EasyChefDemo.Services.Abstract
{


    public interface IMembershipService
    {
        MembershipContext ValidateUser(string username, string password);
        User CreateUser(string username, string email, string password, int[] roles);
        User UpdateUser(string username, string email, string password, int[] roles);

        User UpdateUserEdit(string username, bool Islocked);
        User GetUser(int userId);
        List<Role> GetUserRoles(string username);
    }
}
