using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Coho.UI;
using Coho.UI.CommandManaging;
using Coho.UI.Dialogs;
using Coho.UI.Tasks;
using Coho.UI.Tools;
using Coho.UI.Windows;
using Test.Tasks;

namespace Test;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class RibbonWindow : ApplicationWindow
{
    public RibbonWindow()
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
        MessageBoxResult quest = ThemedMessageBox.Show("Do you want to save changes?", "Save changes", MessageBoxButton.YesNoCancel);

        switch (quest)
        {
            case MessageBoxResult.Yes:
                // Yes button was clicked
                break;
            case MessageBoxResult.No:
                // No button was clicked
                break;
            case MessageBoxResult.Cancel:
                // Cancel button was clicked
                break;
        }
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
        ApplyAccentToChrome = ((CheckBox) sender).IsChecked!.Value;
    }

    private void CheckBox_Click(object sender, RoutedEventArgs e)
    {
        IsSpecialState = ((CheckBox) sender).IsChecked!.Value;
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        List<TaskRunnerBase> tasksToRun = new();
        tasksToRun.Add(new DummyTask1()); // this task will run correctly
        tasksToRun.Add(new DummyTask2()); // this task will fail

        ThemedMultiTaskDialog.Show("Running complex tasks", "Some message to explain what's going on when the process is started...", this, tasksToRun);
    }
}