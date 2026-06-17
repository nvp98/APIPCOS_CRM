using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using APIPCOS_CRM.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIPCOS_CRM.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        private UnitOfWork unitOfWork;
        private IHttpContextAccessor httpContextAccessor;

        public AuthController(PLCOS_Context context, IHttpContextAccessor _httpContextAccessor)
        {
            unitOfWork = new UnitOfWork(context);
            httpContextAccessor = _httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<IActionResult> login([FromBody] UserModel model)
        {
            UserModel user = null;
            user = await Task.FromResult(unitOfWork.UserRepository.Authenticate(model.username, model.password));
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            var authenticationString = $"{user.username}:{model.password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            string token = $"Basic {base64EncodedAuthenticationString}";
            httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", token);
            user.password = null;
            return Ok(user);
        }
        //[AllowAnonymous]
        //[HttpPost("ResetPassword")]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        //public async Task<IActionResult> ResetPassword([FromBody] UserModel model)
        //{
        //    MsgModel msg= new MsgModel();
        //    UserModel user = null;
        //    user = await Task.FromResult(unitOfWork.UserRepository.Authenticate(model.username, model.password));
        //    if (user == null)
        //    {
        //        return BadRequest(new { message = "Username or password is incorrect" });
        //    }
        //    string salt = BCrypt.Net.BCrypt.GenerateSalt();
        //    model.password = BCrypt.Net.BCrypt.HashPassword(model.resetPassword, salt);
        //    msg = await unitOfWork.UserRepository.Update(model);
        //    return Ok(new
        //    {
        //        status = (msg.status == 0) ? "error" : "success",
        //        msg = msg.msg
        //    }); ;
        //}
    }
}
