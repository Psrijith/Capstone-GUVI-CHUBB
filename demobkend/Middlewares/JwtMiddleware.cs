using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace demobkend.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _secretKey = configuration["JwtSettings:SecretKey"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                AttachUserToContext(context, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken?.Claims.First(x => x.Type == "sub").Value;
                var username = jwtToken?.Claims.First(x => x.Type == "name").Value;
                var role = jwtToken?.Claims.First(x => x.Type == "role").Value;

                // Log the extracted claims for debugging purposes
                Console.WriteLine($"Token validated. User ID: {userId}, Username: {username}, Role: {role}");

                context.Items["User"] = new { UserId = userId, Username = username, Role = role };
            }
            catch (Exception ex)
            {
                // Log error here if needed
                Console.WriteLine($"Invalid token: {ex.Message}");
            }
        }


    }
}
