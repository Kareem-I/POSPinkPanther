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
    public partial class MainForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        SqlDataReader dr;

        public MainForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            SaleReport();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDash_Click(object sender, EventArgs e)
        {

        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChForm(new CustomerForm());
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChForm(new UserForm());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            openChForm(new ItemForm());
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            openChForm(new TransactionForm(this));
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to sign out?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Login login = new Login();
                this.Dispose();
                login.ShowDialog();
            }
        }



        //bindning mainfunk. till modul
        private Form aForm = null;
        public void openChForm(Form cForm)
        {
            if (aForm != null)
                aForm.Close();
            aForm = cForm;
            cForm.TopLevel = false;
            cForm.FormBorderStyle = FormBorderStyle.None;
            cForm.Dock = DockStyle.Fill;
            //label3.Text = cForm.Text;
          panel4.Controls.Add(cForm);
            panel4.Tag = cForm;
            cForm.BringToFront();
            cForm.Show();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        public void SaleReport()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");

            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(total),0) AS total FROM tbTrans WHERE transno LIKE'" + date + "%'", cn);
                lblDailySale.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTrans_Click(object sender, EventArgs e)
        {
            openChForm(new TranshistoryForm());
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
