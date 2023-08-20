// *********************************************************
// 
// Coho.UI
// InternalRibbonSettings.cs
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
using Coho.UI.Controls.Ribbon;
using Coho.UI.Interfaces;

namespace Coho.UI;

internal static class InternalFrameworkSettings
{
    internal static IApplicationMainBarControl? CurrentMainBarControl
    {
        get;
        set;
    }

    internal static RibbonKeyboardNavigationUIIndicator? KeyboardNavigationUIIndicator
    {
        get;
        set;
    }
    
    internal static bool IsWindows11
    {
        get
        {
            return Environment.OSVersion.Version.Build >= 22000;
        }
    }

    internal static bool IsMicaSupported
    {
        get
        {
            return (Environment.OSVersion.Version.Build >= 22000
                    && Environment.OSVersion.Version.Build <= 22400) ||
                   Environment.OSVersion.Version.Build >= 22523;
        }
    }
}