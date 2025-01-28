using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace lab_2
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Data Source=D:\\A_Studing\\sibguti\\Программирование и обработка графического интерфейса(часть 1)\\lab_2\\lab_2\\students.db;Version=3;";

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadStudents();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string createStudentsTable = @"CREATE TABLE IF NOT EXISTS Students (
                                                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                 UniqueNumber TEXT UNIQUE NOT NULL,
                                                 FullName TEXT NOT NULL,
                                                 PhoneNumber TEXT)";
                    string createGradesTable = @"CREATE TABLE IF NOT EXISTS Grades (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            UniqueNumber TEXT NOT NULL,
                                            PhysicsGrade INTEGER NOT NULL,
                                            MathGrade INTEGER NOT NULL,
                                            FOREIGN KEY (UniqueNumber) REFERENCES Students (UniqueNumber))";

                    using (var command = new SQLiteCommand(createStudentsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (var command = new SQLiteCommand(createGradesTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при инициализации базы данных: " + ex.Message);
            }
        }

        private void LoadStudents()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var students = new List<Student>();

                string query = "SELECT s.UniqueNumber, s.FullName, s.PhoneNumber, g.PhysicsGrade, g.MathGrade FROM Students s " +
                               "LEFT JOIN Grades g ON s.UniqueNumber = g.UniqueNumber";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                UniqueNumber = reader["UniqueNumber"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                PhysicsGrade = reader["PhysicsGrade"] != DBNull.Value ? (int?)Convert.ToInt32(reader["PhysicsGrade"]) : null,
                                MathGrade = reader["MathGrade"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MathGrade"]) : null
                            };
                            students.Add(student);
                        }
                    }
                }

                lvStudents.ItemsSource = students;
            }
        }

        private void LoadStudents_Click(object sender, RoutedEventArgs e)
        {
            LoadStudents(); // Вызываем метод для загрузки студентов при нажатии кнопки
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string insertStudent = "INSERT INTO Students (UniqueNumber, FullName, PhoneNumber) VALUES (@UniqueNumber, @FullName, @PhoneNumber)";
                    using (var command = new SQLiteCommand(insertStudent, connection))
                    {
                        command.Parameters.AddWithValue("@UniqueNumber", txtUniqueNumber.Text);
                        command.Parameters.AddWithValue("@FullName", txtFullName.Text);
                        command.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
                        command.ExecuteNonQuery();
                    }

                    string insertGrades = "INSERT INTO Grades (UniqueNumber, PhysicsGrade, MathGrade) VALUES (@UniqueNumber, @PhysicsGrade, @MathGrade)";
                    using (var command = new SQLiteCommand(insertGrades, connection))
                    {
                        command.Parameters.AddWithValue("@UniqueNumber", txtUniqueNumber.Text);
                        command.Parameters.AddWithValue("@PhysicsGrade", int.Parse(txtPhysicsGrade.Text));
                        command.Parameters.AddWithValue("@MathGrade", int.Parse(txtMathGrade.Text));
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }

            LoadStudents();
        }

        private void UpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            if (lvStudents.SelectedItem is Student selectedStudent)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        string updateStudent = "UPDATE Students SET FullName = @FullName, PhoneNumber = @PhoneNumber WHERE UniqueNumber = @UniqueNumber";
                        using (var command = new SQLiteCommand(updateStudent, connection))
                        {
                            command.Parameters.AddWithValue("@UniqueNumber", selectedStudent.UniqueNumber);
                            command.Parameters.AddWithValue("@FullName", txtFullName.Text);
                            command.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
                            command.ExecuteNonQuery();
                        }

                        string updateGrades = "UPDATE Grades SET PhysicsGrade = @PhysicsGrade, MathGrade = @MathGrade WHERE UniqueNumber = @UniqueNumber";
                        using (var command = new SQLiteCommand(updateGrades, connection))
                        {
                            command.Parameters.AddWithValue("@UniqueNumber", selectedStudent.UniqueNumber);
                            command.Parameters.AddWithValue("@PhysicsGrade", int.Parse(txtPhysicsGrade.Text));
                            command.Parameters.AddWithValue("@MathGrade", int.Parse(txtMathGrade.Text));
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }

                LoadStudents();
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (lvStudents.SelectedItem is Student selectedStudent)
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string deleteGrades = "DELETE FROM Grades WHERE UniqueNumber = @UniqueNumber";
                    using (var command = new SQLiteCommand(deleteGrades, connection))
                    {
                        command.Parameters.AddWithValue("@UniqueNumber", selectedStudent.UniqueNumber);
                        command.ExecuteNonQuery();
                    }

                    string deleteStudent = "DELETE FROM Students WHERE UniqueNumber = @UniqueNumber";
                    using (var command = new SQLiteCommand(deleteStudent, connection))
                    {
                        command.Parameters.AddWithValue("@UniqueNumber", selectedStudent.UniqueNumber);
                        command.ExecuteNonQuery();
                    }
                }

                LoadStudents();
            }
        }

        private void lvStudents_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvStudents.SelectedItem is Student selectedStudent)
            {
                txtUniqueNumber.Text = selectedStudent.UniqueNumber;
                txtFullName.Text = selectedStudent.FullName;
                txtPhoneNumber.Text = selectedStudent.PhoneNumber;
                txtPhysicsGrade.Text = selectedStudent.PhysicsGrade.ToString();
                txtMathGrade.Text = selectedStudent.MathGrade.ToString();
            }
        }
    }

    public class Student
    {
        public string UniqueNumber { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int? PhysicsGrade { get; set; }
        public int? MathGrade { get; set; }
    }
}