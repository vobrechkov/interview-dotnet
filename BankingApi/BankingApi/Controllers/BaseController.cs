using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankingApi.Controllers
{
    public class BaseController : Controller
    {
        private readonly IOptions<ApiBehaviorOptions> _behaviorOptions;

        public BaseController(IOptions<ApiBehaviorOptions> behaviorOptions)
        {
            _behaviorOptions = behaviorOptions;
        }

        public IActionResult ModelStateValidationBadRequest()
           => _behaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
    }
}
