
using System;
using System.Collections.Generic;

using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace TransmissionRemote.Droid.Fragments
{
    public class FirstItemFragment : Fragment
    {
        private FragmentTabHost fragmentTabHost;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.first_item_click_fragment, container, false);
            this.fragmentTabHost = view.FindViewById<FragmentTabHost>(Resource.Id.tabhost);

            List<string> tabSpecs = new List<string>{ "first", "second", "third", "fourth" };
            List<int> icons = new List<int>
            { 
                Resource.Drawable.patient,
                Resource.Drawable.patient2,
                Resource.Drawable.patient3,
                Resource.Drawable.patient4 
            };
            
            List<Java.Lang.Class> types = new List<Java.Lang.Class>
            {
                Java.Lang.Class.FromType(typeof(SecondItemFragment)),
                Java.Lang.Class.FromType(typeof(ThirdItemFragment)),
                Java.Lang.Class.FromType(typeof(MainFragment)),
                Java.Lang.Class.FromType(typeof(ThirdItemFragment))
            };

            this.CreateTabs(tabSpecs, icons, types);

            return view;
        }

        private void CreateTabs(IList<string> tabSpecs, IList<int> icons, IList<Java.Lang.Class> types)
        {
            for (var i = 0; i < tabSpecs.Count; i++)
            {
                TabHost.TabSpec spec = this.fragmentTabHost.NewTabSpec(tabSpecs[i]);
                spec.SetIndicator(String.Empty, ContextCompat.GetDrawable(this.Context, icons[i]));

                var intent = new Intent(this.Context, typeof(SecondItemFragment));

                spec.SetContent(intent);

                this.fragmentTabHost.Setup(this.Activity, ChildFragmentManager, Resource.Id.realtabcontent);
                this.fragmentTabHost.AddTab(spec, types[i], null);
            }
        }
    }
}

