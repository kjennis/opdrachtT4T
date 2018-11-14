using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using opdracht.data;
using opdracht.interfaces;

namespace opdracht.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly IDataService _dataService;

        public SampleDataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("[action]")]
        public IEnumerable<Track> Tracks()
        {
            return _dataService.getTracks();
        }
    }
}
