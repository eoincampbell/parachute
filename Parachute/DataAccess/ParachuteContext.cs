using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parachute.Entities;


namespace Parachute.DataAccess
{
    public class ParachuteContext : DbContext
    {
        public DbSet<ParachuteAppliedScriptsLog> ParachuteAppliedScriptsLogs { get; set; }
        public DbSet<ParachuteSchemaChangeLog> ParachuteSchemaChangeLogs { get; set; }

        public ParachuteContext()
        {
            
        }
        public ParachuteContext(string connectionString)
            : base(connectionString)
        {
            IDatabaseInitializer<ParachuteContext> x = null;
            Database.SetInitializer(x);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
