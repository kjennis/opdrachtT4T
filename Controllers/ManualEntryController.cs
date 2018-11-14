using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using opdracht.data;
using opdracht.interfaces;

namespace opdracht.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ManualEntryController : Controller
    {
        private readonly IDataService _dataService;

        public ManualEntryController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        [Route("PostTalk")]  
        public ActionResult PostTalk([FromBody] Talk talk)
        {
            _dataService.addTalk(talk);
            return Json("Upload Successful.");
        }
    }
}
