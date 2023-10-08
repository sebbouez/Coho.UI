// *********************************************************
// 
// Coho.UI
// OmnibarControl.xaml.cs
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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Coho.UI.CommandManaging;

namespace Coho.UI.Controls.Omnibar;

/// <summary>
///     Logique d'interaction pour OmnibarControl.xaml
/// </summary>
public partial class OmnibarControl : UserControl
{
    public OmnibarControl()
    {
        InitializeComponent();
        Loaded += OmnibarControl_Loaded;
    }

    private void OmnibarControl_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void BdrOmniBar_MouseLeave(object sender, MouseEventArgs e)
    {
        if (!TbOmniBar.IsFocused && !TbOmniBar.IsKeyboardFocused)
        {
            BdrOmniBar.Opacity = 0.7;
        }
    }

    private void BdrOmniBar_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        BdrOmniBar.Opacity = 1;
    }

    private void BtnCloseFileSearch_Click(object sender, RoutedEventArgs e)
    {
        DoSearch();
    }

    private void DoSearch()
    {
        List<OmnibarSearchResult> availableCommands = new();

        OmnibarSearchServiceBase? defaultSearchService = OmnibarSearchService.OmnibarSearchServices.FirstOrDefault(x => x.IsDefault);
        if (defaultSearchService != null)
        {
            availableCommands.AddRange(defaultSearchService.ExecuteSearch(TbOmniBar.Text));
        }

        foreach (OmnibarSearchServiceBase searchService in OmnibarSearchService.OmnibarSearchServices.Where(x => !x.IsDefault))
        {
            availableCommands.Add(new OmnibarSearchResult
            {
                CommandType = OmnibarSearchResult.EOmnibarCommandType.CustomServiceLauncher,
                DisplayName = searchService.DisplayName,
                CommandTabName = searchService.Description,
                GroupName = OmnibarTexts.ResultsOtherServices,
                Icon = searchService.Icon,
                Tag = searchService
            });
        }

        ListOmnibarResults.ItemsSource = availableCommands;

        ICollectionView filterableOnlineUsers = CollectionViewSource.GetDefaultView(ListOmnibarResults.ItemsSource);
        filterableOnlineUsers.GroupDescriptions.Add(new PropertyGroupDescription(nameof(OmnibarSearchResult.GroupName)));

        BdrFileSearchResults.Visibility = Visibility.Collapsed;
        PopupOmnibarResults.IsOpen = true;
    }

    private void GotoOmniboxResultPage()
    {
        OmnibarSearchResult selectedSearchResult = (OmnibarSearchResult) ListOmnibarResults.SelectedItem;

        switch (selectedSearchResult.CommandType)
        {
            case OmnibarSearchResult.EOmnibarCommandType.CustomServiceLauncher:
                if (selectedSearchResult.Tag is OmnibarSearchServiceBase s)
                {
                    HandlerCustomSearchService(s);
                }

                return;
            case OmnibarSearchResult.EOmnibarCommandType.Others:
                OmnibarSearchService.InvokeOmnibarResultClick(selectedSearchResult);
                break;

            case OmnibarSearchResult.EOmnibarCommandType.RibbonCommand:
                HandleCommandClick(selectedSearchResult);
                break;
        }

        BdrFileSearchResults.Visibility = Visibility.Collapsed;
        TbOmniBar.Text = string.Empty;
        TbPlaceholderText.Visibility = Visibility.Visible;
        PopupOmnibarResults.IsOpen = false;
    }

    private void HandleCommandClick(OmnibarSearchResult selectedSearchResult)
    {
        if (selectedSearchResult.CommandRibbonButton != null)
        {
            selectedSearchResult.CommandRibbonButton.RaiseClick();
        }
        else if (selectedSearchResult.LinkedOriginalObject is MenuItem menuItem)
        {
            menuItem.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
        }
    }

    private void ListOmnibarFilesResults_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (ListOmnibarResults.SelectedItem != null && e.Key == Key.Enter)
        {
            GotoOmniboxResultPage();
        }

        if (e.Key == Key.Up && ListOmnibarResults.SelectedIndex == 0)
        {
            ListOmnibarResults.SelectedItem = null;
            TbOmniBar.Focus();
        }
    }

    private void ListOmnibarResults_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (ListOmnibarResults.SelectedItem != null)
        {
            GotoOmniboxResultPage();
        }
    }

    private void Omnibar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (PopupOmnibarResults == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(TbOmniBar.Text) || TbOmniBar.Text.Length < 2)
        {
            PopupOmnibarResults.IsOpen = false;
            return;
        }

        DoSearch();
    }

    private void HandlerCustomSearchService(OmnibarSearchServiceBase s)
    {
        SearchProgress.Visibility = Visibility.Visible;
        SearchProgress.IsIndeterminate = true;

        s.InternalSearchAsync(TbOmniBar.Text).ContinueWith(r =>
        {
            ListOmnibarResults.ItemsSource = r.Result;

            BdrFileSearchResults.Visibility = Visibility.Visible;
            SearchProgress.IsIndeterminate = false;
            SearchProgress.Visibility = Visibility.Collapsed;
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void TbOmniBar_GotFocus(object sender, RoutedEventArgs e)
    {
        BdrOmniBar.Opacity = 1;

        TbOmniBar.Cursor = Cursors.IBeam;
        Indicator.Background = (Brush) FindResource("AccentColor");
    }

    private void TbOmniBar_PreviewKeyUp(object sender, KeyEventArgs e)
    {
        TbPlaceholderText.Visibility = string.IsNullOrEmpty(TbOmniBar.Text) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void TbOmniBar_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Down && PopupOmnibarResults.IsOpen && ListOmnibarResults.Items.Count > 0)
        {
            ListOmnibarResults.Focus();
            ListOmnibarResults.SelectedIndex = 0;
        }
    }

    private void TbOmniBar_LostFocus(object sender, RoutedEventArgs e)
    {
        BdrOmniBar.Opacity = 0.7;

        TbPlaceholderText.Visibility = string.IsNullOrEmpty(TbOmniBar.Text) ? Visibility.Visible : Visibility.Collapsed;

        TbOmniBar.Cursor = Cursors.Arrow;
        Indicator.Background = Brushes.Transparent;
    }

    internal new void Focus()
    {
        _ = TbOmniBar.Focus();
    }

    internal void SetBorderThickness(double value)
    {
        BdrOmniBar.BorderThickness = new Thickness(value);
    }
}