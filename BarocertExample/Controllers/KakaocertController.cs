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


		/**
		* 카카오톡 사용자에게 본인인증 전자서명을 요청합니다.
		*/
		public IActionResult RequestIdentity()
		{

			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 본인인증 요청 정보 객체
			Identity identity = new Identity();

			// 수신자 정보
			// 휴대폰번호,성명,생년월일 또는 Ci(연계정보)값 중 택 일
			identity.receiverHP = _kakaocertService.encrypt("01012341324");
			identity.receiverName = _kakaocertService.encrypt("홍길동");
			identity.receiverBirthday = _kakaocertService.encrypt("19700101");
			// identity.ci = _kakaocertService.encrypt("");

			// 인증요청 메시지 제목 - 최대 40자
			identity.reqTitle = "인증요청 메시지 제목란";
			// 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
			identity.expireIn = 1000;
			// 서명 원문 - 최대 40자 까지 입력가능
			identity.token = _kakaocertService.encrypt("본인인증요청토큰");

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

		/**
		* 본인인증 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다.
		*/
		public IActionResult GetIdentityStatus()
		{

			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 요청시 반환받은 접수아이디
			String receiptId = "02305020230400000010000000000007";

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

		/**
		* 본인인증 요청시 반환된 접수아이디를 통해 본인인증 서명을 검증합니다. 
		* 검증하기 API는 완료된 전자서명 요청당 1회만 요청 가능하며, 사용자가 서명을 완료후 유효시간(10분)이내에만 요청가능 합니다.
		*/

		public IActionResult VerifyIdentity()
		{

			// 이용기관코드, 파트너 사이트에서 확인
			string clientCode = "023040000001";

			// 요청시 반환받은 접수아이디
			string receiptId = "02305020230400000010000000000007";

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


		/**
		* 카카오톡 사용자에게 전자서명을 요청합니다.(단건)
		*/
		public IActionResult RequestSign()
		{

			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 전자서명 요청 정보 객체
			Sign sign = new Sign();

			// 수신자 정보
			// 휴대폰번호,성명,생년월일 또는 Ci(연계정보)값 중 택 일
			sign.receiverHP = _kakaocertService.encrypt("01012341324");
			sign.receiverName = _kakaocertService.encrypt("홍길동");
			sign.receiverBirthday = _kakaocertService.encrypt("19700101");
			// sign.ci = _kakaocertService.encrypt("");

			// 인증요청 메시지 제목 - 최대 40자
			sign.reqTitle = "전자서명단건테스트";
			// 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
			sign.expireIn = 1000;
			// 서명 원문 - 원문 2,800자 까지 입력가능
			sign.token = _kakaocertService.encrypt("전자서명단건테스트데이터");
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

		/**
		* 전자서명 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다.
		*/
		public IActionResult GetSignStatus()
		{

			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 요청시 반환받은 접수아이디
			String receiptId = "02305020230400000010000000000009";

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

		/**
		* 전자서명 요청시 반환된 접수아이디를 통해 서명을 검증합니다. (단건)
		* 검증하기 API는 완료된 전자서명 요청당 1회만 요청 가능하며, 사용자가 서명을 완료후 유효시간(10분)이내에만 요청가능 합니다.
		*/
		public IActionResult VerifySign()
		{
			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 요청시 반환받은 접수아이디
			String receiptId = "02305020230400000010000000000009";

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

		/**
		* 카카오톡 사용자에게 전자서명을 요청합니다.(복수)
		*/
		public IActionResult RequestMultiSign()
		{

			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 전자서명 요청 정보 객체
			MultiSign multiSign = new MultiSign();

			// 수신자 정보
			// 휴대폰번호,성명,생년월일 또는 Ci(연계정보)값 중 택 일
			multiSign.receiverHP = _kakaocertService.encrypt("01012341324");
			multiSign.receiverName = _kakaocertService.encrypt("홍길동");
			multiSign.receiverBirthday = _kakaocertService.encrypt("19700101");
			// multiSign.ci = _kakaocertService.encrypt("");

			// 인증요청 메시지 제목 - 최대 40자
			multiSign.reqTitle = "전자서명복수테스트";
			// 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
			multiSign.expireIn = 1000;

			// 개별문서 등록 - 최대 20 건
			// 개별 요청 정보 객체
			MultiSignTokens token = new MultiSignTokens();
			// 인증요청 메시지 제목 - 최대 40자
			token.reqTitle = "전자서명복수문서테스트1";
			// 서명 원문 - 원문 2,800자 까지 입력가능
			token.token = _kakaocertService.encrypt("전자서명복수테스트데이터1");
			multiSign.AddToken(token);

			// 개별 요청 정보 객체
			MultiSignTokens token2 = new MultiSignTokens();
			// 인증요청 메시지 제목 - 최대 40자
			token2.reqTitle = "전자서명복수문서테스트2";
			// 서명 원문 - 원문 2,800자 까지 입력가능
			token2.token = _kakaocertService.encrypt("전자서명복수테스트데이터2");
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

		/**
		* 전자서명 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다. (복수)
		*/
		public IActionResult GetMultiSignStatus()
		{

			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 요청시 반환받은 접수아이디
			String receiptId = "02305020230400000010000000000012";

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

		/**
		* 전자서명 요청시 반환된 접수아이디를 통해 서명을 검증합니다. (복수)
		* 검증하기 API는 완료된 전자서명 요청당 1회만 요청 가능하며, 사용자가 서명을 완료후 유효시간(10분)이내에만 요청가능 합니다.
		*/
		public IActionResult VerifyMultiSign()
		{

			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 요청시 반환받은 접수아이디
			String receiptId = "02305020230400000010000000000012";

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

		/**
		 * 카카오톡 사용자에게 자동이체 출금동의 전자서명을 요청합니다.
		 */
		public IActionResult RequestCMS()
		{
			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 출금동의 요청 정보 객체
			CMS cms = new CMS();

			// 수신자 정보
			// 휴대폰번호,성명,생년월일 또는 Ci(연계정보)값 중 택 일
			cms.receiverHP = _kakaocertService.encrypt("01012341324");
			cms.receiverName = _kakaocertService.encrypt("홍길동");
			cms.receiverBirthday = _kakaocertService.encrypt("19700101");
			// cms.ci = _kakaocertService.encrypt("");

			// 인증요청 메시지 제목 - 최대 40자
			cms.reqTitle = "인증요청 메시지 제공란";

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

		/**
		* 자동이체 출금동의 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다.
		*/
		public IActionResult GetCMSStatus()
		{
			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 요청시 반환받은 접수아이디
			String receiptId = "02305020230400000010000000000013";

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

		/**
		* 자동이체 출금동의 요청시 반환된 접수아이디를 통해 서명을 검증합니다.
		* 검증하기 API는 완료된 전자서명 요청당 1회만 요청 가능하며, 사용자가 서명을 완료후 유효시간(10분)이내에만 요청가능 합니다.
		*/
		public IActionResult VerifyCMS()
		{

			// Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
			String clientCode = "023040000001";

			// 요청시 반환받은 접수아이디
			String receiptId = "02305020230400000010000000000013";

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
