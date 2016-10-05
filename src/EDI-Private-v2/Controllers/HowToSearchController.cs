using Microsoft.AspNetCore.Mvc;

namespace EDIPrivate.Controllers
{
    [Route("[controller]")]
    public sealed class HowToSearchController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index() =>
            View();
    }
}