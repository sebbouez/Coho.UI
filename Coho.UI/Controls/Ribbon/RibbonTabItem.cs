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
        DependencyProperty.RegisterAttached("Gesture", typeof(string), typeof(RibbonTabItem), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.RegisterAttached("Header", typeof(string), typeof(RibbonTabItem), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsContextualProperty =
        DependencyProperty.RegisterAttached("IsContextual", typeof(bool), typeof(RibbonTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsOverflownProperty =
        DependencyProperty.RegisterAttached("IsOverflown", typeof(bool), typeof(RibbonTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(RibbonTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public RibbonTabItem()
    {
        Items = new ObservableCollection<UIElement>();
        Loaded += RibbonTabItem_Loaded;
        MouseDown += RibbonTabItem_MouseDown;
    }

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

    public bool IsOverflown
    {
        get
        {
            return (bool) GetValue(IsOverflownProperty);
        }
        set
        {
            SetValue(IsOverflownProperty, value);
        }
    }

    public string Gesture
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
        //RibbonTabItemInnerToolbar t = new();
        //foreach (UIElement item in Items)
        //{
        //    _ = t.Items.Add(item);
        //}
        //Items.Clear();
        //Content = t;

        CommandManager.AddCommandsToCache(this);
    }
}