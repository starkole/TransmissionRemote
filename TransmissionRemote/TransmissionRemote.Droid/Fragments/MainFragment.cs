
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using TransmissionRemote.Models;
using TransmissionRemote.Models.Enums;
using Fragment = Android.Support.V4.App.Fragment;

namespace TransmissionRemote.Droid.Fragments
{
    public class MainFragment : Fragment
    {
        private ExpandableListView torrentsList;
        private TorrentListManager torrentListManager;
        private int counter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.all_torrents_list, container, false);
            this.torrentsList = view.FindViewById<ExpandableListView>(Resource.Id.torrents_list);

            this.torrentListManager = new TorrentListManager();

            var torrentItems = torrentListManager.GetAll();
            var adapter = new TorrentsListAdapter(this, torrentItems);
            torrentsList.SetAdapter(adapter);
            var fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab_add);
            fab.Click += (sender, args) =>
            {
                torrentItems.Add(new TorrentItem
                    { 
                        Id = 10 + counter++, 
                        Name = "Added " + counter, 
                        State = TorrentState.QueuedForDownload 
                    });
                adapter.NotifyDataSetChanged();
                var lastGroupPosition = adapter.GroupCount - 1;
                var lastChildPosition = adapter.GetChildrenCount(lastGroupPosition) - 1;
                this.torrentsList.ExpandGroup(lastGroupPosition, true);
                this.torrentsList.SetSelectedChild(lastGroupPosition, lastChildPosition, true);
            };

            return view;
        }
    }
}

