// *********************************************************
// 
// Coho.UI
// MenuBarQuickAccessToolbar.cs
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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Coho.UI.CommandManaging;
using Coho.UI.Controls.Common;
using Coho.UI.Controls.Ribbon;
using Coho.UI.Dialogs;
using Coho.UI.Tools;
using CommandManager = Coho.UI.CommandManaging.CommandManager;

namespace Coho.UI.Controls.Menus;

public sealed class MenuBarQuickAccessToolbar : ToolBar
{
    private DropDownPopup? _dropDownPopup;
    private ToggleButton? _optionsButton;
    private ToolBarOverflowPanel? _overflowPanel;
    private MenuBar? _parentMenuBar;
    private DropDownPopup? _ribbonOptionsDropDown;
    private ToggleButton? _toggleButton;
    private ToolBarPanel? _toolbarPanel;

    public MenuBarQuickAccessToolbar()
    {
        Style = (Style) FindResource("RibbonQuickActionsToolbar");
        Loaded += RibbonQuickActionsToolbar_Loaded;
        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_toolbarPanel != null && ActualWidth > 0)
        {
            _toolbarPanel.MaxWidth = ActualWidth - 70 > 0 ? ActualWidth - 70 : ActualWidth;
        }
    }

    internal void AddCmd(IRibbonCommand cmd)
    {
        if (_parentMenuBar == null)
        {
            SystemSounds.Beep.Play();
            return;
        }

        string hash = cmd.Name.GetStaticHashCode().ToString(CultureInfo.InvariantCulture);
        _parentMenuBar.QatCommands.Add(hash);
        Refresh();
    }

    internal void AttachParentMenuBar(MenuBar ribbonBar)
    {
        _parentMenuBar = ribbonBar;
    }

    private void BuildOptionsMenu()
    {
        if (_ribbonOptionsDropDown != null)
        {
            return;
        }

        StackPanel st = new();
        MenuItem mi1 = new()
        {
            Header = RibbonText.ToggleQAT,
            Icon = new Rectangle
            {
                Fill = Brushes.Transparent,
                Width = 16,
                Height = 16
            },
            DataContext = _parentMenuBar,
            IsCheckable = true
        };
        Binding showQATIsCheckedBinding = new(nameof(RibbonBar.ShowQAT))
        {
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };
        mi1.SetBinding(MenuItem.IsCheckedProperty, showQATIsCheckedBinding);
        mi1.Click += Mi1_Click;
        st.Children.Add(mi1);

        MenuItem mi2 = new()
        {
            Header = RibbonText.ToggleQATLabels,
            Icon = Brushes.Transparent,
            DataContext = _parentMenuBar
        };
        Binding showQATLabelsIsCheckedBinding = new(nameof(RibbonBar.ShowQATLabels))
        {
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };
        mi2.SetBinding(MenuItem.IsCheckedProperty, showQATLabelsIsCheckedBinding);
        mi2.Click += Mi2_Click;
        st.Children.Add(mi2);

        Separator sep = new()
        {
            Style = (Style) FindResource(MenuItem.SeparatorStyleKey)
        };
        st.Children.Add(sep);

        MenuItem mi3 = new()
        {
            Header = RibbonText.CustomizeQATCommands,
            Icon = (Brush) FindResource("IconSettings")
        };
        mi3.Click += Mi3_Click;
        st.Children.Add(mi3);

        _ribbonOptionsDropDown = new DropDownPopup {Content = st};
        _ribbonOptionsDropDown.SetValue(OverflowModeProperty, OverflowMode.Never);
        _ribbonOptionsDropDown.PopupVisibilityChanged += RibbonOptionsDropDown_PopupVisibilityChanged;
    }

    private void Mi3_Click(object sender, RoutedEventArgs e)
    {
        CustomizeQatDialog dlg = new();
        ObservableCollection<CommandItemModel> items = new();

        foreach (FrameworkElement item in Items)
        {
            if (item is IRibbonCommand i2)
            {
                items.Add(new CommandItemModel
                {
                    Label = i2.Text,
                    Icon = i2.Icon,
                    Hash = item.Tag?.ToString()!
                });
            }
        }

        dlg.Items = items;
        dlg.AvailableItems = CommandManager.GetCommands().Where(x =>
            x.CommandRibbonButton?.GetType() != typeof(OrphanRibbonCommand)
        ).OrderBy(x => x.CommandFullName);


        if (dlg.ShowDialog()!.Value)
        {
            _parentMenuBar!.QatCommands.Clear();

            foreach (CommandItemModel item in dlg.Items)
            {
                _parentMenuBar.QatCommands.Add(item.Hash);
            }

            Refresh();
        }

        _ribbonOptionsDropDown!.ClosePopup();
    }

    private void Mi2_Click(object sender, RoutedEventArgs e)
    {
        ToggleLabels();
        _ribbonOptionsDropDown!.ClosePopup();
    }

    private void Mi1_Click(object sender, RoutedEventArgs e)
    {
        _parentMenuBar!.ShowQAT = !_parentMenuBar.ShowQAT;
        _ribbonOptionsDropDown!.ClosePopup();
    }

    private void RibbonOptionsDropDown_PopupVisibilityChanged(object? sender, bool e)
    {
        _optionsButton.IsChecked = e;
    }

    internal void Refresh()
    {
        Items.Clear();
        foreach (string cmdHash in _parentMenuBar!.QatCommands)
        {
            BuildButton(cmdHash);
        }

        Separator sep = new()
        {
            Style = (Style) FindResource("ToolbarSeparator"),
            Margin = new Thickness(1, 4, 2, 4)
        };
        sep.SetValue(OverflowModeProperty, OverflowMode.Never);

        Rectangle rect = new()
        {
            Fill = (Brush) FindResource("IconSettingsDropDown"),
            Width = 9,
            Height = 9,
            Margin = new Thickness(0, 1, 0, 0)
        };

        _optionsButton = new ToggleButton
        {
            Style = (Style) FindResource("QatStdButton"),
            Content = rect,
            Padding = new Thickness(5, 0, 5, 0),
            MinWidth = 0,
            MinHeight = 28,
            Margin = new Thickness(2, 0, 0, 0)
        };
        _optionsButton.SetValue(OverflowModeProperty, OverflowMode.Never);

        _optionsButton.Click -= Btn_Click;
        _optionsButton.Click += Btn_Click;

        _ = Items.Add(sep);
        _ = Items.Add(_optionsButton);
        _ = Items.Add(_ribbonOptionsDropDown);
    }

    private void Btn_Click(object sender, RoutedEventArgs e)
    {
        bool isChecked = ((ToggleButton) sender).IsChecked!.Value;

        if (isChecked)
        {
            _ribbonOptionsDropDown?.OpenPopup((UIElement) sender);
        }
        else
        {
            _ribbonOptionsDropDown?.ClosePopup();
        }
    }

    internal void RemoveCmd(IRibbonCommand cmd)
    {
        string cmdHash = ((FrameworkElement) cmd).Tag!.ToString()!;
        _parentMenuBar!.QatCommands.Remove(cmdHash);
        Items.Remove(cmd);
    }

    internal void ToggleLabels()
    {
        _parentMenuBar!.ShowQATLabels = !_parentMenuBar.ShowQATLabels;

        foreach (object? item in Items)
        {
            if (item is IRibbonCommand cmd)
            {
                cmd.Display = _parentMenuBar.ShowQATLabels
                    ? RibbonEnums.RibbonButtonDisplay.IconAndText
                    : RibbonEnums.RibbonButtonDisplay.IconOnly;
            }
        }
    }

    private void BuildButton(string cmdHash)
    {
        OmnibarSearchResult? command = CommandManager.GetCommandByHash(cmdHash);

        if (command == null || _parentMenuBar == null)
        {
            return;
        }

        RibbonButton? newBtn = new();
        newBtn.Text = command.DisplayName;
        newBtn.IsInQAT = true;
        newBtn.Gesture = command.Gesture;
        newBtn.Description = command.CommandDescription;
        newBtn.Icon = command.Icon;
        newBtn.Display = _parentMenuBar.ShowQATLabels
            ? RibbonEnums.RibbonButtonDisplay.IconAndText
            : RibbonEnums.RibbonButtonDisplay.IconOnly;
        newBtn.Tag = cmdHash;

        ((MenuItem) command.LinkedOriginalObject).DataContextChanged += delegate(object sender, DependencyPropertyChangedEventArgs e)
        {
            newBtn.DataContext = e.NewValue;
        };

        ((IRibbonCommand) newBtn).OnClick += delegate
        {
            ((MenuItem) command.LinkedOriginalObject).RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
        };

        if (command.LinkedOriginalObject is FrameworkElement fe)
        {
            // on recopie la référence vers le même datacontext
            // certains boutons du ruban ne sont pas bindés sur RibbonActivator
            newBtn.DataContext = fe.DataContext;

            BindingExpression? enableBindingExp = fe.GetBindingExpression(IsEnabledProperty);
            if (enableBindingExp != null)
            {
                Binding? parentBinding = enableBindingExp.ParentBinding;
                newBtn.SetBinding(IsEnabledProperty, parentBinding);
            }

            BindingExpression? checkedBindingExp = fe.GetBindingExpression(ToggleButton.IsCheckedProperty);
            if (checkedBindingExp != null)
            {
                Binding? parentBinding = checkedBindingExp.ParentBinding;
                newBtn.SetBinding(ToggleButton.IsCheckedProperty, parentBinding);
            }

            BindingExpression? switchedBindingExp = fe.GetBindingExpression(RibbonSwitchButton.IsSwitchedProperty);
            if (switchedBindingExp != null)
            {
                Binding? parentBinding = switchedBindingExp.ParentBinding;
                newBtn.SetBinding(RibbonSwitchButton.IsSwitchedProperty, parentBinding);
            }
        }

        _ = Items.Add(newBtn);
    }

    private void RibbonQuickActionsToolbar_Loaded(object sender, RoutedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        BuildOptionsMenu();

        // il faut actualiser les éléments avant de faire ApplyTemplate, car sans élément, le template est vide
        Refresh();

        // doit être fait après Refresh() pour s'assurer qu'il y a des éléments dans la toolbar
        _ = ApplyTemplate();
        _toolbarPanel = (ToolBarPanel) Template.FindName("PART_ToolBarPanel", this);
        _overflowPanel = (ToolBarOverflowPanel) Template.FindName("PART_ToolBarOverflowPanel", this);

        SetValue(KeyboardNavigation.TabNavigationProperty, KeyboardNavigationMode.Continue);
        ClipToBounds = true;

        _toggleButton = (ToggleButton?) Template.FindName("OverFlowButton", this);
        _dropDownPopup = (DropDownPopup?) Template.FindName("DropDownPopupPart", this);

        if (_parentMenuBar == null)
        {
            _parentMenuBar = WpfTools.GetClosestParent<MenuBar>(this);
        }

        if (_dropDownPopup != null)
        {
            _dropDownPopup.PopupVisibilityChanged += DropDownPopup_PopupVisibilityChanged;
        }

        if (_toggleButton != null)
        {
            _toggleButton.Margin = new Thickness(0, 3, 0, 3);
            _toggleButton.Checked -= RibbonDropDownButton_Checked;
            _toggleButton.Checked += RibbonDropDownButton_Checked;
            _toggleButton.Unchecked -= ToggleButton_Unchecked;
            _toggleButton.Unchecked += ToggleButton_Unchecked;
        }
    }

    private void DropDownPopup_PopupVisibilityChanged(object? sender, bool e)
    {
        if (e)
        {
            _toggleButton!.IsChecked = true;

            UIElement? firstItem = _overflowPanel!.Children.OfType<UIElement>().FirstOrDefault();
            if (firstItem != null)
            {
                firstItem.Focus();
                FocusManager.SetFocusedElement(firstItem, null);
            }

            _overflowPanel.Focus();
            _dropDownPopup!.Focus();
        }
        else
        {
            _toggleButton!.IsChecked = false;
        }
    }

    private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
    {
        _dropDownPopup!.ClosePopup();
    }

    private void RibbonDropDownButton_Checked(object sender, RoutedEventArgs e)
    {
        _dropDownPopup!.OpenPopup(_toggleButton!);
    }
}