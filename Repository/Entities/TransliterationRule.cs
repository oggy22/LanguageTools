using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Oggy.Repository.Entities
{
    public class TransliterationRule : INotifyPropertyChanged
    {
        #region Variables and Properties
        public string oldSource { get; private set; }
        
        private string source;
        public string Source
        {
            get
            { return source; }
            set
            {
                //todo: ensure that number of jokers in destination equal to number of jokers in source
                oldSource = source;
                source = value;
                FirePropertyChanged(oldSource);
            }
        }

        private string destination;
        public string Destination
        {
            get
            { return destination; }
            set
            {
                //todo: ensure that number of jokers in destination equal to number of jokers in source
                oldSource = source;
                destination = value;
                FirePropertyChanged(source);
            }
        }

        private bool disabled;
        public bool Disabled
        {
            get
            { return disabled; }
            set
            {
                oldSource = source;
                disabled = value;
                FirePropertyChanged(source);
            }
        }

        private string examples;
        public string Examples
        {
            get
            { return examples; }
            set
            { examples = value; FirePropertyChanged(source); }
        }

        private string counterExamples;
        public string CounterExamples
        {
            get
            { return counterExamples; }
            set
            { counterExamples = value; FirePropertyChanged(source); }
        }
        
        private string comment;
        public string Comment
        {
            get
            { return comment; }
            set
            { comment = value; FirePropertyChanged(source); }
        }
        #endregion

        #region
        public TransliterationRule()
        {
        }

        public TransliterationRule(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }

        public TransliterationRule(string source, string destination, string examples)
        {
            Source = source;
            Destination = destination;
            Examples = examples;
        }
        #endregion

        public override string ToString()
        {
            return Source + "->" + Destination;
        }

        /// <summary>
        /// Fires the PropertyChanged event.
        /// WARNING: source key is sent instead of the property name
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