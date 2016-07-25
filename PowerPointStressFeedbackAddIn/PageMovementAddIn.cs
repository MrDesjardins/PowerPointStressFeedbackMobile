using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;

namespace PowerPointStressFeedbackAddIn
{
    public partial class PageMovementAddIn
    {
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


        private void SendData(int slideNumber)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now + ":" + slideNumber);
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
