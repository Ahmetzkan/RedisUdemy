using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private RedisService _redisService;
        private readonly IDatabase database;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            database = _redisService.getDb(0);
        }
        public IActionResult Index()
        {
            database.StringSet("name", "Ahmet Özkan");
            database.StringSet("visitor", 100);
            return View();
        }

        public IActionResult Show()
        {
            var name = database.StringGet("name");
            database.StringIncrement("visitor", 1);
            if (name.HasValue) ViewBag.name = name.ToString();

            return View();
        }
    }
}
