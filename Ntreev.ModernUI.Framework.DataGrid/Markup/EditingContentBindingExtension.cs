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

using Ntreev.ModernUI.Framework.Controls;
using Ntreev.ModernUI.Framework.DataGrid.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Xceed.Wpf.DataGrid;

namespace Ntreev.ModernUI.Framework.DataGrid.Markup
{
    [MarkupExtensionReturnType(typeof(object))]
    public class EditingContentBindingExtension : MarkupExtension
    {
        private readonly Binding binding = new Binding();

        public EditingContentBindingExtension()
        {
            this.binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = typeof(Cell), };
            this.binding.Mode = BindingMode.TwoWay;
            this.binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.binding.IsAsync = true;
            this.binding.Path = new PropertyPath(nameof(ModernDataCell.EditingContent));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.binding.ProvideValue(serviceProvider);
        }

        public object TargetNullValue
        {
            get { return this.binding.TargetNullValue; }
            set { this.binding.TargetNullValue = value; }
        }

        public IValueConverter Converter
        {
            get { return this.binding.Converter; }
            set { this.binding.Converter = value; }
        }

        public string StringFormat
        {
            get { return this.binding.StringFormat; }
            set { this.binding.StringFormat = value; }
        }
    }
}
