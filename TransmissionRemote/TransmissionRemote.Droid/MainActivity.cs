using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using TransmissionRemote.Models;
using TransmissionRemote.Models.Enums;

namespace TransmissionRemote.Droid
{
    [Activity (Label = "TransmissionRemote.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button> (Resource.Id.myButton);
            ExpandableListView torrentsList = FindViewById<ExpandableListView> (Resource.Id.torrentsList);
            
            

            var torrentItems = new List<TorrentItem>
            {
                new TorrentItem { Id = 1, Name = "First", State = TorrentState.Complete },
                new TorrentItem { Id = 2, Name = "Qfwdsf", State = TorrentState.Downloading },
                new TorrentItem { Id = 3, Name = "fafsdf", State = TorrentState.Downloading },
                new TorrentItem { Id = 4, Name = "dsfasdf", State = TorrentState.Complete },
                new TorrentItem { Id = 5, Name = "Firsdfasdst", State = TorrentState.Complete },
                new TorrentItem { Id = 6, Name = "dsfasd", State = TorrentState.Paused },
                new TorrentItem { Id = 7, Name = "dsaf", State = TorrentState.Complete },
                new TorrentItem { Id = 8, Name = "dsaf", State = TorrentState.Paused },
                new TorrentItem { Id = 9, Name = "dsfa", State = TorrentState.Downloading },
                new TorrentItem { Id = 10, Name = "sdfgf", State = TorrentState.Complete }
            };
            var adapter = new TorrentsListAdapter(this, torrentItems);
            torrentsList.SetAdapter(adapter);

            button.Click += delegate {
                button.Text = string.Format("{0} clicks!", count++);
                torrentItems.Add(new TorrentItem { Id = 10 + count, Name = "Added " + count.ToString(), State = TorrentState.QueuedForDownload } );
                adapter.NotifyDataSetChanged();
            };
        }
    }
}


