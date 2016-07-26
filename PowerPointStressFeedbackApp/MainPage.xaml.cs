using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        }

        private void SkinTemperatureOnReadingChanged(object sender, BandSensorReadingEventArgs<IBandSkinTemperatureReading> e)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   this.txtLog2.Text = DateTime.Now + " Data = " + e.SensorReading.Temperature + "\n" + this.txtLog2.Text;
               });
        }

        private void HeartRate_ReadingChanged(object sender, Microsoft.Band.Sensors.BandSensorReadingEventArgs<Microsoft.Band.Sensors.IBandHeartRateReading> e)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    this.txtLog1.Text = DateTime.Now + " Data = " + e.SensorReading.HeartRate + "\n" + this.txtLog1.Text;
                });
        }

        private async void btnStop_Click(object sender, RoutedEventArgs e)
        {
            this.txtSessionId.IsEnabled = true;
            this.btnStart.IsEnabled = true;
            this.btnStop.IsEnabled = false;

            await this.bandClient.SensorManager.HeartRate.StopReadingsAsync();
            await this.bandClient.SensorManager.SkinTemperature.StopReadingsAsync();
            this.bandClient.Dispose();
            this.bandClient = null;
            this.bandClientInfo = null;
        }
    }
}
