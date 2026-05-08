using System;

    namespace Masroofy.Models
{
    /// <summary>
    /// Simple user model containing name and PIN.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User PIN for simple authentication.
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// Initializes a new <see cref="User"/>.
        /// </summary>
        /// <param name="name">User name.</param>
        /// <param name="pin">User PIN.</param>
        public User(string name, string pin)
        {
            Name = name;
            Pin = pin;
        }
    }
}
