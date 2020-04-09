using BAFactory.Fx.Samples.WebApiSecurity.WebApi.AppCode;
using BAFactory.Fx.Samples.WebApiSecurity.CryptoUtility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;


namespace BAFactory.Fx.Samples.WebApiSecurity.WebApi.Filters
{
    public class HMacSignedMessgeFilterAttribute : Attribute, IAuthenticationFilter
    {
        private static Dictionary<string, string> allowedApps = new Dictionary<string, string>();
        private readonly UInt64 requestMaxAgeInSeconds = 300;
        private readonly string authenticationScheme = "amx";

        public HMacSignedMessgeFilterAttribute()
        {
            if (allowedApps.Count == 0)
            {
                allowedApps.Add("4d53bce03ec34c0a911182d4c228ee6c", "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=");
            }
        }

        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var req = context.Request;

            if (req.Headers.Count(x => x.Key == "amx") > 0)
            {
                var rawAuthzHeader = req.Headers.First(x => x.Key == "amx").Value.FirstOrDefault();

                var authHeader = HmacHeader.Parse(rawAuthzHeader);

                if (authHeader != null)
                {
                    var isValid = ValidateRequest(req, authHeader.AppId, authHeader.Base64Signature, authHeader.Nonce, authHeader.TimeStamp);

                    if (!isValid) context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                }
                else
                {
                    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                }
            }
            else
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            }

            return Task.FromResult(0);
        }

        private bool ValidateRequest(HttpRequestMessage req, string appId, string receivedBase64Signature, string nonce, string requestTimeStamp)
        {
            var calculator = new ApiKeyCalculator();

            var content = req.Content.ReadAsStringAsync().Result;

            var calculatedBase64Signature = calculator.CalculateHmacHash(req.RequestUri.ToString(), req.Method.ToString(), content, appId, allowedApps[appId], nonce, requestTimeStamp);

            var calculatedHeader = HmacHeader.Parse(calculatedBase64Signature);

            return receivedBase64Signature == calculatedHeader.Base64Signature;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new ResultWithChallenge(context.Result);
            return Task.FromResult(0);
        }

        private List<string> GetAuthenticationHeaderValues(string header)
        {
            var credArray = header.Split(':');

            if (credArray.Length == 4)
            {
                return credArray.ToList();
            }
            else
            {
                return null;
            }
        }
    }

    public class ResultWithChallenge : IHttpActionResult
    {
        private readonly string authenticationScheme = "amx";
        private readonly IHttpActionResult next;

        public ResultWithChallenge(IHttpActionResult next)
        {
            this.next = next;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await next.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(authenticationScheme));
            }

            return response;
        }
    }
}