// *********************************************************
// 
// Coho.UI
// MultiTaskDialog.xaml.cs
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Coho.UI.Tasks;
using Coho.UI.Windows;

namespace Coho.UI.Dialogs;

internal partial class MultiTaskDialog : SecondaryWindow
{
    public MultiTaskDialog()
    {
        InitializeComponent();
    }

    internal List<TaskRunnerBase> TasksToRun
    {
        get;
        set;
    } = new();

    private async void RunTasks(List<TaskRunnerBase> tasksToRun)
    {
        BtnOk.IsEnabled = false;
        BtnCancel.IsEnabled = false;
        BorderStatus.Visibility = Visibility.Visible;
        TaskProgressBar.IsIndeterminate = true;
        TaskProgressBar.Visibility = Visibility.Visible;
        TbWorkInProgress.Visibility = Visibility.Visible;

        foreach (TaskRunnerBase taskToRun in tasksToRun)
        {
            taskToRun.Progress += delegate(object? _, int? i)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (i == null)
                    {
                        TaskProgressBar.IsIndeterminate = true;
                    }
                    else
                    {
                        TaskProgressBar.IsIndeterminate = false;
                        TaskProgressBar.Value = i.Value;
                    }
                });
            };

            TbProgress.Text = taskToRun.Title;
            TbProgressDescription.Text = taskToRun.Description;
            try
            {
                await Task.Run(() =>
                {
                    Thread.Sleep(250);
                    taskToRun.Execute();
                });
            }
            catch (Exception ex)
            {
                HandleActionError(ex.Message);
                return;
            }
        }

        Complete();
    }

    private void HandleActionError(string message)
    {
        TaskProgressBar.IsIndeterminate = false;        
        TbWorkInProgress.Visibility = Visibility.Collapsed;
        TaskProgressBar.Visibility = Visibility.Collapsed;
        TbProgress.Foreground = Brushes.Red;
        TbProgressDescription.Foreground = Brushes.Red;
        TbProgress.Text = GenericText.ErrorOccured;
        TbProgressDescription.Text = message;
        BtnOk.IsEnabled = false;
        BtnCancel.IsEnabled = true;
    }

    private void Complete()
    {
        DialogResult = true;
        Close();
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        RunTasks(TasksToRun);
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}