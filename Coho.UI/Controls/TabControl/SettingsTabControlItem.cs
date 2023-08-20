// *********************************************************
// 
// Coho.UI SettingsTabControlItem.cs
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

namespace Coho.UI.Controls.TabControl;

public class SettingsTabControlItem : TabItem
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(SettingsTabControlItem), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached(nameof(Icon), typeof(Brush), typeof(SettingsTabControlItem), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    ///     Gets or sets the text
    /// </summary>
    public string Title
    {
        get
        {
            return (string) GetValue(TitleProperty);
        }
        set
        {
            SetValue(TitleProperty, value);
        }
    }

    public Brush? Icon
    {
        get
        {
            return (Brush) GetValue(IconProperty);
        }
        set
        {
            SetValue(IconProperty, value);
        }
    }

    public SettingsTabControlItem()
    {
        Loaded+= OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (Header == null)
        {
            Header = Title;
        }
    }
}