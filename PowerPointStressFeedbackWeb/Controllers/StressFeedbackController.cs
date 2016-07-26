using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace PowerPointStressFeedbackWeb.Controllers
{
    public class StressFeedbackController : Controller
    {

        private static Dictionary<string, List<PowerPointData>> powerpointData = new Dictionary<string, List<PowerPointData>>();
        private static Dictionary<string, List<BandData>> bandData = new Dictionary<string, List<BandData>>();


        [HttpPost]
        public ActionResult PowerPointData(PowerPointData item)
        {
            if (item == null)
            {
                return this.HttpNotFound();
            }
            if (!powerpointData.ContainsKey(item.SessionId) || powerpointData[item.SessionId] == null)
            {
                powerpointData[item.SessionId] = new List<Controllers.PowerPointData>();
            }
            powerpointData[item.SessionId].Add(item);

            return Json(new { status = "ok" });
        }

        [HttpGet]
        public ActionResult PowerPointDataBySession(string sessionId)
        {
            if (!bandData.ContainsKey(sessionId))
            {
                return this.HttpNotFound();
            }
            var item = powerpointData[sessionId];
            return Json(item);
        }

        [HttpPost]
        public ActionResult BandData(BandData item)
        {
            if (item == null)
            {
                return this.HttpNotFound();
            }
            if (!bandData.ContainsKey(item.SessionId) || bandData[item.SessionId] == null)
            {
                bandData[item.SessionId] = new List<Controllers.BandData>();
            }
            bandData[item.SessionId].Add(item);

            return Json(new {status="ok"});
        }

        [HttpGet]
        public ActionResult BandDataBySession(string sessionId)
        {
            if (!bandData.ContainsKey(sessionId))
            {
                return this.HttpNotFound();
            }

            var item = bandData[sessionId];
            return Json(item);
        }
    }

    public class CommontData
    {
        public string SessionId { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class PowerPointData : CommontData
    {

        public int SlideIndex { get; set; }
    }

    public class BandData : CommontData
    {
        public int HeartBeat { get; set; }
    }
}
