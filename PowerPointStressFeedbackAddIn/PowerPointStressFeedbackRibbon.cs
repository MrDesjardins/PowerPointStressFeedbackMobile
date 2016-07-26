using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new PowerPointStressFeedbackRibbon();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace PowerPointStressFeedbackAddIn
{
    [ComVisible(true)]
    public class PowerPointStressFeedbackRibbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        private static Random random = new Random();
        private Action<bool> isAddInRunning;
        private Action<string> sessionIdChange;
        private Action<string> urlChange;

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public PowerPointStressFeedbackRibbon(Action<bool> isAddInRunning, Action<string> sessionIdChange, Action<string> urlChange)
        {
            this.isAddInRunning = isAddInRunning;
            this.sessionIdChange = sessionIdChange;
            this.urlChange = urlChange;
        }


        public void OnActionCallback(Office.IRibbonControl control, bool isPressed)
        {
            if (control.Id == "onOffButton")
            {
                this.isAddInRunning(isPressed);

            }

        }

        public string GetSessionEditBoxText(Office.IRibbonControl control)
        {
            var randomId = RandomString(8);
            this.sessionIdChange(randomId);
            return randomId;
        }

        public string GetUrlEditBoxText(Office.IRibbonControl control)
        {
            var defaultUrl = "http://localhost:36204/";
            this.urlChange(defaultUrl);
            return defaultUrl;
        }

        private void SessionIdOnChange(Office.IRibbonControl control, string data)
        {
            this.sessionIdChange(data);
        }
        private void UrlOnChange(Office.IRibbonControl control, string data)
        {
            this.urlChange(data);
        }
        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("PowerPointStressFeedbackAddIn.PowerPointStressFeedbackRibbon.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit http://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
