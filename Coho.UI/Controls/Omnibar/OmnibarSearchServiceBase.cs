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
    public abstract string DisplayName
    {
        get;
    }

    public virtual string Description
    {
        get
        {
            return string.Empty;
        }
    }

    public virtual bool IsDefault
    {
        get
        {
            return false;
        }
    }

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