namespace Models
{
    public class Product : Entity
    {
        private string name = string.Empty;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public float Price { get; set; }
        public Order? Order { get; set; }
    }
}