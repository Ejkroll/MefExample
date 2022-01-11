﻿using MefExample.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MefPlugin.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PluginsController : ControllerBase
    {
        private readonly ILogger<PluginsController> _logger;
        private readonly IEnumerable<IPluginService> _pluginServices;

        public PluginsController(ILogger<PluginsController> logger, IEnumerable<IPluginService> pluginServices)
        {
            _logger = logger;
            _pluginServices = pluginServices;
        }

        [HttpGet]
        public IActionResult PluginGetAll(CancellationToken cancellationToken = default)
        {
            var ret = _pluginServices
                .Select(p => new { 
                    Name = p.GetType().Name, 
                    Value = p.DoSomething() 
                })
                .ToList();
            return Ok(ret);
        }
    }
}
