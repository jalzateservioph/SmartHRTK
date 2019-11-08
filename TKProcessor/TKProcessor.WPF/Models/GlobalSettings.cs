using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TKProcessor.WPF.Models
{
    public class GlobalSettings : BaseModel
    {
        private ObservableCollection<Mapping> _payrollCodeMappings;
        private ObservableCollection<Mapping> _payPackageMappings;
        private DateTime _defaultNDStart;
        private DateTime _defaultNDEnd;
        private bool _createDTRForNoWorkDays;
        private bool displayDTRColumnsAsMinutes;

        public GlobalSettings()
        {
            PropertyChanged += GlobalSettings_PropertyChanged;

            PayrollCodeMappings = new ObservableCollection<Mapping>();
            PayPackageMappings = new ObservableCollection<Mapping>();
        }

        private void GlobalSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PayrollCodeMappings))
            {
                PayrollCodeMappingsView = CollectionViewSource.GetDefaultView(PayrollCodeMappings);
                PayrollCodeMappingsView.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
            }
            if (e.PropertyName == nameof(PayPackageMappings))
            {
                PayPackageMappingsView = CollectionViewSource.GetDefaultView(PayPackageMappings);
                PayPackageMappingsView.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
            }
        }

        public void ViewAndSort()
        {
            PayrollCodeMappingsView = CollectionViewSource.GetDefaultView(PayrollCodeMappings);
            PayrollCodeMappingsView.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));

            PayPackageMappingsView = CollectionViewSource.GetDefaultView(PayPackageMappings);
            PayPackageMappingsView.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
        }

        public DateTime DefaultNDStart
        {
            get => _defaultNDStart;
            set
            {
                _defaultNDStart = value;
                NotifyOfPropertyChange();
            }
        }
        public DateTime DefaultNDEnd
        {
            get => _defaultNDEnd;
            set
            {
                _defaultNDEnd = value;
                NotifyOfPropertyChange();
            }
        }
        public bool CreateDTRForNoWorkDays
        {
            get => _createDTRForNoWorkDays;
            set
            {
                _createDTRForNoWorkDays = value;
                NotifyOfPropertyChange();
            }
        }
        public bool DisplayDTRColumnsAsMinutes
        {
            get => displayDTRColumnsAsMinutes;
            set
            {
                displayDTRColumnsAsMinutes = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<Mapping> PayrollCodeMappings
        {
            get => _payrollCodeMappings;
            set
            {
                _payrollCodeMappings = value;
                NotifyOfPropertyChange();
            }
        }
        public ICollectionView PayrollCodeMappingsView { get; set; }
        public ObservableCollection<Mapping> PayPackageMappings
        {
            get => _payPackageMappings;
            set
            {
                _payPackageMappings = value;
                NotifyOfPropertyChange();
            }
        }
        public ICollectionView PayPackageMappingsView { get; set; }
    }
}
