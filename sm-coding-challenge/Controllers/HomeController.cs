using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using sm_coding_challenge.Models;
using sm_coding_challenge.Services.DataProvider;

namespace sm_coding_challenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        private IDataProvider _dataProvider;

        private readonly int _maxIdsPerRequest = 5;

        public HomeController(IDataProvider dataProvider, ILogger<HomeController> logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ResponseCache(Duration = 86400)]
        public async Task<IActionResult> Player(string id)
        {
            try
            {
                if (!ValidId(id))
                {
                    return Error("Invalid id format");

                }
                //check for invalid input and return a failed message
                if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                {
                    return Error("Player ID required");
                }
                var result = await _dataProvider.GetPlayerById(id);
                if (result.Successful)
                {
                    return Json(result.Player);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception e)
            {
                //Log, email alert devs
                _logger.LogError(e.HResult, e.Message, id);
                return StatusCode(500, "An error occured");
            }
        }

        [HttpGet]
        [ResponseCache(Duration = 86400)]
        public async Task<IActionResult> Players(string ids)
        {
            try
            {
                //check for invalid input and return a failed message
                if (string.IsNullOrEmpty(ids) || string.IsNullOrWhiteSpace(ids))
                {
                    return Error("Player ID's required");

                }
                var returnList = new List<PlayerRequestResult>();
                var idList = ids.Split(',');
                idList = IDListTrimmer(idList);
                foreach (var id in idList)
                {
                    if (!ValidId(id))
                    {
                        return Error("Invalid id format");

                    }
                    returnList.Add( await _dataProvider.GetPlayerById(id));
                }
                return Json(returnList);
            }catch(Exception e)
            {
                _logger.LogError(e.HResult, e.Message, ids);
                return StatusCode(500, "Player ID's required");
            }
        }

        [HttpGet]
        [ResponseCache(Duration = 86400)]
        public async Task<IActionResult> LatestPlayers(string ids)
        {
            try
            {
                //check for invalid input and return a failed message
                if (string.IsNullOrEmpty(ids) || string.IsNullOrWhiteSpace(ids))
                {
                    return Error("Player ID's required");
                }
                var returnList = new List<LatestPlayers>();
                var idList = ids.Split(',');
                idList = IDListTrimmer(idList);
                foreach (var id in idList)
                {
                    if (!ValidId(id))
                    {
                        return Error("Invalid id format");
                    }
                    var result = await _dataProvider.GetPlayerById(id);
                    if(!result.Successful)
                    {
                        returnList.Add(new LatestPlayers());
                        continue;
                    }
                    else
                    {
                        returnList.Add(new LatestPlayers() {
                            Receiving = result.Player.Receives.Last(),
                            Rushing = result.Player.Rushes.Last()
                        }); 
                    }
                }
                return Json(returnList);
            }
            catch (Exception e)
            {
                _logger.LogError(e.HResult, e.Message, ids);
                return StatusCode(500, "An error occured while executing your request");
            }
        }

        public IActionResult Error(string message = "Invalid Request")
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return new ObjectResult(new PlayerRequestResult() { Message = message, Successful = false });
        }

        public string[] IDListTrimmer(string[] idList)
        {
            if (idList.Length > _maxIdsPerRequest)
            {
                return idList.Take(_maxIdsPerRequest).ToArray() ;
            }
            return idList;
        }

        public bool ValidId(string id)
        {
            Guid result;
            return Guid.TryParse(id, out result);
        }
    }
}
