using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using project;

public class AuthForm : Form
{
    private Label lblUsername;
    private Label lblPassword;
    private Label lblFullName;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private TextBox txtFullName;
    private Button btnSubmit;
    private string action;

    public AuthForm(string action)
    {
        this.action = action;

        this.Size = new System.Drawing.Size(400, action == "reg" ? 400 : 350);
        this.Text = action == "reg" ? "Регистрация" : "Вход";

        lblUsername = new Label()
        {
            Text = "Логин:",
            Location = new System.Drawing.Point(50, 100),
            Size = new System.Drawing.Size(100, 20),
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        lblPassword = new Label()
        {
            Text = "Пароль:",
            Location = new System.Drawing.Point(50, 150),
            Size = new System.Drawing.Size(100, 20),
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        txtUsername = new TextBox()
        {
            Location = new System.Drawing.Point(150, 100),
            Width = 200,
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        txtPassword = new TextBox()
        {
            Location = new System.Drawing.Point(150, 150),
            Width = 200,
            PasswordChar = '*',
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        if (action == "reg")
        {
            lblFullName = new Label()
            {
                Text = "ФИО:",
                Location = new System.Drawing.Point(50, 200),
                Size = new System.Drawing.Size(100, 20),
                Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
            };

            txtFullName = new TextBox()
            {
                Location = new System.Drawing.Point(150, 200),
                Width = 200,
                Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
            };

            this.Controls.Add(lblFullName);
            this.Controls.Add(txtFullName);
        }

        btnSubmit = new Button()
        {
            Text = action == "reg" ? "Зарегистрироваться" : "Войти",
            Location = new System.Drawing.Point(100, action == "reg" ? 250 : 200),
            Width = 200,
            Font = new Font("Arial", 9.75F, FontStyle.Bold | FontStyle.Italic)
        };

        btnSubmit.Click += BtnSubmit_Click;

        this.Controls.Add(lblUsername);
        this.Controls.Add(lblPassword);
        this.Controls.Add(txtUsername);
        this.Controls.Add(txtPassword);
        this.Controls.Add(btnSubmit);
    }

    private void BtnSubmit_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;
        string fullName = action == "reg" ? txtFullName.Text : null;

        if (action == "reg")
        {
            RegisterUser(username, password, fullName);
        }
        else if (action == "add")
        {
            LoginUser(username, password);
        }
    }

    private void RegisterUser(string username, string password, string fullName)
    {
        string connectionString = @"Server=localhost\SQLEXPRESS;Database=database;Trusted_Connection=True;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Users (Login, Password, Name) VALUES (@Username, @Password, @FullName)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@FullName", fullName);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Пользователь зарегистрирован.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }
    }

    private void LoginUser(string username, string password)
    {
        string connectionString = @"Server=localhost\SQLEXPRESS;Database=database;Trusted_Connection=True;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT UserID FROM Users WHERE Login = @Username AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            connection.Open();
            object result = command.ExecuteScalar();

            if (result != null)
            {
                int userId = Convert.ToInt32(result);
                MessageBox.Show("Вход выполнен успешно.");
                Hide();
                menu form = new menu(userId); 
                form.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }
    }
}
