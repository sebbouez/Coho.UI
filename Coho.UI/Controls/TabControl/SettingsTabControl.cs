// *********************************************************
// 
// Coho.UI SettingsTabControl.cs
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
using System.Windows.Media.Animation;

namespace Coho.UI.Controls.TabControl;

public class SettingsTabControl : System.Windows.Controls.TabControl
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(SettingsTabControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    private TextBlock? _currentTitleTextBlock;
    private Border? _contentBorder;

    public SettingsTabControl()
    {
        Loaded += OnLoaded;
        SelectionChanged += OnSelectionChanged;
    }

    public string Title
    {
        get
        {
            return (string) GetValue(TitleProperty);
        }
        set
        {
            SetValue(TitleProperty, value);
        }
    }

    private bool _isLoaded;

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _isLoaded = true;

        ApplyTemplate();

        _currentTitleTextBlock = (TextBlock) Template.FindName("TbCurrentTabTitle", this);
        _contentBorder = (Border) Template.FindName("BdrContent", this);

        if (SelectedItem is SettingsTabControlItem tabItem)
        {
            _currentTitleTextBlock!.Text = tabItem.Title;
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectedItem is not SettingsTabControlItem tabItem || !_isLoaded)
        {
            return;
        }

        // Ignore WPF routed events from children controls
        if (!ReferenceEquals(e.OriginalSource, this))
        {
            return;
        }

        _currentTitleTextBlock!.Text = tabItem.Title;


        //if (CurrentSessionState.UserSettings.UseAnimations)
        {
            Storyboard sb = new();
            DoubleAnimation opacityAnimation = new()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };
        
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTarget(opacityAnimation, _contentBorder);
            sb.Children.Add(opacityAnimation);
            sb.Begin();
        }
        
    }
}