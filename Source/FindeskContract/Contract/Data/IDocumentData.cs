using Findesk.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Contract.Data
{
    public interface IDocumentData
    {        
        Document Create(Document doc);
        Document Delete(Document doc);
        Document Get(string id);

        Document CreateTemporary(Document doc);
        Document DeleteTemporary(Document doc);
    };
};
