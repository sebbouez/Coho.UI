// *********************************************************
// 
// Coho.UI InfobarManager.cs
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coho.UI.Controls.InfoBar;

public class InfoBarManager : StackPanel
{
    private readonly List<string> _tagCache = new();

    public void AddInfoBar(Brush? icon, string title, string text, Enums.InfoBarMode mode, Action? clickHandler = null)
    {
        InfoBar newbar = new()
        {
            Icon = icon,
            Mode = mode,
        };
        newbar.Closing += BarOnClosing;
        Children.Add(newbar);
        newbar.Show(title, text, clickHandler);
        InternalUpdateLayout();
    }

    public void AddInfoBar(string tag, Brush? icon, string title, string text, Enums.InfoBarMode mode, Action? clickHandler = null)
    {
        lock (_tagCache)
        {
            bool exists = _tagCache.Any(x => string.Equals(tag, x, StringComparison.InvariantCultureIgnoreCase));
            if (exists)
            {
                return;
            }

            _tagCache.Add(tag);

            InfoBar newbar = new()
            {
                Tag = tag,
                Icon = icon,
                Mode = mode,
            };
            newbar.Closing += BarOnClosing;
            Children.Add(newbar);
            newbar.Show(title, text, clickHandler);
            InternalUpdateLayout();
        }
    }

    /// <summary>
    /// Clears all current messages
    /// </summary>
    public void Clear()
    {
        lock (_tagCache)
        {
            _tagCache.Clear();
            Children.Clear();
            InternalUpdateLayout();
        }
    }

    public void Remove(string tag)
    {
        lock (_tagCache)
        {
            IEnumerable<InfoBar> toRemove = Children.OfType<InfoBar>().Where(x => x.Tag != null && x.Tag.ToString() == tag);

            _tagCache.Remove(tag);
            foreach (InfoBar item in toRemove)
            {
                Children.Remove(item);
            }
        }
    }

    private void BarOnClosing(object sender, RoutedEventArgs e)
    {
        Children.Remove((UIElement) sender);
        InternalUpdateLayout();
    }

    private void InternalUpdateLayout()
    {
        if (Children.Count > 0)
        {
            Margin = new Thickness(8, 0, 8, 4);
        }
        else
        {
            Margin = new Thickness(0);
        }

        foreach (InfoBar infoBar in Children.OfType<InfoBar>())
        {
            infoBar.Margin = Children.Count > 1 ? new Thickness(0, 0, 0, 4) : new Thickness(0);
        }
    }
}