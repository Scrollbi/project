using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace project
{
    public class BaseStatement : Form
    {
        protected string selectedImagePath;
        protected int id_;
        protected string connectionString = @"Server=localhost\SQLEXPRESS;Database=database;Trusted_Connection=True;";

        protected string GetUser_NameById(int id)
        {
            string userName = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Name FROM Users WHERE UserID = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        userName = result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден.");
                    }
                }
            }

            return userName;
        }

        protected void LoadPdfFromDatabase(int id, Action<byte[], string> savePdfAction)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT FileData, FileName FROM PdfFiles WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            byte[] pdfData = (byte[])reader["FileData"];
                            string fileName = reader["FileName"].ToString();

                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string filePath = Path.Combine(desktopPath, fileName);

                            savePdfAction(pdfData, filePath);
                        }
                        else
                        {
                            MessageBox.Show("Файл не найден в базе данных.");
                        }
                    }
                }
            }
        }

        protected string LoadImage()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Выберите изображение";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            }
            return null;
        }
    }
}
