using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Findesk.VM.Policy;
using Findesk.VM.Shared;
using Findesk.Web.Code;

using MODEL = Findesk.Model.Policy;
using VIEWMODEL = Findesk.VM.Policy;

namespace Findesk.Web.Controllers
{
    public class PolicyController : AbstractApiController
    {
        public IEnumerable<Policy> GetAll()
        {
            try
            {
                var policies = WebElement.Policy.GetList().ToList();

                policies.ForEach(pol =>
                {
                    if (pol.Dependents != null && pol.Dependents.Count > 0)
                    {
                        pol.Insuree = pol.Dependents[0].User;
                    }
                });

                var vmPolicies = (from mPolicy in policies
                                  select WebElement.ModelMapper.Map<MODEL.Policy, VIEWMODEL.Policy>(mPolicy)).ToList();
                
                return vmPolicies;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }

        [HttpPost]
        public Policy Create(Policy policy)
        {
            try
            {
                var mPol = WebElement.ModelMapper.Map<VIEWMODEL.Policy, MODEL.Policy>(policy);
                mPol = WebElement.Policy.Create(mPol);

                if (mPol.Dependents != null && mPol.Dependents.Count > 0)
                {
                    mPol.Insuree = mPol.Dependents[0].User;
                }

                var vPol = WebElement.ModelMapper.Map<MODEL.Policy, VIEWMODEL.Policy>(mPol);

                return vPol;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }

        [HttpPost]
        public bool Delete(Policy policy)
        {
            try
            {
                var mPol = WebElement.ModelMapper.Map<VIEWMODEL.Policy, MODEL.Policy>(policy);
                mPol = WebElement.Policy.Delete(mPol);
                var vPol = WebElement.ModelMapper.Map<MODEL.Policy, VIEWMODEL.Policy>(mPol);

                return vPol.ID == policy.ID;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }

        [HttpPost]
        public Policy Edit(Policy policy)
        {
            try
            {
                var mPol = WebElement.ModelMapper.Map<VIEWMODEL.Policy, MODEL.Policy>(policy);
                mPol = WebElement.Policy.Update(mPol);

                if (mPol.Dependents != null && mPol.Dependents.Count > 0)
                {
                    mPol.Insuree = mPol.Dependents[0].User;
                }

                var vPol = WebElement.ModelMapper.Map<MODEL.Policy, VIEWMODEL.Policy>(mPol);

                return vPol;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }
    }
};
