// *********************************************************
// 
// Coho.UI Announcer.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Coho.UI.Controls.Announcer;

public class Announcer : ContentControl
{
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.RegisterAttached(nameof(Label), typeof(string), typeof(Announcer), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ItemsPerViewProperty =
        DependencyProperty.RegisterAttached(nameof(ItemsPerView), typeof(int), typeof(Announcer), new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty AnnouncesAreaMarginProperty =
        DependencyProperty.RegisterAttached(nameof(AnnouncesAreaMargin), typeof(Thickness), typeof(Announcer), new FrameworkPropertyMetadata(new Thickness(), FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsLoadingContentProperty =
        DependencyProperty.RegisterAttached(nameof(IsLoadingContent), typeof(bool), typeof(Announcer), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    private int _currentPage;
    private bool _isLoaded;

    private ItemsControl? _itemsGrid;
    private Grid? _mainGrid;
    private Button? _nextButton;
    private Button? _previousButton;

    public Announcer()
    {
        Loaded += OnLoaded;
        ClipToBounds = false;
    }

    public ObservableCollection<object> Items
    {
        get;
    } = new();

    /// <summary>
    /// Gets or sets the state that indicates that content is being loaded
    /// </summary>
    public bool IsLoadingContent
    {
        get
        {
            return (bool) GetValue(IsLoadingContentProperty);
        }
        set
        {
            SetValue(IsLoadingContentProperty, value);
        }
    }

    public IEnumerable<object> ItemsSource
    {
        set
        {
            Items.Clear();

            foreach (object item in value)
            {
                Items.Add(item);
            }

            ShowPage();
        }
    }

    public DataTemplate? AnnounceTemplate
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the description to display on top of the control
    /// </summary>
    public string Label
    {
        get
        {
            return (string) GetValue(LabelProperty);
        }
        set
        {
            SetValue(LabelProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets the number of items per page
    /// </summary>
    public int ItemsPerView
    {
        get
        {
            return (int) GetValue(ItemsPerViewProperty);
        }
        set
        {
            SetValue(ItemsPerViewProperty, value);
        }
    }

    public Thickness AnnouncesAreaMargin
    {
        get
        {
            return (Thickness) GetValue(AnnouncesAreaMarginProperty);
        }
        set
        {
            SetValue(AnnouncesAreaMarginProperty, value);
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_isLoaded)
        {
            return;
        }

        _isLoaded = true;

        ApplyTemplate();

        _mainGrid = (Grid) Template.FindName("MainGrid", this);
        _itemsGrid = (ItemsControl) Template.FindName("ItemsControl", this);
        _itemsGrid.ItemTemplate = AnnounceTemplate;

        _previousButton = (Button) Template.FindName("BtnPrev", this);
        _nextButton = (Button) Template.FindName("BtnNext", this);

        _previousButton.Click += PreviousButtonOnClick;
        _nextButton.Click += NextButtonOnClick;

        ShowPage();
    }

    private void NextButtonOnClick(object sender, RoutedEventArgs e)
    {
        ShowPage(_currentPage + 1);
    }

    private void PreviousButtonOnClick(object sender, RoutedEventArgs e)
    {
        ShowPage(_currentPage - 1);
    }

    private void ShowPage(int pageNum = 0)
    {
        int nextItemIndex = pageNum * ItemsPerView;
        IEnumerable<object> itemsOnPage = Items.Take(new Range(new Index(nextItemIndex), new Index(nextItemIndex + ItemsPerView)));

        _itemsGrid!.Opacity = 0;
        _itemsGrid.ItemsSource = itemsOnPage;

        Animate(pageNum > _currentPage);
        _currentPage = pageNum;

        _previousButton!.IsEnabled = _currentPage != 0;
        _nextButton!.IsEnabled = _currentPage * ItemsPerView + ItemsPerView < Items.Count;
    }

    private void Animate(bool forward)
    {
        Storyboard sb = new();
        DoubleAnimation opacityAnimation = new()
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromMilliseconds(450)
        };

        ThicknessAnimation thicknessAnimation = new()
        {
            From = new Thickness(forward ? 24 : -24, 0, 0, 0),
            To = new Thickness(0, 0, 0, 0),
            Duration = TimeSpan.FromMilliseconds(450),
            EasingFunction = new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            }
        };

        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(OpacityProperty));
        Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(MarginProperty));
        Storyboard.SetTarget(opacityAnimation, _itemsGrid);
        Storyboard.SetTarget(thicknessAnimation, _itemsGrid);
        sb.Children.Add(opacityAnimation);
        sb.Children.Add(thicknessAnimation);
        sb.Begin();
    }
}