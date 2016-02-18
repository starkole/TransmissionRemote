namespace TransmissionRemote.Models.Enums
{    
    public enum TorrentState
    {
        Unknown = 0,
        Downloading,
        Paused,
        Verifying,
        Complete,
        QueuedForDownload
    }
}
