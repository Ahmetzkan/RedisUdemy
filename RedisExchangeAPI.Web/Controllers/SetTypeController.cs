using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private RedisService _redisService;
        private readonly IDatabase database;
        private string listKey = "setNames";
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            database = _redisService.getDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();
            if (database.KeyExists(listKey))
            {
                database.SetMembers(listKey).ToList().ForEach(x => nameList.Add(x.ToString()));
            }
            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            database.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            database.SetAdd(listKey, name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string name)
        {
            await database.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
