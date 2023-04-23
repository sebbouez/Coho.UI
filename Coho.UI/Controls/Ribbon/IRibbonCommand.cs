using System.Windows;
using System.Windows.Media;

namespace Coho.UI.Controls.Ribbon;

public interface IRibbonCommand
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

    IRibbonCommand? OriginalCommand
    {
        get;
        set;
    }

    string Text
    {
        get;
        set;
    }

    event RoutedEventHandler OnClick;

    void RaiseClick();
}