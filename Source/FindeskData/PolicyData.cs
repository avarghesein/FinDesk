using Findesk.Contract.Data;
using Findesk.Model.Policy;
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
    public class PolicyData : DataElement, IPolicyData
    {
        private IDocumentData _docData;

        protected IDocumentData DocumentData
        {
            get
            {
                return _docData ?? (_docData = new DocumentData());
            }
        }

        private IUserData _userData;

        protected IUserData UserData
        {
            get
            {
                return _userData ?? (_userData = new UserData());
            }
        }

        IQueryable<Policy> IPolicyData.GetList()
        {
            return Context.Policies.AsNoTracking().AsQueryable();
        }

        Policy IPolicyData.Get(string id)
        {
            int policyID = -1;
            if (int.TryParse(id, out policyID))
            {
                var policyRet = Context.Policies.Where(policy => policy.ID == policyID)
                    .Include(pol => pol.Dependents)
                    .Include(pol => pol.Dependents.Select(dep => dep.User))
                    .Include(pol => pol.Documents).FirstOrDefault();

                //Context.Entry(policyRet).Collection(pol => pol.Dependents).Query().OfType<User>().Load();

                return policyRet;
            }

            return null;
        }

        Policy IPolicyData.Create(Policy policy)
        {
            policy.Documents.ForEach(doc =>
            {
                DocumentData.Create(doc);
            });

            policy.Dependents.ForEach(dep =>
                {
                    dep.User = UserData.Get(dep.User.ID.ToString());
                });

            policy.WorkGroup = CurrentWorkGroup;
            Context.Policies.Add(policy);


            Context.SaveChanges();
            return policy;
        }

        protected void DeleteDocuments(List<Document> documents)
        {
            documents.ForEach(doc =>
            {
                DocumentData.Delete(doc);
            });
        }

        protected void CreateDocuments(List<Document> documents)
        {
            documents.ForEach(doc =>
            {
                DocumentData.Create(doc);
            });
        }

        protected void ProcessDocuments(Policy oldPol, Policy newPol, bool isDelete)
        {
            List<Document> left = (isDelete ? oldPol.Documents : newPol.Documents) ?? new List<Document>();
            List<Document> right = (isDelete ? newPol.Documents : oldPol.Documents) ?? new List<Document>();

            var final = left.Except(right).ToList();

            if (isDelete)
            {
                DeleteDocuments(final);
            }
            else
            {
                CreateDocuments(final);
            }
        }

        Policy IPolicyData.Update(Policy policy)
        {
            var upPol = (this as IPolicyData).Get(policy.ID.ToString());

            if (upPol != null)
            {
                ProcessDocuments(upPol, policy, false);

                upPol.ChassisNumber = policy.ChassisNumber;
                upPol.DateOfExpiry = policy.DateOfExpiry;
                upPol.DateOfPurchase = policy.DateOfPurchase;
                upPol.Disease = policy.Disease;
                upPol.Name = policy.Name;
                upPol.Number = policy.Number;
                upPol.Premium = policy.Premium;
                upPol.SumAssured = policy.SumAssured;
                upPol.VehicleNumber = policy.VehicleNumber;
               
                upPol.Dependents.ForEach(dep =>
                {
                    var depFound = policy.Dependents.Find(depNew => depNew.UserID == dep.UserID);

                    if (depFound == null)
                    {
                        Context.Entry(dep).State = System.Data.Entity.EntityState.Deleted;                       
                    }
                    else
                    {
                        dep.Relation = depFound.Relation;
                        dep.PolicyID = upPol.ID.Value;
                        dep.Policy = upPol;
                        Context.Entry(dep).State = System.Data.Entity.EntityState.Modified;
                        dep.User = UserData.Get(dep.UserID.ToString());
                    }
                });

                policy.Dependents.ForEach(dep =>
                {
                    var depFound = upPol.Dependents.Find(depNew => depNew.UserID == dep.UserID);

                    if (depFound == null)
                    {
                        dep.PolicyID = upPol.ID.Value;
                        dep.Policy = upPol;
                        dep.User = UserData.Get(dep.UserID.ToString());
                        upPol.Dependents.Add(dep);
                        Context.Entry(dep).State = System.Data.Entity.EntityState.Added;
                    }
                });

                //policy.Dependents = upPol.Dependents;

                upPol.Documents.Clear();

                policy.Documents.ForEach(doc =>
                {
                    upPol.Documents.Add(DocumentData.Get(doc.ID));
                });

                //policy.Documents = upPol.Documents;                

                //(Context as IObjectContextAdapter).ObjectContext.Detach(upPol);
                //policy.WorkGroup = null;
                //Context.Policies.Attach(policy);
                //Context.Entry(policy).Reference(pol => pol.WorkGroup).Load();
                //policy.WorkGroup = CurrentWorkGroup;
                //Context.Entry(policy).State = EntityState.Modified;

                Context.Entry(upPol).State = EntityState.Modified;

                Context.SaveChanges();

                ProcessDocuments(upPol, policy, true);

                return (this as IPolicyData).Get(policy.ID.ToString());
            }

            return null;
        }

        Policy IPolicyData.Delete(Policy policy)
        {
            var delPol = (this as IPolicyData).Get(policy.ID.ToString());

            if (delPol != null)
            {
                Document[] docsCpy = new Document[delPol.Documents.Count];
                delPol.Documents.CopyTo(docsCpy);

                Context.Policies.Remove(delPol);
                Context.Entry(delPol).State = EntityState.Deleted;
                Context.SaveChanges();

                DeleteDocuments(docsCpy.ToList());
                return policy;
            }

            return null;
        }
    };
};
