// *********************************************************
// 
// Coho.UI
// AcrylicContextMenu.cs
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
using System.Windows.Controls;
using System.Windows.Interop;

namespace Coho.UI.Controls.Menus;

public class AcrylicContextMenu : ContextMenu
{
    protected override void OnOpened(RoutedEventArgs e)
    {
        base.OnOpened(e);

        if (InternalFrameworkSettings.IsWindows11)
        {
            HwndSource? hwnd = (HwndSource?) PresentationSource.FromVisual(this);
            if (hwnd != null)
            {
                AcrylicHelper.SetBorder(hwnd.Handle);
                AcrylicHelper.SetBlur(hwnd.Handle);
                AcrylicHelper.SetBorderColor(hwnd.Handle);
            }
        }
        else
        {
            Style = (Style) FindResource("LegacyContextMenuStyle");
        }
    }
}