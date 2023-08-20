// *********************************************************
// 
// Coho.UI FolderPickerTextbox.cs
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
using System.Windows.Forms;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;

namespace Coho.UI.Controls.Textboxes;

public class FolderPickerTextbox : TextBox
{
    private Button? _btnBrowse;
    private bool _isLoaded;

    public new event EventHandler<string>? SelectionChanged;

    public FolderPickerTextbox()
    {
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_isLoaded)
        {
            return;
        }

        _isLoaded = true;

        ApplyTemplate();
        _btnBrowse = (Button) Template.FindName("BtnBrowse", this);
        _btnBrowse.Click += BtnBrowseOnClick;
    }

    private void BtnBrowseOnClick(object sender, RoutedEventArgs e)
    {
        FolderBrowserDialog openFileDlg = new();
        if (openFileDlg.ShowDialog() != DialogResult.Cancel)
        {
            Text = openFileDlg.SelectedPath;
            SelectionChanged?.Invoke(this, openFileDlg.SelectedPath);
        }
    }
}