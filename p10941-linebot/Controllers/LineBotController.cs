using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Line.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace p10941_linebot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly LineBotConfig _lineBotConfig;

        public LineBotController(IServiceProvider serviceProvider)
        {
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _httpContext = _httpContextAccessor.HttpContext;
            _lineBotConfig = new LineBotConfig();
            _lineBotConfig.channelSecret = "e5476f5c7ee7f07ce0ce9b5613b8fd18";
            _lineBotConfig.accessToken = "1dF4XkUwfB6dKQYm+WIAZo/h8MtH/6xKP6ATYW65OQ3sRljJ6pZj/aCEI/iIMFR7hO3bYJLBbYTpo6dlrC4lFtgAq8JU/U6nxaVElbGLUOOqvqHQVaUWj4vCddcXcihHYBPeMHJSzKIewvFN9MhbugdB04t89/1O/w1cDnyilFU=";
        }
        
        //完整的路由網址就是 https://xxx/api/linebot/run
        [HttpPost("run")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var events = await _httpContext.Request.GetWebhookEventsAsync(_lineBotConfig.channelSecret);
                var lineMessagingClient = new LineMessagingClient(_lineBotConfig.accessToken);
                var lineBotApp = new LineBotApp(lineMessagingClient);
                await lineBotApp.RunAsync(events);
            }
            catch (Exception ex)
            {
                // LOG
                //_logger.LogError(JsonConvert.SerializeObject(ex));
            }
            return Ok();
        }
    }
}