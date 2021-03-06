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

using Ntreev.Library.IO;
using Ntreev.Library.Linq;
using Ntreev.Library.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntreev.ModernUI.Framework.ViewModels
{
    public class CategoryTreeViewItemViewModel : TreeViewItemViewModel
    {
        private readonly CategoryName categoryName;

        public CategoryTreeViewItemViewModel(string path)
        {
            NameValidator.ValidateCategoryPath(path);
            this.categoryName = new CategoryName(path);
            this.Target = path;
        }

        public CategoryTreeViewItemViewModel(IItem item)
            : this(item, false)
        {

        }

        public CategoryTreeViewItemViewModel(IItem item, bool categoryOnly)
        {
            NameValidator.ValidateCategoryPath(item.Path);
            this.Target = item;
            this.categoryName = new CategoryName(item.Path);
            foreach (var i in item.Childs)
            {
                if (NameValidator.VerifyCategoryPath(i.Path) == true)
                {
                    new CategoryTreeViewItemViewModel(i).Parent = this;
                }
                else if (categoryOnly == false)
                {
                    new ItemTreeViewItemViewModel(i).Parent = this;
                }
            }
        }

        public static CategoryTreeViewItemViewModel FromItems(string[] items)
        {
            return FromItems(items, false);
        }

        public static CategoryTreeViewItemViewModel FromItems(string[] items, bool categoryOnly)
        {
            var builder = new TreeViewItemViewModelBuilder();
            var viewModels = builder.Create(items, categoryOnly);
            return viewModels[PathUtility.Separator] as CategoryTreeViewItemViewModel;
        }

        public override string DisplayName
        {
            get { return this.categoryName.Name; }
        }

        public string DisplayPath
        {
            get { return this.categoryName.Path; }
        }

        public string Path
        {
            get { return this.categoryName.Path; }
        }

        public string Name
        {
            get { return categoryName.Name; }
        }

        public override int Order
        {
            get { return 1; }
        }
    }
}
