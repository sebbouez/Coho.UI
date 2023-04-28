// *********************************************************
// 
// Coho.UI
// RibbonOverflowButton.cs
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Coho.UI.Controls.Common;

namespace Coho.UI.Controls.Ribbon;

public sealed class RibbonOverflowButton : ContentControl
{
    private DropDownPopup? _dropDownPopup;
    private ToggleButton? _toggleButton;

    public RibbonOverflowButton()
    {
        Loaded += RibbonOverflowButton_Loaded;
    }

    private void RibbonDropDownButton_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            CloseDropDown();
            Focus();
            _toggleButton!.Focus();
        }
    }

    private void RibbonOverflowButton_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyTemplate();

        _toggleButton = (ToggleButton?) Template.FindName("OverFlowButton", this);
        _dropDownPopup = (DropDownPopup?) Template.FindName("DropDownPopupPart", this);

        if (_dropDownPopup != null)
        {
            _dropDownPopup.PopupVisibilityChanged += DropDownPopup_PopupVisibilityChanged;
        }

        if (_toggleButton != null)
        {
            _toggleButton.Checked -= RibbonDropDownButton_Checked;
            _toggleButton.Checked += RibbonDropDownButton_Checked;
        }

        PreviewKeyDown -= RibbonDropDownButton_PreviewKeyDown;
        PreviewKeyDown += RibbonDropDownButton_PreviewKeyDown;
    }

    private void RibbonDropDownButton_Checked(object sender, RoutedEventArgs e)
    {
        _dropDownPopup!.OpenPopup(_toggleButton!);
    }

    private void DropDownPopup_PopupVisibilityChanged(object? sender, bool e)
    {
        bool hadFocus = _toggleButton!.IsFocused;

        if (e)
        {
            if (Content is StackPanel st)
            {
                st.Children.OfType<UIElement>().FirstOrDefault()?.Focus();
            }
        }
        else
        {
            _toggleButton!.IsChecked = false;

            if (hadFocus)
            {
                _toggleButton.Focus();
            }
        }
    }

    public void CloseDropDown()
    {
        _dropDownPopup!.ClosePopup();
    }
}