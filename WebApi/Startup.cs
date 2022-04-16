using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ADO.NET;
using Microsoft.OpenApi.Models;
using Models;
using Models.Models;
using Services;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });

            services.AddControllers();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            
            services.AddScoped<CourseService>();
            services.AddScoped<HomeTaskService>();
            services.AddScoped<StudentService>();

            services.AddScoped<IRepository<Student>>(p => new StudentRepository(connectionString));
            services.AddScoped<IRepository<Course>>(p => new CourseRepository(connectionString));
            services.AddScoped<IRepository<HomeTask>>(p => new HomeTaskRepository(connectionString));
            services.AddScoped<IRepository<HomeTaskAssessment>>(p => new HomeTaskAssessmentRepository(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "WebApp1 v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}