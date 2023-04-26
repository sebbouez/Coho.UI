// *********************************************************
// 
// Coho.UI
// OrphanRibbonCommand.cs
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
using System.Windows.Media;

namespace Coho.UI.Controls.Ribbon;

internal class OrphanRibbonCommand : IRibbonCommand
{
    internal FrameworkElement? Button
    {
        get;
        set;
    }

    public event RoutedEventHandler? OnClick;

    public string Description
    {
        get;
        set;
    } = string.Empty;

    public RibbonEnums.RibbonButtonDisplay Display
    {
        get;
        set;
    }

    public string Gesture
    {
        get;
        set;
    } = string.Empty;

    public Brush? Icon
    {
        get;
        set;
    }

    public bool IsEnabled
    {
        get
        {
            return Button != null ? Button.IsEnabled : false;
        }
        set
        {
            // ne rien faire ici
        }
    }

    public bool IsInQAT
    {
        get;
        set;
    }

    public bool LockEnabledState
    {
        get;
        set;
    } = false;

    public string Name
    {
        get;
        set;
    } = string.Empty;

    public IRibbonCommand? OriginalCommand
    {
        get;
        set;
    }

    public string Text
    {
        get;
        set;
    } = string.Empty;

    public void RaiseClick()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? Clicked;
}