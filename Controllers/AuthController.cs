using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace WebApiSession.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : Controller
	{
		string signinKey = "BuBenimSigninKey";


		[HttpGet]
		public IActionResult Get(string userName,string password)
		{
			//Veritabanı işlemleri
			//GuID
			//Json Web Token (JWT)

			var claims = new[]
			{
				new Claim(ClaimTypes.Name, userName),
				new Claim(JwtRegisteredClaimNames.Email,userName)
			};

			var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey));
			var crendential=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer:"https://www.blabla.com",
				audience:"BuBenimKullandığımAudienceDeğeri",
				claims:claims,
				expires:DateTime.Now.AddDays(15),
				notBefore:DateTime.Now,
				signingCredentials.crendential

				);

			var token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			return token;
		}

	
		[HttpGet("ValidateToken")]
		public bool ValidateToken(string token)
		{
			var securityKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey));

			try
			{
				JwtSecurityTokenHandler handler = new();
				handler.ValidateToken(token, new TokenValidationParameters()
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = securityKey,
					ValidateLifetime = true,
					ValidateAudience = false,
					ValidateIssuer = false
				}, out SecurityToken validatedToken);
				var jwtToken = (JwtSecurityToken)validatedToken;
				var claims=jwtToken.Claims.ToList();
				return true;
			}
			catch (System.Exception)
			{

				return false;
			}
		}
	}
}
