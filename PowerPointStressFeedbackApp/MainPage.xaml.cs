using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Band;
using Microsoft.Band.Sensors;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PowerPointStressFeedbackApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IBandInfo bandClientInfo;
        private IBandClient bandClient;

        public MainPage()
        {
            this.InitializeComponent();
     
        }

        public async Task InitAsync()
        {
            try
            {
                if (this.bandClientInfo == null)
                {
                    await this.FindDevicesAsync();
                }
                if (this.bandClientInfo != null)
                {
                    this.bandClient = await BandClientManager.Instance.ConnectAsync(this.bandClientInfo);
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(x.Message);
            }
        }

        public async Task FindDevicesAsync()
        {
            var bands = await BandClientManager.Instance.GetBandsAsync();
            if (bands != null && bands.Length > 0)
            {
                this.bandClientInfo = bands[0]; // take the first band
            }
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.txtSessionId.IsEnabled = false;
            this.txtUrl.IsEnabled = false;
            this.btnStart.IsEnabled = false;
            this.btnStop.IsEnabled = true;
            await this.InitAsync();
            var sensorManager = this.bandClient.SensorManager;

            //Hearbeat
            sensorManager.HeartRate.ReadingChanged += this.HeartRate_ReadingChanged;
            if (sensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
            {
                await sensorManager.HeartRate.RequestUserConsentAsync();
            }
            if (sensorManager.HeartRate.GetCurrentUserConsent() == UserConsent.Granted)
            {
                await sensorManager.HeartRate.StartReadingsAsync(new CancellationToken());
            }

            //Temperature
            sensorManager.SkinTemperature.ReadingChanged += this.SkinTemperatureOnReadingChanged;
            if (sensorManager.SkinTemperature.GetCurrentUserConsent() != UserConsent.Granted)
            {
                await sensorManager.SkinTemperature.RequestUserConsentAsync();
            }
            if (sensorManager.SkinTemperature.GetCurrentUserConsent() == UserConsent.Granted)
            {
                await sensorManager.SkinTemperature.StartReadingsAsync(new CancellationToken());
            }

            //GSR (Galvanic Skin Response)
            sensorManager.Gsr.ReadingChanged += this.GsrOnReadingChanged;
            if (sensorManager.Gsr.GetCurrentUserConsent() != UserConsent.Granted)
            {
                await sensorManager.Gsr.RequestUserConsentAsync();
            }
            if (sensorManager.Gsr.GetCurrentUserConsent() == UserConsent.Granted)
            {
                await sensorManager.Gsr.StartReadingsAsync(new CancellationToken());
            }
        }

 
        private int lastHeartBeat = 0;
        private double lastTemperature=0;
        private int lastGsr = 0;

        private void SkinTemperatureOnReadingChanged(object sender, BandSensorReadingEventArgs<IBandSkinTemperatureReading> e)
        {
            if (e.SensorReading.Temperature > 0)
            {
                this.lastTemperature = e.SensorReading.Temperature;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {

                        this.txtLog2.Text = DateTime.Now + " Data = " + this.lastTemperature + "\n" + this.txtLog2.Text;
                        this.SendToServer(this.lastHeartBeat, this.lastTemperature, this.lastGsr);
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private void HeartRate_ReadingChanged(object sender, Microsoft.Band.Sensors.BandSensorReadingEventArgs<Microsoft.Band.Sensors.IBandHeartRateReading> e)
        {
            if (e.SensorReading.HeartRate > 0)
            {
                this.lastHeartBeat = e.SensorReading.HeartRate;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.txtLog1.Text = DateTime.Now + " Data = " + this.lastHeartBeat + "\n" + this.txtLog1.Text;
                        this.SendToServer(this.lastHeartBeat, this.lastTemperature, this.lastGsr);
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

        }

        private void GsrOnReadingChanged(object sender, BandSensorReadingEventArgs<IBandGsrReading> e)
        {
            if (e.SensorReading.Resistance > 0)
            {
                this.lastGsr = e.SensorReading.Resistance;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {

                        this.txtLog3.Text = DateTime.Now + " Data = " + this.lastGsr + "\n" + this.txtLog3.Text;
                        this.SendToServer(this.lastHeartBeat, this.lastTemperature, this.lastGsr);
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        /// <summary>
        /// Make sure we have at least 1 heartbeat, temperature and gsr before sending
        /// </summary>
        /// <returns></returns>
        private bool HasMinimumAllData()
        {
            return this.lastHeartBeat != 0 && this.lastGsr != 0 && this.lastTemperature != 0;
        }


        private async void SendToServer(int hearbeat, double temperature, int gsr)
        {
            if (this.HasMinimumAllData())
            {
                using (var client = new HttpClient())
                {
                    var dateTime = DateTime.Now.ToString("yyyyDMMDdd_HHTmmTss"); //Special format see Web controller
                    var sessionId = this.txtSessionId.Text;
                    var url = $"{this.txtUrl.Text}StressFeedback/BandData/{sessionId}/{dateTime}/{hearbeat}/{temperature}/{gsr}";
                    await client.PostAsync(url, null);
                }
            }
        }

        private async void btnStop_Click(object sender, RoutedEventArgs e)
        {
            this.txtSessionId.IsEnabled = true;
            this.txtUrl.IsEnabled = true;
            this.btnStart.IsEnabled = true;
            this.btnStop.IsEnabled = false;

            await this.bandClient.SensorManager.HeartRate.StopReadingsAsync();
            await this.bandClient.SensorManager.SkinTemperature.StopReadingsAsync();
            await this.bandClient.SensorManager.Gsr.StopReadingsAsync();
            this.bandClient.Dispose();
            this.bandClient = null;
            this.bandClientInfo = null;
        }
    }
}
