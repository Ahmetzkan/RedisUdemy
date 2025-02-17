using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase database;

        public string hashKey { get; set; } = "dictionary";

        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;
            database = _redisService.getDb(3);
        }
        public IActionResult Index()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (database.KeyExists(hashKey))
            {   
                foreach (var item in database.HashGetAll(hashKey).ToList())
                {
                    dictionary.Add(item.Name, item.Value);
                }
            }
            return View(dictionary);
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            database.HashSet(hashKey, name, value);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(string name)
        {
            database.HashDelete(hashKey, name);

            return RedirectToAction("Index");
        }
    }
}
