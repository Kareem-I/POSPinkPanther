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
    public partial class CustomerSale : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        SqlDataReader dr;
        TransactionForm sale;


        public CustomerSale(TransactionForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            sale = form;
            ShowCustomer();
        }




        public void ShowCustomer()
        {
            try
            {     int i = 0;
                dgvCusSale.Rows.Clear();
                // cm = new SqlCommand( "SELECT * FROM tbUser WHERE CONCAT(name,address,phone,role,dob) LIKE '%" + txtSearch.Text + "%'", cn);  (fullname,phone,email,cdob)
                cm = new SqlCommand("SELECT cid,fullname,phone FROM tbCustomer", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCusSale.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());

                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ShowCustomer();
        }

        private void dgvCusSale_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string clNmn = dgvCusSale.Columns[e.ColumnIndex].HeaderText;
            if (clNmn == "Choose")
            {
                cn.Open();
                cm = new SqlCommand("UPDATE tbTrans SET cid =" + dgvCusSale.Rows[e.RowIndex].Cells[1].Value.ToString() + " WHERE transno=" + sale.lbltrans.Text +"", cn);                                                                        //"DELETE FROM supplier WHERE supplier_id =" + textBox1.Text;
                cm.ExecuteNonQuery();
                cn.Close();
                sale.ShowTrans();
                this.Dispose();
            }
        }
    }
}
