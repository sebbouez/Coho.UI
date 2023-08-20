// *********************************************************
// 
// Coho.UI TreeViewItemExtensions.cs
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
using System.Windows.Controls;
using System.Windows.Media;

namespace Coho.UI.Tools;

public static class TreeViewItemExtensions
{
    private static TreeViewItem GetParent(TreeViewItem item)
    {
        DependencyObject parent = VisualTreeHelper.GetParent(item);
        while (!(parent is TreeViewItem || parent is TreeView))
        {
            parent = VisualTreeHelper.GetParent(parent);
        }
        return parent as TreeViewItem;
    }

    public static int GetDepth(this TreeViewItem item)
    {
        TreeViewItem parent;
        while ((parent = GetParent(item)) != null)
        {
            return GetDepth(parent) + 1;
        }
        return 0;
    }
}