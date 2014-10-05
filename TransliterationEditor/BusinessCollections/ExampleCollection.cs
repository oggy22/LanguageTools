using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Repository.Entities;
using System.Collections.ObjectModel;
using Oggy.Repository;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows;

namespace TransliterationEditor.BusinessCollections
{
    public class ExampleCollection : ObservableCollection<TransliterationExample>
    {
        private ARepository repository = null;

        public ExampleCollection(ARepository repository)
            : base(repository.ListExamples(enabledOnly:false))
        {
            this.repository = repository;

            foreach (var example in this)
                example.PropertyChanged += new PropertyChangedEventHandler(example_PropertyChanged);

            this.CollectionChanged += new NotifyCollectionChangedEventHandler(ExampleCollection_CollectionChanged);
        }

        public ExampleCollection(IEnumerable<TransliterationExample> rules)
            : base(rules)
        {

        }

        void ExampleCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add :
                    foreach (var item in e.NewItems)
                    {
                        TransliterationExample example = (TransliterationExample)(item);
                        example.PropertyChanged += new PropertyChangedEventHandler(example_PropertyChanged);
                        if (example.Source != string.Empty && example.Source != null)
                            repository.CreateExample(example);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove :
                    foreach (var item in e.OldItems)
                    {
                        TransliterationExample example = (TransliterationExample)(item);
                        repository.DeleteExample(example.Source);
                    }
                    break;
            }
        }

        void example_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TransliterationExample example = (TransliterationExample)(sender);

            // Note that e.PropertyName is actually the previous "source" value
            if (e.PropertyName == null)
            {
                try
                {
                    repository.CreateExample(example);
                }
                catch (Exception)   //the example already exists
                {
                    MessageBox.Show(string.Format("Exmample source={1} already exists", example.Source));
                }
            }
            else
                repository.UpdateExample(example, e.PropertyName);
        }
    }
}