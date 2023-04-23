// *********************************************************
// 
// Coho.UI
// ContextMenuHolder.cs
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
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Animation;

namespace Coho.UI.Controls.Menus;

public class ContextMenuHolder : Decorator
{
    private ContextMenu? _contextMenu;

    public ContextMenuHolder()
    {
        Loaded += ContextMenuHolder_Loaded;
    }

    private void ContextMenu_Opened(object sender, RoutedEventArgs e)
    {
        Point mousePoint = GetMousePosition();
        IntPtr targetHwnd = (PresentationSource.FromVisual(_contextMenu!.PlacementTarget) as HwndSource)!.Handle;
        Screen screen = Screen.FromHandle(targetHwnd);

        bool isOutsideRight = false;
        bool isOutsideBottom = false;

        if (mousePoint.X + _contextMenu.ActualWidth > screen.WorkingArea.Width)
        {
            isOutsideRight = true;
        }

        if (mousePoint.Y + _contextMenu.ActualHeight > screen.WorkingArea.Height)
        {
            isOutsideBottom = true;
        }

        if (isOutsideBottom)
        {
            _contextMenu.VerticalOffset = 20;
        }
        else
        {
            _contextMenu.VerticalOffset = -20;
        }

        if (isOutsideRight)
        {
            _contextMenu.HorizontalOffset = 20;
        }
        else
        {
            _contextMenu.HorizontalOffset = -20;
        }

        _contextMenu.UpdateLayout();
        ((Popup) _contextMenu.Parent).UpdateLayout();
    }

    private void ContextMenuHolder_Loaded(object sender, RoutedEventArgs e)
    {
        _contextMenu = (ContextMenu) TemplatedParent;
        _contextMenu.Opened -= ContextMenu_Opened;
        _contextMenu.Opened += ContextMenu_Opened;
        if (IsVisible)
        {
            Thickness adjustedMargin = new(0);
            ThicknessAnimation marginAnim = new(adjustedMargin, TimeSpan.Zero);
            BeginAnimation(MarginProperty, marginAnim);
            UpdateLayout();
        }
    }

    public static Point GetMousePosition()
    {
        NativeMethods.Win32Point w32Mouse = new();
        NativeMethods.GetCursorPos(ref w32Mouse);
        return new Point(w32Mouse.X, w32Mouse.Y);
    }
}