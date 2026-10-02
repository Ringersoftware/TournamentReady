using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace Tvw.PcConfig;


public partial class PcConfigManager : ObservableObject
{

    public PcConfig PcConfig { get; private set; }
    public PcConfig PcConfigNew { get; private set; }

    [ObservableProperty] public partial bool HostnameTask { get; set; } = true;
    [ObservableProperty] public partial bool UltraVncTask { get; set; } = true;
    [ObservableProperty] public partial bool ObsStudioTask { get; set; } = true;
    [ObservableProperty] public partial bool AutologonTask { get; set; } = true;

    private PcConfigManager()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async Task<PcConfigManager> BuildAsync(ILogger? logger = null)
    {
        var pcConfigManager = new PcConfigManager
        {
            PcConfig = await PcConfig.GetStateAsync(logger),
            PcConfigNew = new (logger)
            {
                Hostname = "Matte1",
                UltraVncInstalled = true,
                ObsStudioInstalled = true,
                AutologonEnabled = true,
            },
        };

        pcConfigManager.HostnameTask = pcConfigManager.PcConfig.Hostname != pcConfigManager.PcConfigNew.Hostname;
        pcConfigManager.UltraVncTask = pcConfigManager.PcConfig.UltraVncInstalled != pcConfigManager.PcConfigNew.UltraVncInstalled;
        pcConfigManager.ObsStudioTask = pcConfigManager.PcConfig.ObsStudioInstalled != pcConfigManager.PcConfigNew.ObsStudioInstalled;
        pcConfigManager.AutologonTask = pcConfigManager.PcConfig.AutologonEnabled != pcConfigManager.PcConfigNew.AutologonEnabled;

        return pcConfigManager;
    }

    [RelayCommand]
    public async Task SetPcConfigAsync(CancellationToken cancellationToken = default)
    {
        List<Task> tasks = [];
        if (HostnameTask) tasks.Add(PcConfigNew.SetHostnameAsync(cancellationToken));
        if (UltraVncTask) tasks.Add(PcConfigNew.ProcessUltraVncAsync(cancellationToken));
        if (ObsStudioTask) tasks.Add(PcConfigNew.ProcessObsStudioAsync(cancellationToken));
        if (AutologonTask) tasks.Add(PcConfigNew.ProcessAutologonAsync(cancellationToken));

        await Task.WhenAll(tasks);
    }

    public async Task SaveConfigAsync(string filePath)
        => await File.WriteAllTextAsync(filePath, PcConfig.ToString());

    public async Task LoadConfigAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var loadedConfig = JsonSerializer.Deserialize<PcConfig>(json) ?? throw new InvalidOperationException("PcConfig konnte nicht geladen werden.");

        PcConfigNew = loadedConfig;
        HostnameTask = PcConfig.Hostname != PcConfigNew.Hostname;
        UltraVncTask = PcConfig.UltraVncInstalled != PcConfigNew.UltraVncInstalled;
        ObsStudioTask = PcConfig.ObsStudioInstalled != PcConfigNew.ObsStudioInstalled;
        AutologonTask = PcConfig.AutologonEnabled != PcConfigNew.AutologonEnabled;

        OnPropertyChanged(nameof(PcConfigNew));
    }
}
