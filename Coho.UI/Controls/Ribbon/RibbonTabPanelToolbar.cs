// *********************************************************
// 
// Coho.UI
// RibbonTabPanelToolbar.cs
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

using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Coho.UI.Controls.Ribbon;

public sealed class RibbonTabPanelToolbar : ToolBar
{
    public RibbonTabPanelToolbar()
    {
        Style = (Style) FindResource("RibbonTabPanelToolbarStyle");
        SizeChanged += AnimatedToolbar_SizeChanged;
    }

    private void AnimatedToolbar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        foreach (RibbonTabItem tab in Items.OfType<RibbonTabItem>())
        {
            tab.IsOverflown = false;
        }
    }
}