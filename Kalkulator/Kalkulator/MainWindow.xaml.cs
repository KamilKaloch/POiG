using System;
using System.Collections.Generic;
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
using System.Globalization;
using System.Threading;
namespace Kalkulator
{
    public partial class MainWindow : Window
    {
        private const string dEntry = "";
        private const string dtHistory = "";
        private const string dLastEntry = "";
        private const string dOperation = "";
        string entry;
        string history;
        string lastEntry;
        string operation;
        bool hasSeparator;
        bool toReset;
        public MainWindow()
        {
            InitializeComponent();
            SetDecimalSeparator(DetectDecimalSeparator());
            entry = "0";
            lastEntry = dLastEntry;
            history = dtHistory;
            operation = dOperation;
            hasSeparator = false;
            toReset = false;
            TextCompositionManager.AddTextInputHandler(this, new TextCompositionEventHandler(OnTextComposition));
            UpdateEntry();
            UpdateHistory();
        }
        private static string DetectDecimalSeparator()
        {
            return Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }
        private void SetDecimalSeparator(string separator)
        {
            DecimalSeparator.Content = separator;
        }
        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            AddDigit(button.Content.ToString());
            Equals_btn.Focus();
        }
        private void DecimalSeparator_Click(object sender, RoutedEventArgs e)
        {
            AddSeparator(DecimalSeparator.Content.ToString());
            Equals_btn.Focus();
        }
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            Backspace();
            Equals_btn.Focus();
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            Equals_btn.Focus();
        }
        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            ClearEntry();
            Equals_btn.Focus();
        }
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            Operatation(button.Content.ToString());

            Equals_btn.Focus();
        }
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            Equals();
            Equals_btn.Focus();
        }
        private void Negate_Click(object sender, RoutedEventArgs e)
        {
            Negate();
            Equals_btn.Focus();
        }
        private void UpdateEntry()
        {
            if (entry == dEntry)
            {
                Entry_l.Content = "0";
                return;
            }
            if (entry.Contains("E"))
            {
                Entry_l.Content = entry.Substring(0, entry.IndexOf("E"));
                E_tb.Text = entry.Substring(entry.IndexOf("E"));
                return;
            }
            Entry_l.Content = entry;
        }
        private void UpdateHistory()
        {
            History_tb.Text = history;
        }
        private string Calculate()
        {
            string result = String.Empty;
            if (operation == "+")
                result = (Double.Parse(lastEntry) + Double.Parse(entry)).ToString();
            else if (operation == "-")
                result = (Double.Parse(lastEntry) - Double.Parse(entry)).ToString();
            else if (operation == "\u00D7")
                result = (Double.Parse(lastEntry) * Double.Parse(entry)).ToString();
            else if (operation == "\u00F7")
            {
                double denominator = Double.Parse(entry);
                if (denominator == 0) return "Can't divide by 0";
                result = (Double.Parse(lastEntry) / denominator).ToString();
            }
           // if (result.Contains("E")) result = result.Substring(0, (result.IndexOf("E")));
            return result;
        }
        private void Reset()
        {
            E_tb.Text = string.Empty;
            entry = dEntry;
            lastEntry = dLastEntry;
            history = dtHistory;
            operation = dOperation;
            hasSeparator = false;
            toReset = false;
            Add_btn.IsEnabled = true;
            Substract_btn.IsEnabled = true;
            Multiply_btn.IsEnabled = true;
            Divide_btn.IsEnabled = true;
            Negate_btn.IsEnabled = true;
            DecimalSeparator.IsEnabled = true;
            Equals_btn.IsEnabled = true;
            UpdateEntry();
            UpdateHistory();
        }
        private void SaveResult()
        {
            entry = lastEntry;
            lastEntry = dLastEntry;
            history = dtHistory;
            operation = dOperation;
            hasSeparator = false;
            toReset = false;
            UpdateEntry();
            UpdateHistory();
        }
        private void Error(string message)
        {
            entry = message;
            Entry_l.Content = entry;
            toReset = true;
            Add_btn.IsEnabled = false;
            Substract_btn.IsEnabled = false;
            Multiply_btn.IsEnabled = false;
            Divide_btn.IsEnabled = false;
            Negate_btn.IsEnabled = false;
            DecimalSeparator.IsEnabled = false;
            Equals_btn.IsEnabled = false;
        }
        private void AddDigit(string digit)
        {
            if (toReset) Reset();
            if (entry == "0") entry = dEntry;
            if (entry.Length < 15) entry += digit;
            UpdateEntry();
        }
        private void AddSeparator(string separator)
        {
            if (toReset) Reset();
            if (hasSeparator) return;
            if (entry == dEntry) entry = "0";
            entry += DecimalSeparator.Content.ToString();
            hasSeparator = true;
            UpdateEntry();
        }
        private void Backspace()
        {
            if (toReset) Reset();
            if (entry.Length < 2)
            {
                entry = dEntry;
                UpdateEntry();
                return;
            }

            if (entry.EndsWith(DecimalSeparator.Content.ToString())) hasSeparator = false;

            entry = entry.Substring(0, entry.Length - 1);
            UpdateEntry();
        }
        private void Operatation(string op)
        {
            if (toReset) SaveResult();

            if ((entry == dEntry || entry == "0") && lastEntry == dLastEntry) return;

            Double.TryParse(entry, out double val);
            if (val == 0) entry = "0";
            if (entry == dEntry)
            {
                operation = op;
                history = history.Substring(0, history.Length - 2);
                history += $"{operation} ";
                UpdateHistory();
                return;
            }

            if (entry.EndsWith(DecimalSeparator.Content.ToString()))
            {
                entry = entry.Substring(0, entry.Length - 1);
                hasSeparator = false;
            }

            if (lastEntry == dLastEntry)
            {
                lastEntry = entry;
            }
            else
            {
                string result = Calculate();
                if (!Double.TryParse(result, out _))
                {
                    Error(result);
                    return;
                }
                lastEntry = entry;
                entry = result;
            }
            operation = op;
            history += $"{lastEntry} {operation} ";
            lastEntry = entry;

            UpdateEntry();
            UpdateHistory();
            entry = dEntry;
            hasSeparator = false;
        }
        private void Equals()
        {
            if (toReset) SaveResult();

            if (entry.EndsWith(DecimalSeparator.Content.ToString()))
            {
                entry = entry.Substring(0, entry.Length - 1);
                hasSeparator = false;
            }

            if (entry == dEntry) entry = lastEntry;

            history += $"{entry} = ";

            if (lastEntry != dLastEntry)
            {
                string result = Calculate();
                if (!Double.TryParse(result, out double _))
                {
                    Error(result);
                    return;
                }
                entry = result;
            }

            UpdateEntry();
            UpdateHistory();
            lastEntry = entry;
            entry = dEntry;
            toReset = true;
        }
        private void Negate()
        {
            if (toReset) SaveResult();
            if ((entry == dEntry || entry == "0") && lastEntry == dLastEntry) return;
            if (entry == dEntry && lastEntry != dLastEntry)
            {
                entry = lastEntry;
                lastEntry = dLastEntry;
            }

            if (entry.StartsWith("-"))
            {
                entry = entry.Substring(1);
                UpdateEntry();
                return;
            }
            entry = entry.Insert(0, "-");
            UpdateEntry();
        }
        private void ClearEntry()
        {
            if (toReset) Reset();
            entry = dEntry;

            hasSeparator = false;

            UpdateEntry();
        }
        private void OnTextComposition(object sender, TextCompositionEventArgs e)
        {
            string text = e.Text;
            if (int.TryParse(text, out _)) AddDigit(text);
            else if (text == "\b") Backspace();
            else if (text == "\u001b") Reset();
            if (!Add_btn.IsEnabled) return;
            if (text == DecimalSeparator.Content.ToString()) AddSeparator(text);
            else if (text == "/") Operatation("\u00F7");
            else if (text == "*") Operatation("\u00D7");
            else if (text == "-" || text == "+") Operatation(text);
            else if (text == "=" || text == "\r") Equals();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    ClearEntry();
                    break;
                case Key.F9:
                    if (!Add_btn.IsEnabled) return;
                    Negate();
                    break;
            }
        }
    }
}
