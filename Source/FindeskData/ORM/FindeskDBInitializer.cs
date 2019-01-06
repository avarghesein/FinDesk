using Findesk.Model.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Data.ORM
{
    internal class FindeskDBInitializer : DropCreateDatabaseIfModelChanges<FindeskDBContext>
    {
        private Type loadSQL = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        private Type loadSQLCompact = typeof(System.Data.Entity.SqlServerCompact.SqlCeProviderServices);

        protected override void Seed(FindeskDBContext ctx)
        {
            if(ctx.WorkGroups.Count() <= 0)
            {
                WorkGroup wkGroup = new WorkGroup()
                {
                    Name = "Local",
                    Modules = new List<ModuleWrapper>()
                    { 
                        new ModuleWrapper() { Module = Module.Users },
                        new ModuleWrapper() { Module = Module.Policy },
                    }
                };

                WorkGroupMember wkMember = new WorkGroupMember()
                {
                    LoginID = "Admin",
                    Name = "Admin",
                };

                WorkGroupMemberRole wkRole = new WorkGroupMemberRole()
                {
                    Role = WorkGroupRole.Admin,
                    WorkGroup = wkGroup,
                    WorkGroupMember = wkMember,
                };

                wkGroup.WorkGroupMemberRoles = new List<WorkGroupMemberRole>() { wkRole };
                wkMember.WorkGroupMemberRoles = new List<WorkGroupMemberRole>() { wkRole };

                ctx.WorkGroups.Add(wkGroup);
                ctx.WorkGroupMembers.Add(wkMember);

                ctx.SaveChanges();                
            }
            
        }
    };
};
