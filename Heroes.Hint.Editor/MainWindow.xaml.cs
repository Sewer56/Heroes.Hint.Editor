using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Heroes.Hint.Editor.Model;
using Heroes.Hint.Library.Structure;
using Heroes.Hint.Library.Structure.Heroes.Enum;
using Microsoft.Win32;
using Reloaded.WPF.Theme.Default;
using Reloaded.WPF.Utilities;

namespace Heroes.Hint.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReloadedWindow
    {
        private const string NameOfItemsSource = "EntriesSource";

        public MainPageViewModel RealViewModel { get; set; }
        private ResourceManipulator _manipulator;
        private CollectionViewSource _source;

        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            RealViewModel = new MainPageViewModel();
            RealViewModel.PropertyChanged += RealViewModelOnPropertyChanged;

            InitializeComponent();
            _manipulator = new ResourceManipulator((FrameworkElement) this.Content);
            _source = _manipulator.Get<CollectionViewSource>(NameOfItemsSource);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"{((Exception) e.ExceptionObject).Message}, {((Exception) e.ExceptionObject).StackTrace}");
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter     = "Binary Files (.bin)|*.bin";
            if (String.IsNullOrEmpty(RealViewModel.CurrentFileLocation))
            {
                openDialog.InitialDirectory = Path.GetDirectoryName(RealViewModel.CurrentFileLocation);
                openDialog.FileName = RealViewModel.CurrentFileLocation;
            }

            if ((bool) openDialog.ShowDialog())
                RealViewModel.OpenFile(openDialog.FileName);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Binary Files (.bin)|*.bin";
            if (String.IsNullOrEmpty(RealViewModel.CurrentFileLocation))
            {
                saveDialog.InitialDirectory = Path.GetDirectoryName(RealViewModel.CurrentFileLocation);
                saveDialog.FileName = RealViewModel.CurrentFileLocation;
            }

            if ((bool) saveDialog.ShowDialog())
                RealViewModel.SaveFile(saveDialog.FileName);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            RealViewModel.SaveCurrentPath();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            RealViewModel.RemoveCurrentEntry();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            RealViewModel.AddNewEntry();
        }

        private void RealViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RealViewModel.TextFilter))
                _source?.View?.Refresh();
        }

        private void EntriesSource_Filter(object sender, FilterEventArgs e)
        {
            if (String.IsNullOrEmpty(RealViewModel.TextFilter))
            {
                e.Accepted = true;
                return;
            }

            var managedEntry = e.Item as ManagedEntry;
            e.Accepted = managedEntry.Text.IndexOf(RealViewModel.TextFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
