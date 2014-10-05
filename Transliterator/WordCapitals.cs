using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Transliterator
{
    /// <summary>
    /// Enumeration for different cases of capitalizing words
    /// </summary>
    public enum WordCapitals
    {       
        /// <summary>
        /// If all the letters are in lower case
        /// </summary>
        LowerCase,

        /// <summary>
        /// If all the letters are in uppercase,
        /// </summary>
        UpperCase,

        /// <summary>
        /// If the first letter is uppercase and others are in lowercase
        /// </summary>
        CapitalLetter,

        /// <summary>
        /// Represents the case which does not fall in none of above
        /// </summary>
        None
    }
}