using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        string connStr = "server=osp74.ru;port=33666;user=st6;database=st6_kr;password=Q123q123!;";
        MySqlConnection conn;
        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection(connStr);
        }

        internal static string CalculateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        int i = 1;

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql1 = "SELECT password FROM User WHERE login = '" + textBox1.Text + "'";
            MySqlCommand command1 = new MySqlCommand(sql1, conn);
            string pass = Convert.ToString(command1.ExecuteScalar());
            conn.Close();

            if (checkPassword(pass, i) == true)
            {
                MessageBox.Show("Успешно");
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
                i++;
            }
        }

        private bool checkPassword(string userPassword, int numberOfItterations)
        {
            Rfc2898DeriveBytes PBKDF2 = new Rfc2898DeriveBytes(userPassword, 8, numberOfItterations);
            byte[] hash = PBKDF2.Salt;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            string salt = sb.ToString();

            bool passwordsMach;

            if ((CalculateMD5Hash(textBox2.Text) + salt) == (userPassword + salt))
            {
                passwordsMach = true;
            }
            else
            {
                passwordsMach = false;
            }
            return passwordsMach;
        }
    }
}
