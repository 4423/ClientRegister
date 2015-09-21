using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientRegster.Models;

namespace ClientRegster.Controls
{
    /// <summary>
    /// AutoCompleteTextBox.xaml の相互作用ロジック
    /// </summary>
    public partial class AutoCompleteTextBox : UserControl
    {
        public AutoCompleteTextBox()
        {
            InitializeComponent();

            this.textBox.TextChanged += (sender, e) => Text = textBox.Text;
            this.SizeChanged += (sender, e) => this.listBox.Width = this.ActualWidth;

            this.textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            this.listBox.KeyDown += ListBox_KeyDown;
        }


        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            //Tab または Shift + Tab でリスト項目を上下
            if (Keyboard.Modifiers == ModifierKeys.Shift && e.Key == Key.Tab)
            {
                if (this.listBox.SelectedIndex > 0)
                {
                    this.listBox.SelectedIndex--;
                }
            }
            else if (e.Key == Key.Tab)
            {
                this.listBox.SelectedIndex++;
            }
            //決定
            else if (e.Key == Key.Enter)
            {
                this.textBox.Text = this.listBox.SelectedItem as string;
                this.popup.IsOpen = false;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!this.listBox.HasItems) { return; }
            if (this.listBox.IsFocused) { return; }

            //下矢印 または　Tab でリスト項目にフォーカスをあてる
            if (e.Key == Key.Down || e.Key == Key.Tab)
            {
                e.Handled = true;

                this.listBox.Focus();
                Keyboard.Focus(this.listBox);
                this.listBox.Items.MoveCurrentToFirst();
                var item = this.listBox.ItemContainerGenerator.ContainerFromItem(this.listBox.Items.CurrentItem) as ListBoxItem;
                item.Focus();
            }
        }

        #region - DependencyProperty -

        public static readonly DependencyProperty CandidatesProperty =
           DependencyProperty.Register("Candidates", typeof(IEnumerable), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null, CandidatesChanged));        
        public IEnumerable Candidates
        {
            get { return (IEnumerable)GetValue(CandidatesProperty); }
            set { SetValue(CandidatesProperty, value); }
        }
        private static void CandidatesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox ctrl = d as AutoCompleteTextBox;
            if (ctrl != null)
            {
                ctrl.listBox.ItemsSource = ctrl.Candidates;
            }
        }
        

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(String.Empty, TextChanged));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox ctrl = d as AutoCompleteTextBox;
            if (ctrl != null)
            {
                ctrl.textBox.Text = ctrl.Text;
                ctrl.UpdateCandidates();
            }
        }


        public static readonly new DependencyProperty HeightProperty =
           DependencyProperty.Register("Height", typeof(double), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(0.0d, HeightChanged));
        public new double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        private static void HeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox ctrl = d as AutoCompleteTextBox;
            if (ctrl != null)
            {
                ctrl.textBox.Height = ctrl.Height;
            }
        }


        public static readonly new DependencyProperty WidthProperty =
           DependencyProperty.Register("Width", typeof(double), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(0.0d, WidthChanged));
        public new double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        private static void WidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox ctrl = d as AutoCompleteTextBox;
            if (ctrl != null)
            {
                ctrl.textBox.Width = ctrl.Width;
            }
        }

        #endregion


        private void UpdateCandidates()
        {
            string input = this.textBox.Text;
            if (String.IsNullOrWhiteSpace(input))
            {
                this.popup.IsOpen = false;
                return;
            }
            if (this.Candidates.IsEmpty()) { return; }

            this.listBox.ItemsSource = null;
            this.listBox.Items.Clear();

            foreach (string c in this.Candidates)
            {
                if (c.IndexOf(input) != -1)
                {
                    this.listBox.Items.Add(c);
                }
            }

            this.popup.IsOpen = this.listBox.HasItems;
        }
    }
}
