// *********************************************************
// 
// Coho.UI InputBoxDialog.xaml.cs
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
using System.Windows.Input;

namespace Coho.UI.Dialogs;

public partial class InputBoxDialog : SecondaryWindow
{
    public InputBoxDialog()
    {
        InitializeComponent();
        ContentRendered += OnContentRendered;
        Loaded += OnLoaded;
    }

    public string Value
    {
        get;
        set;
    } = string.Empty;

    private void OnContentRendered(object? sender, EventArgs e)
    {
        InvalidateVisual();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Value))
        {
            TbInput.Text = Value;
        }

        TbInput.Focus();
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        Value = TbInput.Text;
        DialogResult = true;
        Close();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void TbInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            BtnOk_Click(null, new RoutedEventArgs());
        }
    }
}