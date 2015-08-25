using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class AddToRepairProductDialogViewModel : ViewModelBase
    {
        public AddToRepairProductDialogViewModel(MainViewModel model)
        {
            _mainViewModel = model;

            AddProductCommand = new DelegateCommand(AddProduct, Validate);

            Categories = model.Context.Categories.Include(x => x.Models).ToList();
            SelectedCategory = Categories.FirstOrDefault();
            if (SelectedCategory != null) 
                Models = SelectedCategory.Models.Select(x => x.Name).ToList();
        }

        private readonly MainViewModel _mainViewModel;

        public DelegateCommand AddProductCommand { get; set; }

        private string _serialNumber;

        public string SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                _serialNumber = value;
                NotifyPropertyChanged();
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        private string _modelName;

        public string ModelName
        {
            get { return _modelName; }
            set
            {
                _modelName = value;
                NotifyPropertyChanged();
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        private string _notes;

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                NotifyPropertyChanged();
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        private List<Category> _categories;

        public List<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                NotifyPropertyChanged();
            }
        }

        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                Models = value.Models.Select(x => x.Name).ToList();
                NotifyPropertyChanged();
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        private List<string> _models;

        public List<string> Models
        {
            get { return _models; }
            set
            {
                _models = value;
                NotifyPropertyChanged();
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(ModelName)) return false;
            if (ModelName.Length > 50)
            {
                MessageBox.Show(("Название модели не может быть длинее 50 символов"), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            if (Notes != null && Notes.Length > 50)
            {
                MessageBox.Show(("Примечание не может быть длинее 50 символов"), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            if (SerialNumber != null && SerialNumber.Length > 50)
            {
                MessageBox.Show(("Серийный номер не может быть длинее 50 символов"), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void AddProduct()
        {
            ModelName = Trim(ModelName);
            SerialNumber = Trim(SerialNumber);
            Notes = Trim(Notes);

            Product product = new Product {SerialNumber = SerialNumber, Notes = Notes};

            if (Models.Contains(ModelName)) product.Model = GetModelIDByName(ModelName);

            if (!Models.Contains(ModelName) || product.Model == null)
            {
                Model model = new Model {Name = ModelName, CategoryID = SelectedCategory.ID};

                _mainViewModel.Context.Models.Add(model);

                product.Model = model;
            }

            _mainViewModel.Context.Products.Add(product);

            // TODO fix operatorID

            Transaction transaction = new Transaction
            {
                Product = product,
                Date = DateTime.Now,
                TypeID = (int) TranType.ToRepair,
                OperatorID = 1,
            };

            _mainViewModel.Context.Transactions.Add(transaction);
            try
            {
                _mainViewModel.Context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось добавить новый товар"), "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed add new bought transaction. " + e.Message);
            }

            _mainViewModel.ToRepairProductsViewModel.Update();

            _mainViewModel.ToRepairProductsViewModel.AddDialogViewModel = null;
        }

        private Model GetModelIDByName(string modelName)
        {
            return SelectedCategory.Models.FirstOrDefault(x => x.Name == modelName);
        }

        private string Trim(string s)
        {
            if (s == null) return null;
                
            return s = s.Trim();
        }
    }
}
