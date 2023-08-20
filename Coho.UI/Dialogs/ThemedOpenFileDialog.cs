// *********************************************************
// 
// Coho.UI ThemedOpenFileDialog.cs
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

using System.Collections.Generic;
using System.Windows;

namespace Coho.UI.Dialogs;

public static class ThemedOpenFileDialog
{
    public static string? Show(string title, Dictionary<string, string> fileTypes, Window owner)
    {
        ThemedSpecialDialogOptions options = new()
        {
            FileTypes = fileTypes
        };
        return Show(title, options, owner);
    }

    /// <summary>
    ///     Shows a messagebox that supports the themed UI
    /// </summary>
    /// <param name="title">Title of the dialog</param>
    /// <param name="options"></param>
    /// <param name="owner">Dialog owner to display on top of</param>
    /// <param name="defaultButtonText">Text of the default button</param>
    /// <param name="secondaryButtonText">Text of the secondary button</param>
    /// <returns></returns>
    public static string? Show(string title, ThemedSpecialDialogOptions options, Window owner, string? defaultButtonText = null, string? secondaryButtonText = null)
    {
        OpenFileDialog dlg = new()
        {
            Owner = owner,
            Title = title,
            Options = options,
            Width = options.DefaultWidth,
            Height = options.DefaultHeight
        };

        if (!string.IsNullOrEmpty(defaultButtonText))
        {
            dlg.BtnOpen.Content = defaultButtonText;
            dlg.BtnOpen.Style = (Style) dlg.FindResource("PrimaryButton");
        }

        if (!string.IsNullOrEmpty(secondaryButtonText))
        {
            dlg.BtnCancel.Content = secondaryButtonText;
        }

        if (dlg.ShowDialog()!.Value)
        {
            return dlg.FileName;
        }

        return null;
    }
}