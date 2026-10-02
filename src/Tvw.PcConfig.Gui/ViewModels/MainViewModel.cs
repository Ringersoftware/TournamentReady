using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace Tvw.PcConfig.Gui.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ILogger<MainViewModel> logger;

    [ObservableProperty] public partial PcConfigManager? ConfigManager { get; set; }

    public MainViewModel(ILogger<MainViewModel> logger)
    {
        this.logger = logger;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        logger.LogInformation("Lade PC-Konfiguration.");
        ConfigManager = await PcConfigManager.BuildAsync(logger);
        logger.LogInformation("PC-Konfiguration geladen.");
    }

    [RelayCommand]
    public async Task SaveConfigAsync()
    {
        if (ConfigManager is null)
        {
            return;
        }

        var dialog = new SaveFileDialog
        {
            Filter = "JSON-Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
            DefaultExt = ".json",
            FileName = "PcConfig.json",
            AddExtension = true,
            OverwritePrompt = true,
        };

        if (dialog.ShowDialog() == true)
        {
            await ConfigManager.SaveConfigAsync(dialog.FileName);
        }
    }

    [RelayCommand]
    public async Task LoadConfigAsync()
    {
        if (ConfigManager is null)
        {
            return;
        }

        var dialog = new OpenFileDialog
        {
            Filter = "JSON-Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
            DefaultExt = ".json",
            CheckFileExists = true,
            Multiselect = false,
        };

        if (dialog.ShowDialog() == true)
        {
            await ConfigManager.LoadConfigAsync(dialog.FileName);
        }
    }
}
