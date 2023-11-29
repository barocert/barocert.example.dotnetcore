using Barocert;
using Barocert.kakaocert;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BarocertExample.Controllers
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

        /*
        * 카카오톡 이용자에게 본인인증을 요청합니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/identity/api#RequestIdentity
        */
        public IActionResult RequestIdentity()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 본인인증 요청 정보 객체
            Identity identity = new Identity();

            // 수신자 휴대폰번호 - 11자 (하이픈 제외)
            identity.receiverHP = _kakaocertService.encrypt("01067668440");
            // 수신자 성명 - 80자
            identity.receiverName = _kakaocertService.encrypt("정우석");
            // 수신자 생년월일 - 8자 (yyyyMMdd)
            identity.receiverBirthday = _kakaocertService.encrypt("19900911");

            // 인증요청 메시지 제목 - 최대 40자
            identity.reqTitle = "본인인증 요청 메시지 제목";
            // 커스텀 메시지 - 최대 500자
            identity.extraMessage = _kakaocertService.encrypt("본인인증 커스텀 메시지");
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            identity.expireIn = 1000;
            // 서명 원문 - 최대 40자 까지 입력가능
            identity.token = _kakaocertService.encrypt("본인인증 요청 원문");

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Talk Message 인증방식
            identity.appUseYN = false;

            // App to App 방식 이용시, 호출할 URL
            identity.returnURL = "https://www.kakaocert.com";

            try
            {
                var result = _kakaocertService.requestIdentity(clientCode, identity);
                return View("requestIdentity", result);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }
        }

        /*
        * 본인인증 요청 후 반환받은 접수아이디로 본인인증 진행 상태를 확인합니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/identity/api#GetIdentityStatus
        */
        public IActionResult GetIdentityStatus()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 요청시 반환받은 접수아이디
            String receiptId = "02309010230400000010000000000001";

            try
            {
                var result = _kakaocertService.getIdentityStatus(clientCode, receiptId);
                return View("getIdentityStatus", result);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }

        }

        /*
        * 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
        * 반환받은 전자서명값(signedData)과 [1. RequestIdentity] 함수 호출에 입력한 Token의 동일 여부를 확인하여 이용자의 본인인증 검증을 완료합니다.
        * 카카오 보안정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류가 반환됩니다.
        * 전자서명 완료일시로부터 10분 이후에 검증 API를 호출하면 오류가 반환됩니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/identity/api#VerifyIdentity
        */
        public IActionResult VerifyIdentity()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "023040000001";

            // 요청시 반환받은 접수아이디
            string receiptId = "02309010230400000010000000000001";

            try
            {
                var result = _kakaocertService.verifyIdentity(clientCode, receiptId);
                return View("verifyIdentity", result);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }
        }


        /*
        * 카카오톡 이용자에게 단건(1건) 문서의 전자서명을 요청합니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/sign/api-single#RequestSign
        */
        public IActionResult RequestSign()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 전자서명 요청 정보 객체
            Sign sign = new Sign();


            // 수신자 휴대폰번호 - 11자 (하이픈 제외)
            sign.receiverHP = _kakaocertService.encrypt("01067668440");
            // 수신자 성명 - 80자
            sign.receiverName = _kakaocertService.encrypt("정우석");
            // 수신자 생년월일 - 8자 (yyyyMMdd)
            sign.receiverBirthday = _kakaocertService.encrypt("19900911");

            // 서명 요청 제목 - 최대 40자
            sign.signTitle = "전자서명(단건) 서명 요청 제목";
            // 커스텀 메시지 - 최대 500자
            sign.extraMessage = _kakaocertService.encrypt("전자서명(단건) 커스텀 메시지");
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            sign.expireIn = 1000;
            // 서명 원문 - 원문 2,800자 까지 입력가능
            sign.token = _kakaocertService.encrypt("전자서명(단건) 요청 원문");
            // 서명 원문 유형
            // TEXT - 일반 텍스트, HASH - HASH 데이터
            sign.tokenType = "TEXT";

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Talk Message 인증방식
            sign.appUseYN = false;

            // App to App 방식 이용시, 호출할 URL
            // sign.returnURL "https://www.kakaocert.com";

            try
            {
                var result = _kakaocertService.requestSign(clientCode, sign);
                return View("requestSign", result);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }
        }

        /*
        * 전자서명(단건) 요청 후 반환받은 접수아이디로 인증 진행 상태를 확인합니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/sign/api-single#GetSignStatus
        */
        public IActionResult GetSignStatus()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 요청시 반환받은 접수아이디
            String receiptId = "02309010230400000010000000000002";

            try
            {
                var resultObj = _kakaocertService.getSignStatus(clientCode, receiptId);
                return View("getSignStatus", resultObj);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }

        }

        /*
        * 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
        * 카카오 보안정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류가 반환됩니다.
        * 전자서명 완료일시로부터 10분 이후에 검증 API를 호출하면 오류가 반환됩니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/sign/api-single#VerifySign
        */
        public IActionResult VerifySign()
        {
            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 요청시 반환받은 접수아이디
            String receiptId = "02309010230400000010000000000002";

            try
            {
                var resultObj = _kakaocertService.verifySign(clientCode, receiptId);
                return View("verifySign", resultObj);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }
        }

        /*
        * 카카오톡 이용자에게 복수(최대 20건) 문서의 전자서명을 요청합니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/sign/api-multi#RequestMultiSign
        */
        public IActionResult RequestMultiSign()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 전자서명 요청 정보 객체
            MultiSign multiSign = new MultiSign();

            // 수신자 휴대폰번호 - 11자 (하이픈 제외)
            multiSign.receiverHP = _kakaocertService.encrypt("01067668440");
            // 수신자 성명 - 80자
            multiSign.receiverName = _kakaocertService.encrypt("정우석");
            // 수신자 생년월일 - 8자 (yyyyMMdd)
            multiSign.receiverBirthday = _kakaocertService.encrypt("19900911");

            // 인증요청 메시지 제목 - 최대 40자
            multiSign.reqTitle = "전자서명(복수) 요청 메시지 제목";
            // 커스텀 메시지 - 최대 500자
            multiSign.extraMessage = _kakaocertService.encrypt("전자서명(복수) 커스텀 메시지");
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            multiSign.expireIn = 1000;

            // 개별문서 등록 - 최대 20 건
            // 개별 요청 정보 객체
            MultiSignTokens token = new MultiSignTokens();
            // 인증요청 메시지 제목 - 최대 40자
            token.signTitle = "전자서명(복수) 서명 요청 제목 1";
            // 서명 원문 - 원문 2,800자 까지 입력가능
            token.token = _kakaocertService.encrypt("전자서명(복수) 요청 원문 1");
            multiSign.AddToken(token);

            // 개별 요청 정보 객체
            MultiSignTokens token2 = new MultiSignTokens();
            // 인증요청 메시지 제목 - 최대 40자
            token2.signTitle = "전자서명(복수) 서명 요청 제목 2";
            // 서명 원문 - 원문 2,800자 까지 입력가능
            token2.token = _kakaocertService.encrypt("전자서명(복수) 요청 원문 2");
            multiSign.AddToken(token2);

            // 서명 원문 유형
            // TEXT - 일반 텍스트, HASH - HASH 데이터
            multiSign.tokenType = "TEXT";

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Talk Message 인증방식
            multiSign.appUseYN = false;

            // App to App 방식 이용시, 에러시 호출할 URL
            // multiSign.returnURL = "https://www.kakaocert.com";

            try
            {
                var result = _kakaocertService.requestMultiSign(clientCode, multiSign);
                return View("requestMultiSign", result);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }
        }

        /*
        * 전자서명(복수) 요청 후 반환받은 접수아이디로 인증 진행 상태를 확인합니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/sign/api-multi#GetMultiSignStatus
        */
        public IActionResult GetMultiSignStatus()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 요청시 반환받은 접수아이디
            String receiptId = "02309010230400000010000000000003";

            try
            {
                var resultObj = _kakaocertService.getMultiSignStatus(clientCode, receiptId);
                return View("getMultiSignStatus", resultObj);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }

        }

        /*
        * 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
        * 카카오 보안정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류가 반환됩니다.
        * 전자서명 완료일시로부터 10분 이후에 검증 API를 호출하면 오류가 반환됩니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/sign/api-multi#VerifyMultiSign
        */
        public IActionResult VerifyMultiSign()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 요청시 반환받은 접수아이디
            String receiptId = "02309010230400000010000000000003";

            try
            {
                var result = _kakaocertService.verifyMultiSign(clientCode, receiptId);
                return View("verifyMultiSign", result);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }

        }

        /*
        * 카카오톡 이용자에게 자동이체 출금동의를 요청합니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/cms/api#RequestCMS
        */
        public IActionResult RequestCMS()
        {
            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 출금동의 요청 정보 객체
            CMS cms = new CMS();

            // 수신자 휴대폰번호 - 11자 (하이픈 제외)
            cms.receiverHP = _kakaocertService.encrypt("01067668440");
            // 수신자 성명 - 80자
            cms.receiverName = _kakaocertService.encrypt("정우석");
            // 수신자 생년월일 - 8자 (yyyyMMdd)
            cms.receiverBirthday = _kakaocertService.encrypt("19900911");

            // 인증요청 메시지 제목 - 최대 40자
            cms.reqTitle = "출금동의 요청 메시지 제목";
            // 커스텀 메시지 - 최대 500자
            cms.extraMessage = _kakaocertService.encrypt("출금동의 커스텀 메시지");
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            cms.expireIn = 1000;

            // 청구기관명 - 최대 100자
            cms.requestCorp = _kakaocertService.encrypt("청구기관명란");
            // 출금은행명 - 최대 100자
            cms.bankName = _kakaocertService.encrypt("출금은행명란");
            // 출금계좌번호 - 최대 32자
            cms.bankAccountNum = _kakaocertService.encrypt("9-4324-5117-58");
            // 출금계좌 예금주명 - 최대 100자
            cms.bankAccountName = _kakaocertService.encrypt("예금주명 입력란");
            // 출금계좌 예금주 생년월일 - 8자
            cms.bankAccountBirthday = _kakaocertService.encrypt("19930112");
            // 출금유형
            // CMS - 출금동의용, FIRM - 펌뱅킹, GIRO - 지로용
            cms.bankServiceType = _kakaocertService.encrypt("CMS"); // CMS, FIRM, GIRO

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Talk Message 인증방식
            cms.appUseYN = false;

            // App to App 방식 이용시, 에러시 호출할 URL
            cms.returnURL = "https://www.kakaocert.com";

            try
            {
                var result = _kakaocertService.requestCMS(clientCode, cms);
                return View("requestCMS", result);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }
        }

        /*
        * 자동이체 출금동의 요청 후 반환받은 접수아이디로 인증 진행 상태를 확인합니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/cms/api#GetCMSStatus
        */
        public IActionResult GetCMSStatus()
        {
            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 요청시 반환받은 접수아이디
            String receiptId = "02309010230400000010000000000004";

            try
            {
                var resultObj = _kakaocertService.getCMSStatus(clientCode, receiptId);
                return View("getCMSStatus", resultObj);
            }
            catch (BarocertException ke)
            {
                return View("exception", ke);
            }

        }

        /*
        * 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
        * 카카오 보안정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류가 반환됩니다.
        * 전자서명 완료일시로부터 10분 이후에 검증 API를 호출하면 오류가 반환됩니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/cms/api#VerifyCMS
        */
        public IActionResult VerifyCMS()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 요청시 반환받은 접수아이디
            String receiptId = "02309010230400000010000000000004";

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

        /*
        * 완료된 전자서명을 검증하고 전자서명 데이터 전문(signedData)을 반환 받습니다.
        * 카카오 보안정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류가 반환됩니다.
        * 전자서명 완료일시로부터 10분 이후에 검증 API를 호출하면 오류가 반환됩니다.
        * https://developers.barocert.com/reference/kakao/dotnetcore/login/api#VerifyLogin
        */
        public IActionResult VerifyLogin()
        {
            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            String clientCode = "023040000001";

            // 요청시 반환받은 트랜잭션 아이디
            String txID = "0189bd3760-7779-48c1-b8af-688606214f11";

            try
            {
                var resultObj = _kakaocertService.verifyLogin(clientCode, txID);
                return View("VerifyLogin", resultObj);
            }
            catch (BarocertException ke)
            {
                return View("Exception", ke);
            }
        }

    }
}
