// *********************************************************
// 
// Coho.UI
// WindowDependencyProperties.cs
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

namespace Coho.UI.Controls;

internal class WindowDependencyProperties : DependencyObject
{
    public static readonly DependencyProperty IsFakeHoverProperty =
        DependencyProperty.RegisterAttached("IsFakeHover", typeof(bool), typeof(WindowDependencyProperties), new PropertyMetadata(default(bool)));

    public static void SetIsFakeHover(UIElement element, bool value)
    {
        element.SetValue(IsFakeHoverProperty, value);
    }

    public static bool GetIsFakeHover(UIElement element)
    {
        return (bool) element.GetValue(IsFakeHoverProperty);
    }
}