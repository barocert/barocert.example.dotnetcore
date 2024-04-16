/*
*Barocert PASS API .NETCore SDK Example
*
* 업데이트 일자 : 2024-04-16
* 연동기술지원 연락처: 1600-9854
* 연동기술지원 이메일: code @linkhubcorp.com
*
* < 테스트 연동개발 준비사항>
*   1) API Key 변경 (연동신청 시 메일로 전달된 정보)
*       - LinkID : 링크허브에서 발급한 링크아이디
*       - SecretKey : 링크허브에서 발급한 비밀키
*   2) SDK 환경설정 필수 옵션 설정
*       - IPRestrictOnOff : 인증토큰 IP 검증 설정, true-사용, false-미사용, (기본값: true)
*       - UseStaticIP : 통신 IP 고정, true-사용, false-미사용, (기본값: false)
*/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Barocert.passcert;
using Microsoft.AspNetCore.Routing;

public class PasscertInstance
{
    // 링크아이디
    private string linkID = "TESTER";
    // 비밀키
    private string secretKey = "SwWxqU+0TErBXy/9TVjIPEnI0VTUMMSQZtJf3Ed8q3I=";

    public PasscertService passcertService;

    public PasscertInstance()
    {
        // Passcert 서비스 객체 초기화
        passcertService = new PasscertService(linkID, secretKey);

		// 인증토큰 IP 검증 설정, true-사용, false-미사용, (기본값: true)
		passcertService.IPRestrictOnOff = true;

		// 통신 IP 고정, true-사용, false-미사용, (기본값: false)
		passcertService.UseStaticIP = false;
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

            services.AddSingleton<PasscertInstance>();
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
                    template: "{controller=Passcert}/{action=Index}");
            });
        }
    }
}
