using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using TransmissionRemote.Models;
using TransmissionRemote.Models.Enums;

namespace TransmissionRemote
{
    internal class TorrentsListAdapter : BaseExpandableListAdapter
    {
        private readonly IList<TorrentItem> torrentItems;
        private readonly Activity context;

        public TorrentsListAdapter(Activity context, IList<TorrentItem> torrentItems)
        {
            this.context = context;
            this.torrentItems = torrentItems;
        }

        public override int GroupCount => torrentItems.GroupBy(i => i.State).Count();

        protected List<TorrentItem> TorrentsList { get; set; }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.TorrentListGroup, null);
            var group = getGroupByPosition(groupPosition);
            header.FindViewById<TextView>(Resource.Id.TorrentStatus).Text = group?.Key.ToString() ?? "Error group not found";
            return header;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.TorrentListItem, null);
            var item = getItemByPosition(groupPosition, childPosition);
            row.FindViewById<TextView>(Resource.Id.TorrentId).Text = item?.Id.ToString() ?? "Error no item found";
            row.FindViewById<TextView>(Resource.Id.TorrentName).Text = item?.Name.ToString() ?? "Error no item found";

            return row;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            var group = torrentItems.GroupBy(i => i.State).ElementAtOrDefault(groupPosition);
            var result = group?.Count() ?? 0;
            return result;
        }

        public override void NotifyDataSetChanged()
        {
            context.RunOnUiThread(base.NotifyDataSetChanged);
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition) =>
            new JavaObjectWrapper<TorrentItem> { Obj = getItemByPosition(groupPosition, childPosition) };

        public override long GetChildId(int groupPosition, int childPosition) => childPosition;

        public override Java.Lang.Object GetGroup(int groupPosition) =>
            new JavaObjectWrapper<IGrouping<TorrentState, TorrentItem>> { Obj = getGroupByPosition(groupPosition) };

        public override long GetGroupId(int groupPosition) => groupPosition;

        public override bool IsChildSelectable(int groupPosition, int childPosition) => false;

        public override bool HasStableIds => true;

        private IGrouping<TorrentState, TorrentItem> getGroupByPosition(int groupPosition) =>
            torrentItems.GroupBy(i => i.State).ElementAtOrDefault(groupPosition);

        private TorrentItem getItemByPosition(int groupPosition, int childPosition) =>
            getGroupByPosition(groupPosition)?.ElementAtOrDefault(childPosition);

    }
}