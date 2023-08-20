using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommandManager = Coho.UI.CommandManaging.CommandManager;

namespace Coho.UI.Controls.Ribbon;

public sealed class RibbonTabItem : ContentControl
{
    public static readonly DependencyProperty GestureProperty =
        DependencyProperty.RegisterAttached(nameof(Gesture), typeof(string), typeof(RibbonTabItem), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.RegisterAttached(nameof(Header), typeof(string), typeof(RibbonTabItem), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsContextualProperty =
        DependencyProperty.RegisterAttached(nameof(IsContextual), typeof(bool), typeof(RibbonTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsOverflownProperty =
        DependencyProperty.RegisterAttached(nameof(IsOverflown), typeof(bool), typeof(RibbonTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.RegisterAttached(nameof(IsSelected), typeof(bool), typeof(RibbonTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public RibbonTabItem()
    {
        Items = new ObservableCollection<UIElement>();
        Loaded += RibbonTabItem_Loaded;
        MouseDown += RibbonTabItem_MouseDown;
    }

    // ReSharper disable once CollectionNeverUpdated.Global => Used in Xaml
    public ObservableCollection<UIElement> Items
    {
        get;
        set;
    }

    internal RibbonBar? ParentRibbon
    {
        get;
        set;
    }

    /// <summary>
    /// Gets if the tab is selected
    /// </summary>
    public bool IsSelected
    {
        get
        {
            return (bool) GetValue(IsSelectedProperty);
        }
        internal set
        {
            SetValue(IsSelectedProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets if the tab is contextual
    /// </summary>
    public bool IsContextual
    {
        get
        {
            return (bool) GetValue(IsContextualProperty);
        }
        set
        {
            SetValue(IsContextualProperty, value);
        }
    }

    /// <summary>
    /// Gets if the tab is removed due to the limited place
    /// </summary>
    public bool IsOverflown
    {
        get
        {
            return (bool) GetValue(IsOverflownProperty);
        }
        internal set
        {
            SetValue(IsOverflownProperty, value);
        }
    }

    internal string Gesture
    {
        get
        {
            return (string) GetValue(GestureProperty);
        }
        set
        {
            SetValue(GestureProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets the title of the tab
    /// </summary>
    public string Header
    {
        get
        {
            return (string) GetValue(HeaderProperty);
        }
        set
        {
            SetValue(HeaderProperty, value);
        }
    }

    private void RibbonTabItem_MouseDown(object sender, MouseButtonEventArgs e)
    {
        ParentRibbon!.SelectTab(this, true);
    }

    internal void FocusFirstItem()
    {
        Items.FirstOrDefault()?.Focus();
    }

    private void RibbonTabItem_Loaded(object sender, RoutedEventArgs e)
    {
        ApplyTemplate();
        CommandManager.AddRibbonCommandsToCache(this);
    }
}