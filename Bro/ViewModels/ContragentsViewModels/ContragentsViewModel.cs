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
            _context = context;

            Contragents = new ObservableCollection<ContragentViewModel>(GetContragents(context));
            ContragentsView = CollectionViewSource.GetDefaultView(Contragents);

            AddContragentCommand = new DelegateCommand(AddContragent);

            DeleteContragentCommand = new DelegateCommand(DeleteContragent, () => SelectedContragent != null);
            
            EditContragentCommand = new DelegateCommand(EditContragent, () => SelectedContragent != null);
        }

        private readonly Context _context;

        private ContragentViewModel _selectedContragent;

        public ContragentViewModel SelectedContragent
        {
            get { return _selectedContragent; }
            set
            {
                _selectedContragent = value;
                NotifyPropertyChanged();
                DeleteContragentCommand.RaiseCanExecuteChanged();
                EditContragentCommand.RaiseCanExecuteChanged();
            }
        }

        #region Delegate commands
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

        #endregion

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
            MessageBoxResult answer = MessageBox.Show("Удалить выбранного контрагента?", "Question", MessageBoxButton.YesNo);

            if (answer != MessageBoxResult.Yes) return;

            var contragentToDelete = _context.Contragents.ToList()
                .LastOrDefault(x => x.ID == SelectedContragent.ID);

            if (contragentToDelete.ContragentTransactions.Any()  || contragentToDelete.OperatorTransactions.Any())
            {
                MessageBox.Show("Нельзя удалить контрагента, у которого есть транзакции", "Error", MessageBoxButton.OK);
                return;
            }

            try
            {
                _context.Contragents.Remove(contragentToDelete);
                _context.SaveChanges();

                MessageBox.Show("Контрагент удалён.", "Confirmation", MessageBoxButton.OK);
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось удалить контрагента", "Error", MessageBoxButton.OK);
                Logging.WriteToLog("Failed to remove contagent. " + e.Message);
            }
        }

        protected abstract void AddContragent();

        protected abstract void EditContragent();

        protected abstract List<ContragentViewModel> GetContragents(Context context);
    }
}
