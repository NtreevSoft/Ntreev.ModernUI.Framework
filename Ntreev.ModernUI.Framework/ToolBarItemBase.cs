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
using Ntreev.Library.Linq;
using Caliburn.Micro;
using System.Windows.Input;
using System.ComponentModel.Composition;
using System.Windows;
using System.Collections;
using System.Globalization;
using System.ComponentModel.Composition.Hosting;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using Ntreev.ModernUI.Framework.Controls;

namespace Ntreev.ModernUI.Framework
{
    public abstract class ToolBarItemBase : PropertyChangedBase, IToolBarItem, ICommand
    {
        private bool isEnabled;
        private string displayName;
        private EventHandler canExecuteChanged;
        private object icon;

        protected ToolBarItemBase()
        {
            CommandManager.RequerySuggested += this.InvokeCanExecuteChangedEvent;
        }

        public string DisplayName
        {
            get { return this.displayName ?? string.Empty; }
            set
            {
                this.displayName = value;
                this.NotifyOfPropertyChange(nameof(this.DisplayName));
            }
        }

        public ICommand Command
        {
            get { return this; }
        }

        public bool IsVisible
        {
            get
            {
                if (this.HideOnDisabled == true && this.isEnabled == false)
                    return false;
                return true;
            }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
        }

        public object Icon
        {
            get
            {
                if (this.icon is string uri)
                {
                    if (uri.StartsWith("pack://application:,,,") == false)
                        uri = "pack://application:,,," + uri;
                    return new IconButton()
                    {
                        Source = new BitmapImage(new Uri(uri)),
                    };
                }
                else if (this.icon is Type viewType)
                {
                    return ViewLocator.GetOrCreateViewType(viewType);
                }
                else
                {
                    return this.icon;
                }
            }
            set
            {
                this.icon = value;
                this.NotifyOfPropertyChange(nameof(this.Icon));
            }
        }

        public bool HideOnDisabled
        {
            get; set;
        }

        protected virtual void OnExecute(object parameter)
        {

        }

        protected virtual bool OnCanExecute(object parameter)
        {
            return true;
        }

        protected void InvokeCanExecuteChangedEvent()
        {
            this.canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void InvokeCanExecuteChangedEvent(object sender, EventArgs e)
        {
            this.canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                this.canExecuteChanged += value;
            }

            remove
            {
                this.canExecuteChanged -= value;
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            try
            {
                var oldValue = this.isEnabled;
                var newValue = this.OnCanExecute(parameter);
                if (oldValue != newValue)
                {
                    this.isEnabled = newValue;
                }

                return this.isEnabled;
            }
            finally
            {
                this.Dispatcher.InvokeAsync(() =>
                {
                    this.NotifyOfPropertyChange(nameof(this.IsEnabled));
                    this.NotifyOfPropertyChange(nameof(this.IsVisible));
                });
            }
        }

        void ICommand.Execute(object parameter)
        {
            this.OnExecute(parameter);
        }

        #endregion
    }
}