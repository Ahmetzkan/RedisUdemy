using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private RedisService _redisService;
        private readonly IDatabase database;
        private string listKey = "names";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            database = _redisService.getDb(1);
        }
        public IActionResult Index()
        {
            List<string> nameList = new List<string>();
            if (database.KeyExists(listKey))
            {
                foreach (var item in database.ListRange(listKey).ToList())
                {
                    nameList.Add(item.ToString());
                }
                ;
            }
            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            database.ListRightPush(listKey, name);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string name)
        {
            database.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            database.ListLeftPopAsync(listKey).Wait();
            return RedirectToAction("Index");
        }
    }
}
