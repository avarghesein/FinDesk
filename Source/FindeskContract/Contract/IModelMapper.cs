using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Contract
{
    public interface IModelMapper
    {
        T New<T>() where T:new();

        TargetModel Map<SourceModel, TargetModel>(SourceModel model) 
            where SourceModel: class
            where TargetModel:class;
    };
};
