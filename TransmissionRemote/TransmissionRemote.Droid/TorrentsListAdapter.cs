using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using TransmissionRemote.Models;
using TransmissionRemote.Models.Enums;

namespace TransmissionRemote.Droid
{
    internal class TorrentsListAdapter : BaseExpandableListAdapter
    {
        private readonly IList<TorrentItem> torrentItems;
        private readonly FragmentActivity context;

        public TorrentsListAdapter(FragmentActivity context, IList<TorrentItem> torrentItems)
        {
            this.context = context;
            this.torrentItems = torrentItems;
        }

        public override int GroupCount
        {
            get
            {
                return torrentItems.GroupBy(i => i.State).Count(); 
            }
        }

        protected List<TorrentItem> TorrentsList { get; set; }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.torrent_list_group, null);
            var group = getGroupByPosition(groupPosition);
            var status = header.FindViewById<TextView>(Resource.Id.torrent_status);
            status.Text = group == null ? "Error group not found" : group.Key.ToString();
            return header;
        }

        public override View GetChildView(
            int groupPosition, 
            int childPosition, 
            bool isLastChild, 
            View convertView,
            ViewGroup parent)
        {
            View row = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.torrent_list_item, null);
            var item = getItemByPosition(groupPosition, childPosition);
            if (item == null)
            {
                row.FindViewById<TextView>(Resource.Id.torrent_description).Text = "Error no item found";
                return row;
            }

            row.FindViewById<TextView>(Resource.Id.torrent_description).Text = item.ToString();
            row.FindViewById<ProgressBar>(Resource.Id.torrent_progress_bar).Max = 100;
            row.FindViewById<ProgressBar>(Resource.Id.torrent_progress_bar).Progress = item.Id;

            return row;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            var group = torrentItems.GroupBy(i => i.State).ElementAtOrDefault(groupPosition);
            var result = group == null ? 0 : group.Count();
            return result;
        }

        public override void NotifyDataSetChanged()
        {
            context.RunOnUiThread(base.NotifyDataSetChanged);
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return   new JavaObjectWrapper<TorrentItem> 
            { 
                Obj = getItemByPosition(groupPosition, childPosition) 
            };
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return new JavaObjectWrapper<IGrouping<TorrentState, TorrentItem>> 
            { 
                Obj = getGroupByPosition(groupPosition) 
            };
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return false;
        }

        public override bool HasStableIds { get { return true; } }

        private IGrouping<TorrentState, TorrentItem> getGroupByPosition(int groupPosition)
        {
            return  torrentItems.GroupBy(i => i.State).ElementAtOrDefault(groupPosition);
        }

        private TorrentItem getItemByPosition(int groupPosition, int childPosition)
        {
            return getGroupByPosition(groupPosition).ElementAtOrDefault(childPosition);
        }
    }
}
