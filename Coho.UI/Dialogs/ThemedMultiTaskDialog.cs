// *********************************************************
// 
// Coho.UI ThemedMultiTaskDialog.cs
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
using System.Windows;
using Coho.UI.Tasks;

namespace Coho.UI.Dialogs;

public static class ThemedMultiTaskDialog
{
    public static void Show(string title, string message, List<TaskRunnerBase> tasksToRun)
    {
        Show(title, message, Application.Current.MainWindow!, tasksToRun);
    }

    public static void Show(string title, string message, Window owner, List<TaskRunnerBase> tasksToRun)
    {
        if (tasksToRun == null)
        {
            throw new NullReferenceException("Please provide a list of tasks to run.");
        }

        MultiTaskDialog dlg = new();
        dlg.TasksToRun = tasksToRun;
        dlg.Owner = owner;
        dlg.Title = title;
        dlg.TbTitle.Text = title;
        dlg.TbMessage.Text = message;

        dlg.ShowDialog();
    }
}