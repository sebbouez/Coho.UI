// *********************************************************
// 
// Coho.UI TaskRunnerBase.cs
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
using System.Configuration;

namespace Coho.UI.Tasks;

public abstract class TaskRunnerBase
{

    /// <summary>
    /// Gets the title of the task
    /// </summary>
    public abstract string Title
    {
        get;
    }

    /// <summary>
    /// Gets a text that describes what the task does
    /// </summary>
    public abstract string Description
    {
        get;
    }
    
    /// <summary>
    /// Method called to run the task
    /// </summary>
    public abstract void Execute();

    internal event EventHandler<int?>? Progress; 
    
    /// <summary>
    /// Reports task progress to the UI
    /// </summary>
    /// <param name="progressValue">The progress percentage, value must be between 0 and 100, set to null for indeterminate progress</param>
    public void ReportProgress(int? progressValue)
    {
        Progress?.Invoke(this, progressValue);
    }
}