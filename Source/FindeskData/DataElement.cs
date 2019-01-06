using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Globalization;
using Findesk.Data.ORM;
using Findesk.Model.Shared;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace Findesk.Data
{
    public abstract class DataElement
    {
        protected FindeskDBContext Context
        {
            get
            {
                FindeskDBContext _ctx = CallContext.GetData("FindeskDBContext") as FindeskDBContext;
                if(_ctx == null)
                {
                    _ctx = new FindeskDBContext();
                    CallContext.SetData("FindeskDBContext",_ctx);
                }

                return _ctx;
            }
        }

        protected WorkGroup CurrentWorkGroup
        {
            get
            {
                var wgp = Context.WorkGroups.AsQueryable().Where(gp => gp.Name == "Local").FirstOrDefault();
                return wgp;
            }
        }
    };
};
