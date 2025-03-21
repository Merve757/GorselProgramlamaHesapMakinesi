using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;

namespace MauiApp3.Views
{
    public partial class ScientificPage : ContentPage
    {
        private double _firstNumber = 0;
        private string _operator = "";
        private bool _isNewEntry = true;
        private string _currentInput = "";
        private bool _isTrigonometricMode = false; // Trigonometri modu kontrolü

        public ScientificPage()
        {
            InitializeComponent();
            // Trigonometri butonlarını başlangıçta gizle
            sinButton.IsVisible = false;
            cosButton.IsVisible = false;
            tanButton.IsVisible = false;
            cotButton.IsVisible = false;
        }

        private void OnDigitClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (_isNewEntry || ResultLabel.Text == "0")
                {
                    _currentInput = button.Text == "." ? "0." : button.Text;
                }
                else
                {
                    if (button.Text == "." && _currentInput.Contains(".")) return;
                    _currentInput += button.Text;
                }

                _isNewEntry = false;
                ResultLabel.Text = _currentInput;
            }
        }

        private void OnOperatorClicked(object sender, EventArgs e)
        {
            if (sender is Button button && double.TryParse(_currentInput, out _firstNumber))
            {
                _operator = button.Text;
                _currentInput = "";
                _isNewEntry = true;
                ResultLabel.Text = $"{_firstNumber} {_operator}";
            }
        }

        private void OnEqualsClicked(object sender, EventArgs e)
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
                "x^y" => Math.Pow(_firstNumber, secondNumber),
                _ => 0
            };

            ResultLabel.Text = result.ToString("G15");
            _currentInput = ResultLabel.Text;
            _operator = "";
            _isNewEntry = true;
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            _currentInput = "";
            _operator = "";
            _firstNumber = 0;
            ResultLabel.Text = "0";
        }

        private void OnPercentageClicked(object sender, EventArgs e)
        {
            if (double.TryParse(_currentInput, out double value))
            {
                value /= 100;
                _currentInput = value.ToString();
                ResultLabel.Text = _currentInput;
            }
        }

        private void OnNegateClicked(object sender, EventArgs e)
        {
            if (double.TryParse(_currentInput, out double value))
            {
                _currentInput = (-value).ToString();
                ResultLabel.Text = _currentInput;
            }
        }

        private void OnScientificOperatorClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string operation = button.Text;
                double result = 0;

                if (operation == "π")
                {
                    result = Math.PI;
                }
                else if (operation == "e")
                {
                    result = Math.E;
                }
                else if (operation == "exp")
                {
                    if (double.TryParse(_currentInput, out double value))
                    {
                        result = Math.Exp(value);
                    }
                    else
                    {
                        ResultLabel.Text = "Hata!";
                        return;
                    }
                }
                else if (double.TryParse(_currentInput, out double value))
                {
                    if (_isTrigonometricMode)
                    {
                        // Trigonometrik fonksiyonlar için radyan cinsinden hesaplama
                        double radians = value * Math.PI / 180;
                        result = operation switch
                        {
                            "sin" => Math.Sin(radians),
                            "cos" => Math.Cos(radians),
                            "tan" => Math.Tan(radians),
                            "cot" => 1 / Math.Tan(radians),
                            _ => 0
                        };
                    }
                    else
                    {
                        result = operation switch
                        {
                            "log" => Math.Log10(value),
                            "ln" => Math.Log(value),
                            "√x" => Math.Sqrt(value),
                            "10^x" => Math.Pow(10, value),
                            "x^y" => Math.Pow(_firstNumber, value),
                            "n!" => Factorial((int)value),
                            "1/x" => 1 / value,
                            "|x|" => Math.Abs(value),
                            "x^2" => Math.Pow(value, 2),
                            _ => 0
                        };
                    }
                }
                else
                {
                    ResultLabel.Text = "Hata!";
                    return;
                }

                ResultLabel.Text = result.ToString("G15");
                _currentInput = ResultLabel.Text;
                _isNewEntry = true;
            }
        }

        private double Factorial(int n)
        {
            if (n == 0) return 1;
            double result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }

        private void OnBackspaceClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentInput))
            {
                _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
                ResultLabel.Text = string.IsNullOrEmpty(_currentInput) ? "0" : _currentInput;
                _isNewEntry = string.IsNullOrEmpty(_currentInput);
            }
        }

        private void OnTrigonometryClicked(object sender, EventArgs e)
        {
            _isTrigonometricMode = !_isTrigonometricMode;
            // Trigonometri butonlarının görünürlüğünü ayarla
            sinButton.IsVisible = _isTrigonometricMode;
            cosButton.IsVisible = _isTrigonometricMode;
            tanButton.IsVisible = _isTrigonometricMode;
            cotButton.IsVisible = _isTrigonometricMode;
        }

        // Trigonometri butonlarına tıklama olayları
        private void OnSinClicked(object sender, EventArgs e)
        {
            OnScientificOperatorClicked(sinButton, e);
        }

        private void OnCosClicked(object sender, EventArgs e)
        {
            OnScientificOperatorClicked(cosButton, e);

        }

        private void OnTanClicked(object sender, EventArgs e)
        {
            OnScientificOperatorClicked(tanButton, e);
        }

        private void OnCotClicked(object sender, EventArgs e)
        {
            OnScientificOperatorClicked(cotButton, e);
        }
    }
}