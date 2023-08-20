// *********************************************************
// 
// Coho.UI StringExtensions.cs
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
using System.Linq;

namespace Coho.UI.Extensions;

public static class StringExtensions
{
    public static bool In(this string str, string values)
    {
        if (string.IsNullOrEmpty(values))
        {
            return false;
        }

        string[] op = values.Split(new[] {';', ','}, StringSplitOptions.RemoveEmptyEntries);
        return str.In(op);
    }

    public static bool In(this string str, string[] values)
    {
        if (values.Length == 0)
        {
            return false;
        }

        return values.Any(x => !string.IsNullOrEmpty(x) && str.Equals(x.Trim(), StringComparison.InvariantCultureIgnoreCase));
    }

    public static bool In(this string str, List<string> values)
    {
        if (values.Count == 0)
        {
            return false;
        }

        return values.Exists(x => !string.IsNullOrEmpty(x) && str.Equals(x.Trim(), StringComparison.InvariantCultureIgnoreCase));
    }
}