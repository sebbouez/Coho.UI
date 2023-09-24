// *********************************************************
// 
// Coho.UI CommandItemModel.cs
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

using System.Windows.Media;

namespace Coho.UI.Controls.Common;

public class CommandItemModel
{
  
        public CommandItemModel()
        {
            Hash = string.Empty;
            Label = string.Empty;
        }

        public string Hash
        {
            get;
            set;
        }

        public Brush? Icon
        {
            get;
            set;
        }

        public string Label
        {
            get;
            set;
        }
    
}