using System.Windows;
using Tvw.PcConfig.Gui.ViewModels;

namespace Tvw.PcConfig.Gui.Views;

/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Window
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
