// *********************************************************
// 
// Coho.UI InfoBar.cs
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static Coho.UI.Enums;

namespace Coho.UI.Controls.InfoBar;

public class InfoBar : Control
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached(nameof(Icon), typeof(Brush), typeof(InfoBar), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(nameof(Text), typeof(string), typeof(InfoBar), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(InfoBar), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ModeProperty =
        DependencyProperty.RegisterAttached(nameof(Mode), typeof(InfoBarMode), typeof(InfoBar), new FrameworkPropertyMetadata(InfoBarMode.Info, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty HasClickEventAttachedProperty =
        DependencyProperty.RegisterAttached(nameof(HasClickEventAttached), typeof(bool), typeof(InfoBar), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    private Border? _borderMain;
    private Action? _clickAction;
    private Button? _closeButton;
    private bool _isVisible;

    public InfoBar()
    {
        Loaded += InfoBar_Loaded;
    }

    public bool HasClickEventAttached
    {
        get
        {
            return (bool) GetValue(HasClickEventAttachedProperty);
        }
        set
        {
            SetValue(HasClickEventAttachedProperty, value);
        }
    }

    public Brush? Icon
    {
        get
        {
            return (Brush?) GetValue(IconProperty);
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

    public void Show()
    {
        _isVisible = true;
        Visibility = Visibility.Visible;
    }

    public void Show(string title, string text, Action? action = null)
    {
        Show();
        Title = title;
        Text = text;
        _clickAction = action;
        HasClickEventAttached = action != null;
    }

    public void Hide()
    {
        _isVisible = false;
        _clickAction = null;
        Visibility = Visibility.Collapsed;
    }

    private void InfoBar_Loaded(object sender, RoutedEventArgs e)
    {
        _ = ApplyTemplate();

        _closeButton = (Button) Template.FindName("ButtonCloseBar", this);
        _borderMain = (Border) Template.FindName("BorderMain", this);

        if (_closeButton != null)
        {
            _closeButton.Click -= _closeButton_Click;
            _closeButton.Click += _closeButton_Click;
        }

        if (_borderMain != null)
        {
            _borderMain.PreviewMouseDown -= BorderMainOnPreviewMouseDown;
            _borderMain.PreviewMouseDown += BorderMainOnPreviewMouseDown;
        }

        Visibility = _isVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    private void BorderMainOnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.Source is Button btn && string.Equals(btn.Name, "ButtonCloseBar", StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        if (e.Source is Rectangle)
        {
            return;
        }

        if (_clickAction != null)
        {
            _clickAction();
        }
    }

    private void _closeButton_Click(object sender, RoutedEventArgs e)
    {
        Closing?.Invoke(this, e);
        Visibility = Visibility.Collapsed;
        e.Handled = true;
    }
}