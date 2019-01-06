using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Findesk.VM.Shared;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using Findesk.Web.Code;

using MODEL = Findesk.Model.Shared;
using VIEWMODEL = Findesk.VM.Shared;

namespace Findesk.Web.Controllers
{
    public class UserController : AbstractApiController
    {
        public IEnumerable<User> GetAll()
        {
            try
            {
                var users = WebElement.User.GetList().ToList();

                var vmUsers = (from mUsr in users
                               select WebElement.ModelMapper.Map<MODEL.User, VIEWMODEL.User>(mUsr)).ToList();

                return vmUsers;
            }
            catch(Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }

        [HttpPost]
        public User Create(User user)
        {
            try
            {
                var mUsr = WebElement.ModelMapper.Map<VIEWMODEL.User, MODEL.User>(user);
                mUsr = WebElement.User.Create(mUsr);
                var vUsr = WebElement.ModelMapper.Map<MODEL.User, VIEWMODEL.User>(mUsr);

                return vUsr;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }

        [HttpPost]
        public bool Delete(User user)
        {
            try
            {
                var mUsr = WebElement.ModelMapper.Map<VIEWMODEL.User, MODEL.User>(user);
                mUsr = WebElement.User.Delete(mUsr);
                var vUsr = WebElement.ModelMapper.Map<MODEL.User, VIEWMODEL.User>(mUsr);

                return vUsr != null;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }

        [HttpPost]
        public User Edit(User user)
        {
            try
            {
                var mUsr = WebElement.ModelMapper.Map<VIEWMODEL.User, MODEL.User>(user);
                mUsr = WebElement.User.Update(mUsr);
                var vUsr = WebElement.ModelMapper.Map<MODEL.User, VIEWMODEL.User>(mUsr);

                return vUsr;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }
    };
};
