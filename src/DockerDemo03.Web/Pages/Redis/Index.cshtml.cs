using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DockerDemo03.Web.Pages.Redis
{
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly IConnectionMultiplexer _redis;
        public string RedisValue { get; set; }

        public Index(ILogger<Index> logger, IConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
        }

        public async Task OnGetAsync()
        {
            var db = _redis.GetDatabase();

            var existingValue = await db.StringGetAsync("MyKey");

            // Verificar si el key ya existe en Redis
            if (!existingValue.HasValue)
            {
                // Guardar un valor nuevo en Redis
                await db.StringSetAsync("MyKey", "Hello, Redis!");
            }

            // Recuperar el valor de Redis
            RedisValue = await db.StringGetAsync("MyKey");
        }
    }
}