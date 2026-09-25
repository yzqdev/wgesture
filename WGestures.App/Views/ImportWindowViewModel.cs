using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using WGestures.App.Migrate;

namespace WGestures.App.Views;

public enum ImportOption
{
    None,
    Replace,
    Merge
}

internal class ImportEventArgs : EventArgs
{
    public ConfigAndGestures ConfigAndGestures { get; private set; }
    public ImportOption GesturesImportOption { get; private set; }
    public ImportOption ConfigImportOption { get; private set; }

    public bool Success { get; set; }
    public string ErrorMessage { get; set; }

    public ImportEventArgs(ConfigAndGestures confAndGest, ImportOption gestImpOpt, ImportOption confImpOpt)
    {
        Success = true;
        ConfigAndGestures = confAndGest;
        GesturesImportOption = gestImpOpt;
        ConfigImportOption = confImpOpt;
    }
}

public partial class ImportWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _filePath = "请选择要导入的文件";

    [ObservableProperty]
    private bool _isImportOptionsVisible;

    [ObservableProperty]
    private bool _importGesturesChecked;

    [ObservableProperty]
    private bool _importConfigChecked;

    [ObservableProperty]
    private int _gestureImportOptionIndex;

    [ObservableProperty]
    private bool _isErrorVisible;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isOkEnabled;

    private ConfigAndGestures _configAndGestures;
    private ImportOption _gesturesImportOption = ImportOption.None;
    private ImportOption _configImportOption = ImportOption.None;

    internal event EventHandler<ImportEventArgs> Import;
    public event EventHandler CloseRequest;

    public ImportWindowViewModel()
    {
        ImportGesturesChecked = false;
        ImportConfigChecked = false;
        GestureImportOptionIndex = 0;
        UpdateOkEnabled();
    }

    [RelayCommand]
    private void SelectFile()
    {
        var dialog = new OpenFileDialog
        {
            DefaultExt = ".wgb",
            Filter = "WGestures备份文件 (*.wgb)|*.wgb|WGestures 1.2手势文件|*.json",
            Title = "选择要导入的文件"
        };

        if (dialog.ShowDialog() == true)
        {
            FilePath = dialog.FileName;
            IsErrorVisible = false;

            try
            {
                _configAndGestures = MigrateService.Import(FilePath);
            }
            catch (MigrateException ex)
            {
                ShowError(ex.Message);
                return;
            }

            var containsGestures = _configAndGestures.GestureIntentStore != null;
            var containsConfig = _configAndGestures.Config != null;

            ImportGesturesChecked = containsGestures;
            ImportGesturesChecked = containsGestures;

            ImportConfigChecked = containsConfig;
            ImportConfigChecked = containsConfig;

            IsImportOptionsVisible = true;
            UpdateOkEnabled();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseRequest?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void Ok()
    {
        IsErrorVisible = false;
        OnImport();
    }

    partial void OnImportGesturesCheckedChanged(bool value)
    {
        if (!value)
        {
            _gesturesImportOption = ImportOption.None;
        }
        else
        {
            _gesturesImportOption = GestureImportOptionIndex == 0 ? ImportOption.Merge : ImportOption.Replace;
        }
        UpdateOkEnabled();
    }

    partial void OnImportConfigCheckedChanged(bool value)
    {
        if (!value)
        {
            _configImportOption = ImportOption.None;
        }
        else
        {
            _configImportOption = ImportOption.Merge;
        }
        UpdateOkEnabled();
    }

    partial void OnGestureImportOptionIndexChanged(int value)
    {
        if (ImportGesturesChecked)
        {
            _gesturesImportOption = value == 0 ? ImportOption.Merge : ImportOption.Replace;
        }
    }

    private void UpdateOkEnabled()
    {
        IsOkEnabled = ImportConfigChecked || ImportGesturesChecked;
    }

    private void ShowError(string msg)
    {
        ErrorMessage = msg;
        IsErrorVisible = true;
    }

    private void OnImport()
    {
        if (_configAndGestures == null) return;

        var args = new ImportEventArgs(_configAndGestures, _gesturesImportOption, _configImportOption);
        Import?.Invoke(this, args);

        if (args.Success)
        {
            MessageBox.Show("导入成功！", "导入完成", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            if (!string.IsNullOrEmpty(args.ErrorMessage))
            {
                ShowError(args.ErrorMessage);
            }
        }
    }
}