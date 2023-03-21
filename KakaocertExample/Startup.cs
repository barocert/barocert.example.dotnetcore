using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kakaocert;

public class KakaocertInstance
{
    // 파트너 신청 후 메일로 발급받은 링크아이디(LinkID)와 비밀키(SecretKey)값 으로 변경하시기 바랍니다.
    private string linkID = "BKAKAO";
    private string secretKey = "egkxYN99ZObjLa3c0nr9/riG+a0VDkZu87LSGR8c37U=";

    public KakaocertService kakaocertService;

    public KakaocertInstance()
    {
        //Kakaocert 서비스 객체 초기화
        kakaocertService = new KakaocertService(linkID, secretKey);

        //인증토큰의 IP제한기능 사용여부, 권장(true)
        kakaocertService.IPRestrictOnOff = true;

        //카카오써트 API 서비스 고정 IP 사용여부, true-사용, false-미사용, 기본값(false)
        kakaocertService.UseStaticIP = false;

        // 로컬시스템 시간 사용 여부 true-사용, false-미사용, 기본값(true)
        kakaocertService.UseLocalTimeYN = true;
    }
}

namespace KakaocertExample
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

            services.AddSingleton<KakaocertInstance>();
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
                    template: "{controller=Kakaocert}/{action=Index}");
            });
        }
    }
}
