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

        [HttpGet]
        public ActionResult Index(string sessionId)
        {
            if (sessionId != null)
            {
                var data = this.GetMergedData(sessionId);
                return View(data);
            }
            return View();
        }

        [HttpPost]
        [Route("StressFeedback/PowerPointData/{sessionId}/{dateTime}/{slideIndex}")]
        public ActionResult PowerPointData(string sessionId, string dateTime, int slideIndex)
        {
            var dateTime1 = tranformStringToDate(dateTime);
            if (!powerpointData.ContainsKey(sessionId) || powerpointData[sessionId] == null)
            {
                powerpointData[sessionId] = new List<Controllers.PowerPointData>();
            }
            powerpointData[sessionId].Add(new PowerPointData {SessionId = sessionId, DateTime= dateTime1, SlideIndex = slideIndex});

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
        public ActionResult BandData(string sessionId, string dateTime, int hearbeat, double temperature)
        {
            var dateTime1 = tranformStringToDate(dateTime);
            if (!bandData.ContainsKey(sessionId) || bandData[sessionId] == null)
            {
                bandData[sessionId] = new List<Controllers.BandData>();
            }
            bandData[sessionId].Add(new BandData { SessionId = sessionId, DateTime = dateTime1, HeartBeat = hearbeat, Temperature = temperature});

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

        public MergedData GetMergedData(string sessionId)
        {
            if (!StressFeedbackController.powerpointData.ContainsKey(sessionId) ||
                !StressFeedbackController.bandData.ContainsKey(sessionId))
            {
                return null;
            }
            var powerPointData = StressFeedbackController.powerpointData[sessionId];
            var bandData = StressFeedbackController.bandData[sessionId];
            var mergedData = new MergedData();
            mergedData.SessionId = sessionId;
            mergedData.Data = new List<MergedDataItem>();
            var indexBandData = 0;
            for (int powerpointIndex = 0; powerpointIndex < powerPointData.Count; powerpointIndex++)
            {
                var pointData = powerPointData[powerpointIndex];
                if (indexBandData >= bandData.Count)
                {
                    break;
                }
                //For each slide, we collect all the Band data to get Min, Average, Max
                var item = new MergedDataItem();
                item.SlideIndex = pointData.SlideIndex;
                item.Time = pointData.DateTime;
                var count = 0;
                var sum = 0;
                while (indexBandData < bandData.Count && bandData[indexBandData].DateTime<pointData.DateTime)
                {
                    count++;
                    var dd = bandData[indexBandData];
                    item.HeartBeatMinimum = dd.HeartBeat < item.HeartBeatMinimum ? dd.HeartBeat : item.HeartBeatMinimum;
                    item.HeartBeatMaximum = dd.HeartBeat > item.HeartBeatMaximum ? dd.HeartBeat : item.HeartBeatMaximum;
                    sum += dd.HeartBeat;
                }
                item.HeartBeatAverage = sum/count;
                mergedData.Data.Add(item);
            }

            return mergedData;
        }

        /// <summary>
        /// Transform 2016D07D25_23T33T55 => 2016-07-25 23:33:55 
        /// </summary>
        /// <param name="dateInString"></param>
        /// <returns></returns>
        private DateTime tranformStringToDate(string dateInString)
        {
            var d= dateInString.Replace("D", "-").Replace("T", ":").Replace("_", " ");
            return DateTime.Parse(d, null, System.Globalization.DateTimeStyles.RoundtripKind);
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
        public double Temperature { get; set; }
    }

    public class MergedData
    {
        public string SessionId { get; set; }

        public List<MergedDataItem> Data{get;set;}
    }

    public class MergedDataItem
    {
        public DateTime Time { get; set; }
        public int SlideIndex { get; set; }

        public int HeartBeatMinimum { get; set; }
        public int HeartBeatMaximum { get; set; }
        public int HeartBeatAverage { get; set; }

        public double Temperature { get; set; }

        public MergedDataItem()
        {
            this.HeartBeatMinimum = 100;
            this.HeartBeatAverage = 0;
            this.HeartBeatMaximum = 0;
            this.Temperature = 0;
        }
    }
}
