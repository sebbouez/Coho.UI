// *********************************************************
// 
// Coho.UI
// ApplicationWindow.cs
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Shell;
using Coho.UI.Controls.Omnibar;
using Button = System.Windows.Controls.Button;
using Control = System.Windows.Controls.Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Panel = System.Windows.Controls.Panel;

namespace Coho.UI.Windows;

public class ApplicationWindow : FluentWindow
{
    private Control? _backstageView;
    private Grid? _chromeHeaderGrid;
    private OmnibarControl? _chromeOmnibar;
    private StackPanel? _chromeTitleButtonsStackPanel;
    private ContentPresenter? _mainContentPresenter;
    private Grid? _mainGrid;
    private Button? _maximizeButton;
    private Rectangle? _micaShadeRectangle;
    private Button? _restoreButton;

    public ApplicationWindow()
    {
        WindowChrome.SetWindowChrome(this, new WindowChrome
        {
            CaptionHeight = 40,
            ResizeBorderThickness = new Thickness(6),
            CornerRadius = new CornerRadius(0),
            GlassFrameThickness = new Thickness(-1),
            UseAeroCaptionButtons = true
        });

        CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
        CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
        CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
        CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));

        Loaded += ApplicationWindow_Loaded;
        StateChanged += ApplicationWindow_StateChanged;
        PreviewKeyDown += ApplicationWindow_PreviewKeyDown;
        Deactivated += ApplicationWindow_Deactivated;
        SourceInitialized += Window_SourceInitialized;

        MinHeight = 650;
        MinWidth = 750;

        Style = (Style) FindResource("ApplicationWindowStyle");
    }

    private void ApplicationWindow_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (InternalFrameworkSettings.CurrentMainBarControl == null)
        {
            return;
        }

        Keys key = (Keys) KeyInterop.VirtualKeyFromKey(e.Key);

        if (Keyboard.Modifiers == ModifierKeys.Alt)
        {
            if (!InternalFrameworkSettings.CurrentMainBarControl.HandleKeyboardNavigation(key))
            {
                ToggleKeyboardNav();
            }
        }

        if (InternalFrameworkSettings.KeyboardNavigationUIIndicator != null)
        {
            if (InternalFrameworkSettings.KeyboardNavigationUIIndicator.ShowTips || Keyboard.Modifiers == ModifierKeys.Alt)
            {
                if (InternalFrameworkSettings.CurrentMainBarControl.HandleKeyboardNavigation(key))
                {
                    ToggleKeyboardNav();
                }
            }

            if (InternalFrameworkSettings.KeyboardNavigationUIIndicator.ShowTips && key.ToString().Contains("NumPad", StringComparison.InvariantCultureIgnoreCase))
            {
                if (InternalFrameworkSettings.CurrentMainBarControl.HandleKeyboardNavigation(key))
                {
                    InternalFrameworkSettings.KeyboardNavigationUIIndicator.ShowTips = false;
                }
            }
        }
    }

    private void ToggleKeyboardNav()
    {
        if (InternalFrameworkSettings.KeyboardNavigationUIIndicator != null)
        {
            InternalFrameworkSettings.KeyboardNavigationUIIndicator.ShowTips = !InternalFrameworkSettings.KeyboardNavigationUIIndicator.ShowTips;
        }
    }

    private void Window_SourceInitialized(object? sender, EventArgs e)
    {
        OnSourceInitializedBase(this);
    }

    #region Properties

    public static readonly DependencyProperty IsSpecialStateProperty =
        DependencyProperty.RegisterAttached(nameof(IsSpecialState), typeof(bool), typeof(ApplicationWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ApplyAccentToChromeProperty =
        DependencyProperty.RegisterAttached(nameof(ApplyAccentToChrome), typeof(bool), typeof(ApplicationWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ShowOmnibarProperty =
        DependencyProperty.RegisterAttached(nameof(ShowOmnibar), typeof(bool), typeof(ApplicationWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public new static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached(nameof(Icon), typeof(Brush), typeof(ApplicationWindow), new FrameworkPropertyMetadata(Brushes.Silver, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty MasterMessageProperty =
        DependencyProperty.RegisterAttached(nameof(MasterMessage), typeof(string), typeof(ApplicationWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

    public override bool IsSpecialState
    {
        get
        {
            return (bool) GetValue(IsSpecialStateProperty);
        }
        set
        {
            SetValue(IsSpecialStateProperty, value);
            UpdateGlowBorder(IsActive, WindowState == WindowState.Maximized);
        }
    }

    /// <summary>
    ///     Gets or sets if the chrome title bar should use the application accent color
    /// </summary>
    public bool ApplyAccentToChrome
    {
        get
        {
            return (bool) GetValue(ApplyAccentToChromeProperty);
        }
        set
        {
            SetValue(ApplyAccentToChromeProperty, value);
        }
    }

    public string? MasterMessage
    {
        get
        {
            return (string?) GetValue(MasterMessageProperty);
        }
        set
        {
            SetValue(MasterMessageProperty, value);
        }
    }

    /// <summary>
    ///     Gets or sets the list of buttons that appear next to the application title
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    public List<UIElement> ChromeTitleButtons
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the visibility of a subtle shade over the Mica effect
    /// </summary>
    public bool ShowMicaShade
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the visibility of the omnibar search box
    /// </summary>
    public bool ShowOmnibar
    {
        get
        {
            return (bool) GetValue(ShowOmnibarProperty);
        }
        set
        {
            SetValue(ShowOmnibarProperty, value);
        }
    }

    public new Brush Icon
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

    #endregion Properties

    #region Window Management

    private void ApplicationWindow_Deactivated(object? sender, EventArgs e)
    {
        if (InternalFrameworkSettings.KeyboardNavigationUIIndicator != null)
        {
            InternalFrameworkSettings.KeyboardNavigationUIIndicator.ShowTips = false;
        }
    }

    private void ApplicationWindow_Loaded(object sender, RoutedEventArgs e)
    {
        IsWindowLoaded = true;

        ApplyTemplate();
        ChromeBorder = (Border?) Template.FindName("BdrChrome", this);
        _mainGrid = (Grid?) Template.FindName("MainContainerGrid", this);
        _chromeHeaderGrid = (Grid?) Template.FindName("ChromeHeaderGrid", this);
        _chromeTitleButtonsStackPanel = (StackPanel?) Template.FindName("ChromeTitleButtonsStackPanel", this);
        _restoreButton = (Button?) Template.FindName("ChromeRestoreButton", this);
        _maximizeButton = (Button?) Template.FindName("ChromeMaximizeButton", this);
        _micaShadeRectangle = (Rectangle?) Template.FindName("MicaShadeRectangle", this);
        _chromeOmnibar = (OmnibarControl) Template.FindName("ChromeOmnibar", this);
        _mainContentPresenter = (ContentPresenter?) Template.FindName("MainContentPresenter", this);

        if (_restoreButton != null && _maximizeButton != null)
        {
            ChromeVirtualButtons.Add(_restoreButton);
            ChromeVirtualButtons.Add(_maximizeButton);

            _restoreButton.IsHitTestVisible = true;
            _maximizeButton.IsHitTestVisible = true;
        }

        if (!EnableMica)
        {
            Background = (Brush) FindResource("Workspace2Background");
        }

        if (ShowMicaShade && _micaShadeRectangle != null)
        {
            _micaShadeRectangle.Visibility = Visibility.Visible;
        }

        foreach (UIElement item in ChromeTitleButtons)
        {
            _chromeTitleButtonsStackPanel!.Children.Add(item);
        }

        UpdateGlowBorder(true, WindowState == WindowState.Maximized);
    }

    private void ApplicationWindow_StateChanged(object? sender, EventArgs e)
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

    #endregion Window Management

    #region Public methods

    /// <summary>
    ///     Sets the keyboard focus to the Omnibar search box.
    /// </summary>
    public void FocusOmnibar()
    {
        _chromeOmnibar?.Focus();
    }

    /// <summary>
    ///     Removes the Backstage control and displays the content of the window.
    /// </summary>
    public void HideBackstageView()
    {
        _mainGrid!.Children.Remove(_backstageView);
        _backstageView = null;
        _chromeHeaderGrid!.Visibility = Visibility.Visible;
        _mainContentPresenter!.Visibility = Visibility.Visible;
    }

    /// <summary>
    ///     Hides all the visible controls and displays the provided <paramref name="control" /> using all the available space
    ///     of the current window.
    /// </summary>
    /// <param name="control">The <see cref="Control" /> to display in the whole window.</param>
    /// <exception cref="ArgumentException"></exception>
    public void ShowBackstageView(Control control)
    {
        if (control == null)
        {
            throw new ArgumentNullException(nameof(control));
        }

        _backstageView = control;
        _mainContentPresenter!.Visibility = Visibility.Collapsed;
        _chromeHeaderGrid!.Visibility = Visibility.Collapsed;

        _backstageView.SetValue(Panel.ZIndexProperty, 60);
        _backstageView.SetValue(Grid.RowSpanProperty, 9);
        _mainGrid!.Children.Add(_backstageView);
    }

    #endregion Public methods

    #region Windows Chrome

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

    #endregion Windows Chrome
}