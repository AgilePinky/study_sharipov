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

namespace lab_1_3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlanetListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PlanetListBox.SelectedItem is System.Windows.Controls.ListBoxItem selectedItem)
            {
                string planet = selectedItem.Content.ToString();
                InfoTextBox.Text = GetPlanetInfo(planet);
            }
        }

        private string GetPlanetInfo(string planet)
        {
            switch (planet)
            {
                case "Меркурий":
                    return "Меркурий — ближайшая к Солнцу планета.";
                case "Венера":
                    return "Венера — вторая планета от Солнца и близнец Земли.";
                case "Земля":
                    return "Земля — третья планета от Солнца и единственная известная планета, на которой поддерживается жизнь.";
                case "Марс":
                    return "Марс известен как Красная планета из-за своего красноватого цвета.";
                case "Юпитер":
                    return "Юпитер – самая большая планета Солнечной системы.";
                case "Сатурн":
                    return "Сатурн известен своей обширной системой колец.";
                case "Уран":
                    return "Уран — ледяной гигант с уникальным боковым вращением.";
                case "Нептун":
                    return "Нептун — самая дальняя планета от Солнца, известная своим темно-синим цветом.";
                default:
                    return "Ошибка.";
            }
        }
    }
}
