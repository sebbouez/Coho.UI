using System.Windows;
using System.Windows.Media;

namespace Coho.UI.Controls.Ribbon;

internal interface IRibbonCommand
{
    string Description
    {
        get;
        set;
    }

    RibbonEnums.RibbonButtonDisplay Display
    {
        get;
        set;
    }

    string Gesture
    {
        get;
        set;
    }

    Brush? Icon
    {
        get;
        set;
    }

    bool IsEnabled
    {
        get;
        set;
    }

    bool IsInQAT
    {
        get;
        set;
    }

    bool LockEnabledState
    {
        get;
        set;
    }

    string Name
    {
        get;
        set;
    }

   internal IRibbonCommand? OriginalCommand
    {
        get;
        set;
    }

    string Text
    {
        get;
        set;
    }

    internal event RoutedEventHandler OnClick;

    internal void RaiseClick();
}