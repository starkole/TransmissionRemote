using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.Res;
using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;

using Toolbar = Android.Support.V7.Widget.Toolbar;
using TransmissionRemote.Droid.Fragments;
using Java.Lang;
//using Android.Support.Design.Widget;
using Android.Support.V4.View;
using TransmissionRemote.Models.Enums;
using TransmissionRemote.Droid.Preferences;
using com.refractored.fab;
using ZXing;
using ZXing.Mobile;
using Android.Support.V7.Preferences;

namespace TransmissionRemote.Droid
{
	[Activity (Label = "ServerPreferencesActivity")]			
	public class ServerPreferencesActivity : Fragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			base.OnCreateView(inflater, container, savedInstanceState);

			var view = inflater.Inflate(Resource.Layout.server_preferences_layout, container, false);
			var fragmentTransaction = this.Activity.SupportFragmentManager.BeginTransaction();
			fragmentTransaction.Replace(Resource.Id.server_preferences_content_frame, new ServerPreferencesFragment());
			fragmentTransaction.AddToBackStack(null);
			fragmentTransaction.Commit();

			var fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab_qrcode_scan);
			fab.Click += onPreferenceButtonClick;

			return view;
		}

		private async void onPreferenceButtonClick(object sender, EventArgs e)
		{
			MobileBarcodeScanner.Initialize (this.Activity.Application);
			var scanner = new MobileBarcodeScanner();
			//We can customize the top and bottom text of the default overlay
			scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
			scanner.BottomText = "Wait for the barcode to automatically scan!";

			//Start scanning
			var result = await scanner.Scan();
			if (result != null) {
				handleScanResult (result.Text);
			} else {
				Toast.MakeText (this.Activity, "No barcode was scanned.", ToastLength.Short).Show ();
			}

		}


		private void handleScanResult(string result)
		{
			if (string.IsNullOrEmpty(result))
			{
				Toast.MakeText(this.Activity, "Empty barcode was scanned.", ToastLength.Short).Show();
			}

			var splittedBySlashes = result.Split ('/');
			var splittedByColon = splittedBySlashes [2].Split (':');
			var address = splittedBySlashes[0] + "//" + splittedByColon[0];
			var port = splittedByColon [1];

			ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this.Activity.ApplicationContext);
			var prefsEditor = sharedPreferences.Edit ();
			prefsEditor.PutString ("server_addres", address);
			prefsEditor.PutString ("server_port", port);
			prefsEditor.Commit();
		}
	}
}

