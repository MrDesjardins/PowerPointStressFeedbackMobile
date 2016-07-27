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
            this.Application.SlideShowNextClick += this.OnSlideShowNextClick;
            this.Application.SlideShowOnNext += this.OnSlideShowOnNext;
            this.Application.SlideShowNextSlide += this.OnSlideShowNextSlide;
            this.Application.SlideShowOnPrevious += this.OnSlideShowOnPrevious;
  
        }

        private void OnSlideShowOnNext(PowerPoint.SlideShowWindow wn)
        {
            System.Diagnostics.Debug.WriteLine("OnSlideShowOnNext :" + DateTime.Now + ":" + wn.View.Slide.SlideNumber);
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

        private void OnSlideShowOnPrevious(PowerPoint.SlideShowWindow wn)
        {
            System.Diagnostics.Debug.WriteLine("OnSlidePrevious :" + DateTime.Now + ":" + wn.View.Slide.SlideNumber);
        }
        /// <summary>
        /// This is invoked what ever if we move back or forth
        /// </summary>
        /// <param name="wn"></param>
        private void OnSlideShowNextClick(PowerPoint.SlideShowWindow wn, PowerPoint.Effect neffect)
        {
            int slideNumber = wn.View.Slide.SlideNumber;
            System.Diagnostics.Debug.WriteLine("OnSlideShowNextClick :" + DateTime.Now + ":" + slideNumber);
            SendData(slideNumber);
        }


        private void OnSlideShowNextSlide(PowerPoint.SlideShowWindow wn)
        {
            System.Diagnostics.Debug.WriteLine("OnSlideNext :" + DateTime.Now + ":" + wn.View.Slide.SlideNumber);

        }


        private void OnSlideShowEnd(PowerPoint.Presentation pres)
        {
            System.Diagnostics.Debug.WriteLine("OnSlideShowEnd :" + DateTime.Now);
            SendData(-1);
        }



        private void OnSlideShowBegin(PowerPoint.SlideShowWindow wn)
        {
            System.Diagnostics.Debug.WriteLine("OnSlideShowBegin :" + DateTime.Now + ":" + wn.View.Slide.SlideNumber);
            //SendData(wn.View.Slide.SlideNumber);
        }


        private async void SendData(int slideNumber)
        {
            if (this.isAddInRunning)
            {
                System.Diagnostics.Debug.WriteLine("SendData :" + DateTime.Now + ":" + slideNumber);
                using (var client = new HttpClient())
                {
                    var dateTime = DateTime.Now.ToString("yyyyDMMDdd_HHTmmTss"); //Special format see Web controller
                    var url = $"{this.url}StressFeedback/PowerPointData/{this.sessionId}/{dateTime}/{slideNumber}/";
                    await client.PostAsync(url, null);
                }
            }
        }


        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new PowerPointStressFeedbackRibbon(this.IsAddInRunningMethod, this.SessionIdChange, this.UrlChange);
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
