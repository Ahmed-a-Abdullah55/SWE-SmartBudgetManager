namespace Masroofy.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Pin { get; set; }

        public User(string name, string pin)
        {
            Name = name;
            Pin = pin;
        }
    }
}
