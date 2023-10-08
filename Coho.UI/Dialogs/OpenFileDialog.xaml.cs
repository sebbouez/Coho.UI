// *********************************************************
// 
// Coho.UI OpenFileDialog.xaml.cs
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
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Coho.UI.Win32;
using Coho.UI.Windows;

namespace Coho.UI.Dialogs;

internal partial class OpenFileDialog : SecondaryWindow
{
    internal OpenFileDialog()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    internal string? FileName
    {
        get;
        set;
    }

    internal ThemedSpecialDialogOptions Options
    {
        get;
        set;
    } = new();

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // If you don't provide a value for the initial directory, we start with "My docs" known folder
        if (string.IsNullOrEmpty(Options.InitialDirectory))
        {
            Options.InitialDirectory = KnownFolders.GetPath(KnownFolder.Documents);
        }

        Explorer.ShowSpecialFolders = Options.ShowDefaultSpecialFolders;

        BuildFileTypesCombo();
        _ = Explorer.ExploreDirectory(Options.InitialDirectory);
    }

    private void BuildFileTypesCombo()
    {
        CbFileType.IsEnabled = false;
        foreach (KeyValuePair<string, string> keyValuePair in Options.FileTypes)
        {
            CbFileType.Items.Add(new ComboBoxItem
            {
                Content = keyValuePair.Key,
                Tag = keyValuePair.Value
            });
        }

        if (Options.FileTypes.Any())
        {
            Explorer.FilesSearchPattern = Options.FileTypes.First().Value;
            CbFileType.SelectedIndex = 0;
        }

        CbFileType.IsEnabled = true;
    }

    private void BtnOpen_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Explorer.CurrentPath))
        {
            SystemSounds.Beep.Play();
            return;
        }

        if (string.IsNullOrEmpty(TbFileName.Text))
        {
            SystemSounds.Beep.Play();
            return;
        }

        FileName = Path.Combine(Explorer.CurrentPath, TbFileName.Text);

        if (!File.Exists(FileName))
        {
            FileNotFoundException ex = new();
            ThemedMessageBox.Show(ex.Message, "Error", this, MessageBoxButton.OK);
            return;
        }

        DialogResult = true;
        Close();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        FileName = null;
        DialogResult = false;
        Close();
    }

    private void Explorer_FileSelected(object? sender, string e)
    {
        FileName = e;
        TbFileName.Text = Path.GetFileName(FileName);
    }

    private void Explorer_FileDoubleClicked(object? sender, string e)
    {
        FileName = e;
        BtnOpen_Click(null, new RoutedEventArgs());
    }

    private void CbFileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CbFileType != null && CbFileType.IsEnabled && CbFileType.SelectedItem is ComboBoxItem item)
        {
            Explorer.FilesSearchPattern = item.Tag!.ToString()!;
            _ = Explorer.ExploreDirectory(Explorer.CurrentPath!);
        }
    }

    private bool ExtensionMatchDesiredFileTypes(string filePath)
    {
        FileInfo fi = new(filePath);

        if (string.IsNullOrEmpty(fi.Extension))
        {
            return false;
        }

        return Options.FileTypes.Values.Any(x => x.Split(';', StringSplitOptions.RemoveEmptyEntries).Any(y => y.Trim('*').Equals(fi.Extension, StringComparison.InvariantCulture)));
    }

    private void TbFileName_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        if (string.IsNullOrEmpty(TbFileName.Text))
        {
            return;
        }

        try
        {
            DirectoryInfo d = new(TbFileName.Text);

            if (string.IsNullOrEmpty(d.Extension) && d.Exists)
            {
                // Case of an existing directory
                _ = Explorer.ExploreDirectory(TbFileName.Text);
            }
            else if (!string.IsNullOrEmpty(d.Extension))
            {
                // case of a file
                if (!File.Exists(TbFileName.Text))
                {
                    FileNotFoundException ex = new();
                    ThemedMessageBox.Show(ex.Message, "Error", this, MessageBoxButton.OK);
                    return;
                }

                // Check if the file name has a matching extension
                if (!ExtensionMatchDesiredFileTypes(TbFileName.Text))
                {
                    SystemSounds.Beep.Play();
                    return;
                }

                FileName = TbFileName.Text;
                BtnOpen_Click(null, new RoutedEventArgs());
            }
        }
        catch (Exception ex)
        {
            ThemedMessageBox.Show(ex.Message, "Error", this, MessageBoxButton.OK);
        }
    }
}