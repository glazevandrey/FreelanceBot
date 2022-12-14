using FreelanceBot.Bot;
using FreelanceBot.Executors;
using FreelanceBot.Quartz.Jobs;
using FreelanceBot.Quartz;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FreelanceBot
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
            services.AddControllers();
            services.AddTransient<BotService>()
                    .AddTransient<BotWorker>()
                    .AddTransient<MessageExecutor>()
                    .AddTransient<ParseJob>()
                    .AddTransient<IQuartzService, QuartzService>(); 

            var serviceProvider = services.BuildServiceProvider();
            QuartzStartup.Start(serviceProvider, Configuration); // ?????? ?????????? ???????????? ????? 

            BotWorker.InitBot(serviceProvider); // ????????????? ????
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
