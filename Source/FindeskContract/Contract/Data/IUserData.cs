using Findesk.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Contract.Data
{
    public interface IUserData
    {
        IQueryable<User> GetList();
        User Get(string id);
        User Create(User user);
        User Update(User user);
        User Delete(User user);
    };
};
