// *********************************************************
// 
// Coho.UI
// AcrylicSubMenu.cs
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
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace Coho.UI.Controls.Menus;

public class AcrylicSubMenu : Popup
{
    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        if (InternalFrameworkSettings.IsWindows11)
        {
            HwndSource? hwnd = (HwndSource?) PresentationSource.FromVisual(Child);
            if (hwnd != null)
            {
                AcrylicHelper.SetBlur(hwnd.Handle);
                AcrylicHelper.SetBorder(hwnd.Handle);
                AcrylicHelper.SetBorderColor(hwnd.Handle);
            }
        }
    }
}