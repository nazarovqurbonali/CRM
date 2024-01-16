namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<MentorPosition> MentorPositions { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<UserRole>().HasKey(x=>new {x.UserId,x.RoleId});
            modelBuilder.Entity<StudentGroup>().HasKey(x => new { x.StudentId, x.GroupId });
            modelBuilder.Entity<MentorPosition>().HasKey(x => new { x.MentorId, x.PositionId });

            modelBuilder.Entity<UserRole>().HasData(new List<UserRole>
            {
                new UserRole{RoleId = 1,UserId =1},
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}







