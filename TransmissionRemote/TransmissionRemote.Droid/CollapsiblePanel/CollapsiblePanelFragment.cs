using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.Design.Widget;

using Fragment = Android.Support.V4.App.Fragment;

namespace TransmissionRemote.Droid.CollapsiblePanel
{
    public class CollapsiblePanelFragment: Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_collapsible_panel, container, false);

            var collapsingToolbarLayout = view.FindViewById<CollapsingToolbarLayout>(Resource.Id.cp_collapsing);
            collapsingToolbarLayout.SetTitle(GetString(Resource.String.app_name));

            return view;
        }
    }
}