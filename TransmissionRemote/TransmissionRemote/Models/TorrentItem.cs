using TransmissionRemote.Models.Enums;

namespace TransmissionRemote.Models
{
    public class TorrentItem : BaseModel
    {
        private string name;
        private TorrentState state;
        private bool isVisible;

        public string Name
        {
            get { return name; }
            set { SetField(ref name, value, "Name"); }
        }

        public int Id { get; set; }

        public TorrentState State
        {
            get { return state; }
            set { SetField(ref state, value, "State"); }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { SetField(ref isVisible, value, "IsVisible"); }
        }

        public override string ToString()
		{
			return Id + ". " + Name;
		}
    }
}
