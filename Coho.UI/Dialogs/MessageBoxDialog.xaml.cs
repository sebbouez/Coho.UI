// *********************************************************
// 
// Coho.UI
// MessageBoxDialog.xaml.cs
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

using System;
using System.Windows;
using Coho.UI.Windows;

namespace Coho.UI.Dialogs;

internal partial class MessageBoxDialog : SecondaryWindow
{
    internal MessageBoxDialog()
    {
        InitializeComponent();
        ContentRendered += MessageboxDialog_ContentRendered;
        Loaded += MessageboxDialog_Loaded;
    }

    private void MessageboxDialog_Loaded(object sender, RoutedEventArgs e)
    {
        if (BtnOk.Visibility == Visibility.Visible)
        {
            BtnOk.Focus();
        }
        else if (BtnYes.Visibility == Visibility.Visible)
        {
            BtnYes.Focus();
        }
    }

    private void MessageboxDialog_ContentRendered(object? sender, EventArgs e)
    {
        InvalidateVisual();
    }

    private void BtnYes_Click(object sender, RoutedEventArgs e)
    {
        DataContext = MessageBoxResult.Yes;
        Close();
    }

    private void BtnNo_Click(object sender, RoutedEventArgs e)
    {
        DataContext = MessageBoxResult.No;
        Close();
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        DataContext = MessageBoxResult.OK;
        Close();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DataContext = MessageBoxResult.Cancel;
        Close();
    }
}