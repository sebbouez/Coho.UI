// *********************************************************
// 
// Coho.UI
// SecondaryWindow.cs
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using Coho.UI.Controls.Ribbon;

namespace Coho.UI;

public class SecondaryWindow: Window
{
    public static readonly DependencyProperty HideCloseButtonProperty =
        DependencyProperty.RegisterAttached(nameof(HideCloseButton), typeof(bool), typeof(SecondaryWindow),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ExtendContentAreaProperty =
        DependencyProperty.RegisterAttached(nameof(ExtendContentArea), typeof(bool), typeof(SecondaryWindow),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty HideWindowTitleProperty =
        DependencyProperty.RegisterAttached(nameof(HideWindowTitle), typeof(bool), typeof(SecondaryWindow),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    private Border? _bdrChrome;
    private bool _isLoaded;
    private Button? _maximizeButton;
    private Button? _restoreButton;
    private bool _isDialog;

    public SecondaryWindow()
    {
        WindowChrome.SetWindowChrome(this,
            new WindowChrome
            {
                CaptionHeight = 40,
                ResizeBorderThickness = new Thickness(6),
                CornerRadius = new CornerRadius(0),
                GlassFrameThickness = new Thickness(-1),
                UseAeroCaptionButtons = true
            });

        CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
        CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow,
            OnCanResizeWindow));
        CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow,
            OnCanMinimizeWindow));
        CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow,
            OnCanResizeWindow));

        Loaded += SecondaryWindow_Loaded;
        StateChanged += SecondaryWindow_StateChanged;
        PreviewKeyUp += OnPreviewKeyUp;
        Activated+= OnActivated;
        Deactivated+= OnDeactivated;

        Style = (Style) FindResource("SecondaryWindowStyle");
    }

    private void OnDeactivated(object? sender, EventArgs e)
    {
        UpdateGlowBorder(false);
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        UpdateGlowBorder(true);
    }

    private void UpdateGlowBorder(bool activate, bool maximized=false)
    {
        if (!InternalRibbonSettings.IsWindows11)
        {
            if (_bdrChrome != null)
            {
                Color color = activate
                    ? (Color)FindResource("ChromeBorderActiveColor")
                    : (Color)FindResource("ChromeBorderDefaultColor");
                _bdrChrome.BorderThickness = new Thickness(1);
                _bdrChrome.BorderBrush = new SolidColorBrush(color);
                return;
            }
        }

        IntPtr handle = new WindowInteropHelper(this).Handle;

        if (handle != IntPtr.Zero && _isLoaded)
        {
            Color color = activate
                ? (Color)FindResource("ChromeBorderActiveColor")
                : (Color)FindResource("ChromeBorderDefaultColor");
            NativeMethods.COLORREF colorRef = new(color);
            int attrValue = maximized ? -2 : (int)colorRef.dwColor;
            _ = NativeMethods.DwmSetWindowAttribute(handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR,
                ref attrValue, 4);
        }
    }
    
    public bool CloseOnEscapeKeyPress
    {
        get;
        set;
    }

    public bool HideCloseButton
    {
        get
        {
            return (bool) GetValue(HideCloseButtonProperty);
        }
        set
        {
            SetValue(HideCloseButtonProperty, value);
        }
    }

    public bool ExtendContentArea
    {
        get
        {
            return (bool) GetValue(ExtendContentAreaProperty);
        }
        set
        {
            SetValue(ExtendContentAreaProperty, value);
        }
    }

    public bool HideWindowTitle
    {
        get
        {
            return (bool) GetValue(HideWindowTitleProperty);
        }
        set
        {
            SetValue(HideWindowTitleProperty, value);
        }
    }

    public new bool? ShowDialog()
    {
        _isDialog = true;
        return base.ShowDialog();
    }

    private void OnPreviewKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape && CloseOnEscapeKeyPress)
        {
            if (_isDialog)
            {
                DialogResult = false;
            }

            Close();
        }
    }

    private void SecondaryWindow_StateChanged(object? sender, EventArgs e)
    {
        if (!_isLoaded || _maximizeButton == null || _restoreButton == null || _bdrChrome == null)
        {
            return;
        }

        if (WindowState == WindowState.Maximized)
        {
            _maximizeButton.Visibility = Visibility.Collapsed;
            _maximizeButton.IsHitTestVisible = false;
            _maximizeButton.SetValue(Panel.ZIndexProperty, 0);
            _restoreButton.Visibility = Visibility.Visible;
            _restoreButton.IsHitTestVisible = true;
            _restoreButton.SetValue(Panel.ZIndexProperty, 2);
            _bdrChrome.Padding = new Thickness(6);
        }
        else
        {
            _maximizeButton.Visibility = Visibility.Visible;
            _maximizeButton.IsHitTestVisible = true;
            _maximizeButton.SetValue(Panel.ZIndexProperty, 2);
            _restoreButton.Visibility = Visibility.Collapsed;
            _restoreButton.IsHitTestVisible = false;
            _restoreButton.SetValue(Panel.ZIndexProperty, 0);
            _bdrChrome.Padding = new Thickness(0);
        }
    }

    private void SecondaryWindow_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyTemplate();

        _maximizeButton = (Button) Template.FindName("BtnChromeMaximize", this);
        _restoreButton = (Button) Template.FindName("BtnChromeRestore", this);
        _bdrChrome = (Border) Template.FindName("BdrChrome", this);

        _isLoaded = true;
    }

    #region WindowChrome

    private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = ResizeMode != ResizeMode.NoResize;
    }

    private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
    }

    private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
    {
        SystemCommands.CloseWindow(this);
    }

    private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
    {
        SystemCommands.MaximizeWindow(this);
    }

    private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
    {
        SystemCommands.MinimizeWindow(this);
    }

    private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
    {
        SystemCommands.RestoreWindow(this);
    }

    #endregion
}