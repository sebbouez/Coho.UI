// *********************************************************
// 
// Test
// DummyTask1.cs
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
using Coho.UI.Tasks;

namespace Test.Tasks;

public class DummyTask2 : TaskRunnerBase
{
    public override string Title
    {
        get
        {
            return "Dummy task 2";
        }
    }

    public override string Description
    {
        get
        {
            return "Running a dummy task...";
        }
    }

    public override void Execute()
    {
        // We report progress with NULL for indeterminate action progress time
        ReportProgress(null);
        
        System.Threading.Thread.Sleep(2200);
        throw new Exception("Some error message...");
    }
}