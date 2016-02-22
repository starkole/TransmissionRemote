using System;
using Android.App;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;

namespace TransmissionRemote.Droid
{
    public class TrActionBarDrawerEventArgs : EventArgs
    {
        public View DrawerView { get; set; }
        public float SlideOffset { get; set; }
        public int NewState { get; set; }
    }

    public delegate void TrActionBarDrawerChangedEventHandler(object s, TrActionBarDrawerEventArgs e);

    public class TrActionBarDrawerToggle : Android.Support.V7.App.ActionBarDrawerToggle
    {
		private readonly Activity _activity;

        #region Ctor

        public TrActionBarDrawerToggle(Activity activity,
                                       DrawerLayout drawerLayout,
                                       int openDrawerContentDescRes,
                                       int closeDrawerContentDescRes)
            : base(activity,
                   drawerLayout,
                   openDrawerContentDescRes,
                   closeDrawerContentDescRes)
        {
			this._activity = activity;
        }

        public TrActionBarDrawerToggle(Activity activity,
                                       DrawerLayout drawerLayout,
                                       Toolbar toolbar,
                                       int openDrawerContentDescRes,
                                       int closeDrawerContentDescRes)
            : base(activity,
                   drawerLayout,
                   toolbar,
                   openDrawerContentDescRes,
                   closeDrawerContentDescRes)
        {
			this._activity = activity;
        }

        #endregion

        #region Events

        public event TrActionBarDrawerChangedEventHandler DrawerClosed;
        public event TrActionBarDrawerChangedEventHandler DrawerOpened;
        public event TrActionBarDrawerChangedEventHandler DrawerSlide;
        public event TrActionBarDrawerChangedEventHandler DrawerStateChanged;

        #endregion

        #region Overrides

        public override void OnDrawerClosed(View drawerView)
        {
            if (null != this.DrawerClosed)
                this.DrawerClosed(this, new TrActionBarDrawerEventArgs { DrawerView = drawerView });
            base.OnDrawerClosed(drawerView);
			this._activity.InvalidateOptionsMenu();
        }

        public override void OnDrawerOpened(View drawerView)
        {
            if (null != this.DrawerOpened)
                this.DrawerOpened(this, new TrActionBarDrawerEventArgs { DrawerView = drawerView });
            base.OnDrawerOpened(drawerView);
			this._activity.InvalidateOptionsMenu();
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            if (null != this.DrawerSlide)
                this.DrawerSlide(this, new TrActionBarDrawerEventArgs
                    {
                        DrawerView = drawerView,
                        SlideOffset = slideOffset
                    });
            base.OnDrawerSlide(drawerView, slideOffset);
        }

        public override void OnDrawerStateChanged(int newState)
        {
            if (null != this.DrawerStateChanged)
                this.DrawerStateChanged(this, new TrActionBarDrawerEventArgs
                    {
                        NewState = newState
                    });
            base.OnDrawerStateChanged(newState);
        }

        #endregion
    }
}