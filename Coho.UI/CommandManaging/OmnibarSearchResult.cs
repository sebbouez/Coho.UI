// *********************************************************
// 
// Coho.UI
// OmnibarSearchResult.cs
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

using System.Windows.Media;
using Coho.UI.Controls.Ribbon;

namespace Coho.UI.CommandManaging;

public class OmnibarSearchResult
{
    public OmnibarSearchResult()
    {
        CommandType = EOmnibarCommandType.Others;
    }

    internal IRibbonCommand? CommandRibbonButton
    {
        get;
        set;
    }

    internal object LinkedOriginalObject
    {
        get;
        set;
    }

    internal RibbonTabItem? CommandRibbonTab
    {
        get;
        set;
    }

    public string CommandDescription
    {
        get;
        set;
    } = string.Empty;

    public int CommandHash
    {
        get;
        set;
    }

    public string DisplayName
    {
        get;
        set;
    } = string.Empty;

    public string CommandFullName
    {
        get;
        set;
    } = string.Empty;

    public string CommandTabName
    {
        get;
        set;
    } = string.Empty;

    internal EOmnibarCommandType CommandType
    {
        get;
        set;
    }

    public object? Tag
    {
        get;
        set;
    }

    public string? Gesture
    {
        get
        {
            return CommandRibbonButton?.Gesture;
        }
    }

    public string GroupName
    {
        get;
        set;
    } = string.Empty;

    public Brush? Icon
    {
        get;
        set;
    }

    internal enum EOmnibarCommandType
    {
        CustomServiceLauncher,
        RibbonCommand,
        Others
    }
}