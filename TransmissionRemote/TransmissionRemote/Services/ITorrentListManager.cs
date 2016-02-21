using System.Collections.Generic;
using TransmissionRemote.Models;

namespace TransmissionRemote.Services
{
    public interface ITorrentListManager
    {
        IList<TorrentItem> GetAll();
    }
}
