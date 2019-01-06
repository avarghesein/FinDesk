using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Findesk.Contract;

namespace Findesk.VM.Mapper
{
    public class Mapper : IModelMapper
    {
        private bool _isConfigured = false;

        private void Configure()
        {
            if (!_isConfigured)
            {
                SharedMaps.Map();
                PolicyMaps.Map();

                _isConfigured = true;
            }
        }

        T IModelMapper.New<T>()
        {
            return new T();
        }

        TargetModel IModelMapper.Map<SourceModel, TargetModel>(SourceModel model)
        {
            try
            {
                Configure();

                if (model == null)
                {
                    return null;
                }

                return AutoMapper.Mapper.Map<TargetModel>(model);
               
            }
            catch(Exception eX)
            {
                throw;
            }
            finally
            { 
            }

            return null;
        }

    };
};
