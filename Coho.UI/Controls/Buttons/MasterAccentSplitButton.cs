// *********************************************************
// 
// Coho.UI
// MasterAccentSplitButton.cs
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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Coho.UI.Controls.Common;

namespace Coho.UI.Controls.Buttons;

public class MasterAccentSplitButton : Button
{
    public static readonly DependencyProperty DropDownContentProperty =
        DependencyProperty.Register(nameof(DropDownContent), typeof(object), typeof(MasterAccentSplitButton), null);

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.RegisterAttached(nameof(Icon), typeof(Brush), typeof(MasterAccentSplitButton), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(nameof(Text), typeof(string), typeof(MasterAccentSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    private Button? _buttonPart;
    private DropDownPopup? _dropDownPopup;
    private ToggleButton? _toggleButtonPart;

    public MasterAccentSplitButton()
    {
        SetValue(ToolTipService.ShowOnDisabledProperty, true);
        Loaded += MasterAccentSplitButton_Loaded;
        Click += MasterAccentSplitButton_Click;
    }

    /// <summary>
    /// Gets or sets the content of the dropdown popup
    /// </summary>
    public object DropDownContent
    {
        get
        {
            return GetValue(DropDownContentProperty);
        }
        set
        {
            SetValue(DropDownContentProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets the icon displayed in the button
    /// </summary>
    public Brush Icon
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

    /// <summary>
    /// Gets or sets the text displayed in the button
    /// </summary>
    public string Text
    {
        get
        {
            return (string) GetValue(TextProperty);
        }
        set
        {
            SetValue(TextProperty, value);
        }
    }

    private void MasterAccentSplitButton_Click(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is FrameworkElement el)
        {
            if (el.Name == _buttonPart!.Name)
            {
                return;
            }

            if (el.Name == _toggleButtonPart!.Name)
            {
                _dropDownPopup!.SetPopupState(_toggleButtonPart.IsChecked!.Value);
                e.Handled = true;
            }
        }
    }

    private void MasterAccentSplitButton_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyTemplate();

        _buttonPart = (Button) Template.FindName("BtnLeftPart", this);
        _toggleButtonPart = (ToggleButton) Template.FindName("BtnDropDownPart", this);
        _dropDownPopup = (DropDownPopup) Template.FindName("DropDownPopupPart", this);
        _dropDownPopup.PopupVisibilityChanged += DropDownPopup_PopupVisibilityChanged;
    }

    private void DropDownPopup_PopupVisibilityChanged(object? sender, bool e)
    {
        if (!e)
        {
            _toggleButtonPart!.IsChecked = e;
        }
    }
}