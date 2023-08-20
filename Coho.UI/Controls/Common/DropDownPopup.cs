// *********************************************************
// 
// Coho.UI
// DropDownPopup.cs
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
using System.Windows.Controls.Primitives;

namespace Coho.UI.Controls.Common;

public class DropDownPopup : ContentControl
{
    public static readonly DependencyProperty PlacementProperty =
        DependencyProperty.Register(nameof(Placement), typeof(PlacementMode), typeof(DropDownPopup), new PropertyMetadata(PlacementMode.Bottom));

    private Popup? _popup;

    public DropDownPopup()
    {
        Focusable = false;

        if (InternalFrameworkSettings.IsWindows11)
        {
            Template = (ControlTemplate)FindResource("DropDownPopupTemplateAcrylic");
        }
        else
        {
            Template = (ControlTemplate)FindResource("DropDownPopupTemplate");
        }

        Loaded += DropDownPopup_Loaded;
    }

    public PlacementMode Placement
    {
        get
        {
            return (PlacementMode) GetValue(PlacementProperty);
        }
        set
        {
            SetValue(PlacementProperty, value);
        }
    }

    internal event EventHandler<bool>? PopupVisibilityChanged;

    private void DropDownPopup_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyTemplate();
        _popup = (Popup?) Template.FindName("InnerPopup", this);

        if (_popup != null)
        {
            _popup.Opened -= Popup_Opened;
            _popup.Opened += Popup_Opened;

            _popup.Closed -= Popup_Closed;
            _popup.Closed += Popup_Closed;
        }
    }

    private void Popup_Closed(object? sender, EventArgs e)
    {
        PopupVisibilityChanged?.Invoke(this, _popup!.IsOpen);
    }

    private void Popup_Opened(object? sender, EventArgs e)
    {
        PopupVisibilityChanged?.Invoke(this, _popup!.IsOpen);
    }

    internal void OpenPopup(UIElement relativeElement)
    {
        _popup!.PlacementTarget = relativeElement;
        _popup!.Placement = Placement;
        _popup!.IsOpen = true;
        _popup!.UpdateLayout();
        _popup!.InvalidateMeasure();
    }

    private void ResetFocus()
    {
        if (_popup!.PlacementTarget != null)
        {
            _popup!.PlacementTarget.Focus();
        }
    }

    internal void SetPopupState(bool opened)
    {
        _popup!.IsOpen = opened;

        if (!opened)
        {
            ResetFocus();
        }
    }

    internal void ClosePopup()
    {
        _popup!.IsOpen = false;
        ResetFocus();
    }
}