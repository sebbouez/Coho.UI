// *********************************************************
// 
// Coho.UI
// OmnibarSearchServiceBase.cs
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
using System.Threading.Tasks;
using System.Windows.Media;
using Coho.UI.CommandManaging;

namespace Coho.UI.Controls.Omnibar;

public abstract class OmnibarSearchServiceBase
{
    /// <summary>
    /// Gets the name of the search service, will be displayed
    /// </summary>
    public abstract string DisplayName
    {
        get;
    }

    /// <summary>
    /// Gets the description of the search service, will be displayed
    /// </summary>
    public virtual string Description
    {
        get
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Indicates if the search service is the default one in the Omnibar
    /// </summary>
    public virtual bool IsDefault
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the icon to represent the search service, will be displayed
    /// </summary>
    public virtual Brush Icon
    {
        get
        {
            return Brushes.Transparent;
        }
    }

    internal Task<IEnumerable<OmnibarSearchResult>> InternalSearchAsync(string terms)
    {
        return Task.Run(() =>
        {
            return ExecuteSearch(terms);
        });
    }

    public virtual IEnumerable<OmnibarSearchResult> ExecuteSearch(string terms)
    {
        return new List<OmnibarSearchResult>();
    }
}