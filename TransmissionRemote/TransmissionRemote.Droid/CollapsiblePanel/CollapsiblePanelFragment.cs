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
using Android.Widget;

namespace TransmissionRemote.Droid.CollapsiblePanel
{
    public class CollapsiblePanelFragment: Fragment
    {
        LinearLayout toolbarBottomPart;
        AppBarLayout appBar;
        int initialHeight;
        int smallHeight;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_collapsible_panel, container, false);

            var collapsingToolbarLayout = view.FindViewById<CollapsingToolbarLayout>(Resource.Id.cp_collapsible_toolbar);
            collapsingToolbarLayout.Click += onCollapsingToolbarLayoutClick;

            toolbarBottomPart = view.FindViewById<LinearLayout>(Resource.Id.ct_large_part);
            appBar = view.FindViewById<AppBarLayout>(Resource.Id.cp_appbar);
            initialHeight = appBar.LayoutParameters.Height;
            smallHeight = initialHeight / 3;

            // Set initial panel state to be small
            onCollapsingToolbarLayoutClick(collapsingToolbarLayout, new EventArgs());

            return view;
        }

        private void onCollapsingToolbarLayoutClick(object sender, EventArgs e)
        {
            if (toolbarBottomPart.Visibility != ViewStates.Visible)
            {
                toolbarBottomPart.Visibility = ViewStates.Visible;
                appBar.LayoutParameters.Height = initialHeight;
                appBar.RequestLayout();
            }
            else
            {
                toolbarBottomPart.Visibility = ViewStates.Gone;
                appBar.LayoutParameters.Height = smallHeight;
                appBar.RequestLayout();
            }
        }
    }
}