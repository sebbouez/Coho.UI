// *********************************************************
// 
// Coho.UI
// RibbonOmnibarSearchService.cs
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

using System.Collections.Generic;
using System.Linq;
using Coho.UI.CommandManaging;

namespace Coho.UI.Controls.Omnibar;

public class RibbonOmnibarSearchService : OmnibarSearchServiceBase
{
    public override bool IsDefault
    {
        get
        {
            return true;
        }
    }

    public override string DisplayName
    {
        get
        {
            return "Search command";
        }
    }

    public override IEnumerable<OmnibarSearchResult> ExecuteSearch(string terms)
    {
        IEnumerable<OmnibarSearchResult> foundCommands = CommandManager.FindCommands(terms);

        IEnumerable<OmnibarSearchResult> filteredCommands = from a in foundCommands
            where a.CommandRibbonButton.IsEnabled && a.CommandRibbonTab.IsEnabled && a.CommandRibbonTab.IsVisible
            select a;

        return filteredCommands;
    }
}