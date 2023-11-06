using Barocert;
using Barocert.navercert;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BarocertExample.Controllers
{
    public class NavercertController : Controller
    {
        private readonly NavercertService _navercertService;

        public NavercertController(NavercertInstance NCinstance)
        {
            // Navercert 서비스 객체 생성
            _navercertService = NCinstance.navercertService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /**
        * 네이버 이용자에게 본인인증을 요청합니다.
        * https://developers.barocert.com/reference/naver/dotnetcore/identity/api#RequestIdentity
        */
        public IActionResult RequestIdentity()
        {

            // Navercert 이용기관코드, Navercert 파트너 사이트에서 확인
            String clientCode = "023090000021";

            // 본인인증 요청 정보 객체
            Identity identity = new Identity();

            // 수신자 휴대폰번호 - 11자 (하이픈 제외)
            identity.receiverHP = _navercertService.encrypt("01012341234");
            // 수신자 성명 - 80자
            identity.receiverName = _navercertService.encrypt("홍길동");
            // 수신자 생년월일 - 8자 (yyyyMMdd)
            identity.receiverBirthday = _navercertService.encrypt("19700101");

            // 고객센터 연락처 - 최대 12자
            identity.callCenterNum = "1600-9854";
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            identity.expireIn = 1000;

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Talk Message 인증방식
            identity.appUseYN = false;
            // ApptoApp 인증방식에서 사용
            // 모바일장비 유형('ANDROID', 'IOS'), 대문자 입력(대소문자 구분)
            // identity.deviceOSType = "IOS";
            // ApptoApp 인증방식에서 사용
            // 호출할 URL
            // identity.returnURL = "navercert://sign";

            try
            {
                var result = _navercertService.requestIdentity(clientCode, identity);
                return View("requestIdentity", result);
            }
            catch (BarocertException ne)
            {
                return View("exception", ne);
            }
        }

        /*
        * 본인인증 요청 후 반환받은 접수아이디로 본인인증 진행 상태를 확인합니다.
        * https://developers.barocert.com/reference/naver/dotnetcore/identity/api#GetIdentityStatus
        */
        public IActionResult GetIdentityStatus()
        {

            // Navercert 이용기관코드, Navercert 파트너 사이트에서 확인
            String clientCode = "023090000021";

            // 요청시 반환받은 접수아이디
            String receiptId = "02311060230900000210000000000005";

            try
            {
                var result = _navercertService.getIdentityStatus(clientCode, receiptId);
                return View("getIdentityStatus", result);
            }
            catch (BarocertException ne)
            {
                return View("exception", ne);
            }

        }

        /*
        * 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
        * 반환받은 전자서명값(signedData)과 [1. RequestIdentity] 함수 호출에 입력한 Token의 동일 여부를 확인하여 이용자의 본인인증 검증을 완료합니다.
        * 네이버 보안정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류가 반환됩니다.
        * https://developers.barocert.com/reference/naver/dotnetcore/identity/api#VerifyIdentity
        */
        public IActionResult VerifyIdentity()
        {

            // Navercert 이용기관코드, Navercert 파트너 사이트에서 확인
            string clientCode = "023090000021";

            // 요청시 반환받은 접수아이디
            string receiptId = "02311060230900000210000000000005";

            try
            {
                var result = _navercertService.verifyIdentity(clientCode, receiptId);
                return View("verifyIdentity", result);
            }
            catch (BarocertException ne)
            {
                return View("exception", ne);
            }
        }


        /*
        * 네이버 이용자에게 단건(1건) 문서의 전자서명을 요청합니다.
        * https://developers.barocert.com/reference/naver/dotnetcore/sign/api-single#RequestSign
        */
        public IActionResult RequestSign()
        {

            // Navercert 이용기관코드, Navercert 파트너 사이트에서 확인
            String clientCode = "023090000021";

            // 전자서명 요청 정보 객체
            Sign sign = new Sign();

            // 수신자 휴대폰번호 - 11자 (하이픈 제외)
            sign.receiverHP = _navercertService.encrypt("01012341234");
            // 수신자 성명 - 80자
            sign.receiverName = _navercertService.encrypt("홍길동");
            // 수신자 생년월일 - 8자 (yyyyMMdd)
            sign.receiverBirthday = _navercertService.encrypt("19700101");

            // 인증요청 메시지 제목 - 최대 40자
            sign.reqTitle = "전자서명(단건) 요청 메시지 제목";
            // 고객센터 연락처
            sign.callCenterNum = "1600-9854";
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            sign.expireIn = 1000;
            // 전자서명 요청 메시지
            sign.reqMessage = _navercertService.encrypt("전자서명(단건) 요청 메시지");
            // 서명 원문 - 원문 2,800자 까지 입력가능
            sign.token = _navercertService.encrypt("전자서명(단건) 요청 원문");
            // 서명 원문 유형
            // 'TEXT' - 일반 텍스트, 'HASH' - HASH 데이터
            sign.tokenType = "TEXT";

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Talk Message 인증방식
            sign.appUseYN = false;
            // ApptoApp 인증방식에서 사용
            // 모바일장비 유형('ANDROID', 'IOS'), 대문자 입력(대소문자 구분)
            // sign.deviceOSType = "IOS";
            // ApptoApp 인증방식에서 사용
            // 에러시 호출할 URL
            // sign.returnURL = "navercert://sign";

            try
            {
                var result = _navercertService.requestSign(clientCode, sign);
                return View("requestSign", result);
            }
            catch (BarocertException ne)
            {
                return View("exception", ne);
            }
        }

        /*
        * 전자서명(단건) 요청 후 반환받은 접수아이디로 인증 진행 상태를 확인합니다.
        * https://developers.barocert.com/reference/naver/dotnetcore/sign/api-single#GetSignStatus
        */
        public IActionResult GetSignStatus()
        {

            // Navercert 이용기관코드, Navercert 파트너 사이트에서 확인
            String clientCode = "023090000021";

            // 요청시 반환받은 접수아이디
            String receiptId = "02311060230900000210000000000006";

            try
            {
                var resultObj = _navercertService.getSignStatus(clientCode, receiptId);
                return View("getSignStatus", resultObj);
            }
            catch (BarocertException ne)
            {
                return View("exception", ne);
            }

        }

        /*
         * 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
         * 네이버 보안정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류가 반환됩니다.
         * https://developers.barocert.com/reference/naver/dotnetcore/sign/api-single#VerifySign
         */
        public IActionResult VerifySign()
        {
            // Navercert 이용기관코드, Navercert 파트너 사이트에서 확인
            String clientCode = "023090000021";

            // 요청시 반환받은 접수아이디
            String receiptId = "02311060230900000210000000000006";

            try
            {
                var resultObj = _navercertService.verifySign(clientCode, receiptId);
                return View("verifySign", resultObj);
            }
            catch (BarocertException ne)
            {
                return View("Exception", ne);
            }
        }

        /*
         * 네이버 이용자에게 복수(최대 50건) 문서의 전자서명을 요청합니다.
         * https://developers.barocert.com/reference/naver/java/sign/api-multi#RequestMultiSign
         */
        public IActionResult RequestMultiSign()
        {

            // Navercert 이용기관코드, Navercert 파트너 사이트에서 확인
            String clientCode = "023090000021";

            // 전자서명(복수) 요청 정보 객체
            MultiSign multiSign = new MultiSign();

            // 수신자 휴대폰번호 - 11자 (하이픈 제외)
            multiSign.receiverHP = _navercertService.encrypt("01012341234");
            // 수신자 성명 - 80자
            multiSign.receiverName = _navercertService.encrypt("홍길동");
            // 수신자 생년월일 - 8자 (yyyyMMdd)
            multiSign.receiverBirthday = _navercertService.encrypt("19700101");

            // 인증요청 메시지 제목 - 최대 40자
            multiSign.reqTitle = "전자서명(복수) 요청 메시지 제목";
            // 고객센터 연락처 - 최대 12자
            multiSign.callCenterNum = "1600-9854";
            // 요청 메시지 - 최대 500자
            multiSign.reqMessage = _navercertService.encrypt("전자서명(복수) 요청 메시지");
            // 인증요청 만료시간 - 최대 1,000(초)까지 입력 가능
            multiSign.expireIn = 1000;

            // 개별문서 등록 - 최대 50건
            // 개별 요청 정보 객체
            MultiSignTokens token = new MultiSignTokens();
            // 서명 원문 유형
            // TEXT - 일반 텍스트, HASH - HASH 데이터
            token.tokenType = "TEXT";
            // 서명 원문 - 최대 2,800자까지 입력가능
            token.token = _navercertService.encrypt("전자서명(복수) 요청 원문 1");
            multiSign.addToken(token);

            // 개별문서 등록 - 최대 50건
            // 개별 요청 정보 객체
            MultiSignTokens token2 = new MultiSignTokens();
            // 서명 원문 유형
            // TEXT - 일반 텍스트, HASH - HASH 데이터
            token2.tokenType = "TEXT";
            // 서명 원문 - 최대 2,800자까지 입력가능
            token2.token = _navercertService.encrypt("전자서명(복수) 요청 원문 2");
            multiSign.addToken(token2);

            // AppToApp 인증요청 여부
            // true - AppToApp 인증방식, false - Talk Message 인증방식
            multiSign.appUseYN = false;
            // ApptoApp 인증방식에서 사용
            // 모바일장비 유형('ANDROID', 'IOS'), 대문자 입력(대소문자 구분)
            // multiSign.deviceOSType = "IOS";
            // ApptoApp 인증방식에서 사용
            // 에러시 호출할 URL
            // multiSign.returnURL = "navercert://sign";

            try
            {
                var result = _navercertService.requestMultiSign(clientCode, multiSign);
                return View("requestMultiSign", result);
            }
            catch (BarocertException ne)
            {
                return View("Exception", ne);
            }
        }

        /*
         * 전자서명(복수) 요청 후 반환받은 접수아이디로 인증 진행 상태를 확인합니다.
         * https://developers.barocert.com/reference/naver/dotnetcore/sign/api-multi#GetMultiSignStatus
         */
        public IActionResult GetMultiSignStatus()
        {

            // Navercert 이용기관코드, Navercert 파트너 사이트에서 확인
            String clientCode = "023090000021";

            // 요청시 반환받은 접수아이디
            String receiptId = "02311060230900000210000000000007";

            try
            {
                var resultObj = _navercertService.getMultiSignStatus(clientCode, receiptId);
                return View("getMultiSignStatus", resultObj);
            }
            catch (BarocertException ne)
            {
                return View("exception", ne);
            }

        }

        /*
         * 완료된 전자서명을 검증하고 전자서명값(signedData)을 반환 받습니다.
         * 네이버 보안정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류가 반환됩니다.
         * https://developers.barocert.com/reference/naver/dotnetcore/sign/api-multi#VerifyMultiSign
         */
        public IActionResult VerifyMultiSign()
        {

            // Navercert 이용기관코드, Navercert 파트너 사이트에서 확인
            String clientCode = "023090000021";

            // 요청시 반환받은 접수아이디
            String receiptId = "02311060230900000210000000000007";

            try
            {
                var result = _navercertService.verifyMultiSign(clientCode, receiptId);
                return View("verifyMultiSign", result);
            }
            catch (BarocertException ne)
            {
                return View("Exception", ne);
            }

        }
    }
}
