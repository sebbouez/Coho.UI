using System.ComponentModel;

namespace Coho.UI.Controls.Ribbon;

internal class RibbonKeyboardNavigationUIIndicator : INotifyPropertyChanged
{
    private bool _showTips;

    public RibbonKeyboardNavigationUIIndicator()
    {
        InternalRibbonSettings.KeyboardNavigationUIIndicator = this;
    }

    public bool ShowTips
    {
        get
        {
            return _showTips;
        }
        set
        {
            _showTips = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowTips)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}