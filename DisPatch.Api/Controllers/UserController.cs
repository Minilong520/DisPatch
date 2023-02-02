using DisPatch.Implements.User.Services;
using DisPatch.Model.Basis;
using DisPatch.Model.User;
using Microsoft.AspNetCore.Mvc;

namespace DisPatch.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public RES_Basis Login(REQ_Login reqLogin)
        {
            return new SLogin().Execute(reqLogin);
        }

    }
}
