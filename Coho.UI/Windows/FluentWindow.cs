// *********************************************************
// 
// Coho.UI FluentWindow.cs
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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Coho.UI.Controls;
using Coho.UI.Tools;

namespace Coho.UI.Windows;

public abstract class FluentWindow : Window
{
    protected readonly List<Button> ChromeVirtualButtons = new();

    public FluentWindow()
    {
        StateChanged += OnStateChanged;
        Activated += OnActivated;
        Deactivated += OnDeactivated;

        UIController.ThemeChanged += UIControllerOnThemeChanged;
    }

    private void UIControllerOnThemeChanged(object? sender, EventArgs e)
    {
        if (IsWindowLoaded)
        {
            OnSourceInitializedBase(this);
        }
    }

    public virtual bool IsSpecialState
    {
        get;
        set;
    }

    protected void OnSourceInitializedBase(Window sender)
    {
        HwndSource? windowHandleSource = HwndSource.FromHwnd(new WindowInteropHelper(sender).Handle);
        if (windowHandleSource == null)
        {
            return;
        }

        if (EnableMica && InternalFrameworkSettings.IsWindows11 && InternalFrameworkSettings.IsMicaSupported)
        {
            ApplyMica(windowHandleSource);
        }
        else
        {
            if (Background == null || Background == Brushes.Transparent)
            {
                Background = (Brush) FindResource("Workspace2Background");
            }
        }


        try
        {
            // Permet de masquer les boutons du chrome par défaut Windows
            // sinon l'utilisation du WindowChrome WPF affiche par défaut les boutons
            // et ils se superposent aux boutons de mon chrome perso
            NativeMethods.SetWindowLong32(new HandleRef(null, windowHandleSource.Handle), NativeMethods.GWL_STYLE, NativeMethods.GetWindowLongPtr32(windowHandleSource.Handle, NativeMethods.GWL_STYLE).ToInt32() & ~NativeMethods.WS_SYSMENU);

            // Permet de gérer les évènements de mouseover
            // sur mon chrome perso, cela permet de provoquer le Snap Assist sur Windows 11
            windowHandleSource.AddHook(WndProc);
        }
        catch
        {
            // rien de spécial
        }
    }

    protected bool IsWindowLoaded
    {
        get;
        set;
    }

    protected Border? ChromeBorder
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets if the window should use the Mica effect (only Windows 11)
    /// </summary>
    public bool EnableMica
    {
        get;
        set;
    }

    private void OnStateChanged(object? sender, EventArgs e)
    {
        UpdateGlowBorder(IsActive, WindowState == WindowState.Maximized);
    }

    private void OnDeactivated(object? sender, EventArgs e)
    {
        UpdateGlowBorder(false);
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        UpdateGlowBorder(true);
    }

    protected void ApplyMica(HwndSource source)
    {
        int trueValue = 0x01;
        int falseValue = 0x00;

        int micaValue = 2;
        //None = 1,
        //Mica = 2,
        //Acrylic = 3,
        //Tabbed = 4

        if (UIController.Theme == ThemeScheme.Dark)
        {
            _ = NativeMethods.DwmSetWindowAttribute(source.Handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)));
        }
        else
        {
            _ = NativeMethods.DwmSetWindowAttribute(source.Handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref falseValue, Marshal.SizeOf(typeof(int)));
        }

        if (Environment.OSVersion.Version.Build >= 22523)
        {
            _ = NativeMethods.DwmSetWindowAttribute(source.Handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref micaValue, Marshal.SizeOf(typeof(int)));
        }
        else
        {
            _ = NativeMethods.DwmSetWindowAttribute(source.Handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));
        }
    }

    protected void ChromeButton_Clear()
    {
        foreach (Button btn in ChromeVirtualButtons)
        {
            btn.SetValue(WindowDependencyProperties.IsFakeHoverProperty, false);
        }
    }

    protected bool ChromeButton_Click(IntPtr lParam)
    {
        Button? btn = GetChromeButton(lParam);

        if (btn != null && ChromeVirtualButtons.Contains(btn) && btn.Tag is bool t && t)
        {
            btn.Tag = false;
            RoutedCommand routedCommand = (RoutedCommand) btn.Command;
            routedCommand.Execute(btn.CommandParameter, btn);
            return true;
        }

        return false;
    }

    protected IntPtr ChromeButton_Hover(IntPtr lParam, ref bool handled)
    {
        Button? btn = GetChromeButton(lParam);
        if (btn != null && ChromeVirtualButtons.Contains(btn))
        {
            btn.SetValue(WindowDependencyProperties.IsFakeHoverProperty, true);
            handled = true;
            return new IntPtr(9);
        }

        return IntPtr.Zero;
    }

    protected bool ChromeButton_Pressed(IntPtr lParam)
    {
        Button? btn = GetChromeButton(lParam);

        if (btn != null && ChromeVirtualButtons.Contains(btn))
        {
            btn.Tag = true;
            return true;
        }

        return false;
    }

    private Button? GetChromeButton(IntPtr lParam)
    {
        Point point = new(NativeMethods.GetXLParam(lParam.ToInt32Unchecked()), NativeMethods.GetYLParam(lParam.ToInt32Unchecked()));
        Point point2 = PointFromScreen(point);

        // Perform the hit test against a given portion of the visual object tree.
        HitTestResult result = VisualTreeHelper.HitTest(this, point2);

        if (result == null)
        {
            return null;
        }

        return WpfTools.GetClosestParent<Button>(result.VisualHit);
    }

    protected void UpdateGlowBorder(bool activate, bool maximized = false)
    {
        if (!InternalFrameworkSettings.IsWindows11)
        {
            if (ChromeBorder != null)
            {
                Color color = activate
                    ? (Color) FindResource("ChromeBorderActiveColor")
                    : (Color) FindResource("ChromeBorderDefaultColor");
                ChromeBorder.BorderThickness = new Thickness(1);
                ChromeBorder.BorderBrush = new SolidColorBrush(color);
                return;
            }
        }

        IntPtr handle = new WindowInteropHelper(this).Handle;

        if (handle != IntPtr.Zero && IsWindowLoaded)
        {
            Color color = activate
                ? (Color) FindResource("ChromeBorderActiveColor")
                : (Color) FindResource("ChromeBorderDefaultColor");

            if (IsSpecialState)
            {
                color = (Color) FindResource("SpacialStateColor");
            }

            NativeMethods.COLORREF colorRef = new(color);
            int attrValue = maximized ? -2 : (int) colorRef.dwColor;
            _ = NativeMethods.DwmSetWindowAttribute(handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR,
                ref attrValue, 4);
        }
    }

    private bool IsConnectedToPresentationSource()
    {
        return PresentationSource.FromDependencyObject(this) != null;
    }

    protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (!IsConnectedToPresentationSource())
        {
            return new IntPtr(0);
        }

        switch (msg)
        {
            case 512:
            case 674:
            case 675:
                ChromeButton_Clear();
                return IntPtr.Zero;

            case 161: // mousedown
                handled = ChromeButton_Pressed(lParam);
                return IntPtr.Zero;

            case 162: // mouseup ?
                handled = ChromeButton_Click(lParam);
                return IntPtr.Zero;

            case 132:
                return ChromeButton_Hover(lParam, ref handled);

            default:
                return IntPtr.Zero;
        }
    }
}