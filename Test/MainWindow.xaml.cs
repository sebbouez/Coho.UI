using System.Windows;
using System.Windows.Controls;
using Coho.UI;
using Coho.UI.CommandManaging;
using Coho.UI.Tools;

namespace Test;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : ApplicationWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Global.AppWindow = this;


        OmnibarSearchService.RegisterOmnibarSearchService(new CustomOmnibarSearchService());
        OmnibarSearchService.SearchResultClicked += OmnibarSearchServiceOnSearchResultClicked;


        LocalRibbonControl.QatCommands.Add(LocalRibbonControl.GetCommandIdentifier(Btn1));
        LocalRibbonControl.QatCommands.Add(LocalRibbonControl.GetCommandIdentifier(Btn2));
    }

    private void OmnibarSearchServiceOnSearchResultClicked(object? sender, OmnibarSearchResult e)
    {
        MessageBox.Show("Clicked " + e.CommandTabName);
    }

    private void localRibbonControl_FileButtonClicked(object sender, RoutedEventArgs e)
    {
        ShowBackstageView(new Backstage());
    }

    private void MasterAccentSplitButton_Click(object sender, RoutedEventArgs e)
    {
        var t = ThemedMessageBox.Show("Message", "title", MessageBoxButton.YesNo);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        LocalRibbonControl.ShowContextualTab(TabOutils);
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        LocalRibbonControl.HideContextualTab(TabOutils);
    }

    private void btnaaa22_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("ok");
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        ApplyAccentToChrome = (sender as CheckBox).IsChecked.Value;
    }
}