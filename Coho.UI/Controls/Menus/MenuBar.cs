// *********************************************************
// 
// Coho.UI MenuBar.cs
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
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Coho.UI.CommandManaging;
using Coho.UI.Controls.Ribbon;
using Coho.UI.Interfaces;
using Coho.UI.Tools;

namespace Coho.UI.Controls.Menus;

public sealed class MenuBar : ItemsControl, IApplicationMainBarControl
{
    public static readonly DependencyProperty ExtraButtonsProperty =
        DependencyProperty.RegisterAttached(nameof(ExtraButtons), typeof(List<UIElement>), typeof(MenuBar),
            new FrameworkPropertyMetadata(new List<UIElement>(), FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ShowQATProperty =
        DependencyProperty.RegisterAttached(nameof(ShowQAT), typeof(bool), typeof(MenuBar),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ShowQATLabelsProperty =
        DependencyProperty.RegisterAttached(nameof(ShowQATLabels), typeof(bool), typeof(MenuBar),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    private StackPanel? _extraButtonsStackPanel;
    private Menu? _innerMenu;
    private ContextMenu? _qatButtonsContextMenu;
    private MenuBarQuickAccessToolbar? _qatToolbar;
    private Border? _qatToolbarHolder;

    internal List<MenuItem> CachedItems
    {
        get;
        private set;
    } = new();

    public MenuBar()
    {
        Loaded += OnLoaded;
    }

    /// <summary>
    /// Gets the list of commands that are available in the Quick Access Toolbar area.
    /// </summary>
    /// <remarks>
    /// You should set all the commands in the <see cref="Window.Loaded"/> event.
    /// You should save all the commands in the <see cref="Window.Closing"/> event.
    /// </remarks>
    public List<string> QatCommands
    {
        get;
    } = new();

    public bool ShowQATLabels
    {
        get
        {
            return (bool) GetValue(ShowQATLabelsProperty);
        }
        set
        {
            SetValue(ShowQATLabelsProperty, value);
        }
    }

    public bool ShowQAT
    {
        get
        {
            return (bool) GetValue(ShowQATProperty);
        }
        set
        {
            SetValue(ShowQATProperty, value);
        }
    }

    public List<UIElement> ExtraButtons
    {
        get
        {
            return (List<UIElement>) GetValue(ExtraButtonsProperty);
        }
        set
        {
            SetValue(ExtraButtonsProperty, value);
        }
    }

    /// <summary>
    /// Returns the identifier of the provided <paramref name="cmd"/>. It is used to memorize the QAT commands for example. 
    /// </summary>
    /// <param name="cmd">A <see cref="FrameworkElement"/> that belongs to the menu children.</param>
    /// <returns>A <see cref="string"/> that is produced using the <see cref="FrameworkElement.Name"/> property.</returns>
    /// <exception cref="NullReferenceException">Occurs when the provided <paramref name="cmd"/> has no <see cref="FrameworkElement.Name"/> property.</exception>
    public string GetCommandIdentifier(MenuItem cmd)
    {
        if (string.IsNullOrEmpty(cmd.Name))
        {
            throw new NullReferenceException("The provided component has no 'Name' property.");
        }

        return cmd.Name.GetStaticHashCode().ToString();
    }

    ContextMenu IApplicationMainBarControl.GetItemContextMenu(IRibbonCommand cmd)
    {
        if (cmd.IsInQAT)
        {
            return _qatButtonsContextMenu!;
        }

        return null!;
    }

    bool IApplicationMainBarControl.HandleKeyboardNavigation(Keys key)
    {
        return false;
    }

    public bool EnableAnimations
    {
        get;
        set;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        InternalFrameworkSettings.CurrentMainBarControl = this;

        ApplyTemplate();

        _extraButtonsStackPanel = (StackPanel?) Template.FindName("ExtraButtonsStackPanel", this);

        if (_extraButtonsStackPanel != null)
        {
            foreach (UIElement element in ExtraButtons)
            {
                _extraButtonsStackPanel.Children.Add(element);
            }
        }

        _innerMenu = (Menu) Template.FindName("InnerMenu", this);

        _qatToolbarHolder = (Border) Template.FindName("QatToolbarHolder", this);
        _qatToolbar = (MenuBarQuickAccessToolbar) _qatToolbarHolder.Child;
        _qatToolbar.AttachParentMenuBar(this);


        foreach (MenuItem item in Items.OfType<MenuItem>())
        {
            CachedItems.Add(item);
        }

        Items.Clear();

        foreach (MenuItem item in CachedItems)
        {
            _innerMenu.Items.Add(item);
        }

        CommandManager.RebuildCommandsCache(this);
    }
}