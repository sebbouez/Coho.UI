// *********************************************************
// 
// Coho.UI FolderExplorerItem.cs
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

using System.ComponentModel;

namespace Coho.UI.Controls.Common;

public class FolderExplorerItem : INotifyPropertyChanged
{
    private string? _name;
    private object? _thumbnail;

    public FolderExplorerItem(string fullPath, FolderExplorerItemType itemType)
    {
        FullPath = fullPath;
        ItemType = itemType;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string FullPath
    {
        get;
        private set;
    }

    public FolderExplorerItemType ItemType
    {
        get;
        private set;
    }

    public string Name
    {
        get
        {
            return _name ?? string.Empty;
        }
        set
        {
            _name = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }
    }

    public object? Thumbnail
    {
        get
        {
            return _thumbnail;
        }
        set
        {
            _thumbnail = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Thumbnail)));
        }
    }
}