using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SignalRServer.Providers;

namespace SignalRServer.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        private readonly NewsStore _newsStore;

        public NewsController(NewsStore newsStore)
        {
            _newsStore = newsStore;
        }

        [HttpPost]
        public IActionResult AddGroup([FromQuery] string group)
        {
            if (string.IsNullOrEmpty(group))
            {
                return BadRequest();
            }
            _newsStore.AddGroup(group);
            return Created("AddGroup", group);
        }

        [HttpGet]
        public List<string> GetAllGroups()
        {
            try
            {
                var a = _newsStore.GetAllGroups();
                return a;
            }
            catch (Exception ex)
            {
                throw;    
            }
            
        }
    }
}
