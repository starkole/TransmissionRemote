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
using com.refractored.fab;
using TransmissionRemote.Models;
using TransmissionRemote.Models.Enums;

using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace TransmissionRemote.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        #region Declarations

        private int counter;
        private TrActionBarDrawerToggle drawerToggle;
        private TorrentListManager torrentListManager;
        private DrawerLayout drawerLayout;
        private static readonly string[] drawerMenuSections =
            {
                "Browse", "Friends", "Profile"
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
            showToast("Toolbar item pressed: " + item.TitleFormatted);
            return this.drawerToggle.OnOptionsItemSelected(item) || base.OnOptionsItemSelected(item);
        }

        #endregion

        #region Helpers

        private void setupDrawerMenu()
        {
            var drawerListView = FindViewById<ListView>(Resource.Id.left_drawer_list);
            drawerListView.Adapter = new ArrayAdapter<string>(this, Resource.Layout.item_menu, drawerMenuSections);
            drawerListView.ItemClick += (sender, args) =>
            {
                showToast("Drawer menu item selected: " + args.Position);
                drawerListView.SetItemChecked(args.Position, true);
                drawerLayout.CloseDrawers();
            };
        }

        private void setupTorrentsList()
        {
            ExpandableListView torrentsList = FindViewById<ExpandableListView>(Resource.Id.torrents_list);
            torrentListManager = new TorrentListManager();
            var torrentItems = torrentListManager.GetAll();
            var adapter = new TorrentsListAdapter(this, torrentItems);
            torrentsList.SetAdapter(adapter);
            var fab = FindViewById<FloatingActionButton>(Resource.Id.fab_add);
            fab.Click += (sender, args) =>
            {
                torrentItems.Add(new TorrentItem
                    { 
                        Id = 10 + counter++, 
                        Name = "Added " + counter.ToString(), 
                        State = TorrentState.QueuedForDownload 
                    });
                adapter.NotifyDataSetChanged();
                var lastGroupPosition = adapter.GroupCount - 1;
                var lastChildPosition = adapter.GetChildrenCount(lastGroupPosition) - 1;
                torrentsList.ExpandGroup(lastGroupPosition, true);
                torrentsList.SetSelectedChild(lastGroupPosition, lastChildPosition, true);
            };
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

            this.drawerToggle.DrawerClosed += (o, args) =>
            {
                this.InvalidateOptionsMenu();
            };

            this.drawerToggle.DrawerOpened += (o, args) =>
            {
                this.InvalidateOptionsMenu();
            };

            this.drawerLayout.SetDrawerListener(this.drawerToggle);
        }

        private void showToast(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        #endregion

    }
}


