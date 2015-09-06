using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bro.Helpers;
using BroData;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;

namespace Bro.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        public ReportsViewModel(Context context)
        {
            _context = context;

            SalesmanReportCommand = new DelegateCommand(SalesmanReport, () => ThroughDate >= FromDate && SelectedSalesman != null);
            GeneralReportCommand = new DelegateCommand(GeneralReport, () => ThroughDate >= FromDate);

            FromDate = DateTime.Today.AddDays(-14);
            ThroughDate = DateTime.Today.AddDays(1);

            Salesmen = context.Salesmen.ToList().Select(x => new SalesmanViewModel(x)).ToList();
            SelectedSalesman = Salesmen.FirstOrDefault();
        }

        private readonly Context _context;

        private DateTime _fromDate;

        public DateTime FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                NotifyPropertyChanged();
                SalesmanReportCommand.RaiseCanExecuteChanged();
                GeneralReportCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime _throughDate;

        public DateTime ThroughDate
        {
            get { return _throughDate; }
            set
            {
                _throughDate = value;
                NotifyPropertyChanged();
                SalesmanReportCommand.RaiseCanExecuteChanged();
                GeneralReportCommand.RaiseCanExecuteChanged();
            }
        }

        private List<SalesmanViewModel> _salesmen;

        public List<SalesmanViewModel> Salesmen
        {
            get { return _salesmen; }
            set
            {
                _salesmen = value;
                NotifyPropertyChanged();
            }
        }

        private SalesmanViewModel _selectedSalesman;

        public SalesmanViewModel SelectedSalesman
        {
            get { return _selectedSalesman; }
            set
            {
                _selectedSalesman = value;
                NotifyPropertyChanged();
                SalesmanReportCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _salesmanReportCommand;

        public DelegateCommand SalesmanReportCommand
        {
            get { return _salesmanReportCommand; }
            set
            {
                _salesmanReportCommand = value;
                NotifyPropertyChanged();
            }
        }

        private DelegateCommand _generalReportCommand;

        public DelegateCommand GeneralReportCommand
        {
            get { return _generalReportCommand; }
            set
            {
                _generalReportCommand = value;
                NotifyPropertyChanged();
            }
        }

        private void SalesmanReport()
        {
            if (SelectedSalesman == null) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx", InitialDirectory = Directory.GetCurrentDirectory()};
            saveFileDialog.ShowDialog();

            if (!saveFileDialog.FileName.EndsWith(".xlsx"))
            {
                //MessageBox.Show("Файл для сохранения не выбран", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ExcelExport export = new ExcelExport();
            export.SalesmanReport(saveFileDialog.FileName, SelectedSalesman.ID, FromDate, ThroughDate, _context);

            MessageBox.Show("Отчет готов");
        }

        private void GeneralReport()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx", InitialDirectory = Directory.GetCurrentDirectory() };
            saveFileDialog.ShowDialog();

            if (!saveFileDialog.FileName.EndsWith(".xlsx"))
            {
                //MessageBox.Show("Файл для сохранения не выбран", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ExcelExport export = new ExcelExport();
            export.GeneralReport(saveFileDialog.FileName, FromDate, ThroughDate, _context);

            MessageBox.Show("Отчет готов");
        }
    }
}
