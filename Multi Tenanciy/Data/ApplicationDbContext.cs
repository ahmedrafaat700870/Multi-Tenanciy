namespace Multi_Tenanciy.Data
{
    public class ApplicationDbContext : DbContext
    {
        private string _tenantId;


        private ITenentServices _tenentServices;
        public ApplicationDbContext(DbContextOptions options , ITenentServices tenentServices) : base(options)
        {
            _tenentServices = tenentServices;
            if(_tenentServices is not null)
            {
                _tenantId = _tenentServices.GetCurrentTenant()?.TId;
            }
        }
        public DbSet<Products> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>().HasQueryFilter(x => x.TenantId == _tenantId);
            
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _tenentServices.GetConnectionString();

            if(!string.IsNullOrEmpty(connectionString))
            {
                var dbProvider = _tenentServices.GetDataBaseProvider();
                if(dbProvider?.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e =>e.State == EntityState.Added))
            {
                entry.Entity.TenantId = _tenantId;
            }


            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
                entry.Entity.TenantId = _tenantId;

            return base.SaveChanges();
        }


    }
}
