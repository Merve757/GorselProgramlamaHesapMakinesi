using Microsoft.Maui.Controls;
using System;

namespace MauiApp3.Views
{
    public partial class StandardPage : ContentPage
    {
        private double _firstNumber = 0;
        private string _operator = "";
        private bool _isNewEntry = true;
        private string _currentInput = "";
        private double _memoryValue = 0;
        private double memoryValue;

        public StandardPage()
        {
            InitializeComponent();
        }

        private void OnNumber(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (_isNewEntry || ResultLabel.Text == "0")
                {
                    _currentInput = button.Text == "," ? "0," : button.Text;
                }
                else
                {
                    if (button.Text == "," && _currentInput.Contains(",")) return;
                    _currentInput += button.Text;
                }

                _isNewEntry = false;
                ResultLabel.Text = _currentInput;
            }
        }

        private void OnOperator(object sender, EventArgs e)
        {
            if (sender is Button button && double.TryParse(_currentInput, out _firstNumber))
            {
                _operator = button.Text;
                _currentInput = "";
                _isNewEntry = true;
                ResultLabel.Text = $"{_firstNumber} {_operator}";
            }
        }
       

        private void OnEquals(object sender, EventArgs e)
        {
            if (!double.TryParse(_currentInput, out double secondNumber))
            {
                ResultLabel.Text = "Hata!";
                return;
            }

            double result = _operator switch
            {
                "+" => _firstNumber + secondNumber,
                "-" => _firstNumber - secondNumber,
                "×" => _firstNumber * secondNumber,
                "÷" => secondNumber != 0 ? _firstNumber / secondNumber : throw new DivideByZeroException(),
                _ => 0
            };

            ResultLabel.Text = result.ToString("G15");
            _currentInput = ResultLabel.Text;
            _operator = "";
            _isNewEntry = true;
        }

        private void OnClear(object sender, EventArgs e)
        {
            _currentInput = "";
            _operator = "";
            _firstNumber = 0;
            ResultLabel.Text = "0";
        }
        private void OnInverse(object sender, EventArgs e)
        {
            if (double.TryParse(_currentInput, out double value))
            {
                if (value == 0)
                {
                    ResultLabel.Text = "Hata!";
                    return;
                }
                value = 1 / value;
                _currentInput = value.ToString();
                ResultLabel.Text = _currentInput;
            }
        }
        private void OnDecimal(object sender, EventArgs e)
        {
            if (!_currentInput.Contains(","))
            {
                _currentInput += ",";
                ResultLabel.Text = _currentInput;
            }
        }



        private void OnClearEntry(object sender, EventArgs e)
        {
            _currentInput = "";
            ResultLabel.Text = "0";
            _isNewEntry = true;
        }
        private void OnMemoryStore(object sender, EventArgs e)
        {
            if (double.TryParse(ResultLabel.Text, out double result))
            {
                memoryValue = result;
            }
        }
        private void OnPercentage(object sender, EventArgs e)
        {
            if (double.TryParse(_currentInput, out double value))
            {
                value /= 100;
                _currentInput = value.ToString();
                ResultLabel.Text = _currentInput;
            }
        }

        private void OnBackspace(object sender, EventArgs e)
        {
            _currentInput = _currentInput.Length > 1
                ? _currentInput.Substring(0, _currentInput.Length - 1)
                : "0";
            ResultLabel.Text = _currentInput;
        }
        private void OnSquare(object sender, EventArgs e)
        {
            if (double.TryParse(_currentInput, out double value))
            {
                value = Math.Pow(value, 2);
                _currentInput = value.ToString();
                ResultLabel.Text = _currentInput;
            }
        }
        private void OnSquareRoot(object sender, EventArgs e)
        {
            if (double.TryParse(_currentInput, out double value))
            {
                if (value < 0)
                {
                    ResultLabel.Text = "Hata!";
                    return;
                }
                value = Math.Sqrt(value);
                _currentInput = value.ToString();
                ResultLabel.Text = _currentInput;
            }
        }



        private void OnNegate(object sender, EventArgs e)
        {
            if (double.TryParse(ResultLabel.Text, out double value))
            {
                _currentInput = (-value).ToString();
                ResultLabel.Text = _currentInput;
            }
        }

        private void OnMemoryClear(object sender, EventArgs e) => _memoryValue = 0;
        private void OnMemoryRecall(object sender, EventArgs e) => ResultLabel.Text = _memoryValue.ToString();
        private void OnMemoryAdd(object sender, EventArgs e)
        {
            if (double.TryParse(ResultLabel.Text, out double value)) _memoryValue += value;
        }
        private void OnMemorySubtract(object sender, EventArgs e)
        {
            if (double.TryParse(ResultLabel.Text, out double value)) _memoryValue -= value;
        }
    }
}
