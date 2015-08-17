using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels
{
    public abstract class ContragentsViewModel : ViewModelBase
    {
        protected ContragentsViewModel(Context context)
        {
            Contragents = new ObservableCollection<ContragentViewModel>(GetContragents(context));
            ContragentsView = CollectionViewSource.GetDefaultView(Contragents);

            DeleteContragentCommand = new DelegateCommand(DeleteContragent, () => SelectedContragent != null);
            AddContragentCommand = new DelegateCommand(AddContragent);
            EditContragentCommand = new DelegateCommand(EditContragent, () => SelectedContragent != null);
        }

        private ContragentViewModel _selectedContragent;

        public ContragentViewModel SelectedContragent
        {
            get { return _selectedContragent; }
            set
            {
                _selectedContragent = value;
                NotifyPropertyChanged();
                DeleteContragentCommand.RaiseCanExecuteChanged();
                AddContragentCommand.RaiseCanExecuteChanged();
                EditContragentCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _deleteContragentCommand;

        public DelegateCommand DeleteContragentCommand
        {
            get { return _deleteContragentCommand; }
            set
            {
                _deleteContragentCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _addContragentCommand;

        public DelegateCommand AddContragentCommand
        {
            get { return _addContragentCommand; }
            set
            {
                _addContragentCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _editContragentCommand;

        public DelegateCommand EditContragentCommand
        {
            get { return _editContragentCommand; }
            set
            {
                _editContragentCommand = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ContragentViewModel> _contragents;

        public ObservableCollection<ContragentViewModel> Contragents
        {
            get { return _contragents; }
            set
            {
                _contragents = value;
                NotifyPropertyChanged();
            }
        }

        private ICollectionView _contragentsView;

        public ICollectionView ContragentsView
        {
            get { return _contragentsView; }
            set
            {
                _contragentsView = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Delete selected contragent
        /// </summary>
        public void DeleteContragent()
        {
            MessageBox.Show(String.Format("Contragent {0} was deleted", SelectedContragent.ID));
        }

        protected abstract void AddContragent();

        protected abstract void EditContragent();

        protected abstract List<ContragentViewModel> GetContragents(Context context);
    }
}
