using System;
using System.Windows;
using WGestures.App.Views;

namespace WGestures.App.Views;

public partial class ImportWindow : Window
{
    private readonly ImportWindowViewModel _viewModel;

    internal event EventHandler<ImportEventArgs> Import;

    public ImportWindow()
    {
        InitializeComponent();
        _viewModel = new ImportWindowViewModel();
        DataContext = _viewModel;
        _viewModel.Import += OnViewModelImport;
        _viewModel.CloseRequest += (s, e) => Close();
    }

    private void OnViewModelImport(object sender, ImportEventArgs e)
    {
        Import?.Invoke(this, e);
    }
}