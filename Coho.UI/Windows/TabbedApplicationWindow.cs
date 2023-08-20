// *********************************************************
// 
// Coho.UI
// TabbedApplicationWindow.cs
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
using System.Windows.Shell;

namespace Coho.UI.Windows;

public class TabbedApplicationWindow : FluentWindow
{
    private Control? _backstageView;
    private Grid? _chromeHeaderGrid;
    private StackPanel? _chromeTitleButtonsStackPanel;
    private ContentPresenter? _mainContentPresenter;
    private Grid? _mainGrid;
    private Button? _maximizeButton;
    private Button? _restoreButton;
    
    public TabbedApplicationWindow()
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
        // PreviewKeyDown += ApplicationWindow_PreviewKeyDown;
        // Deactivated += ApplicationWindow_Deactivated;
        SourceInitialized += Window_SourceInitialized;

        MinHeight = 650;
        MinWidth = 750;

        Style = (Style) FindResource("TabbedApplicationWindowStyle");
    }

    private void Window_SourceInitialized(object? sender, EventArgs e)
    {
        OnSourceInitializedBase(this);
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