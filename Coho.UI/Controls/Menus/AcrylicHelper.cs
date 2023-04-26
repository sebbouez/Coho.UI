// *********************************************************
// 
// Coho.UI
// AcrylicHelper.cs
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
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace Coho.UI.Controls.Menus;

internal static class AcrylicHelper
{
    internal static void SetBlur(IntPtr hwnd)
    {
        AccentPolicy accent = new();
        int accentStructSize = Marshal.SizeOf(accent);
        accent.AccentState = AccentState.AccentEnableAcrylicBlurBehind;

        //accent.AccentFlags = 0x20 | 0x40 | 0x80 | 0x100;
        // on utilise le paramètre de window au lieu de popup, car il donne un meilleur effet blurry
        accent.AccentFlags = 2;

        //if (UserState.UserSettings.DarkTheme)
        //{
        //    accent.GradientColor = 0x99161616;  
        //}
        //else
        {
            accent.GradientColor = 0x00FFFFFF;
        }

        IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
        Marshal.StructureToPtr(accent, accentPtr, false);

        WindowCompositionAttributeData data = new();
        data.Attribute = WindowCompositionAttribute.WcaAccentPolicy;
        data.SizeOfData = accentStructSize;
        data.Data = accentPtr;
        SetWindowCompositionAttribute(hwnd, ref data);
        Marshal.FreeHGlobal(accentPtr);
    }

    internal static void SetBorderColor(IntPtr handle)
    {
        Color color = ((SolidColorBrush) Application.Current.MainWindow!.FindResource("WindowBorder")).Color;
        NativeMethods.COLORREF colorref = new(color);
        int attrValue = (int) colorref.dwColor;
        _ = NativeMethods.DwmSetWindowAttribute(handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, ref attrValue, 4);
    }

    [DllImport("user32.dll")]
    internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    public static void SetBorder(IntPtr handle)
    {
        NativeMethods.DWMWINDOWATTRIBUTE attribute = NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
        int pref = (int) DwmWindowCornerPreference.DwmwcpRound;
        NativeMethods.DwmSetWindowAttribute(handle, attribute, ref pref, sizeof(uint));
    }

    internal enum AccentState
    {
        AccentDisabled = 0,
        AccentEnableGradient = 1,
        AccentEnableTransparentGradient = 2,
        AccentEnableBlurBehind = 3,
        AccentEnableAcrylicBlurBehind = 4,
        AccentInvalidState = 5
    }

    // The DWM_WINDOW_CORNER_PREFERENCE enum for DwmSetWindowAttribute's third parameter, which tells the function
    // what value of the enum to set.
    internal enum DwmWindowCornerPreference
    {
        DwmwcpDefault = 0,
        DwmwcpDonotround = 1,
        DwmwcpRound = 2,
        DwmwcpRoundsmall = 3
    }

    internal enum WindowCompositionAttribute
    {
        WcaAccentPolicy = 19
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public uint GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }
}