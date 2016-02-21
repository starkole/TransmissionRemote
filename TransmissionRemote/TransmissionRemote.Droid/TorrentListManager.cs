using System.Collections.Generic;

using TransmissionRemote.Models;
using TransmissionRemote.Models.Enums;
using TransmissionRemote.Services;

namespace TransmissionRemote.Droid
{
    class TorrentListManager : ITorrentListManager
    {
        public IList<TorrentItem> GetAll()
        {
            var torrentItems = new List<TorrentItem>
            {
                new TorrentItem { Id = 1, Name = "First", State = TorrentState.Complete },
                new TorrentItem { Id = 2, Name = "Start", State = TorrentState.Downloading },
                new TorrentItem { Id = 3, Name = "Settlement", State = TorrentState.Downloading },
                new TorrentItem { Id = 4, Name = "Pork", State = TorrentState.Complete },
                new TorrentItem { Id = 5, Name = "Beef", State = TorrentState.Complete },
                new TorrentItem { Id = 6, Name = "Mutton", State = TorrentState.Paused },
                new TorrentItem { Id = 7, Name = "Fish", State = TorrentState.Complete },
                new TorrentItem { Id = 8, Name = "Vegetables", State = TorrentState.Paused },
                new TorrentItem { Id = 9, Name = "Milk", State = TorrentState.Downloading },
                new TorrentItem { Id = 10, Name = "Kitten", State = TorrentState.Complete }
            };

            return torrentItems;
        }
    }
}