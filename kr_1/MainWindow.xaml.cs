using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace kr_1
{

    public class PuzzleGenerator
    {
        private int[,] board;
        private int rows;
        private int columns;
        private Random random = new Random();

        // Конструктор, принимающий количество строк и колонок
        public PuzzleGenerator(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            board = new int[rows, columns];
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < rows * columns; i++)
            {
                numbers.Add(i);
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int index = random.Next(numbers.Count);
                    board[i, j] = numbers[index];
                    numbers.RemoveAt(index);
                }
            }
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public bool IsMoveValid(int emptyRow, int emptyCol, int targetRow, int targetCol)
        {
            return (Math.Abs(emptyRow - targetRow) == 1 && emptyCol == targetCol) ||
                   (Math.Abs(emptyCol - targetCol) == 1 && emptyRow == targetRow);
        }

        public bool CheckWin()
        {
            int count = 1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (board[i, j] != count)
                    {
                        return false;
                    }
                    count++;
                }
            }
            return true;
        }
    }
    public partial class MainWindow : Window
    {
        private PuzzleGenerator puzzleGenerator;
        private Button[,] buttons;
        private int emptyRow;
        private int emptyCol;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame(0, 0); // Инициализация с размером 4x4 по умолчанию
        }

        private void InitializeGame(int rows, int columns)
        {
            puzzleGenerator = new PuzzleGenerator(rows, columns);
            buttons = new Button[rows, columns];
            int[,] board = puzzleGenerator.GetBoard();

            GameGrid.Rows = rows;
            GameGrid.Columns = columns;
            GameGrid.Children.Clear(); // Очищаем предыдущие кнопки

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Content = board[i, j] == 0 ? "" : board[i, j].ToString();
                    buttons[i, j].Click += Button_Click;
                    GameGrid.Children.Add(buttons[i, j]);
                    Grid.SetRow(buttons[i, j], i);
                    Grid.SetColumn(buttons[i, j], j);

                    if (board[i, j] == 0)
                    {
                        emptyRow = i;
                        emptyCol = j;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            if (puzzleGenerator.IsMoveValid(emptyRow, emptyCol, row, col))
            {
                // Поменять местами контент кнопок
                buttons[emptyRow, emptyCol].Content = button.Content;
                button.Content = "";

                // Обновить пустую позицию
                emptyRow = row;
                emptyCol = col;

                // Проверить на победу
                if (puzzleGenerator.CheckWin())
                {
                    MessageBox.Show("Вы выиграли!");
                }
            }
        }

        // Обработчик для изменения количества строк
        private void RowsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RowsComboBox.SelectedItem != null && ColumnsComboBox.SelectedItem != null)
            {
                int rows = int.Parse(((ComboBoxItem)RowsComboBox.SelectedItem).Content.ToString());
                int columns = int.Parse(((ComboBoxItem)ColumnsComboBox.SelectedItem).Content.ToString());
                InitializeGame(rows, columns);
            }
        }

        // Обработчик для изменения количества колонок
        private void ColumnsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RowsComboBox.SelectedItem != null && ColumnsComboBox.SelectedItem != null)
            {
                int rows = int.Parse(((ComboBoxItem)RowsComboBox.SelectedItem).Content.ToString());
                int columns = int.Parse(((ComboBoxItem)ColumnsComboBox.SelectedItem).Content.ToString());
                InitializeGame(rows, columns);
            }
        }
    }
}