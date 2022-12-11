using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShopPOS
{
    public partial class Login : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        SqlDataReader dr; 
        public Login()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            
                try
                {
                    string _name = "", _role = "";
                    cn.Open();
                    cm = new SqlCommand("SELECT name,role FROM tbUser WHERE name=@name and password=@password", cn);
                    cm.Parameters.AddWithValue("@name", txtusername.Text);
                    cm.Parameters.AddWithValue("@password", txtpass.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        _name = dr["name"].ToString();
                        _role = dr["role"].ToString();
                        MessageBox.Show("Welcome  " + _name + " |", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MainForm main = new MainForm();
                        main.lblUser.Text = _name;
                        main.lblrole.Text = _role;
                        if (_role != "Admin")
                            main.btnUser.Enabled = false;
                        this.Hide();
                        main.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Wrong username/password", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    dr.Close();
                    cn.Close();
                    MessageBox.Show(ex.Message, title);
                }

            }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }

        
    }

