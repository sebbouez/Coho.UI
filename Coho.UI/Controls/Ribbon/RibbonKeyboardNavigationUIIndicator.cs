// *********************************************************
// 
// Coho.UI
// RibbonKeyboardNavigationUIIndicator.cs
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

using System.ComponentModel;

namespace Coho.UI.Controls.Ribbon;

internal class RibbonKeyboardNavigationUIIndicator : INotifyPropertyChanged
{
    private bool _showTips;

    public RibbonKeyboardNavigationUIIndicator()
    {
        InternalFrameworkSettings.KeyboardNavigationUIIndicator = this;
    }

    public bool ShowTips
    {
        get
        {
            return _showTips;
        }
        set
        {
            _showTips = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowTips)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}