// *********************************************************
// 
// Coho.UI
// OmnibarSearchService.cs
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

using System;
using System.Collections.Generic;
using Coho.UI.Controls.Omnibar;

namespace Coho.UI.CommandManaging;

public static class OmnibarSearchService
{
    internal static List<OmnibarSearchServiceBase> OmnibarSearchServices = new();

    /// <summary>
    ///     Occurs when the user clicks a result in the omnibar list.
    /// </summary>
    public static event EventHandler<OmnibarSearchResult>? SearchResultClicked;

    /// <summary>
    ///     Used to register a custom search service in the Omnibar.
    /// </summary>
    /// <param name="service">Your custom search service provider, of base type <see cref="OmnibarSearchServiceBase" /></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void RegisterOmnibarSearchService(OmnibarSearchServiceBase service)
    {
        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        OmnibarSearchServices.Add(service);
    }

    internal static void InvokeOmnibarResultClick(OmnibarSearchResult item)
    {
        SearchResultClicked?.Invoke(null, item);
    }
}