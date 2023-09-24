// *********************************************************
// 
// Coho.UI SecondaryWindow.cs
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
using System.Windows.Shell;

namespace Coho.UI.Windows;

public class SecondaryWindow : FluentWindow
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

    private bool _isDialog;
    private Button? _maximizeButton;
    private Button? _restoreButton;

    public SecondaryWindow()
    {
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
        SourceInitialized += Window_SourceInitialized;

        Style = (Style) FindResource("SecondaryWindowStyle");
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

    private void Window_SourceInitialized(object? sender, EventArgs e)
    {
        OnSourceInitializedBase(this);
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
        if (!IsWindowLoaded || _maximizeButton == null || _restoreButton == null || ChromeBorder == null)
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
            ChromeBorder.Padding = new Thickness(6);
        }
        else
        {
            _maximizeButton.Visibility = Visibility.Visible;
            _maximizeButton.IsHitTestVisible = true;
            _maximizeButton.SetValue(Panel.ZIndexProperty, 2);
            _restoreButton.Visibility = Visibility.Collapsed;
            _restoreButton.IsHitTestVisible = false;
            _restoreButton.SetValue(Panel.ZIndexProperty, 0);
            ChromeBorder.Padding = new Thickness(0);
        }
    }

    private void SecondaryWindow_Loaded(object sender, RoutedEventArgs e)
    {
        IsWindowLoaded = true;

        ApplyTemplate();

        _maximizeButton = (Button) Template.FindName("BtnChromeMaximize", this);
        _restoreButton = (Button) Template.FindName("BtnChromeRestore", this);
        ChromeBorder = (Border) Template.FindName("BdrChrome", this);

        if (_restoreButton != null && _maximizeButton != null)
        {
            ChromeVirtualButtons.Add(_restoreButton);
            ChromeVirtualButtons.Add(_maximizeButton);

            _restoreButton.IsHitTestVisible = true;
            _maximizeButton.IsHitTestVisible = true;
        }
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