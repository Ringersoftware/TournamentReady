using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Text.Json;

namespace Tvw.PcConfig;

public partial class PcConfig(ILogger? logger = null) : ObservableObject
{
    public PcConfig() : this(null)
    {
    }

    [ObservableProperty] public partial string Hostname { get; set; } = string.Empty;
    [ObservableProperty] public partial bool AutologonEnabled { get; set; } = false;
    [ObservableProperty] public partial bool UltraVncInstalled { get; set; } = false;
    [ObservableProperty] public partial bool ObsStudioInstalled { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async Task<PcConfig> GetStateAsync(ILogger? logger = null)
    {
        var config = new PcConfig(logger);

        await Task.Run(() =>
        {
            config.Hostname = Environment.MachineName;
            config.UltraVncInstalled = IsInstalled("UltraVNC");
            config.ObsStudioInstalled = IsInstalled("OBS Studio", true);
            config.AutologonEnabled = LocalUserExists("ringen");
        });

        return config;
    }
    public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

    public async Task SetHostnameAsync(CancellationToken cancellationToken = default)
    {

    }

    public async Task ProcessUltraVncAsync(CancellationToken cancellationToken = default)
    {
        var isInstalled = IsInstalled("UltraVNC");

        if (!UltraVncInstalled && isInstalled) //Uninstall
        {

            var startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\uvnc\UltraVNC\unins000.exe",
                Arguments = $"/VERYSILENT",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

            logger?.LogInformation("UltraVNC uninstalled.");

        }
        if (UltraVncInstalled && !isInstalled) // Install
        {
            var installer = new DirectoryInfo("Resources/UltraVNC").GetFiles().FirstOrDefault(i => i.Name.Contains("Setup.exe"));

            if (installer == null)
            {
                logger?.LogError("Bitte UltraVNC downloaden (https://uvnc.com/downloads/ultravnc/) und in den Ordner 'Resources/UltraVNC' ablegen.");
            }
            else
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = installer.FullName,
                    Arguments = "/VERYSILENT",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using var process = new Process { StartInfo = startInfo };
                process.Start();
                await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

                logger?.LogInformation("UltraVNC installed.");
            }
        }
    }

    public async Task ProcessObsStudioAsync(CancellationToken cancellationToken = default)
    {
        var isInstalled = IsInstalled("OBS Studio", true);

        if (!ObsStudioInstalled && isInstalled) //Uninstall
        {

            var startInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\obs-studio\uninstall.exe",
                Arguments = $"/S",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

            logger?.LogInformation("ObsStudio uninstalled.");

        }
        if (ObsStudioInstalled && !isInstalled) // Install
        {
            var installer = new DirectoryInfo("Resources/ObsStudio").GetFiles().FirstOrDefault(i => i.Name.Contains("Installer.exe"));

            if (installer == null)
            {
                logger?.LogError("Bitte ObsStudio downloaden (https://obsproject.com/de/download) und in den Ordner 'Resources/ObsStudio' ablegen.");
            }
            else
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = installer.FullName,
                    Arguments = "/S",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using var process = new Process { StartInfo = startInfo };
                process.Start();
                await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

                logger?.LogInformation("ObsStudio installed.");
            }
        }
    }

    public async Task ProcessAutologonAsync(CancellationToken cancellationToken = default)
    {
        var username = "ringen";
        var password = "Durchdreher2";

        var isEnabled = LocalUserExists(username, out var userPrincipal);

        if (!AutologonEnabled && isEnabled) //Disable
        {
            using var key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon",
                writable: true) ?? throw new InvalidOperationException("Winlogon-Registry konnte nicht geöffnet werden.");

            key.SetValue("AutoAdminLogon", "0", RegistryValueKind.String);
            key.DeleteValue("DefaultUserName", false);
            key.DeleteValue("DefaultDomainName", false);
            key.DeleteValue("DefaultPassword", false);

            userPrincipal?.Delete();

            logger?.LogInformation("Autologon disabled.");

        }
        if (AutologonEnabled && !isEnabled) // Enable
        {
            var autologon = new DirectoryInfo("Resources/Autologon").GetFiles().First(i => i.Name == "Autologon64.exe");

            using var context = new PrincipalContext(ContextType.Machine);

            using var user = new UserPrincipal(context)
            {
                SamAccountName = username,
                Enabled = true,
                PasswordNeverExpires = true
            };

            user.SetPassword(password);
            user.Save();

            var startInfo = new ProcessStartInfo
            {
                FileName = autologon.FullName,
                Arguments = $"{username} {Environment.MachineName} {password}",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

            logger?.LogInformation("Autologon enabled.");
        }
    }

    private static bool IsInstalled(string software, bool wow6432Node = false)
    {
        try
        {
            // Prüfe in der Registry nach UltraVNC
            string registryPath = wow6432Node ? @"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall" : @"Software\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var key = Registry.LocalMachine.OpenSubKey(registryPath))
            {
                if (key != null)
                {
                    foreach (var subKeyName in key.GetSubKeyNames())
                    {
                        using (var subKey = key.OpenSubKey(subKeyName))
                        {
                            var displayName = subKey?.GetValue("DisplayName") as string;
                            if (displayName != null && displayName.Contains(software, StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    private static bool LocalUserExists(string username)
    => LocalUserExists(username, out _);

    private static bool LocalUserExists(string username, out UserPrincipal? userPrincipal)
    {
        try
        {
            userPrincipal = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, username);
            return userPrincipal != null;
        }
        catch
        {
            userPrincipal = null;
            return false;
        }
    }

}
