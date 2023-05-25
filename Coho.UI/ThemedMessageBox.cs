// *********************************************************
// 
// Coho.UI
// ThemedMessageBox.cs
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

public static class ThemedMessageBox
{
    
    /// <summary>
    /// Shows a messagebox that supports the themed UI
    /// </summary>
    /// <param name="message">Message to display in the center area</param>
    /// <param name="title">Title of the dialog</param>
    /// <param name="button">Buttons to display</param>
    /// <returns></returns>
    public static MessageBoxResult Show(string message, string title, MessageBoxButton button)
    {
        return Show(message, title, Application.Current.MainWindow!, button);
    }

    /// <summary>
    /// Shows a messagebox that supports the themed UI
    /// </summary>
    /// <param name="message">Message to display in the center area</param>
    /// <param name="title">Title of the dialog</param>
    /// <param name="button">Buttons to display</param>
    /// <param name="defaultButtonText">Text of the default button</param>
    /// <param name="secondaryButtonText">Text of the secondary button</param>
    /// <returns></returns>
    public static MessageBoxResult Show(string message, string title, MessageBoxButton button, string defaultButtonText, string secondaryButtonText)
    {
        return Show(message, title, Application.Current.MainWindow!, button, defaultButtonText, secondaryButtonText);
    }

    /// <summary>
    /// Shows a messagebox that supports the themed UI
    /// </summary>
    /// <param name="message">Message to display in the center area</param>
    /// <param name="title">Title of the dialog</param>
    /// <param name="owner">Dialog owner to display on top of</param>
    /// <param name="button">Buttons to display</param>
    /// <param name="defaultButtonText">Text of the default button</param>
    /// <param name="secondaryButtonText">Text of the secondary button</param>
    /// <returns></returns>
    public static MessageBoxResult Show(string message, string title, Window owner, MessageBoxButton button, string? defaultButtonText = null, string? secondaryButtonText = null)
    {
        MessageBoxDialog dlg = new();

        switch (button)
        {
            case MessageBoxButton.OK:
                dlg.BtnOk.Visibility = Visibility.Visible;
                dlg.BtnCancel.Visibility = Visibility.Collapsed;
                dlg.BtnYes.Visibility = Visibility.Collapsed;
                dlg.BtnNo.Visibility = Visibility.Collapsed;
                break;

            case MessageBoxButton.OKCancel:
                dlg.BtnOk.Visibility = Visibility.Visible;
                dlg.BtnCancel.Visibility = Visibility.Visible;
                dlg.BtnYes.Visibility = Visibility.Collapsed;
                dlg.BtnNo.Visibility = Visibility.Collapsed;
                break;

            case MessageBoxButton.YesNoCancel:
                dlg.BtnOk.Visibility = Visibility.Collapsed;
                dlg.BtnCancel.Visibility = Visibility.Visible;
                dlg.BtnYes.Visibility = Visibility.Visible;
                dlg.BtnNo.Visibility = Visibility.Visible;

                if (!string.IsNullOrEmpty(defaultButtonText))
                {
                    dlg.BtnYes.Content = defaultButtonText;
                    dlg.BtnYes.Style = (Style) dlg.FindResource("PrimaryButton");
                }

                if (!string.IsNullOrEmpty(secondaryButtonText))
                {
                    dlg.BtnNo.Content = secondaryButtonText;
                }

                break;

            case MessageBoxButton.YesNo:
                dlg.BtnOk.Visibility = Visibility.Collapsed;
                dlg.BtnCancel.Visibility = Visibility.Collapsed;
                dlg.BtnYes.Visibility = Visibility.Visible;
                dlg.BtnNo.Visibility = Visibility.Visible;

                if (!string.IsNullOrEmpty(defaultButtonText))
                {
                    dlg.BtnYes.Content = defaultButtonText;
                    dlg.BtnYes.Style = (Style) dlg.FindResource("PrimaryButton");
                }

                if (!string.IsNullOrEmpty(secondaryButtonText))
                {
                    dlg.BtnNo.Content = secondaryButtonText;
                }

                break;
        }

        dlg.Owner = owner;
        dlg.Title = title;
        dlg.TbTitle.Text = title;
        dlg.TbMessage.Text = message;
        dlg.HideWindowTitle = true;
        dlg.ExtendContentArea = true;
        dlg.ShowDialog();

        if (dlg.DataContext is MessageBoxResult r)
        {
            return r;
        }

        return MessageBoxResult.Cancel;
    }
}