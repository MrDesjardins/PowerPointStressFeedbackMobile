using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;


namespace PowerPointStressFeedbackWeb.Controllers
{
    public class StressFeedbackController : Controller
    {

        private static Dictionary<string, List<PowerPointData>> powerpointData = new Dictionary<string, List<PowerPointData>>();
        private static Dictionary<string, List<BandData>> bandData = new Dictionary<string, List<BandData>>();

        /// <summary>
        /// Test data for a fake session id
        /// </summary>
        static StressFeedbackController()
        {
            // Test session
            var fakeSession = "12345";
            var testBandData = new List<BandData>();
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 00), HeartBeat = 60, Temperature = 37.5d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 01), HeartBeat = 61, Temperature = 37.5d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 02), HeartBeat = 62, Temperature = 37.5d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 03), HeartBeat = 61, Temperature = 37.5d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 04), HeartBeat = 62, Temperature = 37.5d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 05), HeartBeat = 62, Temperature = 37.5d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 06), HeartBeat = 62, Temperature = 37.6d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 07), HeartBeat = 63, Temperature = 37.6d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 08), HeartBeat = 63, Temperature = 37.6d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 09), HeartBeat = 64, Temperature = 37.6d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 10), HeartBeat = 64, Temperature = 37.7d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 11), HeartBeat = 65, Temperature = 37.7d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 12), HeartBeat = 65, Temperature = 37.7d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 13), HeartBeat = 67, Temperature = 37.7d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 14), HeartBeat = 67, Temperature = 37.7d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 15), HeartBeat = 66, Temperature = 37.8d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 16), HeartBeat = 65, Temperature = 37.8d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 17), HeartBeat = 65, Temperature = 37.8d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 18), HeartBeat = 64, Temperature = 37.8d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 19), HeartBeat = 66, Temperature = 37.9d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 20), HeartBeat = 67, Temperature = 37.9d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 21), HeartBeat = 70, Temperature = 37.9d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 22), HeartBeat = 70, Temperature = 37.8d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 23), HeartBeat = 72, Temperature = 37.8d });
            testBandData.Add(new BandData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 24), HeartBeat = 72, Temperature = 37.9d });

            var testPowerpointData = new List<PowerPointData>();
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 03), SlideIndex = 0 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 07), SlideIndex = 1 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 10), SlideIndex = 2 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 15), SlideIndex = 3 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 20), SlideIndex = 4 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 24), SlideIndex = 5 });
            powerpointData.Add(fakeSession, testPowerpointData);
            bandData.Add(fakeSession, testBandData);
        }

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
        [Route("StressFeedback/PowerPointData/{sessionId}/{dateTime}/{slideIndex}/")]
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
        [Route("StressFeedback/BandData/{sessionId}/{dateTime}/{hearbeat}/{temperature:decimal}/")]
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
                var sumHeartBeat = 0;
                var sumTemperature = 0d;
                while (indexBandData < bandData.Count && bandData[indexBandData].DateTime<pointData.DateTime)
                {
                    count++;
                    var dd = bandData[indexBandData];
                    item.HeartBeatMinimum = dd.HeartBeat < item.HeartBeatMinimum ? dd.HeartBeat : item.HeartBeatMinimum;
                    item.HeartBeatMaximum = dd.HeartBeat > item.HeartBeatMaximum ? dd.HeartBeat : item.HeartBeatMaximum;
                    sumHeartBeat += dd.HeartBeat;
                    sumTemperature += dd.Temperature;
                    indexBandData++;
                }
                item.HeartBeatAverage = sumHeartBeat/count;
                item.Temperature = sumTemperature/count;
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
