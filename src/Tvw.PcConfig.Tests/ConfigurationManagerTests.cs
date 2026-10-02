using Microsoft.Extensions.Logging.Testing;

namespace Tvw.PcConfig.Tests;

[TestClass]
public class ConfigurationManagerTests
{
    private FakeLogger _logger = null!;

    [TestInitialize]
    public void InitializeTest()
    {
        _logger = new FakeLogger();
    }

    [TestCleanup]
    public void CleanupTest()
    {
        // Logger-Ausgaben zur Konsole schreiben
        if (_logger.Collector.Count > 0)
        {
            Console.WriteLine("--- Logger-Ausgaben ---");
            foreach (var record in _logger.Collector.GetSnapshot())
            {
                Console.WriteLine($"[{record.Level}] {record.Message}");
            }
        }
    }

    [TestMethod]
    public async Task SaveActualConfigurationAsync()
    {
        var manager = await PcConfigManager.BuildAsync(_logger);

        await manager.SaveConfigAsync(@"c:\temp\PcConfig.json");

        Console.WriteLine(manager.PcConfig);
        Assert.IsNotEmpty(manager.PcConfig.Hostname);
    }

    [TestMethod]
    public async Task RestoreConfigurationAsync()
    {
        var manager = await PcConfigManager.BuildAsync(_logger);
        await manager.LoadConfigAsync(@"c:\temp\PcConfig.json");

        Console.WriteLine(manager.PcConfigNew);
        Assert.IsNotEmpty(manager.PcConfigNew.Hostname);
    }

    [TestMethod]
    public async Task InstallUltraVNCAsync()
    {
        var manager = await PcConfigManager.BuildAsync(_logger);

        //Install
        manager.PcConfigNew.UltraVncInstalled = true;
        await manager.PcConfigNew.ProcessUltraVncAsync(TestContext.CancellationToken);

        //Uninstall
        manager.PcConfigNew.UltraVncInstalled = false;
        await manager.PcConfigNew.ProcessUltraVncAsync(TestContext.CancellationToken);

        Console.WriteLine(manager.PcConfigNew);
        Assert.IsNotEmpty(manager.PcConfigNew.Hostname);
    }

    [TestMethod]
    public async Task InstallObsStudioAsync()
    {
        var manager = await PcConfigManager.BuildAsync(_logger);

        //Install
        manager.PcConfigNew.ObsStudioInstalled = true;
        await manager.PcConfigNew.ProcessObsStudioAsync(TestContext.CancellationToken);

        //Uninstall
        manager.PcConfigNew.ObsStudioInstalled = false;
        await manager.PcConfigNew.ProcessObsStudioAsync(TestContext.CancellationToken);

        Console.WriteLine(manager.PcConfigNew);
        Assert.IsNotEmpty(manager.PcConfigNew.Hostname);
    }

    [TestMethod]
    public async Task EnableAutologonAsync()
    {
        var manager = await PcConfigManager.BuildAsync(_logger);

        //Install
        manager.PcConfigNew.AutologonEnabled = true;
        await manager.PcConfigNew.ProcessAutologonAsync(TestContext.CancellationToken);

        ////Uninstall
        manager.PcConfigNew.AutologonEnabled = false;
        await manager.PcConfigNew.ProcessAutologonAsync(TestContext.CancellationToken);

        Console.WriteLine(manager.PcConfigNew);
        Assert.IsNotEmpty(manager.PcConfigNew.Hostname);
    }

    public TestContext TestContext { get; set; }
}
