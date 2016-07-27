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
            for (int i = 0; i < 10; i++)
            {
                var minute = 9 + i;
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 00), HeartBeat = 60, Temperature = 37.4d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 01), HeartBeat = 61, Temperature = 37.4d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 02), HeartBeat = 62, Temperature = 37.5d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 03), HeartBeat = 61, Temperature = 37.5d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 04), HeartBeat = 62, Temperature = 37.5d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 05), HeartBeat = 62, Temperature = 37.5d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 06), HeartBeat = 62, Temperature = 37.6d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 07), HeartBeat = 63, Temperature = 37.6d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 08), HeartBeat = 63, Temperature = 37.6d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 09), HeartBeat = 64, Temperature = 37.6d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 10), HeartBeat = 64, Temperature = 37.7d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 11), HeartBeat = 65, Temperature = 37.7d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 12), HeartBeat = 65, Temperature = 37.7d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 13), HeartBeat = 67, Temperature = 37.7d, Gsr = 32600});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 14), HeartBeat = 67, Temperature = 37.7d, Gsr = 32600});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 15), HeartBeat = 66, Temperature = 37.8d, Gsr = 32600});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 16), HeartBeat = 65, Temperature = 37.8d, Gsr = 32600});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 17), HeartBeat = 65, Temperature = 37.8d, Gsr = 32600});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 18), HeartBeat = 64, Temperature = 37.8d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 19), HeartBeat = 66, Temperature = 37.9d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 20), HeartBeat = 67, Temperature = 37.9d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 21), HeartBeat = 70, Temperature = 37.9d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 22), HeartBeat = 70, Temperature = 37.8d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 23), HeartBeat = 72, Temperature = 37.8d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 24), HeartBeat = 72, Temperature = 37.9d, Gsr = 32800 });
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 25), HeartBeat = 78, Temperature = 37.12d, Gsr = 32800});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 26), HeartBeat = 78, Temperature = 37.12d, Gsr = 32800});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 27), HeartBeat = 78, Temperature = 37.12d, Gsr = 32800});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 28), HeartBeat = 79, Temperature = 37.12d, Gsr = 52800});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 29), HeartBeat = 81, Temperature = 37.25d, Gsr = 52800});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 30), HeartBeat = 80, Temperature = 37.30d, Gsr = 52000 });
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 32), HeartBeat = 78, Temperature = 37.4d, Gsr = 38000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 34), HeartBeat = 76, Temperature = 37.4d, Gsr = 38000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 36), HeartBeat = 75, Temperature = 37.5d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 38), HeartBeat = 74, Temperature = 37.5d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 40), HeartBeat = 74, Temperature = 37.5d, Gsr = 32500});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 41), HeartBeat = 73, Temperature = 37.5d, Gsr = 32500});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 42), HeartBeat = 73, Temperature = 37.6d, Gsr = 32500});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 44), HeartBeat = 73, Temperature = 37.6d, Gsr = 32500});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 46), HeartBeat = 73, Temperature = 37.6d, Gsr = 32500});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 48), HeartBeat = 72, Temperature = 37.6d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 50), HeartBeat = 72, Temperature = 37.7d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 52), HeartBeat = 71, Temperature = 37.7d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 54), HeartBeat = 71, Temperature = 37.6d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 56), HeartBeat = 73, Temperature = 37.6d, Gsr = 32000});
                testBandData.Add(new BandData {SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, minute, 58), HeartBeat = 71, Temperature = 37.5d, Gsr = 32000 });
            }

            var testPowerpointData = new List<PowerPointData>();
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 10), SlideIndex = 1 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 09, 30), SlideIndex = 2 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 10, 05), SlideIndex = 3 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 10, 55), SlideIndex = 4 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 11, 05), SlideIndex = 5 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 11, 15), SlideIndex = 6 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 12, 00), SlideIndex = 7 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 12, 20), SlideIndex = 8 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 12, 30), SlideIndex = 9 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 12, 50), SlideIndex = 10 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 13, 00), SlideIndex = 11 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 13, 05), SlideIndex = 12 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 14, 12), SlideIndex = 13 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 15, 06), SlideIndex = 14 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 15, 24), SlideIndex = 15 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 16, 44), SlideIndex = 16 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 17, 00), SlideIndex = 17 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 17, 20), SlideIndex = 18 });
            testPowerpointData.Add(new PowerPointData { SessionId = fakeSession, DateTime = new DateTime(2016, 07, 26, 11, 17, 58), SlideIndex = 19 });
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
        [Route("StressFeedback/BandData/{sessionId}/{dateTime}/{hearbeat}/{temperature:decimal}/{gsr}")]
        public ActionResult BandData(string sessionId, string dateTime, int hearbeat, double temperature, int gsr)
        {
            var dateTime1 = tranformStringToDate(dateTime);
            if (!bandData.ContainsKey(sessionId) || bandData[sessionId] == null)
            {
                bandData[sessionId] = new List<Controllers.BandData>();
            }
            bandData[sessionId].Add(new BandData { SessionId = sessionId, DateTime = dateTime1, HeartBeat = hearbeat, Temperature = temperature, Gsr = gsr});

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
            var powerPointStartDateTime = powerPointData.OrderBy(p=>p.DateTime).First().DateTime;
            mergedData.BeforePresentationHeartBeat = Convert.ToInt32(bandData.Where(band=>band.DateTime < powerPointStartDateTime).Average(band => band.HeartBeat));
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
                if (powerpointIndex+1 >= powerPointData.Count)
                {
                    item.SecondsOnSlide = 0;
                }
                else
                {
                    item.SecondsOnSlide = (powerPointData[powerpointIndex + 1].DateTime - pointData.DateTime).Seconds;
                }
                item.Time = pointData.DateTime;
                var count = 0;
                var sumHeartBeat = 0;
                var sumTemperature = 0d;
                var sumGsr = 0;
                while (indexBandData < bandData.Count && bandData[indexBandData].DateTime<pointData.DateTime)
                {
                    count++;
                    var dd = bandData[indexBandData];
                    item.HeartBeatMinimum = dd.HeartBeat < item.HeartBeatMinimum ? dd.HeartBeat : item.HeartBeatMinimum;
                    item.HeartBeatMaximum = dd.HeartBeat > item.HeartBeatMaximum ? dd.HeartBeat : item.HeartBeatMaximum;
                    sumHeartBeat += dd.HeartBeat;
                    sumTemperature += dd.Temperature;
                    sumGsr += dd.Gsr;
                    indexBandData++;
                }
                if (count != 0) // No Band data for the Slide (can occur if change really fast)
                {
                    item.HeartBeatAverage = sumHeartBeat/count;
                    item.Temperature = sumTemperature/count;
                    item.GsrAverage = Convert.ToInt32(sumGsr/count);
                }
                else
                {
                    //Take previous data if we are not the first slide
                    if (powerpointIndex>0)
                    {
                        item.Temperature = mergedData.Data[mergedData.Data.Count - 1].Temperature;
                        item.HeartBeatAverage = mergedData.Data[mergedData.Data.Count - 1].HeartBeatAverage;
                        item.HeartBeatMinimum = mergedData.Data[mergedData.Data.Count - 1].HeartBeatMinimum;
                        item.HeartBeatMaximum = mergedData.Data[mergedData.Data.Count - 1].HeartBeatMaximum;
                        item.GsrAverage = mergedData.Data[mergedData.Data.Count - 1].GsrAverage;
                    }
                }
                mergedData.Data.Add(item);              
            }
            mergedData.HeartBeatAverage = Convert.ToInt32(bandData.Average(d => d.HeartBeat));
            mergedData.HeartBeatMaximum = Convert.ToInt32(bandData.Max(d => d.HeartBeat));
            mergedData.HeartBeatMinimum = Convert.ToInt32(bandData.Min(d => d.HeartBeat));
            mergedData.TemperatureAverage = bandData.Average(d => d.Temperature);
            mergedData.TemperatureMaximum = bandData.Max(d => d.Temperature);
            mergedData.TemperatureMinimum = bandData.Min(d => d.Temperature);
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

        public int Gsr { get; set; }
    }

    public class MergedData
    {
        public string SessionId { get; set; }

        public List<MergedDataItem> Data{get;set;}

        public int HeartBeatMinimum { get; set; }
        public int HeartBeatMaximum { get; set; }
        public int HeartBeatAverage { get; set; }
        public double TemperatureMinimum { get; set; }
        public double TemperatureMaximum { get; set; }
        public double TemperatureAverage { get; set; }

        public int BeforePresentationHeartBeat { get; set; }

        public MergedData()
        {
        }
    }

    public class MergedDataItem
    {
        public DateTime Time { get; set; }
        public int SlideIndex { get; set; }

        public int HeartBeatMinimum { get; set; }
        public int HeartBeatMaximum { get; set; }
        public int HeartBeatAverage { get; set; }

        public double Temperature { get; set; }

        public int GsrAverage { get; set; }

        public int SecondsOnSlide { get; set; }

        public MergedDataItem()
        {
            this.HeartBeatMinimum = 100;
            this.HeartBeatAverage = 0;
            this.HeartBeatMaximum = 0;
            this.Temperature = 0;
            this.GsrAverage = 0;
        }
    }
}
