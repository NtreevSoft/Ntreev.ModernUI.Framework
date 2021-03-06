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
    public class FlagControlItem : ContentControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(long), typeof(FlagControlItem),
            new UIPropertyMetadata(ValuePropertyChangedCallback));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(FlagControlItem),
            new UIPropertyMetadata(true, IsSelectedPropertyChangedCallback));

        public static readonly DependencyProperty FlagTypeProperty =
            DependencyProperty.Register("FlagType", typeof(FlagControlItemType), typeof(FlagControlItem));

        public static RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(FlagControlItem));

        public static RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(FlagControlItem));

        private bool isUpdating;

        public FlagControlItem()
        {

        }

        public long Value
        {
            get { return (long)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        public FlagControlItemType FlagType
        {
            get { return (FlagControlItemType)this.GetValue(FlagTypeProperty); }
        }

        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        public event RoutedEventHandler Unselected
        {
            add { AddHandler(UnselectedEvent, value); }
            remove { RemoveHandler(UnselectedEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var flagControl = ItemsControl.ItemsControlFromItemContainer(this) as FlagControl;
            if (flagControl != null)
            {
                this.isUpdating = true;
                try
                {
                    if (flagControl.Value.HasValue == true)
                    {
                        long controlValue = (long)flagControl.Value;
                        if (this.Value == 0)
                            this.IsSelected = controlValue == this.Value;
                        else
                            this.IsSelected = (controlValue & this.Value) == this.Value;
                    }
                    else
                    {
                        this.IsSelected = false;
                    }
                }
                finally
                {
                    this.isUpdating = false;
                }
            }
        }

        private static void ValuePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flagControl = ItemsControl.ItemsControlFromItemContainer(d) as FlagControl;
            var flagItem = d as FlagControlItem;
            var value = (long)e.NewValue;

            if (value == 0)
            {
                d.SetValue(FlagTypeProperty, FlagControlItemType.None);
            }
            else if (value == -1)
            {
                d.SetValue(FlagTypeProperty, FlagControlItemType.All);
            }
            else
            {
                var b = (value & (value - 1)) == 0;
                if (b == true)
                    d.SetValue(FlagTypeProperty, FlagControlItemType.Single);
                else
                    d.SetValue(FlagTypeProperty, FlagControlItemType.Multiple);
            }
        }

        private static void IsSelectedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flagControl = ItemsControl.ItemsControlFromItemContainer(d) as FlagControl;
            var flagItem = d as FlagControlItem;
            var isSelected = (bool)e.NewValue;

            if (flagControl != null && flagItem.isUpdating == false)
            {
                if (isSelected == true)
                    flagControl.AddFlag(flagItem.Value);
                else
                    flagControl.RemoveFlag(flagItem.Value);
            }

            if (isSelected == true)
            {
                flagItem.RaiseEvent(new RoutedEventArgs(SelectedEvent));
            }
            else
            {
                flagItem.RaiseEvent(new RoutedEventArgs(UnselectedEvent));
            }
        }

        private static object IsSelectedPropertyCoerceValueCallback(DependencyObject d, object baseValue)
        {
            return baseValue;
        }
    }
}
