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
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Shell;
using Coho.UI.Controls;
using Coho.UI.Controls.Omnibar;
using Coho.UI.Controls.Ribbon;
using Coho.UI.Tools;
using Button = System.Windows.Controls.Button;
using Control = System.Windows.Controls.Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Panel = System.Windows.Controls.Panel;

namespace Coho.UI;

public class ApplicationWindow : Window
{
    private readonly List<Button> _chromeVirtualButtons = new();
    private Control? _backstageView;
    private Border? _bdrChrome;
    private Grid? _chromeHeaderGrid;
    private OmnibarControl? _chromeOmnibar;
    private StackPanel? _chromeTitleButtonsStackPanel;
    private bool _isLoaded;
    private ContentPresenter? _mainContentPresenter;
    private Grid? _mainGrid;
    private Image? _iconImage;
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
        SourceInitialized += ApplicationWindow_SourceInitialized;
        PreviewKeyDown += ApplicationWindow_PreviewKeyDown;
        Activated += ApplicationWindow_Activated;
        Deactivated += ApplicationWindow_Deactivated;

        MinHeight = 650;
        MinWidth = 750;

        Style = (Style) FindResource("ApplicationWindowStyle");
    }

    private void ApplicationWindow_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (InternalRibbonSettings.CurrentRibbon == null)
        {
            return;
        }

        Keys key = (Keys) KeyInterop.VirtualKeyFromKey(e.Key);

        if (Keyboard.Modifiers == ModifierKeys.Alt)
        {
            if (!InternalRibbonSettings.CurrentRibbon.HandleKeyboardNavigation(key))
            {
                ToggleKeyboardNav();
            }
        }

        if (InternalRibbonSettings.KeyboardNavigationUIIndicator != null)
        {
            if (InternalRibbonSettings.KeyboardNavigationUIIndicator.ShowTips || Keyboard.Modifiers == ModifierKeys.Alt)
            {
                if (InternalRibbonSettings.CurrentRibbon.HandleKeyboardNavigation(key))
                {
                    ToggleKeyboardNav();
                }
            }

            if (InternalRibbonSettings.KeyboardNavigationUIIndicator.ShowTips && key.ToString().Contains("NumPad", StringComparison.InvariantCultureIgnoreCase))
            {
                if (InternalRibbonSettings.CurrentRibbon.HandleKeyboardNavigation(key))
                {
                    InternalRibbonSettings.KeyboardNavigationUIIndicator.ShowTips = false;
                }
            }
        }
    }

    private void ToggleKeyboardNav()
    {
        if (InternalRibbonSettings.KeyboardNavigationUIIndicator != null)
        {
            InternalRibbonSettings.KeyboardNavigationUIIndicator.ShowTips = !InternalRibbonSettings.KeyboardNavigationUIIndicator.ShowTips;
        }
    }

    #region Properties

    public static readonly DependencyProperty ApplyAccentToChromeProperty =
        DependencyProperty.RegisterAttached(nameof(ApplyAccentToChrome), typeof(bool), typeof(ApplicationWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ShowOmnibarProperty =
        DependencyProperty.RegisterAttached(nameof(ShowOmnibar), typeof(bool), typeof(ApplicationWindow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
    
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

    /// <summary>
    /// Gets or sets the list of buttons that appear next to the application title
    /// </summary>
    public List<UIElement> ChromeTitleButtons
    {
        get;
        set;
    } = new();

    /// <summary>
    /// Gets or sets if the window should use the Mica effect (only Windows 11)
    /// </summary>
    public bool EnableMica
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the visibility of a subtle shade over the Mica effect
    /// </summary>
    public bool ShowMicaShade
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the visibility of the omnibar search box
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

    #endregion Properties

    #region Window Management

    private static void ApplyMica(HwndSource source, bool darkThemeEnabled)
    {
        int trueValue = 0x01;
        int falseValue = 0x00;

        int micaValue = 2;
        //None = 1,
        //Mica = 2,
        //Acrylic = 3,
        //Tabbed = 4

        if (darkThemeEnabled)
        {
            _ = NativeMethods.DwmSetWindowAttribute(source.Handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)));
        }
        else
        {
            _ = NativeMethods.DwmSetWindowAttribute(source.Handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref falseValue, Marshal.SizeOf(typeof(int)));
        }

        if (Environment.OSVersion.Version.Build >= 22523)
        {
            _ = NativeMethods.DwmSetWindowAttribute(source.Handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref micaValue, Marshal.SizeOf(typeof(int)));
        }
        else
        {
            _ = NativeMethods.DwmSetWindowAttribute(source.Handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));
        }
    }

    private void ApplicationWindow_Activated(object? sender, EventArgs e)
    {
        UpdateGlowBorder(true, WindowState == WindowState.Maximized);
    }

    private void ApplicationWindow_Deactivated(object? sender, EventArgs e)
    {
        if (InternalRibbonSettings.KeyboardNavigationUIIndicator != null)
        {
            InternalRibbonSettings.KeyboardNavigationUIIndicator.ShowTips = false;
        }

        UpdateGlowBorder(false, WindowState == WindowState.Maximized);
    }

    private void ApplicationWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _isLoaded = true;

        ApplyTemplate();
        _bdrChrome = (Border?) Template.FindName("BdrChrome", this);
        _mainGrid = (Grid?) Template.FindName("MainContainerGrid", this);
        _iconImage = (Image?) Template.FindName("ImageIcon", this);
        _chromeHeaderGrid = (Grid?) Template.FindName("ChromeHeaderGrid", this);
        _chromeTitleButtonsStackPanel = (StackPanel?) Template.FindName("ChromeTitleButtonsStackPanel", this);
        _restoreButton = (Button?) Template.FindName("ChromeRestoreButton", this);
        _maximizeButton = (Button?) Template.FindName("ChromeMaximizeButton", this);
        _micaShadeRectangle = (Rectangle?) Template.FindName("MicaShadeRectangle", this);
        _chromeOmnibar = (OmnibarControl) Template.FindName("ChromeOmnibar", this);
        _mainContentPresenter = (ContentPresenter?) Template.FindName("MainContentPresenter", this);

        if (_restoreButton != null && _maximizeButton != null)
        {
            _chromeVirtualButtons.Add(_restoreButton);
            _chromeVirtualButtons.Add(_maximizeButton);
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

        _iconImage!.Source = this.Icon;
        
        UpdateGlowBorder(true, WindowState == WindowState.Maximized);
    }

    private void ApplicationWindow_SourceInitialized(object? sender, EventArgs e)
    {
        HwndSource? windowHandleSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
        if (windowHandleSource == null)
        {
            return;
        }

        if (EnableMica && InternalRibbonSettings.IsWindows11 && InternalRibbonSettings.IsMicaSupported)
        {
            ApplyMica(windowHandleSource, false);
        }
        else
        {
            Background = (Brush) FindResource("Workspace2Background");
        }

        try
        {
            // Permet de masquer les boutons du chrome par défaut Windows
            // sinon l'utilisation du WindowChrome WPF affiche par défaut les boutons
            // et ils se superposent aux boutons de mon chrome perso
            NativeMethods.SetWindowLong32(new HandleRef(null, windowHandleSource.Handle), NativeMethods.GWL_STYLE, NativeMethods.GetWindowLongPtr32(windowHandleSource.Handle, NativeMethods.GWL_STYLE).ToInt32() & ~NativeMethods.WS_SYSMENU);

            // Permet de gérer les évènements de mouseover
            // sur mon chrome perso, cela permet de provoquer le Snap Assist sur Windows 11
            windowHandleSource.AddHook(WndProc);
        }
        catch
        {
            // rien de spécial
        }
    }

    private void ApplicationWindow_StateChanged(object? sender, EventArgs e)
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

        UpdateGlowBorder(IsActive, WindowState == WindowState.Maximized);
    }

    #endregion Window Management

    #region Public methods

    /// <summary>
    ///     Permet de mettre le focus dans la barre globale
    /// </summary>
    public void FocusOmnibar()
    {
        _chromeOmnibar?.Focus();
    }

    public void HideBackstageView()
    {
        _mainGrid!.Children.Remove(_backstageView);
        _backstageView = null;
        _chromeHeaderGrid!.Visibility = Visibility.Visible;
        _mainContentPresenter!.Visibility = Visibility.Visible;
    }

    public void ShowBackstageView(Control control)
    {
        _backstageView = control;
        _mainContentPresenter!.Visibility = Visibility.Collapsed;
        _chromeHeaderGrid!.Visibility = Visibility.Collapsed;

        _backstageView.SetValue(Panel.ZIndexProperty, 60);
        _backstageView.SetValue(Grid.RowSpanProperty, 9);
        _mainGrid!.Children.Add(_backstageView);
    }

    #endregion Public methods

    #region Windows Chrome

    private void ChromeButton_Clear()
    {
        foreach (Button btn in _chromeVirtualButtons)
        {
            btn.SetValue(WindowDependencyProperties.IsFakeHoverProperty, false);
        }
    }

    private bool ChromeButton_Click(IntPtr lParam)
    {
        Button? btn = GetChromeButton(lParam);

        if (btn != null && _chromeVirtualButtons.Contains(btn) && btn.Tag is bool t && t)
        {
            btn.Tag = false;
            RoutedCommand routedCommand = (RoutedCommand) btn.Command;
            routedCommand.Execute(btn.CommandParameter, btn);
            return true;
        }

        return false;
    }

    private IntPtr ChromeButton_Hover(IntPtr lParam, ref bool handled)
    {
        Button? btn = GetChromeButton(lParam);
        if (btn != null && _chromeVirtualButtons.Contains(btn))
        {
            btn.SetValue(WindowDependencyProperties.IsFakeHoverProperty, true);
            handled = true;
            return new IntPtr(9);
        }

        return IntPtr.Zero;
    }

    private bool ChromeButton_Pressed(IntPtr lParam)
    {
        Button? btn = GetChromeButton(lParam);

        if (btn != null && _chromeVirtualButtons.Contains(btn))
        {
            btn.Tag = true;
            return true;
        }

        return false;
    }

    private Button? GetChromeButton(IntPtr lParam)
    {
        Point point = new(NativeMethods.GetXLParam(lParam.ToInt32Unchecked()), NativeMethods.GetYLParam(lParam.ToInt32Unchecked()));
        Point point2 = PointFromScreen(point);

        // Perform the hit test against a given portion of the visual object tree.
        HitTestResult result = VisualTreeHelper.HitTest(this, point2);

        if (result == null)
        {
            return null;
        }

        return WpfTools.GetClosestParent<Button>(result.VisualHit);
    }

    private bool IsConnectedToPresentationSource()
    {
        return PresentationSource.FromDependencyObject(this) != null;
    }

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

    private void UpdateGlowBorder(bool activate, bool maximized)
    {
        if (!InternalRibbonSettings.IsWindows11)
        {
            if (_bdrChrome != null)
            {
                Color color = activate
                    ? (Color) FindResource("ChromeBorderActiveColor")
                    : (Color) FindResource("ChromeBorderDefaultColor");
                _bdrChrome.BorderThickness = new Thickness(1);
                _bdrChrome.BorderBrush = new SolidColorBrush(color);
                return;
            }
        }

        IntPtr handle = new WindowInteropHelper(this).Handle;

        if (handle != IntPtr.Zero && _isLoaded)
        {
            Color color = activate ? (Color) FindResource("ChromeBorderActiveColor") : (Color) FindResource("ChromeBorderDefaultColor");
            NativeMethods.COLORREF colorRef = new(color);
            int attrValue = maximized ? -2 : (int) colorRef.dwColor;
            _ = NativeMethods.DwmSetWindowAttribute(handle, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, ref attrValue, 4);
        }
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (!IsConnectedToPresentationSource())
        {
            return new IntPtr(0);
        }

        switch (msg)
        {
            case 512:
            case 674:
            case 675:
                ChromeButton_Clear();
                return IntPtr.Zero;

            case 161: // mousedown
                handled = ChromeButton_Pressed(lParam);
                return IntPtr.Zero;

            case 162: // mouseup ?
                handled = ChromeButton_Click(lParam);
                return IntPtr.Zero;

            case 132:
                return ChromeButton_Hover(lParam, ref handled);

            default:
                return IntPtr.Zero;
        }
    }

    #endregion Windows Chrome
}