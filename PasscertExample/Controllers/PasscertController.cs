using Barocert;
using Barocert.passcert;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BarocertExample.Controllers
{
	public class PasscertController : Controller
	{
		private readonly PasscertService _passcertService;

		public PasscertController(PasscertInstance PCinstance)
		{
            // Passcert 서비스 객체 생성
            _passcertService = PCinstance.passcertService;
		}

		public IActionResult Index()
		{
			return View();
		}
        
        /*
		* 패스 이용자에게 본인인증을 요청합니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/identity/api#RequestIdentity
		*/
        public IActionResult RequestIdentity()
		{

			// Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
			String clientCode = "023070000014";

			// 본인인증 요청 정보 객체
			Identity identity = new Identity();

			// 수신자 휴대폰번호 - 11자 (하이픈 제외)
			identity.receiverHP = _passcertService.encrypt("01012341234");
  		    // 수신자 성명 - 80자
			identity.receiverName = _passcertService.encrypt("홍길동");
      		// 수신자 생년월일 - 8자 (yyyyMMdd)
			identity.receiverBirthday = _passcertService.encrypt("19900911");

			// 인증요청 메시지 제목 - 최대 40자
			identity.reqTitle = "인증요청 메시지 제목란";
            // 인증요청 메시지 - 최대 500자
            identity.reqMessage = _passcertService.encrypt("인증요청 메시지");
            // 고객센터 연락처 - 최대 12자
            identity.callCenterNum = "1600-9854";
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            identity.expireIn = 1000;
            // 서명 원문 - 최대 2,800자 까지 입력가능
            identity.token = _passcertService.encrypt("본인인증요청토큰");

            // 사용자 동의 필요 여부
            identity.userAgreementYN = true;
            // 사용자 정보 포함 여부
            identity.receiverInfoYN = true;

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Push 인증방식
            identity.appUseYN = false;
            // ApptoApp 인증방식에서 사용
            // 통신사 유형('SKT', 'KT', 'LGU'), 대문자 입력(대소문자 구분)
            // identity.telcoType = "SKT";
            // ApptoApp 인증방식에서 사용
            // 모바일장비 유형('ANDROID', 'IOS'), 대문자 입력(대소문자 구분)
            // identity.deviceOSType = "IOS";

            try
            {
				var result = _passcertService.requestIdentity(clientCode, identity);
				return View("requestIdentity", result);
			}
			catch (BarocertException ke)
			{
				return View("exception", ke);
			}
		}

        /**
		* 본인인증 요청 후 반환받은 접수아이디로 본인인증 진행 상태를 확인합니다.
        * 상태확인 함수는 본인인증 요청 함수를 호출한 당일 23시 59분 59초까지만 호출 가능합니다.
        * 본인인증 요청 함수를 호출한 당일 23시 59분 59초 이후 상태확인 함수를 호출할 경우 오류가 반환됩니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/identity/api#GetIdentityStatus
		*/
        public IActionResult GetIdentityStatus()
		{

            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            String clientCode = "023070000014";

			// 요청시 반환받은 접수아이디
			String receiptId = "02309010230700000140000000000003";

			try
			{
				var result = _passcertService.getIdentityStatus(clientCode, receiptId);
				return View("getIdentityStatus", result);
			}
			catch (BarocertException ke)
			{
				return View("exception", ke);
			}

		}

        /**
		* 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
        * 반환받은 전자서명값(signedData)과 [1. RequestIdentity] 함수 호출에 입력한 Token의 동일 여부를 확인하여 이용자의 본인인증 검증을 완료합니다.
        * 검증 함수는 본인인증 요청 함수를 호출한 당일 23시 59분 59초까지만 호출 가능합니다.
        * 본인인증 요청 함수를 호출한 당일 23시 59분 59초 이후 검증 함수를 호출할 경우 오류가 반환됩니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/identity/api#VerifyIdentity
		*/

        public IActionResult VerifyIdentity()
		{

            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            string clientCode = "023070000014";

			// 요청시 반환받은 접수아이디
			string receiptId = "02309010230700000140000000000003";

            // 검증 요청 정보 객체
            IdentityVerify identityVerify = new IdentityVerify();
            // 검증 요청 휴대폰번호 - 11자 (하이픈 제외)
            identityVerify.receiverHP = _passcertService.encrypt("01012341234");
            // 검증 요청 성명 - 최대 80자
            identityVerify.receiverName = _passcertService.encrypt("홍길동");

            try
			{
				var result = _passcertService.verifyIdentity(clientCode, receiptId, identityVerify);
				return View("verifyIdentity", result);
			}
			catch (BarocertException ke)
			{
				return View("exception", ke);
			}
		}


        /**
		* 패스 이용자에게 문서의 전자서명을 요청합니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/sign/api#RequestSign
		*/
        public IActionResult RequestSign()
		{

            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            String clientCode = "023070000014";

			// 전자서명 요청 정보 객체
			Sign sign = new Sign();

			// 수신자 휴대폰번호 - 11자 (하이픈 제외)
			sign.receiverHP = _passcertService.encrypt("01012341234");
  		    // 수신자 성명 - 80자
			sign.receiverName = _passcertService.encrypt("홍길동");
      		// 수신자 생년월일 - 8자 (yyyyMMdd)
			sign.receiverBirthday = _passcertService.encrypt("19900911");

			// 인증요청 메시지 제목 - 최대 40자
			sign.reqTitle = "전자서명 메시지 제목";
            // 인증요청 메시지 - 최대 500자
            sign.reqMessage = _passcertService.encrypt("전자서명 메시지");
            // 고객센터 연락처 - 최대 12자
            sign.callCenterNum = "1600-9854";
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            sign.expireIn = 1000;
			// 서명 원문 - 원문 2,800자 까지 입력가능
			sign.token = _passcertService.encrypt("전자서명요청토큰");
            // 서명 원문 유형
            // 'TEXT' - 일반 텍스트, 'HASH' - HASH 데이터, 'URL' - URL 데이터
            // 원본데이터(originalTypeCode, originalURL, originalFormatCode) 입력시 'TEXT'사용 불가
            sign.tokenType = "URL";

            // 사용자 동의 필요 여부
            sign.userAgreementYN = true;
            // 사용자 정보 포함 여부
            sign.receiverInfoYN = true;

            // 원본유형코드
            // 'AG' - 동의서, 'AP' - 신청서, 'CT' - 계약서, 'GD' - 안내서, 'NT' - 통지서, 'TR' - 약관
            sign.originalTypeCode = "TR";
            // 원본조회URL
            sign.originalURL = "https://www.passcert.co.kr";
            // 원본형태코드
            // ('TEXT', 'HTML', 'DOWNLOAD_IMAGE', 'DOWNLOAD_DOCUMENT')
            sign.originalFormatCode = "HTML";

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Push 인증방식
            sign.appUseYN = false;
            // ApptoApp 인증방식에서 사용
            // 통신사 유형('SKT', 'KT', 'LGU'), 대문자 입력(대소문자 구분)
            // sign.telcoType = "SKT";
            // ApptoApp 인증방식에서 사용
            // 모바일장비 유형('ANDROID', 'IOS'), 대문자 입력(대소문자 구분)
            // sign.deviceOSType = "IOS";

            try
            {
				var result = _passcertService.requestSign(clientCode, sign);
				return View("requestSign", result);
			}
			catch (BarocertException ke)
			{
				return View("exception", ke);
			}
		}

        /**
		* 전자서명 요청 후 반환받은 접수아이디로 인증 진행 상태를 확인합니다.
        * 상태확인 함수는 전자서명 요청 함수를 호출한 당일 23시 59분 59초까지만 호출 가능합니다.
        * 전자서명 요청 함수를 호출한 당일 23시 59분 59초 이후 상태확인 함수를 호출할 경우 오류가 반환됩니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/sign/api#GetSignStatus
		*/
        public IActionResult GetSignStatus()
		{

			// Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
			String clientCode = "023070000014";

			// 요청시 반환받은 접수아이디
			String receiptId = "02309010230700000140000000000004";

			try
			{
				var resultObj = _passcertService.getSignStatus(clientCode, receiptId);
				return View("getSignStatus", resultObj);
			}
			catch (BarocertException ke)
			{
				return View("exception", ke);
			}

		}

        /**
		* 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
        * 검증 함수는 전자서명 요청 함수를 호출한 당일 23시 59분 59초까지만 호출 가능합니다.
        * 전자서명 요청 함수를 호출한 당일 23시 59분 59초 이후 검증 함수를 호출할 경우 오류가 반환됩니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/sign/api#VerifySign
		*/
        public IActionResult VerifySign()
		{
            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            String clientCode = "023070000014";

			// 요청시 반환받은 접수아이디
			String receiptId = "02309010230700000140000000000004";

            // 검증 요청 정보 객체
            SignVerify signVerify = new SignVerify();
            // 검증 요청 휴대폰번호 - 11자 (하이픈 제외)
            signVerify.receiverHP = _passcertService.encrypt("01012341234");
            // 검증 요청 성명 - 최대 80자
            signVerify.receiverName = _passcertService.encrypt("홍길동");

            try
			{
				var resultObj = _passcertService.verifySign(clientCode, receiptId, signVerify);
				return View("verifySign", resultObj);
			}
			catch (BarocertException ke)
			{
				return View("Exception", ke);
			}
		}

        /**
		 * 패스 이용자에게 자동이체 출금동의를 요청합니다.
		 * https://developers.barocert.com/reference/pass/dotnetcore/cms/api#RequestCMS
		 */
        public IActionResult RequestCMS()
		{
            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            String clientCode = "023070000014";

			// 출금동의 요청 정보 객체
			CMS cms = new CMS();

			// 수신자 휴대폰번호 - 11자 (하이픈 제외)
			cms.receiverHP = _passcertService.encrypt("01012341234");
  		    // 수신자 성명 - 80자
			cms.receiverName = _passcertService.encrypt("홍길동");
      		// 수신자 생년월일 - 8자 (yyyyMMdd)
			cms.receiverBirthday = _passcertService.encrypt("19900911");

			// 인증요청 메시지 제목 - 최대 40자
			cms.reqTitle = "출금동의 메시지 제목";
            // 인증요청 메시지 - 최대 500자
            cms.reqMessage = _passcertService.encrypt("출금동의 메시지");
            // 고객센터 연락처 - 최대 12자
            cms.callCenterNum = "1600-9854";
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            cms.expireIn = 1000;
            // 사용자 동의 필요 여부
            cms.userAgreementYN = true;
            // 사용자 정보 포함 여부
            cms.receiverInfoYN = true;

            // 출금은행명 - 최대 100자
			cms.bankName = _passcertService.encrypt("국민은행");
			// 출금계좌번호 - 최대 31자
			cms.bankAccountNum = _passcertService.encrypt("9-****-5117-58");
			// 출금계좌 예금주명 - 최대 100자
			cms.bankAccountName = _passcertService.encrypt("홍길동");
            // 출금유형
            // CMS - 출금동의, OPEN_BANK - 오픈뱅킹
            cms.bankServiceType = _passcertService.encrypt("CMS");
            // 출금액
            cms.bankWithdraw = _passcertService.encrypt("1,000,000원");

            // 사용자 동의 필요 여부
            cms.userAgreementYN = true;
            // 사용자 정보 포함 여부
            cms.receiverInfoYN = true;

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Push 인증방식
            cms.appUseYN = false;
            // ApptoApp 인증방식에서 사용
            // 통신사 유형('SKT', 'KT', 'LGU'), 대문자 입력(대소문자 구분)
            // cms.telcoType = "SKT";
            // ApptoApp 인증방식에서 사용
            // 모바일장비 유형('ANDROID', 'IOS'), 대문자 입력(대소문자 구분)
            // cms.deviceOSType = "IOS";

            try
            {
				var result = _passcertService.requestCMS(clientCode, cms);
				return View("requestCMS", result);
			}
			catch (BarocertException ke)
			{
				return View("exception", ke);
			}
		}

        /**
		* 자동이체 출금동의 요청 후 반환받은 접수아이디로 인증 진행 상태를 확인합니다.
        * 상태확인 함수는 자동이체 출금동의 요청 함수를 호출한 당일 23시 59분 59초까지만 호출 가능합니다.
        * 자동이체 출금동의 요청 함수를 호출한 당일 23시 59분 59초 이후 상태확인 함수를 호출할 경우 오류가 반환됩니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/cms/api#GetCMSStatus
		*/
        public IActionResult GetCMSStatus()
		{
            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            String clientCode = "023070000014";

			// 요청시 반환받은 접수아이디
			String receiptId = "02309010230700000140000000000005";

            try
			{
				var resultObj = _passcertService.getCMSStatus(clientCode, receiptId);
				return View("getCMSStatus", resultObj);
			}
			catch (BarocertException ke)
			{
				return View("exception", ke);
			}

		}

        /**
		* 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
        * 검증 함수는 자동이체 출금동의 요청 함수를 호출한 당일 23시 59분 59초까지만 호출 가능합니다.
        * 자동이체 출금동의 요청 함수를 호출한 당일 23시 59분 59초 이후 검증 함수를 호출할 경우 오류가 반환됩니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/cms/api#VerifyCMS
		*/
        public IActionResult VerifyCMS()
		{

            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            String clientCode = "023070000014";

			// 요청시 반환받은 접수아이디
			String receiptId = "02309010230700000140000000000005";

            // 검증 요청 정보 객체
            CMSVerify cmsVerify = new CMSVerify();
            // 검증 요청 휴대폰번호 - 11자 (하이픈 제외)
            cmsVerify.receiverHP = _passcertService.encrypt("01012341234");
            // 검증 요청 성명 - 최대 80자
            cmsVerify.receiverName = _passcertService.encrypt("홍길동");

            try
            {
				var resultObj = _passcertService.verifyCMS(clientCode, receiptId, cmsVerify);
				return View("VerifyCMS", resultObj);
			}
			catch (BarocertException ke)
			{
				return View("Exception", ke);
			}

		}

        /**
		* 패스 이용자에게 간편로그인을 요청합니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/login/api#RequestLogin
		*/
        public IActionResult RequestLogin()
        {

            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            String clientCode = "023070000014";

            // 간편로그인 요청 정보 객체
            Login login = new Login();

            // 수신자 휴대폰번호 - 11자 (하이픈 제외)
            login.receiverHP = _passcertService.encrypt("01012341234");
            // 수신자 성명 - 80자
            login.receiverName = _passcertService.encrypt("홍길동");
            // 수신자 생년월일 - 8자 (yyyyMMdd)
            login.receiverBirthday = _passcertService.encrypt("19900911");

            // 인증요청 메시지 제목 - 최대 40자
            login.reqTitle = "간편로그인 메시지 제목란";
            // 인증요청 메시지 - 최대 500자
            login.reqMessage = _passcertService.encrypt("간편로그인 메시지");
            // 고객센터 연락처 - 최대 12자
            login.callCenterNum = "1600-9854";
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            login.expireIn = 1000;
            // 서명 원문 - 최대 2,800자 까지 입력가능
            login.token = _passcertService.encrypt("간편로그인요청토큰");

            // 사용자 동의 필요 여부
            login.userAgreementYN = true;
            // 사용자 정보 포함 여부
            login.receiverInfoYN = true;

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Push 인증방식
            login.appUseYN = false;
            // ApptoApp 인증방식에서 사용
            // 통신사 유형('SKT', 'KT', 'LGU'), 대문자 입력(대소문자 구분)
            // login.telcoType = "SKT";
            // ApptoApp 인증방식에서 사용
            // 모바일장비 유형('ANDROID', 'IOS'), 대문자 입력(대소문자 구분)
            // login.deviceOSType = "IOS";

            try
            {
                var result = _passcertService.requestLogin(clientCode, login);
                return View("requestLogin", result);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }
        }

        /**
		* 간편로그인 요청 후 반환받은 접수아이디로 진행 상태를 확인합니다.
        * 상태확인 함수는 간편로그인 요청 함수를 호출한 당일 23시 59분 59초까지만 호출 가능합니다.
        * 간편로그인 요청 함수를 호출한 당일 23시 59분 59초 이후 상태확인 함수를 호출할 경우 오류가 반환됩니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/login/api#GetLoginStatus
		*/
        public IActionResult GetLoginStatus()
        {

            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            String clientCode = "023070000014";

            // 요청시 반환받은 접수아이디
            String receiptId = "02309010230700000140000000000006";

            try
            {
                var result = _passcertService.getLoginStatus(clientCode, receiptId);
                return View("getLoginStatus", result);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }

        }

        /**
		* 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
        * 검증 함수는 간편로그인 요청 함수를 호출한 당일 23시 59분 59초까지만 호출 가능합니다.
        * 간편로그인 요청 함수를 호출한 당일 23시 59분 59초 이후 검증 함수를 호출할 경우 오류가 반환됩니다.
		* https://developers.barocert.com/reference/pass/dotnetcore/login/api#VerifyLogin
		*/

        public IActionResult VerifyLogin()
        {

            // Passcert 이용기관코드, Passcert 파트너 사이트에서 확인
            string clientCode = "023070000014";

            // 요청시 반환받은 접수아이디
            string receiptId = "02309010230700000140000000000006";

            // 검증 요청 정보 객체
            LoginVerify loginVerify = new LoginVerify();
            // 검증 요청 휴대폰번호 - 11자 (하이픈 제외)
            loginVerify.receiverHP = _passcertService.encrypt("01012341234");
            // 검증 요청 성명 - 최대 80자
            loginVerify.receiverName = _passcertService.encrypt("홍길동");

            try
            {
                var result = _passcertService.verifyLogin(clientCode, receiptId, loginVerify);
                return View("verifyLogin", result);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }
        }

    }
}
