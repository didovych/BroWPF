using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class AddOnStockProductDialogViewModel : ViewModelBase
    {
        public AddOnStockProductDialogViewModel(MainViewModel model)
        {
            _mainViewModel = model;

            AddProductCommand = new DelegateCommand(AddProduct, Validate);

            Categories = model.Context.Categories.Include(x => x.Models).ToList();
            SelectedCategory = Categories.FirstOrDefault();
            if (SelectedCategory != null) 
                Models = SelectedCategory.Models.Select(x => x.Name).ToList();
            Number = 1;
        }

        private readonly MainViewModel _mainViewModel;

        public DelegateCommand AddProductCommand { get; set; }

        private int _number;

        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                NotifyPropertyChanged();
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

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

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyPropertyChanged();
                if (value < 0)
                    MessageBox.Show(("Цена меньше нуля"), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (value == 0)
                    MessageBox.Show(("Цена равна нулю"), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private decimal? _sellingPrice;

        public decimal? SellingPrice
        {
            get { return _sellingPrice; }
            set
            {
                _sellingPrice = value;
                NotifyPropertyChanged();
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
            if (Number < 1) return false;
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

            List<Product> products = new List<Product>();
            for (int i = 0; i < Number; i++)
                products.Add(new Product
                {
                    SerialNumber = SerialNumber, Notes = Notes, SellingPrice = SellingPrice
                });

            if (Models.Contains(ModelName))
                foreach (var product in products)
                {
                    product.Model = GetModelIDByName(ModelName);
                }

            if (!Models.Contains(ModelName) || products.FirstOrDefault().Model == null)
            {
                Model model = new Model {Name = ModelName, CategoryID = SelectedCategory.ID};

                _mainViewModel.Context.Models.Add(model);

                foreach (var product in products)
                {
                    product.Model = model;
                }
            }

            _mainViewModel.Context.Products.AddRange(products);

            
            List<Transaction> transactions = new List<Transaction>();
            foreach (var product in products)
            {
                // TODO fix operatorID
                transactions.Add(new Transaction
                {
                    Product = product,
                    Date = DateTime.Now,
                    TypeID = (int)TranType.Bought,
                    OperatorID = 1,
                    Price = Price
                });
            }

            _mainViewModel.Context.Transactions.AddRange(transactions);
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

            _mainViewModel.OnStockProductsViewModel.Update();

            _mainViewModel.OnStockProductsViewModel.AddDialogViewModel = null;
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
