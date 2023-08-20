// *********************************************************
// 
// Coho.UI IApplicationMainBarControl.cs
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

using System.Windows.Controls;
using System.Windows.Forms;
using Coho.UI.Controls.Ribbon;

namespace Coho.UI.Interfaces;

internal interface IApplicationMainBarControl
{
    ContextMenu GetItemContextMenu(IRibbonCommand cmd);

    bool HandleKeyboardNavigation(Keys key);
    
    public bool EnableAnimations
    {
        get;
        set;
    }
}