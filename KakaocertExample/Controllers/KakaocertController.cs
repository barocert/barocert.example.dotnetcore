using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Kakaocert;

namespace KakaocertExample.Controllers
{
    public class KakaocertController : Controller
    {
        private readonly KakaocertService _kakaocertService;

        public KakaocertController(KakaocertInstance KCinstance)
        {
            // Kakaocert 서비스 객체 생성
            _kakaocertService = KCinstance.kakaocertService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RequestESign()
        {
			/**
            * 카카오톡 사용자에게 전자서명을 요청합니다.(단건)
            * - https://RequestESign
            */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "023020000003";

            // AppToApp 인증여부,
            // true = AppToApp 방식, false = TalkMessage 방식
            bool isAppUseYN = false;

			// 전자서명 요청 정보 Object
			RequestESign eSignRequest = new RequestESign();

			// 수신자 정보(휴대폰번호, 성명, 생년월일)와 Ci 값 중 택일
			eSignRequest.ReceiverHP("01087674117");
			eSignRequest.ReceiverName("이승환");
			eSignRequest.ReceiverBirthday("19930112");
			// eSignRequest.Ci("");

			eSignRequest.ReqTitle("전자서명단건테스트");
			eSignRequest.ExpireIn(1000);
			eSignRequest.Token("전자서명단건테스트데이터");
			eSignRequest.TokenType("TEXT"); // TEXT, HASH

			// App to App 방식 이용시, 에러시 호출할 URL
			// eSignRequest.ReturnURL("https://kakao.barocert.com");

			try
			{
                var result = _kakaocertService.requestESign(clientCode, eSignRequest, isAppUseYN);
                return View("ResultESign", result);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }
        }

		public IActionResult BulkRequestESign()
		{
			/**
            * 카카오톡 사용자에게 전자서명을 요청합니다.(다건)
            * - https://BulkRequestESign
            */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "020040000001";

			// AppToApp 인증여부,
            // true-AppToApp 방식, false-TalkMessage 방식
			bool isAppUseYN = false;

			// 전자서명 요청 정보 Object
			RequestESign bulkESignRequest = new RequestESign();

			// 수신자 정보(휴대폰번호, 성명, 생년월일)와 Ci 값 중 택일
			bulkESignRequest.ReceiverHP("01087674117");
			bulkESignRequest.ReceiverName("이승환");
			bulkESignRequest.ReceiverBirthday("19930112");
			// bulkESignRequest.Ci("");

			bulkESignRequest.ReqTitle("전자서명단건테스트");
			bulkESignRequest.ExpireIn(1000);

			bulkESignRequest.Tokens = new List<Tokens>();

			Tokens tokens = new Tokens();
            tokens.ReqTitle("전자서명다건문서테스트1");
			tokens.Token("전자서명다건테스트데이터1");
			bulkESignRequest.Tokens.Add(tokens);

			tokens = new Tokens();
			tokens.ReqTitle("전자서명다건문서테스트2");
			tokens.Token("전자서명다건테스트데이터2");
			bulkESignRequest.Tokens.Add(tokens);

			bulkESignRequest.TokenType("TEXT"); // TEXT, HASH

			// App to App 방식 이용시, 에러시 호출할 URL
			// bulkESignRequest.ReturnURL("https://kakao.barocert.com");

			try
			{
				var result = _kakaocertService.bulkRequestESign(clientCode, bulkESignRequest, isAppUseYN);
				return View("BulkResultESign", result);
			}
			catch (BarocertException ke)
			{
				return View("Exception", ke);
			}
		}

		public IActionResult GetESignState()
        {
			/**
            * 전자서명 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다. (단건)
            * - https://GetESignState
            */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "023020000003";

            // 요청시 반환받은 접수아이디
            string receiptId = "0230310143306000000000000000000000000001";

            try
            {
                var resultObj = _kakaocertService.getESignState(clientCode, receiptId);
                return View("GetESignState", resultObj);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }

        }

		public IActionResult GetBulkESignState()
		{
			/**
            * 전자서명 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다. (다건)
            * - https://GetBulkESignState
            */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "023020000003";

			// 요청시 반환받은 접수아이디
			string receiptId = "0230310143306000000000000000000000000001";

			try
			{
				var resultObj = _kakaocertService.getESignState(clientCode, receiptId);
				return View("GetBulkESignState", resultObj);
			}
			catch (BarocertException ke)
			{
				return View("Exception", ke);
			}

		}

		public IActionResult VerifyESign()
        {
			/**
            * 전자서명 요청시 반환된 접수아이디를 통해 서명을 검증합니다. (다건)
            * - https://VerifyESign
            */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "023020000003";

            // 요청시 반환받은 접수아이디
            string receiptId = "0230310143306000000000000000000000000001";

            try
            {
                var resultObj = _kakaocertService.verifyESign(clientCode, receiptId);
                return View("VerifyESign", resultObj);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }

        }

		public IActionResult BulkVerifyESign()
		{
			/**
            * 전자서명 요청시 반환된 접수아이디를 통해 서명을 검증합니다. (다건)
            * - https://bulkVerifyESign
            */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "023020000003";

			// 요청시 반환받은 접수아이디
			string receiptId = "0230310143951000000000000000000000000001";

			try
			{
				var result = _kakaocertService.bulkVerifyESign(clientCode, receiptId);
				return View("BulkVerifyESign", result);
			}
			catch (BarocertException ke)
			{
				return View("Exception", ke);
			}

		}

		public IActionResult RequestVerifyAuth()
        {
			/**
             * 카카오톡 사용자에게 본인인증 전자서명을 요청합니다.
             * - https://RequestVerifyAuth
             */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "020040000001";

			// AppToApp 인증여부,
			// true = AppToApp 방식, false = TalkMessage 방식
			bool isAppUseYN = false;

			RequestVerifyAuth requestObj = new RequestVerifyAuth();

            requestObj.ReceiverHP("01087674117");
			requestObj.ReceiverName("이승환");
			requestObj.ReceiverBirthday("19930112");
			// requestObj.Ci("");

			requestObj.ReqTitle("인증요청 메시지 제목란");
			requestObj.ExpireIn(1000);
			requestObj.Token("본인인증요청토큰");

			// App to App 방식 이용시, 에러시 호출할 URL
			// requestObj.ReturnURL("https://kakao.barocert.com");

			try
            {
                var result = _kakaocertService.requestVerifyAuth(clientCode, requestObj, isAppUseYN);
                return View("RequestVerifyAuth", result);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }
        }    

		public IActionResult GetVerifyAuthState()
        {
            /**
            * 본인인증 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다.
            * - https://GetVerifyAuthState
            */

            // 이용기관코드, 파트너 사이트에서 확인
            string clientCode = "023020000003";

            // 요청시 반환받은 접수아이디
            string receiptId = "0230309201738000000000000000000000000001";

            try
            {
                var result = _kakaocertService.getVerifyAuthState(clientCode, receiptId);
                return View("GetVerifyAuthState", result);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }

        }

		public IActionResult VerifyAuth()
        {
            /**
            * 본인인증 요청시 반환된 접수아이디를 통해 본인인증 서명을 검증합니다.
            * - https://VerifyAuth
            */

            // 이용기관코드, 파트너 사이트에서 확인
            string clientCode = "023020000003";

            // 요청시 반환받은 접수아이디
            string receiptId = "0230309201738000000000000000000000000001";

            try {
                var result = _kakaocertService.verifyAuth(clientCode, receiptId);
                return View("VerifyAuth", result);
            } catch (BarocertException ke) {
                return View("Exception", ke);
            }
        }

        public IActionResult RequestCMS()
        {
			/**
            *  카카오톡 사용자에게 자동이체 출금동의 전자서명을 요청합니다.
            *  - https://RequestCMS
            */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "023020000003";

            // AppToApp 인증여부,
            // true = AppToApp 방식, false = TalkMessage 방식
            bool isAppUseYN = false;

			RequestCMS requestObj = new RequestCMS();

			// 수신자 정보(휴대폰번호, 성명, 생년월일)와 Ci 값 중 택일
			requestObj.ReceiverHP("01087674117");
			requestObj.ReceiverName("이승환");
			requestObj.ReceiverBirthday("19930112");
			// requestObj.Ci("");

			requestObj.ReqTitle("인증요청 메시지 제공란");
			requestObj.ExpireIn(1000);
			requestObj.RequestCorp("청구 기관명란");
			requestObj.BankName("출금은행명란");
			requestObj.BankAccountNum("9-4324-5117-58");
			requestObj.BankAccountName("예금주명 입력란");
			requestObj.BankAccountBirthday("19930112");
			requestObj.BankServiceType("CMS"); // CMS, FIRM, GIRO

			// App to App 방식 이용시, 에러시 호출할 URL
			// requestObj.ReturnURL("");

			try
            {
                var result = _kakaocertService.requestCMS(clientCode, requestObj, isAppUseYN);
                return View("ResultCMS", result);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }
        }

        public IActionResult GetCMSState()
        {
			/**
            * 자동이체 출금동의 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다.
            * - https://GetCMSState
            */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "023020000003";

            // 요청시 반환받은 접수아이디
            string receiptId = "0230309201738000000000000000000000000001";

            try
            {
                var resultObj = _kakaocertService.getCMSState(clientCode, receiptId);
                return View("GetCMSState", resultObj);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }

        }

        public IActionResult VerifyCMS()
        {
			/**
            * 자동이체 출금동의 요청시 반환된 접수아이디를 통해 서명을 검증합니다.
            * - https://VerifyCMS
            */

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "023020000003";

            // 요청시 반환받은 접수아이디
            string receiptId = "0230309201738000000000000000000000000001";

            try
            {
                var resultObj = _kakaocertService.verifyCMS(clientCode, receiptId);
                return View("VerifyCMS", resultObj);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }

        }

    }
}
