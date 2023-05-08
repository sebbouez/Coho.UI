// *********************************************************
// 
// Coho.UI
// NativeMethods.cs
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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace Coho.UI;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class NativeMethods
{
    internal const int GWL_STYLE = -16;

    internal const int WS_SYSMENU = 0x80000;

    internal static int ToInt32Unchecked(this IntPtr value)
    {
        return (int) value.ToInt64();
    }

    [DllImport("dwmapi.dll")]
    internal static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attr, ref int attrValue, int attrSize);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    internal static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    internal static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetCursorPos(ref Win32Point pt);

    internal static int GetXLParam(int lParam)
    {
        return LoWord(lParam);
    }

    internal static int GetYLParam(int lParam)
    {
        return HiWord(lParam);
    }

    internal static int HiWord(int value)
    {
        return (short) (value >> 16);
    }

    internal static int LoWord(int value)
    {
        return (short) (value & 0xFFFF);
    }

    internal enum DWMWINDOWATTRIBUTE : uint
    {
        DWMWA_NCRENDERING_ENABLED = 1u,
        DWMWA_NCRENDERING_POLICY = 2u,
        DWMWA_TRANSITIONS_FORCEDISABLED = 3u,
        DWMWA_ALLOW_NCPAINT = 4u,
        DWMWA_CAPTION_BUTTON_BOUNDS = 5u,
        DWMWA_NONCLIENT_RTL_LAYOUT = 6u,
        DWMWA_FORCE_ICONIC_REPRESENTATION = 7u,
        DWMWA_FLIP3D_POLICY = 8u,
        DWMWA_EXTENDED_FRAME_BOUNDS = 9u,
        DWMWA_HAS_ICONIC_BITMAP = 10u,
        DWMWA_DISALLOW_PEEK = 11u,
        DWMWA_EXCLUDED_FROM_PEEK = 12u,
        DWMWA_CLOAK = 13u,
        DWMWA_CLOAKED = 14u,
        DWMWA_FREEZE_REPRESENTATION = 0xFu,
        DWMWA_PASSIVE_UPDATE_MODE = 0x10u,
        DWMWA_USE_HOSTBACKDROPBRUSH = 17u,
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20u,
        DWMWA_WINDOW_CORNER_PREFERENCE = 33u,
        DWMWA_BORDER_COLOR = 34u,
        DWMWA_CAPTION_COLOR = 35u,
        DWMWA_TEXT_COLOR = 36u,
        DWMWA_VISIBLE_FRAME_BORDER_THICKNESS = 37u,
        DWMWA_MICA_EFFECT = 1029u,
        DWMWA_SYSTEMBACKDROP_TYPE = 38,
        DWMWA_LAST
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32Point
    {
        public int X;
        public int Y;
    }

    internal struct COLORREF
    {
        public uint dwColor;

        public COLORREF(uint dwColor)
        {
            this.dwColor = dwColor;
        }

        public COLORREF(Color color)
        {
            dwColor = (uint) (color.R + (color.G << 8) + (color.B << 16));
        }

        public Color GetMediaColor()
        {
            return Color.FromRgb((byte) (0xFFu & dwColor), (byte) ((0xFF00 & dwColor) >> 8), (byte) ((0xFF0000 & dwColor) >> 16));
        }
    }
}