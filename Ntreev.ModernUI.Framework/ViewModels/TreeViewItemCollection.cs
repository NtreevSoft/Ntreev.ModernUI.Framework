﻿//Released under the MIT License.
//
//Copyright (c) 2018 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the 
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Threading;
using System.Windows;

namespace Ntreev.ModernUI.Framework.ViewModels
{
    public class TreeViewItemCollection<T> : ObservableCollection<T> where T : TreeViewItemViewModel
    {
        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                base.Add(item);
            }
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items.Reverse().ToArray())
            {
                base.Remove(item);
            }
        }

        public void Reposition(T item)
        {
            var comparer = item as IComparable;
            var oldIndex = this.IndexOf(item);
            var newIndex = this.Count - 1;
            var index = 0;
            for (var i = 0; i < this.Count; i++)
            {
                var result = comparer.CompareTo(this[i]);
                if (result == 0)
                    continue;
                if (result < 0)
                {
                    newIndex = index;
                    break;
                }
                index++;
            }
            if (oldIndex != newIndex)
            {
                this.Move(oldIndex, newIndex);
            }
        }
    }

    public class TreeViewItemCollection : TreeViewItemCollection<TreeViewItemViewModel>
    {

    }
}