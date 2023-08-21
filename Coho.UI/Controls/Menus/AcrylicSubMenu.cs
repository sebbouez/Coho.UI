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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;

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
        else
        {
            if (Child is Border checkBdr && !string.IsNullOrEmpty(checkBdr.Name) && checkBdr.Name == "LegacyPopupContainerBorder")
            {
                return;
            }
            
            UIElement? oldChild = Child;
            Child = null;

            Border bdr = new()
            {
                Name = "LegacyPopupContainerBorder",
                Margin = new Thickness(14, 0, 14, 14),
                Effect = new DropShadowEffect()
                {
                    Color = (Color) FindResource("ShadowColor"),
                    Opacity = 0.6, 
                    Direction = -90,
                    BlurRadius = 20,
                    ShadowDepth = 4
                }
            };

            Border innerBdr = new()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = (Brush) FindResource("WindowBorder"),
                MinWidth = 200,
                CornerRadius = new CornerRadius(8),
                Background = (Brush) FindResource("MenuBackground")
            };
            bdr.Child = innerBdr;
            innerBdr.Child = oldChild;

            Child = bdr;
        }
    }
}