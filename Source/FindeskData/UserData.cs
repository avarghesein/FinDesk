using Findesk.Contract.Data;
using Findesk.Data.ORM;
using Findesk.Model.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Data
{
    public class UserData : DataElement, IUserData
    {
        private IDocumentData _docData;

        protected IDocumentData DocumentData
        {
            get
            {
                return _docData ?? (_docData = new DocumentData());
            }
        }

        IQueryable<User> IUserData.GetList()
        {
            return Context.Users.AsNoTracking().AsQueryable();
        }

        User IUserData.Get(string id)
        {
            int userID = -1;
            if (int.TryParse(id, out userID))
            {
                return Context.Users.Where(usr => usr.ID == userID).FirstOrDefault();
            }

            return null;
        }

        User IUserData.Create(User user)
        {
            string newThumb = user.ThumbnailID;

            if (! string.IsNullOrEmpty(newThumb))
            {
                DocumentData.Create(user.Thumbnail);
            }

            user.WorkGroup = CurrentWorkGroup;
            Context.Users.Add(user);
            Context.SaveChanges();
            return user;
        }

        User IUserData.Update(User user)
        {
            var upUsr = (this as IUserData).Get(user.ID.ToString());

            if(upUsr != null)
            {
                string curThumb = upUsr.ThumbnailID;
                string newThumb = user.ThumbnailID;

                if (curThumb != newThumb)
                {
                    if (!string.IsNullOrEmpty(newThumb))
                    {
                        DocumentData.Create(user.Thumbnail);
                    }
                }

                
                (Context as IObjectContextAdapter).ObjectContext.Detach(upUsr);
                user.WorkGroup = null;
                Context.Users.Attach(user);
                Context.Entry(user).Reference(usr => usr.WorkGroup).Load();
                user.WorkGroup = CurrentWorkGroup;
                Context.Entry(user).State = EntityState.Modified;
                
                Context.SaveChanges();

                if (!string.IsNullOrEmpty(curThumb))
                {
                    var tmpDoc = new Document() { ID = curThumb };
                    DocumentData.Delete(tmpDoc);
                }

                return user;
            }

            return null;
        }

        User IUserData.Delete(User user)
        {
            var delUsr = (this as IUserData).Get(user.ID.ToString());

            if (delUsr != null)
            {
                var policyUsers =
                    (from pol in Context.Policies
                    let deps = pol.Dependents
                    from dep in deps
                    where dep.UserID == user.ID
                    select new { Policy = pol, Dependent = dep }).ToList();
                
                if(policyUsers.Count > 0)
                {
                    throw new ApplicationException("User is being used in Policy:" + policyUsers[0].Policy.Number);
                }

                Context.Users.Remove(delUsr);
                Context.Entry(delUsr).State = System.Data.Entity.EntityState.Deleted;
                Context.SaveChanges();
                DocumentData.Delete(delUsr.Thumbnail);
                return user;
            }

            return null;
        }
    };
};
