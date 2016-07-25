using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Sensors;

namespace PowerPointStressFeedbackMobile
{
    [Activity(Label = "Power Point Stress Feedback", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private BandClient bandClient;
        private int count = 1;
        private BandHeartRateSensor heartRate;
        private EditText sessionId;
        private Button startCaptureButton;
        private Button stopCaptureButton;
        private EditText txtLog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            this.SetContentView(Resource.Layout.Main);

            // Get our UI controls from the loaded layout:

            this.startCaptureButton = this.FindViewById<Button>(Resource.Id.StartCapture);
            this.stopCaptureButton = this.FindViewById<Button>(Resource.Id.StopCapturing);
            this.txtLog = this.FindViewById<EditText>(Resource.Id.TxtLog);


            this.startCaptureButton.Click += this.OnStartCaptureButtonOnClick;
            this.stopCaptureButton.Click += this.OnStopCaptureButtonClick;

            this.ConnectToBand();
            //Wait to have a connection to Band
            this.startCaptureButton.Enabled = false;
            this.stopCaptureButton.Enabled = false;
        }


        private async void ConnectToBand()
        {
            var bandClientManager = BandClientManager.Instance;
            // query the service for paired devices
            var pairedBands = await bandClientManager.GetPairedBandsAsync();
            // connect to the first device
            var bandInfo = pairedBands.FirstOrDefault();
            if (bandInfo != null)
            {
                this.bandClient = await bandClientManager.ConnectAsync(bandInfo);
                this.startCaptureButton.Enabled = true;
                this.stopCaptureButton.Enabled = true;
            }
            else
            {
                this.txtLog.Append("Cannot find paired Band");
            }
        }


        private async void OnStartCaptureButtonOnClick(object sender, EventArgs e)
        {
            this.sessionId = this.FindViewById<EditText>(Resource.Id.SessionIdText);
            this.sessionId.Enabled = false;
            this.startCaptureButton.Enabled = false;
            this.stopCaptureButton.Enabled = true;
            var sensorManager = this.bandClient.SensorManager;
            // get the heart rate sensor
            this.heartRate = sensorManager.HeartRate;
            // add a handler
            this.heartRate.ReadingChanged += OnHeartRateOnReadingChanged            ;
            if (this.heartRate.UserConsented == UserConsent.Unspecified)
            {
                var granted = await this.heartRate.RequestUserConsent();
            }
            if (this.heartRate.UserConsented == UserConsent.Granted)
            {
                // start reading, with the interval
                await this.heartRate.StartReadingsAsync(BandSensorSampleRate.Ms16);
            }
        }

        private void OnHeartRateOnReadingChanged(object o, BandSensorReadingEventArgs<BandHeartRateReading> args)
        {
            var quality = args.SensorReading.Quality;
            var heartRate = args.SensorReading.HeartRate;
            this.txtLog.Append(DateTime.Now + ": Heart rate: " + heartRate + "\n");
        }

        private async void OnStopCaptureButtonClick(object sender, EventArgs e)
        {
            this.sessionId.Enabled = true;
            this.startCaptureButton.Enabled = true;
            this.stopCaptureButton.Enabled = false;
            // stop reading
            await this.heartRate.StopReadingsAsync();
        }
    }
}