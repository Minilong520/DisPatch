using DisPatch.Common.Helpers;
using DisPatch.Model.Basis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DisPatch.Common.Authentication
{
    public class DisPatchAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public DisPatchAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            await Task.CompletedTask;

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("缺少Token");
            }

            var token = Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(token))
            {
                //验证token是否正确                
                string[] authVal = new string[] { };                 
                try
                {                    
                    authVal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token.Substring(DisPatchAuthOptions.Scheme.Length + 1))).Split(':');
                }
                catch (Exception)
                {
                    return AuthenticateResult.Fail("Token不合法，无法转码");
                }
                if (authVal.Length != 2)
                {
                    return AuthenticateResult.Fail("Token不合法");
                }

                try
                {
                    AuthorizeHelper.CheckUserAuthorize(token, authVal);
                }
                catch (ExceptionTip_Basis e)
                {
                    return AuthenticateResult.Fail("Token提示：" + ((ExceptionTip_Basis)e).Message);
                }
                catch (Exception ex)
                {
                    return AuthenticateResult.Fail("Token异常：" + ex.Message);
                }

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, authVal[0]) };
                var identity = new ClaimsIdentity(claims, DisPatchAuthOptions.Scheme);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, DisPatchAuthOptions.Scheme);

                return AuthenticateResult.Success(ticket);
            }


            return AuthenticateResult.NoResult();
        }
    }
}
