// *********************************************************
// 
// Coho.UI
// WpfTools.cs
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

using System.Windows;
using System.Windows.Media;

namespace Coho.UI.Tools;

internal static class WpfTools
{
    internal static T? GetClosestParent<T>(DependencyObject child) where T : DependencyObject
    {
        if (child is not Visual)
        {
            return null;
        }

        DependencyObject? parentObject = VisualTreeHelper.GetParent(child);
       
        if (parentObject == null)
        {
            return null;
        }

        return parentObject is T parent ? parent : GetClosestParent<T>(parentObject);
    }
}