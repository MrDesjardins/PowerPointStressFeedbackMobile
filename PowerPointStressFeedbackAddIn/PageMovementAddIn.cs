using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;

namespace PowerPointStressFeedbackAddIn
{
    public partial class PageMovementAddIn
    {
        private bool isAddInRunning = false;
        private string sessionId;
        private string url;

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }


        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

            this.Application.SlideShowBegin += OnSlideShowBegin;
            this.Application.SlideShowEnd += OnSlideShowEnd;
            this.Application.SlideShowOnNext += OnSlideNext;
            this.Application.SlideShowOnPrevious += OnSlidePrevious;
  
        }

        private void IsAddInRunningMethod(bool isRunning)
        {
            this.isAddInRunning = isRunning;
        }

        private void SessionIdChange(string sessionId)
        {
            this.sessionId = sessionId;
        }

        private void UrlChange(string url)
        {
            this.url = url;
        }

        private void OnSlidePrevious(PowerPoint.SlideShowWindow wn)
        {
            SendData(wn.View.Slide.SlideNumber);
        }

        private void OnSlideNext(PowerPoint.SlideShowWindow wn)
        {
            SendData(wn.View.Slide.SlideNumber);
        }


        private void OnSlideShowEnd(PowerPoint.Presentation pres)
        {
            SendData(-1);
        }



        private void OnSlideShowBegin(PowerPoint.SlideShowWindow wn)
        {
            SendData(wn.View.Slide.SlideNumber);
        }


        private async void SendData(int slideNumber)
        {
            if (this.isAddInRunning)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now + ":" + slideNumber);

                using (var client = new HttpClient())
                {
                    //var values = new Dictionary<string, string>
                    //{
                    //   { "thing1", "hello" },
                    //   { "thing2", "world" }
                    //};

                    //var content = new FormUrlEncodedContent(values);
                    var dateTime = DateTime.Now.ToString("yyyyDMMDdd_HHTmmTss"); //Special format see Web controller
                    var url = this.url + "StressFeedback/PowerPointData/"+this.sessionId+"/"+dateTime+"/"+ slideNumber;
                    var response = await client.PostAsync(url, null/*, content*/);
                    //var responseString = await response.Content.ReadAsStringAsync();
                }

            }
        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new PowerPointStressFeedbackRibbon(this.IsAddInRunningMethod, SessionIdChange, UrlChange);
        }



        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
