using Findesk.Contract.Data;
using Findesk.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Findesk.Data
{
    public class DocumentData : DataElement, IDocumentData
    {
        Document IDocumentData.Create(Document doc)
        {
            var docInSess = HttpContext.Current.Session[doc.ID] as Document;

            if (docInSess != null)
            {
                doc.ContentType = docInSess.ContentType;
                doc.Content = docInSess.Content;
                doc.Size = docInSess.Size;
                doc.WorkGroup = CurrentWorkGroup;
                Context.Documents.Add(doc);
                Context.SaveChanges();
                (this as IDocumentData).DeleteTemporary(docInSess);
                return doc;
            }

            return null;
        }

        Document IDocumentData.Delete(Document doc)
        {
            if (doc == null || doc.ID == null) return null;

            var docDel = Context.Documents.Find(doc.ID);

            if (docDel != null)
            {
                Context.Documents.Remove(docDel);
                Context.Entry(docDel).State = System.Data.Entity.EntityState.Deleted;
                Context.SaveChanges();
                return docDel;
            }

            return null;
        }

        Document IDocumentData.Get(string id)
        {
            var docInSess = HttpContext.Current.Session[id] as Document;

            if(docInSess != null)
            {
                return docInSess;
            }

            return Context.Documents.Where(doc => doc.ID == id).FirstOrDefault();
        }


        Document IDocumentData.CreateTemporary(Document doc)
        {
            return (HttpContext.Current.Session[doc.ID] = doc) as Document;
        }

        Document IDocumentData.DeleteTemporary(Document doc)
        {
            var docInSess = HttpContext.Current.Session[doc.ID] as Document;
            HttpContext.Current.Session[doc.ID] = null;
            docInSess.Content = null;
            return docInSess;
        }
    };
};
