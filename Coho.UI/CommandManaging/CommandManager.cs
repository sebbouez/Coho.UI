// *********************************************************
// 
// Coho.UI
// CommandManager.cs
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
using Coho.UI.Controls.Ribbon;
using Coho.UI.Tools;

namespace Coho.UI.CommandManaging;

internal static class CommandManager
{
    private static readonly List<OmnibarSearchResult> CommandsCache = new();

    internal static OmnibarSearchResult? GetCommandByHash(string hashCode)
    {
        return CommandsCache.FirstOrDefault(x => x.CommandHash.ToString(CultureInfo.InvariantCulture) == hashCode);
    }

    internal static List<OmnibarSearchResult> GetCommands()
    {
        return CommandsCache;
    }

    internal static IEnumerable<OmnibarSearchResult> FindCommands(string text, int limit = 6)
    {
        text = text.ToLowerInvariant();
        return CommandsCache
            .Where(x => x.DisplayName.Contains(text, StringComparison.InvariantCultureIgnoreCase)
                        || (!string.IsNullOrEmpty(x.CommandDescription) && x.CommandDescription.Contains(text, StringComparison.InvariantCultureIgnoreCase))
            ).Take(limit).ToList();
    }

    /// <summary>
    ///     Permet d'ajouter des commandes d'un onglet de ruban au cache
    /// </summary>
    /// <param name="tabItem"></param>
    internal static void AddCommandsToCache(RibbonTabItem tabItem)
    {
        foreach (UIElement cmdBtn in tabItem.Items)
        {
            if (cmdBtn.Visibility != Visibility.Visible)
            {
                continue;
            }

            if (cmdBtn is IRibbonCommand ribbonButton)
            {
                OmnibarSearchResult cmd = new()
                {
                    DisplayName = ribbonButton.Text,
                    CommandTabName = tabItem.Header,
                    CommandDescription = ribbonButton.Description,
                    CommandRibbonButton = ribbonButton,
                    CommandRibbonTab = tabItem,
                    Icon = ribbonButton.Icon,
                    CommandFullName = string.Format(CultureInfo.InvariantCulture, "{0} : {1}", tabItem.Header, ribbonButton.Text),
                    CommandHash = ribbonButton.Name.GetStaticHashCode(),
                    CommandType = OmnibarSearchResult.EOmnibarCommandType.RibbonCommand,
                    GroupName = OmnibarTexts.ResultsCommandsGroupName
                };

                CommandsCache.Add(cmd);
            }

            if (cmdBtn is IRibbonCommandWithChildren ribbonDropDown)
            {
                List<IRibbonCommand> subCommands = ribbonDropDown.GetSubCommands();
                foreach (IRibbonCommand subCommand in subCommands)
                {
                    OmnibarSearchResult cmd = new()
                    {
                        DisplayName = subCommand.Text,
                        CommandTabName = string.Format(CultureInfo.InvariantCulture, "{0} > {1}", tabItem.Header, ribbonDropDown.Text),
                        CommandDescription = subCommand.Description,
                        CommandRibbonButton = subCommand,
                        CommandRibbonTab = tabItem,
                        Icon = subCommand.Icon,
                        CommandFullName = string.Format(CultureInfo.InvariantCulture, "{0} : {1} : {2}", tabItem.Header, ribbonDropDown.Text, subCommand.Text),
                        CommandHash = subCommand.Name.GetStaticHashCode(),
                        CommandType = OmnibarSearchResult.EOmnibarCommandType.RibbonCommand,
                        GroupName = OmnibarTexts.ResultsCommandsGroupName
                    };

                    CommandsCache.Add(cmd);
                }
            }
        }
    }

    /// <summary>
    ///     Réinitialise le cache complet des commandes du ruban
    /// </summary>
    /// <param name="ribbon"></param>
    internal static void RebuildCommandsCache(RibbonBar ribbon)
    {
        CommandsCache.Clear();

        foreach (RibbonTabItem tab in ribbon.Items.OfType<RibbonTabItem>())
        {
            AddCommandsToCache(tab);
        }
    }
}