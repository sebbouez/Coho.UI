using System.Collections.Generic;
using System.Windows.Media;
using Coho.UI.CommandManaging;
using Coho.UI.Controls.Omnibar;

namespace Test;

public class CustomOmnibarSearchService : OmnibarSearchServiceBase
{
    public override string DisplayName
    {
        get
        {
            return "Search in files...";
        }
    }

    public override Brush Icon
    {
        get
        {
            return (Brush) App.Current.MainWindow.FindResource("IconMagic");
        }
    }

    public override IEnumerable<OmnibarSearchResult> ExecuteSearch(string terms)
    {
        List<OmnibarSearchResult> commands = new();

        // simulate long action to test the progress bar
        System.Threading.Thread.Sleep(1500);

        commands.Add(new()
        {
            GroupName = "Results in local folder",
            DisplayName = "My file 1",
            CommandDescription = "Open c:\\folder\\file1.txt in the editor",
            CommandTabName = "c:\\folder\\file1.txt"
        });
        commands.Add(new()
        {
            GroupName = "Results in local folder",
            DisplayName = "My file 2",
            CommandDescription = "Open c:\\folder\\file2.txt in the editor",
            CommandTabName = "c:\\folder\\file2.txt"
        });

        return commands;
    }
}