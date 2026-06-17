using Microsoft.AspNetCore.Authorization;

namespace APIPCOS_CRM.Helper
{
    [Authorize]
    [AllowAnonymous]
    public class CheckLogin
    {
        private readonly RequestDelegate _next;

        public CheckLogin(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Kiểm tra xem người dùng đã được xác thực chưa
            if ((context.Request.Path.ToString().Contains("api")))
            {
                await _next(context);
            }
            else
            {
                if (!context.Request.Headers.ContainsKey("Authoriztion"))
                {
                    // Nếu đã đăng nhập, cho phép request tiếp theo tiếp tục xử lý
                    context.Response.Redirect("/Auth/login");

                }
                else
                {
                    // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập hoặc trả về lỗi
                    await _next(context);
                    // Hoặc có thể trả về một mã lỗi Unauthorized (401)
                    // context.Response.StatusCode = 401;
                }
            }

        }
    }
}
