using System.Collections.Generic;

namespace GeekBurger.LabelLoader.Contract
{
    /// <summary>
    /// Defines the OUT contract
    /// </summary>
    interface ILabelImageAddedOut
    {
        /// <summary>
        /// Gets or sets the product name
        /// </summary>
        string ProductName { get; set; }
        /// <summary>
        /// Gets or sets the list of ingredients
        /// </summary>
        IEnumerable<string> Ingredients { get; set; }
    }
}
