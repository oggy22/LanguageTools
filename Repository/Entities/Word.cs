using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Repository.Entities
{
    public class Word : IComparable<Word>
    {
        public static Func<Word, string> toStringHandler = null;

        public static bool ShowDateCreated { get; set; }

        public static bool ShowDateModified { get; set; }

        public string Name { get; set; }

        public string Translation { get; set; }

        public string Type { get; set; }

        public int status { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public override string ToString()
        {
            string toString;

            toString = (toStringHandler == null ? Name : toStringHandler(this));

            if (ShowDateCreated)
                toString += ", " + Created;

            if (ShowDateModified)
                toString += ", " + Modified;

            return toString;
        }

        public int CompareTo(Word other)
        {
            return Name.Length - other.Name.Length;
        }
    }
}