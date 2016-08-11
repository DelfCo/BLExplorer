using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Devices.Bluetooth.Advertisement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

//
// code in here that looks pasted from a sample has been posted from the BluetoothAdvertisement sample in Windows-universal-samples on GitHub.
//
namespace BLExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // The Bluetooth LE advertisement watcher class is used to control and customize Bluetooth LE scanning. 
        private BluetoothLEAdvertisementWatcher watcher;
        private List<BLEAdInfo> adsReceived;



        internal List<BLEAdInfo> AdsReceived
        {
            get
            {
                return adsReceived;
            }

            set
            {
                adsReceived = value;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            AdsReceived = new List<BLEAdInfo>();

            InitBLEWatching();
        }

        private void btAdapters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // fill in information about the selected BT adapter here.
            
            // Dump the current selection to outBlock
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            watcher.Received += OnAdvertisementReceived;
            watcher.Stopped += OnAdvertisementWatcherStopped;

            // Attach handlers for suspension to stop the watcher when the App is suspended. 
            App.Current.Suspending += App_Suspending;
            App.Current.Resuming += App_Resuming;

        }

        /// <summary> 
        /// Invoked immediately before the Page is unloaded and is no longer the current source of a parent Frame. 
        /// </summary> 
        /// <param name="e"> 
        /// Event data that can be examined by overriding code. The event data is representative 
        /// of the navigation that will unload the current Page unless canceled. The 
        /// navigation can potentially be canceled by setting Cancel. 
        /// </param> 
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // Remove local suspension handlers from the App since this page is no longer active. 
            App.Current.Suspending -= App_Suspending;
            App.Current.Resuming -= App_Resuming;

            // Make sure to stop the watcher when leaving the context. Even if the watcher is not stopped, 
            // scanning will be stopped automatically if the watcher is destroyed. 
            watcher.Stop();
            // Always unregister the handlers to release the resources to prevent leaks. 
            watcher.Received -= OnAdvertisementReceived;
            watcher.Stopped -= OnAdvertisementWatcherStopped;

            base.OnNavigatingFrom(e);
        }


        private void InitBLEWatching()
        {
            watcher = new BluetoothLEAdvertisementWatcher();

            // we listen for all advertisers, so we don't create any filters on the watcher.

        }

        // triggered when a BLE advertisement is received.
        // store this so I can list it in the UI.
        private async void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            // BUGBUG: this is too simple. We should only keep the latest advertisement from each device, so we need to replace any existing list entry 
            // that matches the eventArgs.BluetoothAddress and has an older eventArgs.Timestamp
            //adsReceived.Add(eventArgs);

            BLEAdInfo newAd = new BLEAdInfo(eventArgs);
            // new, improved code:
            if (!AdsReceived.Contains(newAd))
            {
                // new item. Add it.
                AdsReceived.Add(newAd);
            }
            else
            {
                // it's a duplicate of a known ad.
                // replace the old ad with the latest ad
                // and trigger UI update

            }
        }

        // triggered when a BLE watcher is stopped.
        // Clean up cached advertiser data, it's no longer useful.
        private async void OnAdvertisementWatcherStopped(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementWatcherStoppedEventArgs eventArgs)
        {
            AdsReceived.Clear();
        }

        /// <summary> 
        /// Invoked when application execution is being suspended.  Application state is saved 
        /// without knowing whether the application will be terminated or resumed with the contents 
        /// of memory still intact. 
        /// </summary> 
        /// <param name="sender">The source of the suspend request.</param> 
        /// <param name="e">Details about the suspend request.</param> 
        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            // Make sure to stop the watcher on suspend. 
            watcher.Stop();
            // Always unregister the handlers to release the resources to prevent leaks. 
            watcher.Received -= OnAdvertisementReceived;
            watcher.Stopped -= OnAdvertisementWatcherStopped;

        }


        /// <summary> 
        /// Invoked when application execution is being resumed. 
        /// </summary> 
        /// <param name="sender">The source of the resume request.</param> 
        /// <param name="e"></param> 
        private void App_Resuming(object sender, object e)
        {
            watcher.Received += OnAdvertisementReceived;
            watcher.Stopped += OnAdvertisementWatcherStopped;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            watcher.Start();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debugger.Break();

        }
    }
}
