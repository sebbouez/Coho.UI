using System.Collections.Generic;
using System.Windows;

namespace Coho.UI.Controls.Ribbon;

internal interface IRibbonCommandWithChildren
{
    string Text
    {
        get;
        set;
    }

    object GetDropDownContent();

    void SetDropDownContent(UIElement content);

    List<IRibbonCommand> GetSubCommands();
}