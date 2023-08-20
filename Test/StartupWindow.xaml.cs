// *********************************************************
// 
// Test
// StartupWindow.xaml.cs
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

using System.Windows;
using Coho.UI.Windows;

namespace Test;

public partial class StartupWindow : SecondaryWindow
{
    public StartupWindow()
    {
        InitializeComponent();
    }

    private void BtnRibbonWindow_OnClick(object sender, RoutedEventArgs e)
    {
        RibbonWindow w = new();
        w.Show();
    }

    private void BtnMenuWindow_OnClick(object sender, RoutedEventArgs e)
    {
        MenuWindow w = new();
        w.Show();
    }

    private void BtnTabbedWindow_OnClick(object sender, RoutedEventArgs e)
    {
        TabbedWindow w = new();
        w.Show();
    }
}