// *********************************************************
// 
// Coho.UI
// RibbonQuickAccessToolbar.cs
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
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Coho.UI.CommandManaging;
using Coho.UI.Controls.Common;
using Coho.UI.Tools;

namespace Coho.UI.Controls.Ribbon;

public sealed class RibbonQuickAccessToolbar : ToolBar
{
    private ToggleButton? _optionsButton;
    private RibbonBar? _parentRibbon;
    private DropDownPopup? _ribbonOptionsDropDown;

    public RibbonQuickAccessToolbar()
    {
        Style = (Style) FindResource("RibbonQuickActionsToolbar");
        Loaded += RibbonQuickActionsToolbar_Loaded;
    }

    internal void AddCmd(IRibbonCommand cmd)
    {
        string hash = cmd.Name.GetStaticHashCode().ToString(CultureInfo.InvariantCulture);
        _parentRibbon!.QatCommands.Add(hash);
        Refresh();
    }

    internal void AttachParentRibbon(RibbonBar ribbonBar)
    {
        _parentRibbon = ribbonBar;
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
            Header = RibbonText.ToggleQAT
        };
        mi1.Click += Mi1_Click;
        st.Children.Add(mi1);

        MenuItem mi2 = new()
        {
            Header = RibbonText.ToggleQATLabels
        };
        mi2.Click += Mi2_Click;
        st.Children.Add(mi2);

        MenuItem mi3 = new()
        {
            Header = RibbonText.CustomizeQATCommands
        };
        mi3.Click += Mi3_Click;
        st.Children.Add(mi3);


        _ribbonOptionsDropDown = new DropDownPopup();
        _ribbonOptionsDropDown.Content = st;
        _ribbonOptionsDropDown.PopupVisibilityChanged += RibbonOptionsDropDown_PopupVisibilityChanged;
    }

    private void Mi2_Click(object sender, RoutedEventArgs e)
    {
        _ribbonOptionsDropDown!.ClosePopup();
        ToggleLabels();
    }

    private void Mi1_Click(object sender, RoutedEventArgs e)
    {
        _ribbonOptionsDropDown!.ClosePopup();
        _parentRibbon!.ShowQAT = !_parentRibbon.ShowQAT;
    }

    private void Mi3_Click(object sender, RoutedEventArgs e)
    {
        // todo
        //CustomizeQatDialog dlg = new();
        //ObservableCollection<RibbonCommandItemModel> items = new();

        //foreach (FrameworkElement item in Items)
        //{
        //    if (item is IRibbonCommand i2)
        //    {
        //        items.Add(new RibbonCommandItemModel()
        //        {
        //            Label = i2.Text,
        //            Hash = item.Tag.ToString()
        //        });
        //    }
        //}

        //dlg.Items = items;
        //dlg.AvailableItems = CommandManager.GetCommands().Where(x => x.CommandRibbonButton.GetType() != typeof(OrphanRibbonCommand)
        //                                                      //&& x.CommandRibbonButton.GetType() != typeof(RibbonDropDownButton)
        //                                                      ).OrderBy(x => x.CommandFullName);

        //var tt = from a in dlg.AvailableItems
        //         select a.CommandHash;

        //dlg.Owner = AppState.AppWindow;

        //if (dlg.ShowDialog().Value)
        //{
        //    UserState.UserSettings.QuickActionsToolbar.Clear();

        //    foreach (RibbonCommandItemModel item in dlg.Items)
        //    {
        //        UserState.UserSettings.QuickActionsToolbar.Add(item.Hash);
        //    }
        //    Refresh();
        //}
    }

    private void RibbonOptionsDropDown_PopupVisibilityChanged(object? sender, bool e)
    {
        _optionsButton!.IsChecked = e;
    }

    internal void Refresh()
    {
        Items.Clear();
        foreach (string cmdHash in _parentRibbon!.QatCommands)
        {
            BuildButton(cmdHash);
        }

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

        _optionsButton.Click -= Btn_Click;
        _optionsButton.Click += Btn_Click;

        _ = Items.Add(_optionsButton);
        _ = Items.Add(_ribbonOptionsDropDown);
    }

    private void Btn_Click(object sender, RoutedEventArgs e)
    {
        bool isChecked = ((ToggleButton) sender).IsChecked!.Value;

        if (isChecked)
        {
            _ribbonOptionsDropDown!.OpenPopup((UIElement) sender);
        }
        else
        {
            _ribbonOptionsDropDown!.ClosePopup();
        }
    }

    internal void RemoveCmd(IRibbonCommand cmd)
    {
        string cmdHash = ((FrameworkElement) cmd).Tag!.ToString()!;
        _parentRibbon!.QatCommands.Remove(cmdHash);
        Items.Remove(cmd);
    }

    internal void ToggleLabels()
    {
        _parentRibbon!.ShowQATLabels = !_parentRibbon.ShowQATLabels;

        foreach (object item in Items)
        {
            if (item is IRibbonCommand cmd)
            {
                cmd.Display = _parentRibbon.ShowQATLabels ? RibbonEnums.RibbonButtonDisplay.IconAndText : RibbonEnums.RibbonButtonDisplay.IconOnly;
            }
        }
    }

    private void BuildButton(string cmdHash)
    {
        OmnibarSearchResult? command = CommandManager.GetCommandByHash(cmdHash);
        if (command == null)
        {
            return;
        }

        Type type = command.CommandRibbonButton!.GetType();

        if (type == typeof(OrphanRibbonCommand))
        {
            type = typeof(RibbonButton);
        }

        IRibbonCommand newBtn = (IRibbonCommand) Activator.CreateInstance(type);
        newBtn.Text = command.DisplayName;
        newBtn.IsInQAT = true;
        newBtn.Gesture = command.Gesture ?? string.Empty;
        newBtn.Description = command.CommandDescription;
        newBtn.Icon = command.CommandRibbonButton.Icon;
        newBtn.OriginalCommand = command.CommandRibbonButton;
        newBtn.Display = _parentRibbon!.ShowQATLabels ? RibbonEnums.RibbonButtonDisplay.IconAndText : RibbonEnums.RibbonButtonDisplay.IconOnly;
        ((FrameworkElement) newBtn).Tag = cmdHash;

        if (command.CommandRibbonButton is FrameworkElement fe)
        {
            BindingExpression? enableBindingExp = fe.GetBindingExpression(IsEnabledProperty);
            if (enableBindingExp != null)
            {
                Binding parentBinding = enableBindingExp.ParentBinding;
                ((FrameworkElement) newBtn).SetBinding(IsEnabledProperty, parentBinding);
            }

            BindingExpression? checkedBindingExp = fe.GetBindingExpression(ToggleButton.IsCheckedProperty);
            if (checkedBindingExp != null)
            {
                Binding parentBinding = checkedBindingExp.ParentBinding;
                ((FrameworkElement) newBtn).SetBinding(ToggleButton.IsCheckedProperty, parentBinding);
            }
        }
        else if (command.CommandRibbonButton is OrphanRibbonCommand oc)
        {
            BindingExpression? enableBindingExp = oc.Button?.GetBindingExpression(IsEnabledProperty);
            if (enableBindingExp != null)
            {
                Binding parentBinding = enableBindingExp.ParentBinding;
                ((FrameworkElement) newBtn).SetBinding(IsEnabledProperty, parentBinding);
            }

            BindingExpression? checkedBindingExp = oc.Button?.GetBindingExpression(ToggleButton.IsCheckedProperty);
            if (checkedBindingExp != null)
            {
                Binding parentBinding = checkedBindingExp.ParentBinding;
                ((FrameworkElement) newBtn).SetBinding(ToggleButton.IsCheckedProperty, parentBinding);
            }
        }

        newBtn.OnClick += NewBtn_OnClick;
        _ = Items.Add(newBtn);
    }

    private void NewBtn_OnClick(object sender, RoutedEventArgs e)
    {
        ((IRibbonCommand) sender).OriginalCommand?.RaiseClick();
    }

    private void RibbonQuickActionsToolbar_Loaded(object sender, RoutedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        BuildOptionsMenu();
        Refresh();
    }
}