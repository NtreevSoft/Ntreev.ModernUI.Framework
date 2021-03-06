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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ntreev.ModernUI.Framework.Controls
{
    [TemplatePart(Name = PART_Cancel, Type = typeof(Button))]
    public class ProgressContent : UserControl
    {
        public const string PART_Cancel = "PART_Cancel";

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(ProgressContent),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(ProgressContent),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty CanCancelProperty =
            DependencyProperty.Register(nameof(CanCancel), typeof(bool), typeof(ProgressContent),
                new PropertyMetadata(false));

        private static readonly DependencyPropertyKey IsCancellationRequestedPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsCancellationRequested), typeof(bool), typeof(ProgressContent),
                new PropertyMetadata(false));
        public static readonly DependencyProperty IsCancellationRequestedProperty = IsCancellationRequestedPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ProgressTemplateProperty =
            DependencyProperty.Register(nameof(ProgressTemplate), typeof(DataTemplate), typeof(ProgressContent),
                new FrameworkPropertyMetadata(null));

        private Button cancelButton;

        public ProgressContent()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.cancelButton != null)
            {
                this.cancelButton.Click -= CancelButton_Click;
            }

            this.cancelButton = this.Template.FindName(PART_Cancel, this) as Button;

            if (this.cancelButton != null)
            {
                this.cancelButton.Click += CancelButton_Click;
            }
        }

        public string Message
        {
            get { return (string)this.GetValue(MessageProperty); }
            set { this.SetValue(MessageProperty, value); }
        }

        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public bool CanCancel
        {
            get { return (bool)this.GetValue(CanCancelProperty); }
            set { this.SetValue(CanCancelProperty, value); }
        }

        public bool IsCancellationRequested
        {
            get { return (bool)this.GetValue(IsCancellationRequestedProperty); }
        }

        public DataTemplate ProgressTemplate
        {
            get { return (DataTemplate)this.GetValue(ProgressTemplateProperty); }
            set { this.SetValue(ProgressTemplateProperty, value); }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.SetValue(IsCancellationRequestedPropertyKey, true);
        }
    }
}
