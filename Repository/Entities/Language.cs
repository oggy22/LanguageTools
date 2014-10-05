using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy.Repository.Entities
{
    public class Language
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Alphabet { get; set; }

        public string Accents { get; set; }

        public string Hint { get; set; }

        public override string ToString()
        {
            return Code + "-" + Name;
        }

        public HashSet<string> WordTypes { get; set; }
    }
}
