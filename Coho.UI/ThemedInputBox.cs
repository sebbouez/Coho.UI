// *********************************************************
// 
// Coho.UI ThemedInputBox.cs
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
using Coho.UI.Dialogs;

namespace Coho.UI;

public static class ThemedInputBox
{
    /// <summary>
    ///     Shows a messagebox that supports the themed UI
    /// </summary>
    /// <param name="message">Message to display in the center area</param>
    /// <param name="title">Title of the dialog</param>
    /// <param name="defaultValue">Default value to display</param>
    /// <returns></returns>
    public static string? Show(string message, string title, string defaultValue = "")
    {
        return Show(message, title, Application.Current.MainWindow!, defaultValue);
    }

    /// <summary>
    ///     Shows a messagebox that supports the themed UI
    /// </summary>
    /// <param name="message">Message to display in the center area</param>
    /// <param name="title">Title of the dialog</param>
    /// <param name="defaultValue">Default value to display</param>
    /// <param name="defaultButtonText">Text of the default button</param>
    /// <param name="secondaryButtonText">Text of the secondary button</param>
    /// <returns></returns>
    public static string? Show(string message, string title, string defaultValue, string defaultButtonText, string secondaryButtonText)
    {
        return Show(message, title, Application.Current.MainWindow!, defaultValue, defaultButtonText, secondaryButtonText);
    }

    /// <summary>
    ///     Shows a messagebox that supports the themed UI
    /// </summary>
    /// <param name="message">Message to display in the center area</param>
    /// <param name="title">Title of the dialog</param>
    /// <param name="owner">Dialog owner to display on top of</param>
    /// <param name="defaultValue">Default value to display</param>
    /// <param name="defaultButtonText">Text of the default button</param>
    /// <param name="secondaryButtonText">Text of the secondary button</param>
    /// <returns></returns>
    public static string? Show(string message, string title, Window owner, string defaultValue = "", string? defaultButtonText = null, string? secondaryButtonText = null)
    {
        InputBoxDialog dlg = new();

        if (!string.IsNullOrEmpty(defaultButtonText))
        {
            dlg.BtnOk.Content = defaultButtonText;
            dlg.BtnOk.Style = (Style) dlg.FindResource("PrimaryButton");
        }

        if (!string.IsNullOrEmpty(secondaryButtonText))
        {
            dlg.BtnCancel.Content = secondaryButtonText;
        }

        dlg.Owner = owner;
        dlg.Value = defaultValue;
        dlg.Title = title;
        dlg.TbTitle.Text = title;
        dlg.TbMessage.Text = message;
        dlg.HideWindowTitle = true;
        dlg.ExtendContentArea = true;
        if (dlg.ShowDialog()!.Value)
        {
            return dlg.Value;
        }

        return null;
    }
}