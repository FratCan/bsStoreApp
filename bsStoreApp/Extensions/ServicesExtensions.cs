using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;

namespace bsStoreApp.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlConnect(this IServiceCollection services,IConfiguration configuration)
        {
            //EF Core projesinde veritabanı bağlamını (DbContext) uygulamanıza kaydetmek ve yapılandırmak için kullanılır. 
            services.AddDbContext<RepositoryContext>(options => options.UseMySQL(configuration.GetConnectionString("sqlConnection")));
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();



        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager,ServiceManager>();


        public static void ConfigureNLogService(this IServiceCollection services) => 
            services.AddSingleton<ILoggerService, LoggerManager>();
        // AddSingleton demek tek bir defa oluşturulacak ve herkes oradan kullancak.
        // Tüm istekler aynısı nesneyi paylaşır.Daha hızlıdır.Daha az bellek kullanılır.
        //İlk itstek sırasında oluşturulur.
        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();
        }
    }
}
