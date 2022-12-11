using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MobileShopPOS
{
    public partial class UserModule : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";

        bool check = false;
       
        
        UserForm userForm;
        
        public UserModule(UserForm user)

        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            userForm = user;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Checker();
                if (check)
                {


                    if (MessageBox.Show("Confirm that you want to register this user.", "User Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbUser(name,address,phone,role,dob,password)VALUES(@name,@address,@phone,@role,@dob,@password)", cn);
                        cm.Parameters.AddWithValue("@name", txtname.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", dtBirth.Value);
                        cm.Parameters.AddWithValue("@password", txtPassword.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();


                        MessageBox.Show("User has been registered!", title);
                        Clear();
                        userForm.ShowUser();
                    }
                }

            }
            catch (Exception x)
            {
                cn.Close();
                MessageBox.Show(x.Message, title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Checker();
                if (check)
                {


                    if (MessageBox.Show("Confirm that you want to update this user.", "Upadate?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbUser SET name=@name,address=@address,phone=@phone,role=@role,dob=@dob,password=@password WHERE id=@id", cn);
                        cm.Parameters.AddWithValue("@id", lbluid.Text);
                        cm.Parameters.AddWithValue("@name", txtname.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", dtBirth.Value);
                        cm.Parameters.AddWithValue("@password", txtPassword.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();


                        MessageBox.Show("Data has been updated!", title);
                        Clear();
                        userForm.ShowUser();
                        this.Dispose();
                    }
                }

            }
            catch (Exception x)
            {
                cn.Close();
                MessageBox.Show(x.Message, title);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
    
            this.Dispose();
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //tar bort lösenordmöjlighet vid registrering av ny anställd


            if (cbRole.Text == "Admin")
            {
                lblpassword.Visible = true;
                txtPassword.Visible = true;

            }
            else
            {
                lblpassword.Visible = false;
                txtPassword.Visible = false;
            }
        }

        public void Checker()
        {
            //Kollar så att namn och roll är ifyllda
            if(txtname.Text=="" || cbRole.Text=="" )
            {
                MessageBox.Show("User cannot be registered as required field(s) is/are missing!");
                return;
            }
            

            check = true;
        }

        #region Method

        public void Clear()
        {
            txtname.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtPassword.Clear();
            cbRole.SelectedIndex = 0;
            dtBirth.Value = DateTime.Now;

            btnUpdate.Visible = false;

        }

        #endregion
    }
}
