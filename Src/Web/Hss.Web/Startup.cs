namespace Hss.Web
{
    using System;
    using System.Reflection;

    using CloudinaryDotNet;

    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;

    using Hss.Common;
    using Hss.Data;
    using Hss.Data.Common;
    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Data.Repositories;
    using Hss.Data.Seeding;
    using Hss.Services;
    using Hss.Services.BlazorModal;
    using Hss.Services.Cron;
    using Hss.Services.Data;
    using Hss.Services.Data.Addresses;
    using Hss.Services.Data.Appointments;
    using Hss.Services.Data.Categories;
    using Hss.Services.Data.CIties;
    using Hss.Services.Data.Countries;
    using Hss.Services.Data.DateTime;
    using Hss.Services.Data.Invoices;
    using Hss.Services.Data.JobsService;
    using Hss.Services.Data.Orders;
    using Hss.Services.Data.Services;
    using Hss.Services.Data.Teams;
    using Hss.Services.Data.Users;
    using Hss.Services.DateTime;
    using Hss.Services.Mapping;
    using Hss.Services.Messaging;
    using Hss.Services.Models.Categories;
    using Hss.Services.Notifier;
    using Hss.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(
                config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().UseSqlServerStorage(
                        this.configuration.GetConnectionString("DefaultConnection"),
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            UsePageLocksOnDequeue = true,
                            DisableGlobalLocks = true,
                        }));

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(options =>
            {
               // options.Filters.Add(typeof(ArgumentNullExceptionFilterAttribute));
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddRazorPages();
            services.AddServerSideBlazor(options =>
            {
                options.DetailedErrors = true;
            });
            services.AddSignalR();
            services.AddHttpContextAccessor();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Cloudinary
            Account account = new Account(
    this.configuration["Cloudinary:Name"],
    this.configuration["Cloudinary:ApiKey"],
    this.configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);

            // Notifier
            services.AddSingleton<Notifier>();

            // Application services
            services.AddTransient<IEmailSender>(serviceProvider => new SendGridEmailSender(this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IServicesService, ServicesService>();
            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<ICitiesService, CitiesService>();
            services.AddTransient<IAddressesService, AddressesService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<ITeamsService, TeamsService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IInvoicesService, InvoicesService>();
            services.AddTransient<IJobsService, JobsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IAppointmentsService, AppointmentsService>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IModalService, ModalService>();
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(CategoryServiceModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Error/HttpError?statusCode={0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 2 });
            app.UseHangfireDashboard(
                "/Administration/Hangfire",
                new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });
            this.SeedHangfireJobs(recurringJobManager);

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                        endpoints.MapBlazorHub();
                        endpoints.MapFallbackToController("Blazor", "Home");
                    });
        }

        private void SeedHangfireJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate<RecurrentOrdersInvoiceGeneratorJob>("MainNewsGetterJob", x => x.GenerateInvoices(), RecurrentOrdersInvoiceGeneratorJob.CronSchedule);
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
