using Findesk.VM.Shared;
using Findesk.Web.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MODEL = Findesk.Model.Shared;
using VIEWMODEL = Findesk.VM.Shared;

namespace Findesk.Web.Controllers
{
    public class SharedController : AbstractApiController
    {
        [HttpPost]
        public Document UploadImageAndGenerateThumbnail(int width = 75, int height = 75)
        {
            try
            {
                var image = System.Web.Helpers.WebImage.GetImageFromRequest();

                string fileName = image.FileName;
                fileName = Path.GetFileNameWithoutExtension(fileName) + ".jpg";


                image = image.Resize(width, height, false, true).Crop(1, 1);

                var croppedImage = image.GetBytes("image/jpg");

                string guid = Guid.NewGuid().ToString();

                var mDoc = new MODEL.Document()
                {
                    ID = guid,
                    Name = fileName,
                    Size = croppedImage.Length,
                    Content = croppedImage,
                    ContentType = "image/jpg"
                };

                var vDoc = WebElement.ModelMapper.Map<MODEL.Document, VIEWMODEL.Document>(WebElement.Document.CreateTemporary(mDoc));

                return vDoc;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }

        [HttpPost]
        public Document UploadDocument()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count > 0)
                {
                    var postedFile = httpRequest.Files[0];
                    string fileName = postedFile.FileName;

                    byte[] content = null;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        postedFile.InputStream.CopyTo(ms);
                        content = ms.ToArray();
                    }

                    string guid = Guid.NewGuid().ToString();

                    var mDoc = new MODEL.Document()
                    {
                        ID = guid,
                        Name = fileName,
                        Size = content.Length,
                        Content = content,
                        ContentType = postedFile.ContentType
                    };

                    var vDoc = WebElement.ModelMapper.Map<MODEL.Document, VIEWMODEL.Document>(WebElement.Document.CreateTemporary(mDoc));

                    return vDoc;
                }

                return null;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }
    };
};
