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
    public partial class CustomerForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        SqlDataReader dr;

        public CustomerForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            ShowCustomer();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModule module = new CustomerModule(this);
            module.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ShowCustomer();
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string clName = dgvCustomer.Columns[e.ColumnIndex].Name;

            if (clName == "Edit")
            {
                CustomerModule module = new CustomerModule(this);
                module.lblcid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtCname.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtCphone.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtCemail.Text = dgvCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.dtCBirth.Text = dgvCustomer.Rows[e.RowIndex].Cells[5].Value.ToString();


                module.btnSave.Visible = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if (clName == "Delete")
            {
                if (MessageBox.Show("Confirm that you want to remove this customer!", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tbCustomer WHERE cid LIKE '" + dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn); //"DELETE FROM supplier WHERE supplier_id =" + textBox1.Text;
                    cm.ExecuteNonQuery();
                
                    cn.Close();
                    MessageBox.Show("Customer has been removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            ShowCustomer();
        }




        public void ShowCustomer()
        {

            int i = 0;
            dgvCustomer.Rows.Clear();
            // cm = new SqlCommand( "SELECT * FROM tbUser WHERE CONCAT(name,address,phone,role,dob) LIKE '%" + txtSearch.Text + "%'", cn);  
            cm = new SqlCommand("SELECT * FROM tbCustomer", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), DateTime.Parse(dr[4].ToString()).ToShortDateString());

            }
            dr.Close();
            cn.Close();
        }
    }
}
