using System;
using Android.OS;
using Android.Support.V7.Preferences;
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using ZXing.Mobile;
using Fragment = Android.Support.V4.App.Fragment;

namespace TransmissionRemote.Droid.Preferences
{
    public class ServerPreferencesFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.fragment_server_preferences, container, false);
            Activity.SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.server_preferences_list_content_frame, new ServerPreferencesListFragment())
                .Commit();

            var scanQrcodeFab = view.FindViewById<FloatingActionButton>(Resource.Id.server_preferences_fab_qrcode_scan);
            scanQrcodeFab.Click -= onScanQrcodeFabClick;
            scanQrcodeFab.Click += onScanQrcodeFabClick;

            return view;
        }

        private async void onScanQrcodeFabClick(object sender, EventArgs e)
        {
            MobileBarcodeScanner.Initialize(Activity.Application);
            var scanner = new MobileBarcodeScanner
            {
                TopText = Resources.GetString(Resource.String.qrcode_scanner_top_text),
                BottomText = Resources.GetString(Resource.String.qrcode_scanner_bottom_text)
            };

            var result = await scanner.Scan();
            if (result == null)
            {
                Toast.MakeText(Activity, Resources.GetString(Resource.String.qrcode_scanner_nothing_scanned_message), ToastLength.Short).Show();
                return;
            }

            handleScanResult(result.Text);
        }

        private void showInvalidQrcodeMessage()
        {
            Toast.MakeText(Activity, Resources.GetString(Resource.String.qrcode_scanner_invalid_qrcode_message), ToastLength.Short).Show();
        }

        private void handleScanResult(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                showInvalidQrcodeMessage();
                return;
            }

            string serverAddress;
            string serverPort;
            string additionalAddressString;

            try
            {
                var uri = new Uri(result);

                if (string.IsNullOrEmpty(uri.Host))
                {
                    showInvalidQrcodeMessage();
                    return;
                }

                serverAddress = uri.Scheme + Uri.SchemeDelimiter + uri.Host;
                serverPort = uri.IsDefaultPort || uri.Port == -1 ? string.Empty : uri.Port.ToString();
                additionalAddressString = uri.PathAndQuery;
            }
            catch (Exception ex)
            {
                if (ex is UriFormatException || ex is InvalidOperationException)
                {
                    showInvalidQrcodeMessage();
                    return;
                }

                throw;
            }

            var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Activity.ApplicationContext);
            var prefsEditor = sharedPreferences.Edit();
            prefsEditor.PutString(Resources.GetString(Resource.String.preference_key_server_address), serverAddress);
            prefsEditor.PutString(Resources.GetString(Resource.String.preference_key_server_port), serverPort);
            prefsEditor.PutString(Resources.GetString(Resource.String.preference_key_server_additional_address), additionalAddressString);
            prefsEditor.Commit();
        }
    }
}

