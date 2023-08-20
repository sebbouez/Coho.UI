// *********************************************************
// 
// Test
// MenuWindow.xaml.cs
// Copyright (c) Sébastien Bouez. All rights reserved.
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// *********************************************************

using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Coho.UI;
using Coho.UI.Windows;

namespace Test;

public partial class MenuWindow : ApplicationWindow
{
    public MenuWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Closing+= OnClosing;
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        // save QAT commands in user settings
        // MainMenuBar.QatCommands => Gives the list of command identifiers to save 
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        
        // Populate the QAT with some commands
        // you can read a settings file instead
        MainMenuBar.QatCommands.Add(MainMenuBar.GetCommandIdentifier(MenuEditUndo));
        MainMenuBar.QatCommands.Add(MainMenuBar.GetCommandIdentifier(MenuEditRedo));
        MainMenuBar.QatCommands.Add(MainMenuBar.GetCommandIdentifier(MenuEditCut));
        MainMenuBar.QatCommands.Add(MainMenuBar.GetCommandIdentifier(MenuEditCopy));


        // Populate the InfoManager with some dummy messages
        InfoBarManager.AddInfoBar(null, "Title", "Message", Enums.InfoBarMode.Info);
        InfoBarManager.AddInfoBar(null, "Title", "Message", Enums.InfoBarMode.Warning);
        InfoBarManager.AddInfoBar(null, "Title", "Message", Enums.InfoBarMode.Error);
    }
}