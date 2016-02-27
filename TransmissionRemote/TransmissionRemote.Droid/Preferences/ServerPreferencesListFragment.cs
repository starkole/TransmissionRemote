using Android.Content;
using Android.OS;
using Android.Support.V7.Preferences;


namespace TransmissionRemote.Droid.Preferences
{
    public class ServerPreferencesListFragment : PreferenceFragmentCompat, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        #region Declarations

        ISharedPreferences sharedPreferences; 
        
        #endregion

        #region Overrides

        public override void OnCreatePreferences(Bundle p0, string p1)
        {            
            sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Activity.ApplicationContext);
        }

        public override void OnResume()
        {
            base.OnResume();            
            initializePreferences();
            PreferenceScreen.SharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
        }

        public override void OnPause()
        {
            base.OnPause();
            PreferenceScreen.SharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
            PreferenceScreen.RemoveAll();
        } 

        #endregion

        #region ISharedPreferencesOnSharedPreferenceChangeListener Implementation

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            var preference = FindPreference(key) as EditTextPreference;
            if (preference != null)
            {
                var currentValue = sharedPreferences.GetString(key, string.Empty);
                preference.Summary = string.IsNullOrEmpty(currentValue) ? Resources.GetString(Resource.String.preference_summary_default_value) : currentValue;
            }
        }

        #endregion

        #region Private Methods

        private void initializePreferences()
        {
            AddPreferencesFromResource(Resource.Layout.fragment_server_preferences_list);
            Preference generateQrCodePrefrence = FindPreference(Resources.GetString(Resource.String.preference_key_server_qrcode_summary));
            generateQrCodePrefrence.PreferenceClick -= onGenerateQrCodePrefrenceClick;
            generateQrCodePrefrence.PreferenceClick += onGenerateQrCodePrefrenceClick;
            refreshPreferences();
        }

        private void onGenerateQrCodePrefrenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            var settingsSummary = buildSettingsSummaryString();
            if (string.IsNullOrEmpty(settingsSummary))
            {
                return;
            }

            var qrcodeGeneratorActivity = new Intent(Activity.ApplicationContext, typeof(QrcodeGeneratorActivity));
            qrcodeGeneratorActivity.PutExtra(AndroidConstants.QrcodeStringKey, settingsSummary);
            StartActivity(qrcodeGeneratorActivity);
        }

        private void refreshPreferences()
        {
            for (int i = 0; i < PreferenceScreen.PreferenceCount; i++)
            {
                OnSharedPreferenceChanged(sharedPreferences, PreferenceScreen.GetPreference(i).Key);
            }
        }

        private string buildSettingsSummaryString()
        {
            var address = sharedPreferences.GetString(Resources.GetString(Resource.String.preference_key_server_address), string.Empty);
            if (string.IsNullOrEmpty(address))
            {
                return string.Empty;
            }

            var port = sharedPreferences.GetString(Resources.GetString(Resource.String.preference_key_server_port), string.Empty);
            var additionalAddress = sharedPreferences.GetString(Resources.GetString(Resource.String.preference_key_server_additional_address), string.Empty);
            var result = string.IsNullOrEmpty(port) ? address : address + ":" + port;
            result = string.IsNullOrEmpty(additionalAddress) ? result : result + "/" + additionalAddress;
            return result;
        } 

        #endregion
    }
}

