
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

namespace lab_1_2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate((a, b) => a + b);
        }

        private void SubtractButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate((a, b) => a - b);
        }

        private void MultiplyButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate((a, b) => a * b);
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate((a, b) => b != 0 ? a / b : double.NaN); // обработка деления на ноль
        }

        private void Calculate(Func<double, double, double> operation)
        {
            if (double.TryParse(InputA.Text, out double a) && double.TryParse(InputB.Text, out double b))
            {
                double result = operation(a, b);
                if (double.IsNaN(result))
                {
                    ResultTextBox.Text = "Ошибка: Деление на ноль";
                }
                else
                {
                    ResultTextBox.Text = result.ToString();
                }
            }
            else
            {
                ResultTextBox.Text = "Ошибка: Введите корректные числа";
            }
        }
    }
}
