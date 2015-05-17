using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Repository.Entities;
using System.Collections.ObjectModel;
using Oggy.Repository;
using System.ComponentModel;
using System.Collections.Specialized;
using Oggy.Transliterator;
using System.Windows;

namespace TransliterationEditor.BusinessCollections
{
    public class RuleCollection : ObservableCollection<TransliterationRule>
    {
        private ARepository repository;

        public Transliterator MyTransliterator { get; set; }

        public RuleCollection()
        { }
        
        public RuleCollection(ARepository repository)
            : base(repository.ListRules(false))
        {
            this.repository = repository;

            foreach (var rule in this)
            {
                rule.PropertyChanged += new PropertyChangedEventHandler(rule_PropertyChanged);
            }

            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(RuleCollection_CollectionChanged);
        }

        void RuleCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add :
                    foreach (var item in e.NewItems)
                    {
                        TransliterationRule rule = (TransliterationRule)item;
                        rule.PropertyChanged += new PropertyChangedEventHandler(rule_PropertyChanged);
						if (rule.Source != null) { }	//todo:
                            //MyTransliterator[rule.Source] = rule.Destination;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove :
                    foreach (var item in e.OldItems)
                    {
                        TransliterationRule rule = (TransliterationRule)item;
						repository.DeleteRule(rule.Source);	//todo:
                        //MyTransliterator[rule.Source] = null;
                    }
                    break;
            }
        }

        void rule_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TransliterationRule rule = (TransliterationRule)sender;

            // Note that e.PropertyName is actually the previous "source" value
            if (e.PropertyName == null)
            {
                try
                {
                    repository.CreateRule(rule);
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("Rule with destination={0} already exists!", rule.Source));
                }
            }
            else
                repository.UpdateRule(rule, e.PropertyName);

            if (!rule.Disabled)
            {
                if (rule.oldSource == rule.Source) {} //todo:
                    //MyTransliterator[rule.Source] = (rule.Destination ?? string.Empty);
                else
                {
					if (rule.oldSource != null) { } //todo:
                        //MyTransliterator[rule.oldSource] = null;
                    //MyTransliterator[rule.Source] = rule.Destination;
                }
            }
            //todo: it was enabled, now is disabled, then remove it from the rules
        }
    }
}
