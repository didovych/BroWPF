using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bro.ViewModels.ProductsViewModels;
using BroData;
using Microsoft.Practices.Prism.Commands;

namespace Bro.ViewModels.Dialogs
{
    public class EditProductDialogViewModel : ViewModelBase
    {
        public EditProductDialogViewModel(Context context, List<int> selectedProductIDs, ProductsViewModel productsViewModel)
        {
            _context = context;
            _productsViewModel = productsViewModel;
            _selectedProductIDs = selectedProductIDs;

            var firstSelectedProduct = _context.Products.FirstOrDefault(x => x.ID == selectedProductIDs.FirstOrDefault());

            EditProductCommand = new DelegateCommand(EditProduct, Validate);

            Categories = _context.Categories.Include(x => x.Models).ToList();

            SelectedCategory = Categories.FirstOrDefault(x => x.ID == firstSelectedProduct.Model.CategoryID);
            if (SelectedCategory != null)
                Models = SelectedCategory.Models.Select(x => x.Name).ToList();

            ModelName = firstSelectedProduct.Model.Name;
            SerialNumber = firstSelectedProduct.SerialNumber;
            Notes = firstSelectedProduct.Notes;
            SellingPrice = firstSelectedProduct.SellingPrice;
            DateSellTo = firstSelectedProduct.DateSellTo;
        }

        private readonly Context _context;

        private readonly List<int> _selectedProductIDs;
        private readonly ProductsViewModel _productsViewModel;

        public DelegateCommand EditProductCommand { get; set; }

        private string _serialNumber;

        public string SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                _serialNumber = value;
                NotifyPropertyChanged();
                EditProductCommand.RaiseCanExecuteChanged();
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
                EditProductCommand.RaiseCanExecuteChanged();
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
                EditProductCommand.RaiseCanExecuteChanged();
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
                EditProductCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime? _dateSellTo;

        public DateTime? DateSellTo
        {
            get { return _dateSellTo; }
            set
            {
                _dateSellTo = value;
                NotifyPropertyChanged();
                EditProductCommand.RaiseCanExecuteChanged();
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
                EditProductCommand.RaiseCanExecuteChanged();
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

        private void EditProduct()
        {
            ModelName = Trim(ModelName);
            SerialNumber = Trim(SerialNumber);
            Notes = Trim(Notes);

            Model model = new Model();

            if (Models.Contains(ModelName)) model = GetModelByName(ModelName);

            if (!Models.Contains(ModelName) || model == null)
            {
                model = new Model {Name = ModelName, CategoryID = SelectedCategory.ID};

                _context.Models.Add(model);
            }

            try
            {
                var productRows = _context.Products.Where(x => _selectedProductIDs.Contains(x.ID)).ToList();

                foreach (var productRow in productRows)
                {
                    productRow.Notes = Notes;
                    productRow.Model = model;
                    productRow.SerialNumber = SerialNumber;
                    productRow.SellingPrice = SellingPrice;
                    productRow.DateSellTo = DateSellTo;
                }

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(("Не удалось изменить данные товара"), "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Logging.WriteToLog("Failed edit product. " + e.InnerException.Message);
            }

            _productsViewModel.Update();

            _productsViewModel.EditProductDialogViewModel = null;
        }

        private Model GetModelByName(string modelName)
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
