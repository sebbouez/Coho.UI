// *********************************************************
// 
// Coho.UI FolderExplorer.xaml.cs
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Coho.UI.Dialogs;
using Coho.UI.Tools;
using Coho.UI.Win32;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Coho.UI.Controls.Common;

internal partial class FolderExplorer : UserControl
{
    private readonly object? _dummyNode = null;
    private CancellationTokenSource _cancelSource = new();

    private ObservableCollection<FolderExplorerItem> _pathItems = new ObservableCollection<FolderExplorerItem>();

    public FolderExplorer()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    internal string? CurrentPath
    {
        get;
        private set;
    }

    internal string FilesSearchPattern
    {
        get;
        set;
    } = "*.*";

    public static readonly DependencyProperty ShowSpecialFoldersProperty =
        DependencyProperty.Register(nameof(ShowSpecialFolders), typeof(bool), typeof(FolderExplorer), new PropertyMetadata(true));

    internal bool ShowSpecialFolders
    {
        get
        {
            return (bool) GetValue(ShowSpecialFoldersProperty);
        }
        set
        {
            SetValue(ShowSpecialFoldersProperty, value);
        }
    }

    public static readonly DependencyProperty ShowNavigationPaneProperty =
        DependencyProperty.Register(nameof(ShowNavigationPane), typeof(bool), typeof(FolderExplorer), new PropertyMetadata(true));

    internal bool ShowNavigationPane
    {
        get
        {
            return (bool) GetValue(ShowNavigationPaneProperty);
        }
        set
        {
            SetValue(ShowNavigationPaneProperty, value);
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _ = RebuildDrivesAsync();
        PathItems.ItemsSource = _pathItems;
    }

    internal event EventHandler<string>? FileDoubleClicked;

    internal event EventHandler<string>? FileSelected;

    private void RefreshPathItems(string path)
    {
        _pathItems.Clear();

        string[] allPaths = path.Split("\\", StringSplitOptions.RemoveEmptyEntries);
        string currentPath = string.Empty;

        foreach (string item in allPaths)
        {
            currentPath = string.Concat(currentPath, string.IsNullOrEmpty(currentPath) ? "" : "\\", item);

            _pathItems.Add(new FolderExplorerItem(currentPath, FolderExplorerItemType.Directory)
            {
                Name = item
            });
        }
    }

    private void LvItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (LvItems.SelectedItem == null)
        {
            return;
        }

        object selectedItem = ((FrameworkElement) e.OriginalSource).DataContext;
        if (selectedItem is not FolderExplorerItem folderItem)
        {
            return;
        }

        if (folderItem.ItemType == FolderExplorerItemType.Directory)
        {
            _ = ExploreDirectory(folderItem.FullPath);
            return;
        }

        FileDoubleClicked?.Invoke(this, folderItem.FullPath);
    }

    private void LvItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LvItems.SelectedItem != null && LvItems.SelectedValue is FolderExplorerItem item && item.ItemType == FolderExplorerItemType.File)
        {
            FileSelected?.Invoke(this, item.FullPath);
        }
    }

    internal async Task ExploreDirectory(string path)
    {
        List<string> allFiles = new();
        List<string> allDirectories = new();

        try
        {
            await Task.Run(() =>
            {
                allDirectories = Directory.GetDirectories(path).ToList();

                string[] allExtPatterns = FilesSearchPattern.Split(';', StringSplitOptions.RemoveEmptyEntries);

                foreach (string extPattern in allExtPatterns)
                {
                    allFiles.AddRange(Directory.GetFiles(path, extPattern));
                }
            });
        }
        catch (Exception ex)
        {
            ThemedMessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            return;
        }

        CurrentPath = path;

        ObservableCollection<FolderExplorerItem> files = new();

        try
        {
            if ((VisualTreeHelper.GetChild(LvItems, 0) as Border)?.Child is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToVerticalOffset(0);
            }
        }
        catch
        {
            // rien
        }

        LvItems.IsEnabled = false;
        _cancelSource.Cancel();

        _cancelSource.Dispose(); // Clean up old token source....
        _cancelSource = new CancellationTokenSource();

        _ = Task.Run(() =>
        {
            foreach (string dir in allDirectories)
            {
                try
                {
                    DirectoryInfo di = new(dir);
                    _ = di.GetAccessControl();
                }
                catch
                {
                    // pas d'accès à ce répertoire
                    continue;
                }

                FolderExplorerItem item = new(dir, FolderExplorerItemType.Directory)
                {
                    Name = Path.GetFileName(dir)
                };


                files.Add(item);
            }

            foreach (string file in allFiles)
            {
                FolderExplorerItem item = new(file, FolderExplorerItemType.File)
                {
                    Name = Path.GetFileName(file)
                };

                files.Add(item);
            }
        }).ContinueWith(r =>
        {
            RefreshPathItems(path);
            LvItems.ItemsSource = files;

            CollectionView view = (CollectionView) CollectionViewSource.GetDefaultView(LvItems.ItemsSource);
            view.SortDescriptions.Add(new SortDescription(nameof(FolderExplorerItem.ItemType), ListSortDirection.Descending));
            view.SortDescriptions.Add(new SortDescription(nameof(FolderExplorerItem.Name), ListSortDirection.Ascending));

            LvItems.IsEnabled = true;

            _ = DisplayThumbnailsAsync(_cancelSource.Token);
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private Task DisplayThumbnailsAsync(CancellationToken cancelToken)
    {
        return Task.Run(() =>
        {
            foreach (FolderExplorerItem item in LvItems.ItemsSource.OfType<FolderExplorerItem>())
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    BitmapSource thumbnail = GetSource(item);
                    thumbnail.Freeze();
                    Dispatcher.BeginInvoke(() =>
                    {
                        item.Thumbnail = thumbnail;
                    }, DispatcherPriority.ApplicationIdle);
                }
                catch
                {
                    // thumbnail fails to load, leave it blank for the moment
                }
            }
        }, cancelToken);
    }

    private BitmapSource GetSource(FolderExplorerItem filePath)
    {
        Bitmap bitmap = Win32Thumbnails.GetThumbnail(filePath.FullPath, 200, 200, Win32Thumbnails.ThumbnailOptions.None);
        return GraphicsTools.ConvertBitmap(bitmap);
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        // todo
    }

    private void FoldersItem_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        LeftPaneScroller.ScrollToVerticalOffset(LeftPaneScroller.VerticalOffset - e.Delta);
    }

    private void LinkDesktop_Click(object sender, RoutedEventArgs e)
    {
        string path = KnownFolders.GetPath(KnownFolder.Desktop);
        _ = ExploreDirectory(path);
    }

    private void LinkDownloads_Click(object sender, RoutedEventArgs e)
    {
        string path = KnownFolders.GetPath(KnownFolder.Downloads);
        _ = ExploreDirectory(path);
    }

    private void LinkDocuments_Click(object sender, RoutedEventArgs e)
    {
        string path = KnownFolders.GetPath(KnownFolder.Documents);
        _ = ExploreDirectory(path);
    }

    private void LinkImages_Click(object sender, RoutedEventArgs e)
    {
        string path = KnownFolders.GetPath(KnownFolder.Pictures);
        _ = ExploreDirectory(path);
    }

    private void LinkMusic_Click(object sender, RoutedEventArgs e)
    {
        string path = KnownFolders.GetPath(KnownFolder.Music);
        _ = ExploreDirectory(path);
    }

    private void LinkVideos_Click(object sender, RoutedEventArgs e)
    {
        string path = KnownFolders.GetPath(KnownFolder.Videos);
        _ = ExploreDirectory(path);
    }

    private Task RebuildDrivesAsync()
    {
        Dispatcher.Invoke(() =>
        {
            FoldersItem.Items.Clear();
        });

        return Task.Run(() =>
        {
            List<FolderExplorerItem> results = new();
            DriveInfo[] myDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in myDrives)
            {
                if (drive.IsReady)
                {
                    FolderExplorerItemType driveType = FolderExplorerItemType.LocalDrive;

                    switch (drive.DriveType)
                    {
                        case DriveType.Network:
                            driveType = FolderExplorerItemType.NetworkDrive;
                            break;
                        case DriveType.Removable:
                            driveType = FolderExplorerItemType.RemovableDrive;
                            break;
                        case DriveType.Fixed:

                            // Maybe not the best way to ensure this drive is Windows drive :(
                            if (Directory.Exists(Path.Combine(drive.RootDirectory.FullName, "windows", "system")))
                            {
                                driveType = FolderExplorerItemType.SystemDrive;
                            }

                            break;
                    }

                    results.Add(new FolderExplorerItem(drive.RootDirectory.FullName, driveType)
                    {
                        Name = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", drive.VolumeLabel, drive.Name.TrimEnd('\\'))
                    });
                }
            }

            return results;
        }).ContinueWith(r =>
        {
            if (r.Exception != null)
            {
                return;
            }

            foreach (FolderExplorerItem explorerItem in r.Result)
            {
                TreeViewItem item = new()
                {
                    DataContext = explorerItem,
                    Tag = explorerItem.FullPath,
                    FontWeight = FontWeights.Normal
                };
                item.Items.Add(_dummyNode);
                item.Expanded += TreeViewFolder_Expanded;
                FoldersItem.Items.Add(item);
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void TreeViewFolder_Expanded(object sender, RoutedEventArgs e)
    {
        TreeViewItem item = (TreeViewItem) sender;
        if (item.Items.Count == 1 && item.Items[0] == _dummyNode)
        {
            item.Items.Clear();
            try
            {
                foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                {
                    try
                    {
                        DirectoryInfo dir = new(s);
                        if (dir.Attributes.HasFlag(FileAttributes.Hidden))
                        {
                            continue;
                        }
                    }
                    catch
                    {
                        continue;
                    }

                    TreeViewItem subitem = new()
                    {
                        DataContext = new FolderExplorerItem(s, FolderExplorerItemType.Directory)
                        {
                            Name = s.Substring(s.LastIndexOf("\\", StringComparison.Ordinal) + 1)
                        },
                        Tag = s,
                        FontWeight = FontWeights.Normal
                    };
                    subitem.Items.Add(_dummyNode);
                    subitem.Expanded += TreeViewFolder_Expanded;
                    item.Items.Add(subitem);
                }
            }
            catch (Exception)
            {
                // ne rien faire
            }
        }
    }

    private void FoldersItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        // si on a cliqué sur la flèche pour etendre ou fermer un noeud, on ne provoque pas de navigation
        if (e.OriginalSource is Rectangle
            || e.OriginalSource is ToggleButton)
        {
            return;
        }

        if (e.Source is TreeViewItem item)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _ = ExploreDirectory(item.Tag.ToString());
            }
        }
    }

    private void BtnUp_OnClick(object sender, RoutedEventArgs e)
    {
        DirectoryInfo di = new(CurrentPath);
        if (di.Parent != null)
        {
            _ = ExploreDirectory(di.Parent.FullName);
        }
        else
        {
            SystemSounds.Beep.Play();
        }
    }

    private void BtnFolderPath_Click(object sender, RoutedEventArgs e)
    {
        FolderExplorerItem item = (FolderExplorerItem) ((FrameworkElement) sender).DataContext;
        _ = ExploreDirectory(item.FullPath);
    }
}