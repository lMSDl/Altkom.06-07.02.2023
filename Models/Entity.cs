using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    public abstract class Entity : INotifyPropertyChanged, IModifiedDate
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }


        public DateTime CeratedDate { get; }
        public DateTime ModifiedDate { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}