// *********************************************************
// 
// Coho.UI ThemedSpecialDialogOptions.cs
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

namespace Coho.UI.Dialogs;

public class ThemedSpecialDialogOptions
{
    public bool ShowDefaultSpecialFolders
    {
        get;
        set;
    } = true;

    public Dictionary<string, string> FileTypes
    {
        get;
        set;
    } = new();

    public string InitialDirectory
    {
        get;
        set;
    } = string.Empty;

    public bool ShowNavigationPane
    {
        get;
        set;
    } = true;

    public int DefaultHeight
    {
        get;
        set;
    } = 650;
    
    public int DefaultWidth
    {
        get;
        set;
    } = 920;

}