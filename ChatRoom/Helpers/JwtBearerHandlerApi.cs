using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Newtonsoft.Json;

namespace ChatRoom.Helper
{
	public class JwtBearerHandlerApi : JwtBearerHandler
	{
		private readonly HttpClient _httpClient;

		public JwtBearerHandlerApi(HttpClient httpClient, IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
			_httpClient = httpClient;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			// Get the token from the Authorization header
			if (!Context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
			{
				return AuthenticateResult.Fail("Authorization header not found.");
			}

			var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
			if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
			{
				return AuthenticateResult.Fail("Bearer token not found in Authorization header.");
			}

			var token = authorizationHeader.Substring("Bearer ".Length).Trim();

			// Call the API to validate the token
			var response = await _httpClient.GetAsync($"BASE_URL/api/Validate?token={token}");

			// Return an authentication failure if the response is not successful
			if (!response.IsSuccessStatusCode)
			{
				return AuthenticateResult.Fail("Token validation failed.");
			}

			// Deserialize the response body to a custom object to get the validation result
			var validationResult = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

			// Return an authentication failure if the token is not valid
			if (!validationResult)
			{
				return AuthenticateResult.Fail("Token is not valid.");
			}

			// Set the authentication result with the claims from the API response
			var principal = GetClaims(token);

			return AuthenticateResult.Success(new AuthenticationTicket(principal, "CustomJwtBearer"));
		}


		private ClaimsPrincipal GetClaims(string Token)
		{
			var handler = new JwtSecurityTokenHandler();
			var token = handler.ReadToken(Token) as JwtSecurityToken;

			var claimsIdentity = new ClaimsIdentity(token.Claims, "Token");
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

			return claimsPrincipal;
		}


	}
}
