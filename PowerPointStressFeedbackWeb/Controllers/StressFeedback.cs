using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace PowerPointStressFeedbackWeb.Controllers
{

    [Route("api/[controller]")]
    public class StressFeedback:Controller
    {

        private static Dictionary<string, List<PowerPointData>> powerpointData = new Dictionary<string, List<PowerPointData>>();
        private static Dictionary<string, List<BandData>> bandData = new Dictionary<string, List<BandData>>();


        [HttpPost("powerpoint")]
        public IActionResult PowerPointData([FromBody] PowerPointData item)
        {
            if (item == null)
            {
                return this.HttpBadRequest();
            }
            if (!powerpointData.ContainsKey(item.SessionId) || powerpointData[item.SessionId] == null)
            {
                powerpointData[item.SessionId] = new List<Controllers.PowerPointData>();
            }
            powerpointData[item.SessionId].Add(item);
        
            return base.CreatedAtRoute("PowerPointDataBySession", new { id = item.SessionId }, item);
        }

        [HttpGet("powerpoint/{sessionId}", Name = "PowerPointDataBySession")]
        public IActionResult PowerPointDataBySession(string sessionId)
        {
            if (!bandData.ContainsKey(sessionId))
            {
                return this.HttpNotFound();
            }
            var item = powerpointData[sessionId];
            return new ObjectResult(item);
        }

        [HttpPost("band")]
        public IActionResult BandData([FromBody] BandData item)
        {
            if (item == null)
            {
                return this.HttpBadRequest();
            }
            if (!bandData.ContainsKey(item.SessionId) || bandData[item.SessionId] == null)
            {
                bandData[item.SessionId] = new List<Controllers.BandData>();
            }
            bandData[item.SessionId].Add(item);

            return base.CreatedAtRoute("BandDataBySession", new { id = item.SessionId }, item);
        }

        [HttpGet("band/{sessionId}", Name = "BandDataBySession")]
        public IActionResult BandDataBySession(string sessionId)
        {
            if (!bandData.ContainsKey(sessionId))
            {
                return this.HttpNotFound();
            }

            var item = bandData[sessionId];
            return new ObjectResult(item);
        }
    }

    public class CommontData
    {
        public string SessionId { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class PowerPointData: CommontData
    {

        public int SlideIndex { get; set; }
    }

    public class BandData: CommontData
    {
        public int HeartBeat { get; set; }
    }
}
