using System.Collections.Generic;

namespace GeekBurger.LabelLoader.Contract
{
    /// <summary>
    /// Defines the OUT contract
    /// </summary>
    public interface ILabelImageAddedOut
    {
        /// <summary>
        /// Gets or sets the item name
        /// </summary>
        string ItemName { get; set; }
        /// <summary>
        /// Gets or sets the list of ingredients
        /// </summary>
        IEnumerable<string> Ingredients { get; set; }
    }
}
