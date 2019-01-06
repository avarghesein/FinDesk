using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using Findesk.Contract;
using Findesk.Contract.Data;

namespace Findesk.Web.Code
{
    public class WebElement
    {
        private IModelMapper _modelMapper;

        public IModelMapper ModelMapper
        {
            get
            {
                return _modelMapper ?? (_modelMapper = new Findesk.VM.Mapper.Mapper());
            }
        }

        private IUserData _user;

        public IUserData User
        {
            get
            {
                return _user ?? (_user = new Findesk.Data.UserData());
            }
        }

        private IPolicyData _policy;

        public IPolicyData Policy
        {
            get
            {
                return _policy ?? (_policy = new Findesk.Data.PolicyData());
            }
        }

        private IDocumentData _document;

        public IDocumentData Document
        {
            get
            {
                return _document ?? (_document = new Findesk.Data.DocumentData());
            }
        }

        public HttpResponseException HttpException(Exception eX)
        {
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(string.Format("{0}", eX.Message)),
                ReasonPhrase = eX.Message
            };

            return new HttpResponseException(msg);
        }
    };
};