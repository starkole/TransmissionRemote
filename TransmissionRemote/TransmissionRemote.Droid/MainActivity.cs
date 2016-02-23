using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.Res;
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
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using TransmissionRemote.Models.Enums;

namespace TransmissionRemote.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        #region Declarations

        private List<TorrentState> filters = new List<TorrentState>();
        private MainFragment mainFragment;
        private TrActionBarDrawerToggle drawerToggle;
        private DrawerLayout drawerLayout;
        private static readonly string[] drawerMenuSections =
            {
                "Browse", "Friends", "Profile", "All Torrents List"
            };

        #endregion

        #region Overrides

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.home, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.main);

            setupDrawerMenu();
            setupTorrentsList();
            setupActionBar();
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            var drawerOpen = this.drawerLayout.IsDrawerOpen((int)GravityFlags.Left);
            for (int i = 0; i < menu.Size(); i++)
            {
                menu.GetItem(i).SetVisible(!drawerOpen);
            }
            var drawer2Open = this.drawerLayout.IsDrawerOpen((int)GravityFlags.End);
            for (int i = 0; i < menu.Size(); i++)
            {
                menu.GetItem(i).SetVisible(!drawer2Open);
            }

            return base.OnPrepareOptionsMenu(menu);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            this.drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            this.drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_share:
                    this.drawerLayout.OpenDrawer(GravityCompat.End);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            } 
        }

        #endregion

        #region Helpers

        private void setupDrawerMenu()
        {
            // Right drawer
            var view = this.FindViewById<NavigationView>(Resource.Id.navigationView);
            view.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
            {
                var itemId = e.MenuItem.ItemId;               
                e.MenuItem.SetChecked(!e.MenuItem.IsChecked);

                switch (itemId)
                {
                    case Resource.Id.nav_home: 
                        this.showToast(itemId.ToString());
                        this.drawerLayout.CloseDrawers();
                        break;

                    case Resource.Id.nav_downloading: 
                        this.drawerLayout.CloseDrawers();
                            if(e.MenuItem.IsChecked)
                            {
                                filters.Add(TorrentState.Downloading);
                            }
                            else
                            {
                                filters.Remove(TorrentState.Downloading);
                            }
                        
                        break;

                    case Resource.Id.nav_complete: 
                        this.drawerLayout.CloseDrawers();                       
                            if(e.MenuItem.IsChecked)
                            {
                                filters.Add(TorrentState.Complete);
                            }
                            else
                            {
                                filters.Remove(TorrentState.Complete);
                            }
                        break;

                    case Resource.Id.nav_paused: 
                        this.drawerLayout.CloseDrawers();
                            if(e.MenuItem.IsChecked)
                            {
                                filters.Add(TorrentState.Paused);
                            }
                            else
                            {
                                filters.Remove(TorrentState.Paused);
                            }                     
                        break;
                }
                if (filters.Count > 0)
                {
                    this.mainFragment.FilterTorrentItems(filters);
                }
            };

            // Left drawer
            var drawerListView = FindViewById<ListView>(Resource.Id.left_drawer_list);
            drawerListView.Adapter = new ArrayAdapter<string>(this, Resource.Layout.item_menu, drawerMenuSections);
            drawerListView.ItemClick += (sender, e) =>
            {
                // Here we create new fragment
                switch (Array.IndexOf(drawerMenuSections, ((TextView)e.View).Text))
                {
                    case 0:
                        var firstItemFragment = new FirstItemFragment();
                        this.createFragment(firstItemFragment);
                        break;
                    case 1:
                        var secondItemFragment = new SecondItemFragment();
                        this.createFragment(secondItemFragment);
                        break;
                    case 2:
                        var thirdItemFragment = new ThirdItemFragment();
                        this.createFragment(thirdItemFragment);
                        break;
                    case 3:
                        this.mainFragment = new MainFragment();
                        this.createFragment(this.mainFragment);
                        break;						
                }

                drawerListView.SetItemChecked(e.Position, true);
                drawerLayout.CloseDrawers();
            };
        }

        private void setupTorrentsList()
        {
            this.mainFragment = new MainFragment();
            this.createFragment(this.mainFragment);
        }

        private void setupActionBar()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = GetString(Resource.String.app_name);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            this.drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            this.drawerLayout.SetDrawerShadow(Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);
            this.drawerToggle = new TrActionBarDrawerToggle(this, this.drawerLayout,
                toolbar,
                Resource.String.drawer_open,
                Resource.String.drawer_close);

            this.drawerLayout.SetDrawerListener(this.drawerToggle);
        }

        private void showToast(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        private void createFragment(Fragment fragment)
        {
            var fragmentTransaction = this.SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Replace(Resource.Id.content_frame, fragment);
            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
        }

        #endregion

    }
}


