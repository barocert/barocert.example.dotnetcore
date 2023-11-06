using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Barocert.navercert;
using Microsoft.AspNetCore.Routing;

public class NavercertInstance
{
    // 파트너 신청 후 메일로 발급받은 링크아이디(LinkID)와 비밀키(SecretKey)값 으로 변경하시기 바랍니다.
    private string linkID = "TESTER";
    private string secretKey = "SwWxqU+0TErBXy/9TVjIPEnI0VTUMMSQZtJf3Ed8q3I=";

    public NavercertService navercertService;

    public NavercertInstance()
    {
        // Navercert 서비스 객체 초기화
        navercertService = new NavercertService(linkID, secretKey);

        // 인증토큰의 IP제한기능 사용여부, 권장(true)
        navercertService.IPRestrictOnOff = true;

        // 네이버써트 API 서비스 고정 IP 사용여부, true-사용, false-미사용, 기본값(false)
        navercertService.UseStaticIP = false;
    }
}

namespace BarocertExample
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
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<NavercertInstance>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Navercert}/{action=Index}");
            });
        }
    }
}
