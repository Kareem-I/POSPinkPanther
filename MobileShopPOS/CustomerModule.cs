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
    public partial class CustomerModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        CustomerForm customer;

        bool check = false;


        public CustomerModule(CustomerForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            customer = form;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Checker();
                if (check)
                {


                    if (MessageBox.Show("Confirm that you want to register this customer.", "Customer Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbCustomer(fullname,phone,email,cdob)VALUES(@fullname,@phone,@email,@cdob)", cn);
                        cm.Parameters.AddWithValue("@fullname", txtCname.Text);
                        cm.Parameters.AddWithValue("@phone", txtCphone.Text);
                        cm.Parameters.AddWithValue("@email", txtCemail.Text);
                        cm.Parameters.AddWithValue("@cdob", dtCBirth.Value);
           

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();


                        MessageBox.Show("Customer has been registered!", title);
                        Clear();
                        customer.ShowCustomer();
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


                    if (MessageBox.Show("Confirm that you want to update this customer.", "Customer updated!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbCustomer SET fullname=@fullname, phone=@phone, email=@email, cdob=@cdob WHERE cid=@cid", cn);
                        cm.Parameters.AddWithValue("@cid", lblcid.Text);
                        cm.Parameters.AddWithValue("@fullname", txtCname.Text);
                        cm.Parameters.AddWithValue("@phone", txtCphone.Text);
                        cm.Parameters.AddWithValue("@email", txtCemail.Text);
                        cm.Parameters.AddWithValue("@cdob", dtCBirth.Value);


                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();


                        MessageBox.Show("Customer has been updated!", title);
                        customer.ShowCustomer();
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
            this.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }


        public void Checker()
        {
            //Kollar så att namn och roll är ifyllda
            if (txtCname.Text == "" || txtCemail.Text == "")
            {
                MessageBox.Show("User cannot be registered as required field(s) is/are missing!");
                return;
            }
          

            check = true;
        }

        public void Clear()
        {
            txtCname.Clear();
            txtCphone.Clear();
            txtCemail.Clear();
            dtCBirth.Value = DateTime.Now;

            btnUpdate.Visible = false;

        }
    }
}
