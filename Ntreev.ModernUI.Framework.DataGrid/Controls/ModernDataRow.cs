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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.DataGrid;

namespace Ntreev.ModernUI.Framework.DataGrid.Controls
{
    public class ModernDataRow : DataRow
    {
        public static readonly DependencyProperty IsDragOverProperty =
            DependencyProperty.RegisterAttached(nameof(IsDragOver), typeof(bool), typeof(ModernDataRow));

        public ModernDataRow()
        {
            this.CommandBindings.Insert(0, new CommandBinding(DataGridCommands.EndEdit, this.EndEdit_Execute, this.EndEdit_CanExecute));
            this.CommandBindings.Add(new CommandBinding(ModernDataGridCommands.SelectInsertion, this.SelectInsertion_Execute, this.SelectInsertion_CanExecute));

            this.InputBindings.Add(new InputBinding(DataGridCommands.CollapseDetails, new KeyGesture(Key.F9)));
            this.InputBindings.Add(new InputBinding(DataGridCommands.ExpandDetails, new KeyGesture(Key.F10)));
        }

        public static bool GetIsDragOver(RowSelector selector)
        {
            return (bool)selector.GetValue(IsDragOverProperty);
        }

        public static void SetIsDragOver(RowSelector selector, bool value)
        {
            selector.SetValue(IsDragOverProperty, value);
        }

        public ModernDataGridControl GridControl
        {
            get { return (ModernDataGridControl)this.GridContext.DataGridControl; }
        }

        public DataGridContext GridContext
        {
            get { return (DataGridContext)this.GetValue(DataGridControl.DataGridContextProperty); }
        }

        public bool IsDragOver
        {
            get { return (bool)this.GetValue(IsDragOverProperty); }
            set { this.SetValue(IsDragOverProperty, value); }
        }

        public bool HasDataContextError
        {
            get { return ModernDataGridControl.GetHasDataContextError(this); }
        }

        public object DataContextError
        {
            get { return ModernDataGridControl.GetDataContextError(this); }
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            try
            {
                base.OnPreviewGotKeyboardFocus(e);
            }
            catch
            {

            }
        }

        protected override Cell CreateCell(ColumnBase column)
        {
            return new ModernDataCell();
        }

        protected override void PrepareContainer(DataGridContext dataGridContext, object item)
        {
            base.PrepareContainer(dataGridContext, item);

            this.RefreshDataContextError();
        }

        protected override void SetDataContext(object item)
        {
            base.SetDataContext(item);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        protected override void EndEditCore()
        {
            base.EndEditCore();
        }

        public void RefreshDataContextError()
        {
            if (this.DataContext is IDataErrorInfo errorInfo && errorInfo.Error != string.Empty)
            {
                ModernDataGridControl.SetDataContextError(this, errorInfo.Error);
            }
            else
            {
                ModernDataGridControl.SetDataContextError(this, null);
            }
            this.DataContextErrorChanged?.Invoke(this, EventArgs.Empty);
        }

        private void EndEdit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var gridContext = DataGridControl.GetDataGridContext(this);
            if (gridContext.CurrentColumn == null)
                return;

            var cell = this.Cells[gridContext.CurrentColumn];
            if (cell.IsBeingEdited == true)
                e.CanExecute = true;
        }

        private void EndEdit_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            var gridContext = DataGridControl.GetDataGridContext(this);
            var cell = this.Cells[gridContext.CurrentColumn];

            try
            {
                cell.EndEdit();
            }
            catch (Xceed.Wpf.DataGrid.DataGridException ex)
            {
                if (ex.InnerException != null)
                    AppMessageBox.ShowError(ex.InnerException.Message);
                else
                    AppMessageBox.ShowError(ex);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowError(ex);
            }
        }

        private void Delete_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppMessageBox.ConfirmDelete() == false)
                return;

            var gridContext = DataGridControl.GetDataGridContext(this);
            var gridControl = gridContext.DataGridControl as ModernDataGridControl;

            if (gridControl.InvokeItemDeletingEvent(this) == true)
            {
                var row = this.DataContext as System.Data.DataRowView;

                try
                {
                    row.Delete();
                }
                catch (Exception ex)
                {
                    AppMessageBox.ShowError(ex.Message);
                }
            }
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void SelectInsertion_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var insertionRow = ModernDataGridControl.GetInsertionRow(this.GridContext);
            if (insertionRow == null)
                return;
            e.CanExecute = this.IsBeingEdited == false;
        }

        private void SelectInsertion_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            var insertionRow = ModernDataGridControl.GetInsertionRow(this.GridContext);
            if (insertionRow == null)
                return;

            this.GridContext.SelectedCellRanges.Clear();
            this.GridContext.CurrentItem = this.GridContext.GetItemFromContainer(insertionRow);
        }

        internal event EventHandler DataContextErrorChanged;
    }
}
