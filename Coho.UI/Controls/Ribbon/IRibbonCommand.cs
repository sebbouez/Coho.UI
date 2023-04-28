// *********************************************************
// 
// Coho.UI
// IRibbonCommand.cs
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
using System.Windows.Media;

namespace Coho.UI.Controls.Ribbon;

internal interface IRibbonCommand
{
    string Description
    {
        get;
        set;
    }

    RibbonEnums.RibbonButtonDisplay Display
    {
        get;
        set;
    }

    string Gesture
    {
        get;
        set;
    }

    Brush? Icon
    {
        get;
        set;
    }

    bool IsEnabled
    {
        get;
        set;
    }

    bool IsInQAT
    {
        get;
        internal set;
    }

    bool LockEnabledState
    {
        get;
        set;
    }

    string Name
    {
        get;
        set;
    }

    internal IRibbonCommand? OriginalCommand
    {
        get;
        set;
    }

    string Text
    {
        get;
        set;
    }

    internal event RoutedEventHandler OnClick;

    internal void RaiseClick();
}