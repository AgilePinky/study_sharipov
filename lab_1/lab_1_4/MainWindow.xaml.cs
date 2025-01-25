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

namespace lab_1_4
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PopulateYears();
            PopulateMonths();
        }

        private void PopulateYears()
        {
            for (int year = DateTime.Now.Year; year >= 1900; year--)
            {
                YearComboBox.Items.Add(year);
            }
        }

        private void PopulateMonths()
        {
            for (int month = 1; month <= 12; month++)
            {
                MonthComboBox.Items.Add(month);
            }
        }

        private void YearComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateDays();
        }

        private void MonthComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateDays();
        }

        private void UpdateDays()
        {
            if (YearComboBox.SelectedItem != null && MonthComboBox.SelectedItem != null)
            {
                int selectedYear = (int)YearComboBox.SelectedItem;
                int selectedMonth = (int)MonthComboBox.SelectedItem;

                DayComboBox.IsEnabled = true;
                DayComboBox.Items.Clear();

                int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    DayComboBox.Items.Add(day);
                }
            }
            else
            {
                DayComboBox.IsEnabled = false;
            }
        }

        private void DayComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DayComboBox.SelectedItem != null && YearComboBox.SelectedItem != null && MonthComboBox.SelectedItem != null)
            {
                int selectedYear = (int)YearComboBox.SelectedItem;
                int selectedMonth = (int)MonthComboBox.SelectedItem;
                int selectedDay = (int)DayComboBox.SelectedItem;

                DateTime selectedDate = new DateTime(selectedYear, selectedMonth, selectedDay);
                DateTime currentDate = DateTime.Now;

                var age = currentDate - selectedDate;

                MessageBox.Show($"Прошло: {age.Days / 365} лет, {(age.Days % 365) / 30} месяцев, {age.Days % 30} дней.");
            }
        }
    }
}
