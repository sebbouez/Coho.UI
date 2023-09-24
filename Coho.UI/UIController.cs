// *********************************************************
// 
// Coho.UI
// UIController.cs
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
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Coho.UI;

public static class UIController
{
    private static ThemeScheme _theme = ThemeScheme.Light;

    private static bool _isInitialized;

    private static readonly Dictionary<string, ThemeScheme?> _customThemedResourceNames = new();

    public static ThemeScheme Theme
    {
        get
        {
            return _theme;
        }
        set
        {
            _theme = value;
            InternalManageTheme();
        }
    }

    internal static event EventHandler? ThemeChanged;

    public static void Init(ThemeScheme initialTheme)
    {
        _isInitialized = true;
        _theme = initialTheme;

        InternalManageTheme();
    }

    private static void InternalManageTheme()
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException("Unable to switch theme when UIController is not initialized.");
        }

        switch (Theme)
        {
            case ThemeScheme.Light:
                InstallTheme("Theme_BaseLight");
                break;
            case ThemeScheme.Dark:
                InstallTheme("Theme_BaseDark");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        ThemeChanged?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    ///     Lets you define your own Resources to be merged depending on the current theme
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="themeScheme"></param>
    /// <example>
    ///     UIController.RegisterThemedMergedResourceName("/DarkTheme.xaml", ThemeScheme.Dark);
    ///     UIController.RegisterThemedMergedResourceName("/LightTheme.xaml", ThemeScheme.Light);
    /// </example>
    public static void RegisterThemedMergedResourceName(string resourceName, ThemeScheme? themeScheme)
    {
        _customThemedResourceNames.TryAdd(resourceName, themeScheme);
    }

    private static void InstallTheme(string baseThemeName)
    {
        List<ResourceDictionary> resourcesToRemove = new();

        for (int i = 0; i < Application.Current.Resources.MergedDictionaries.Count; i++)
        {
            if (Application.Current.Resources.MergedDictionaries[i].Source.OriginalString.Contains("Theme_Base", StringComparison.InvariantCulture))
            {
                resourcesToRemove.Add(Application.Current.Resources.MergedDictionaries[i]);
            }

            _customThemedResourceNames.TryGetValue(Application.Current.Resources.MergedDictionaries[i].Source.OriginalString, out ThemeScheme? dt);

            if (dt != null && dt != Theme)
            {
                resourcesToRemove.Add(Application.Current.Resources.MergedDictionaries[i]);
            }
        }

        foreach (ResourceDictionary resourceDictionary in resourcesToRemove)
        {
            Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
        }

        foreach (KeyValuePair<string, ThemeScheme?> themedResource in _customThemedResourceNames.Where(x =>
                     x.Value == null || x.Value.Equals(Theme)))
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri(themedResource.Key, UriKind.RelativeOrAbsolute)
            });
        }

        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/Coho.UI;component/Themes/{0}.xaml", baseThemeName), UriKind.RelativeOrAbsolute)
        });
    }
}