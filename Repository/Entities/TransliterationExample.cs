using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Oggy.Repository.Entities
{
    public class TransliterationExample : INotifyPropertyChanged
    {
        public void TransliterationChanged()
        {
            if (Transliterator != null)
                Transliterated = Transliterator(source);
            if (Trans_li_te_rator != null)
                Tran_sli_te_rated = Trans_li_te_rator(source);
        }
        
        public static Func<string, string> Transliterator = null;

        public static Func<string, string> Trans_li_te_rator = null;

		public static Func<string, string, int> DestinationFunc = null;

		#region Properties

        private string source;
        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                string oldSource = source;
                source = value;
                TransliterationChanged();
                FirePropertyChanged(oldSource);
            }
        }

        private string destination;
        public string Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
                FirePropertyChanged(source);
            }
        }

        public string Transliterated
        { private set; get; }

        public string Tran_sli_te_rated
        {   private set; get; }

		public int Distance
		{
			get
			{
				return DestinationFunc(Destination, Transliterated);
			}
		}

        public bool Correct
        {
            get
            {
				return Destination == Transliterated;
			}
        }

        private bool disabled;
        public bool Disabled
        {
            get
            { return disabled; }
            set
            {
                disabled = value;
                FirePropertyChanged(source);
            }
        }
        
        #endregion

        public override string ToString()
        {
            return Source + "->" + Destination;
        }

		public bool Contains(string s)
		{
			return source.Contains(s) || (destination ?? string.Empty).Contains(s);
		}

        /// <summary>
        /// Fires the PropertyChanged event.
        /// WARNING: source key is sent instead of property
        /// </summary>
        /// <param name="source"></param>
        private void FirePropertyChanged(string source)
        {
            if (PropertyChanged != null && this.source != null)
                PropertyChanged(this, new PropertyChangedEventArgs(source));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}