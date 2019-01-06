using Findesk.Model.Policy;
using Findesk.Model.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findesk.Data.ORM
{
    public class FindeskDBContext : DbContext
    {
        static Type sqlCompact = typeof(System.Data.SqlServerCe.SqlCeProviderFactory);
        static Type sqlCompactProvider = typeof(System.Data.Entity.SqlServerCompact.SqlCeProviderServices);

        public FindeskDBContext()
            : base("FindeskDBContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }

        public DbSet<WorkGroup> WorkGroups { get; set; }

        public DbSet<WorkGroupMember> WorkGroupMembers { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Policy> Policies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Policy>()
            .HasMany(pol => pol.Documents)
            .WithMany()
            .Map(m =>
            {
                m.ToTable("PolicyDocuments");
                m.MapLeftKey("PolicyID");
                m.MapRightKey("DocumentID");
            });

            modelBuilder.Entity<WorkGroup>()
            .HasMany(wgp => wgp.Modules)
            .WithMany()
            .Map(m =>
            {
                m.ToTable("WorkGroupModules");
                m.MapLeftKey("WorkGroupID");
                m.MapRightKey("ModuleID");
            });

            modelBuilder.Entity<User>()
            .HasOptional(usr => usr.Thumbnail);

            modelBuilder.Entity<User>()
           .HasRequired(usr => usr.WorkGroup)
           .WithMany()
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<Policy>()
           .HasRequired(usr => usr.WorkGroup)
           .WithMany()
           .WillCascadeOnDelete(false);

            modelBuilder.Entity<Document>()
            .HasRequired(usr => usr.WorkGroup)
            .WithMany()
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dependent>()
            .HasRequired(dep => dep.User)
            .WithMany()
            .WillCascadeOnDelete(false);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    };
};
