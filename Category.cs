    namespace Masroofy.Models
{
    /// <summary>
    /// Represents a transaction category.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Category identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Display name of the category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Category"/>.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <param name="name">Category name.</param>
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
