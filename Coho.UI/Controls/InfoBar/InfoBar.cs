// *********************************************************
// 
// Coho.UI
// InfoBar.cs
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
using static Coho.UI.Enums;

namespace Coho.UI.Controls.InfoBar;

public class InfoBar : Control
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached("Icon", typeof(Brush), typeof(InfoBar), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached("Text", typeof(string), typeof(InfoBar), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ModeProperty =
        DependencyProperty.RegisterAttached("Mode", typeof(InfoBarMode), typeof(InfoBar), new FrameworkPropertyMetadata(InfoBarMode.Info, FrameworkPropertyMetadataOptions.AffectsRender));

    private Button? _closeButton;

    public InfoBar()
    {
        Loaded += InfoBar_Loaded;
    }

    public Brush Icon
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

    public string Text
    {
        get
        {
            return (string) GetValue(TextProperty);
        }
        set
        {
            SetValue(TextProperty, value);
        }
    }

    public InfoBarMode Mode
    {
        get
        {
            return (InfoBarMode) GetValue(ModeProperty);
        }
        set
        {
            SetValue(ModeProperty, value);
        }
    }

    public event RoutedEventHandler? Closing;

    private void InfoBar_Loaded(object sender, RoutedEventArgs e)
    {
        _ = ApplyTemplate();

        _closeButton = (Button) Template.FindName("ButtonCloseBar", this);

        if (_closeButton != null)
        {
            _closeButton.Click += CloseButton_Click;
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Closing?.Invoke(this, e);
    }
}