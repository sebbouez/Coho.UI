// *********************************************************
// 
// Coho.UI
// RibbonButtonWithContentHelper.cs
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

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Coho.UI.Controls.Ribbon;

internal static class RibbonButtonWithContentHelper
{
    internal static IEnumerable<IRibbonCommand> GetChildrenItems(FrameworkElement sender, StackPanel? container)
    {
        List<IRibbonCommand> result = new();
        if (container == null)
        {
            return result;
        }

        foreach (object child in container.Children)
        {
            if (child is MenuItem menuItem)
            {
                OrphanRibbonCommand cmd = new()
                {
                    Text = menuItem.Header?.ToString() ?? string.Empty,
                    Name = menuItem.Name,
                    Gesture = menuItem.InputGestureText,
                    IsEnabled = menuItem.IsEnabled,
                    Button = menuItem
                };
                cmd.Clicked += delegate
                {
                    menuItem.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                };
                result.Add(cmd);
            }
        }

        return result;
    }
}