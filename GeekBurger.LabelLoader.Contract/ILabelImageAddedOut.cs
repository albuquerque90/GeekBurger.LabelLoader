using System.Collections.Generic;

namespace GeekBurger.LabelLoader.Contract
{
    /// <summary>
    /// Defines the OUT contract
    /// </summary>
    interface ILabelImageAddedOut
    {
        /// <summary>
        /// Gets or sets the item name
        /// </summary>
        string ItemtName { get; set; }
        /// <summary>
        /// Gets or sets the list of ingredients
        /// </summary>
        IEnumerable<string> Ingredients { get; set; }
    }
}
