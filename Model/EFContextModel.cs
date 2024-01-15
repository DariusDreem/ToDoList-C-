using Framework.Controller;
using Framework.Model;
using Framework.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework
{
    public class EFContextModel : DbContext
    {
        private static readonly Lazy<EFContextModel> instance = new Lazy<EFContextModel>(() => new EFContextModel());

        public static EFContextModel Instance => instance.Value;

        public EFContextView EFCView = new EFContextView();
        public Controller.EFContextController EFCControl;

        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFCore;Trusted_Connection=True;";
        public DbSet<ToDoTaskModel> ToDoTasks { get; set; }
        public DbSet<UserModel> Users { get; set; }

        private EFContextModel()
        {
            OnConfiguring(new DbContextOptionsBuilder());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
            EFCControl = new Controller.EFContextController(this);
        }
    }

    public class EFContextModelFactory //: IDesignTimeDbContextFactory<EFContextModel>
    {
        public EFContextModel CreateDbContext(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<EFContextModel>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EFCore;Trusted_Connection=True;"))
                .BuildServiceProvider();

            return serviceProvider.GetRequiredService<EFContextModel>();
        }
    }
}
