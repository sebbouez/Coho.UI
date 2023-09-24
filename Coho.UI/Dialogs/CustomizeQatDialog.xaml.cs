// *********************************************************
// 
// Coho.UI CustomizeQatDialog.xaml.cs
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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Coho.UI.CommandManaging;
using Coho.UI.Controls.Common;
using Coho.UI.Windows;

namespace Coho.UI.Dialogs;

public partial class CustomizeQatDialog : SecondaryWindow
{
    public CustomizeQatDialog()
    {
        InitializeComponent();
        Loaded+= OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        LstAvailableItems.ItemsSource = AvailableItems;
        LstItems.ItemsSource = Items;
    }
    
    internal IEnumerable<OmnibarSearchResult>? AvailableItems
    {
        get;
        set;
    }

    internal ObservableCollection<CommandItemModel>? Items
    {
        get;
        set;
    }
    
    private void LstAvailableItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        BtnAdd.IsEnabled = LstAvailableItems.SelectedItem != null;
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        if (LstAvailableItems.SelectedItem is OmnibarSearchResult item)
        {
            Items?.Add(new CommandItemModel
            {
                Icon = item.Icon,
                Label = item.DisplayName!,
                Hash = item.CommandHash.ToString(CultureInfo.InvariantCulture)
            });
        }
    }

    private void BtnRemove_Click(object sender, RoutedEventArgs e)
    {
        _ = Items?.Remove((CommandItemModel) LstItems.SelectedItem);
    }

    private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        BtnRemove.IsEnabled = LstItems.SelectedItem != null;
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
    
    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}