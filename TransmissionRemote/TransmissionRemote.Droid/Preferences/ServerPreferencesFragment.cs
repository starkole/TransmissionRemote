using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Preferences;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using com.refractored.fab;
using ZXing;
using ZXing.Mobile;


namespace TransmissionRemote.Droid.Preferences
{
	public class ServerPreferencesFragment : PreferenceFragmentCompat, ISharedPreferencesOnSharedPreferenceChangeListener
	{
		ISharedPreferences sharedPreferences;

		private void refreshPreferences() {
			for (int i=0; i < this.PreferenceScreen.PreferenceCount; i++)
			{
				this.OnSharedPreferenceChanged (sharedPreferences, this.PreferenceScreen.GetPreference (i).Key);
			}
		}

		public override void OnCreatePreferences (Bundle savedInstance, string key) 
		{				
			AddPreferencesFromResource (Resource.Layout.server_preferences_fragment);

			sharedPreferences = PreferenceManager.GetDefaultSharedPreferences (this.Activity.ApplicationContext);

			Preference generateQrCodePrefrence = this.FindPreference ("generated_qr_code");
            generateQrCodePrefrence.PreferenceClick += (object sender, Preference.PreferenceClickEventArgs e) => 
                {
                    var settingsSummary = buildSettingsSummaryString();
                    if (string.IsNullOrEmpty(settingsSummary)) 
                    {
                        return;
                    }

                    var intent = new Intent (this.Activity.ApplicationContext, typeof(QrCodeImageView));
                    intent.PutExtra("foo", "bar");
                    StartActivity(intent);
                };
            

			refreshPreferences ();
		}

		public override void OnResume()
		{
			base.OnResume();
			this.PreferenceScreen.SharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
			refreshPreferences ();
		}

		public override void OnPause()
		{			
			base.OnPause();
			this.PreferenceScreen.SharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
		}

		public void OnSharedPreferenceChanged (ISharedPreferences sharedPreferences, string key)
		{
			var preference = this.FindPreference(key) as EditTextPreference;
			if (preference != null) {
				preference.Summary = sharedPreferences.GetString(key, preference.Summary);
			}
		}

        private string buildSettingsSummaryString()
        {
            var sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this.ApplicationContext);
            var address = sharedPreferences.GetString(Resources.GetString(Resource.String.preference_key_server_address), string.Empty);
            if (string.IsNullOrEmpty(address))
            {
                return;
            }
            var port = sharedPreferences.GetString("server_port", string.Empty);
            var qrCodeString = string.IsNullOrEmpty(port) ? address : address + ":" + port;
        }
	}
}

