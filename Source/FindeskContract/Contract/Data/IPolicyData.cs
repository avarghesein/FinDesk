using Findesk.Model.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Contract.Data
{
    public interface IPolicyData
    {
        IQueryable<Policy> GetList();
        Policy Get(string id);
        Policy Create(Policy user);
        Policy Update(Policy user);
        Policy Delete(Policy user);
    };
};
