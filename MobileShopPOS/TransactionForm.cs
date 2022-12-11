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
    public partial class TransactionForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        SqlDataReader dr;
        MainForm main;
        TransactionForm trans;
        //DataTable dt = new DataTable();
        //SqlDataAdapter adapt;

        public TransactionForm(MainForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            main = form;
            getTrans();
           ShowTrans();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TransProd tp = new TransProd(this);
            tp.employee = main.lblUser.Text;
            tp.ShowDialog();
        }

        private void lbltrans_Click(object sender, EventArgs e)
        {

        }

        public void getTrans()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;
                cn.Open();
                cm = new SqlCommand("SELECT TOP 1 transno FROM tbTrans WHERE transno LIKE '" + sdate + "%' ORDER BY transno DESC", cn);
      

                dr = cm.ExecuteReader();
                dr.Read();

                if(dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));

                    lbltrans.Text = sdate + (count + 1);
                   
                }
                else
                {
                    transno = sdate + "1001";
                    lbltrans.Text = transno;
                }
                dr.Close();
                cn.Close();

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        //public void getReceipt()
        //{
        //    string sdate = DateTime.Now.ToString("yyyyMMdd");
        //    int count;
        //    string transno;
        //    cn.Open();
        //    cm = new SqlCommand("SELECT TOP 1 transno FROM tbTrans WHERE transno LIKE '" + sdate + "%' ORDER BY transno DESC", cn);




        //}

        public void ShowTrans()
        {
            dgvSale.Rows.Clear();
            try
            {
                dgvSale.Rows.Clear();
                int i = 0;
                double total = 0;
                

                cm = new SqlCommand("SELECT transid,itemcode, itemname, qty, price, total, cu.fullname ,employee, transno FROM tbTrans as trans LEFT JOIN tbCustomer cu ON trans.cid = cu.cid WHERE transno LIKE "+lbltrans.Text+"", cn);
                
                cn.Open();
                           dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvSale.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                    total += double.Parse(dr[5].ToString());
                }
                lbltot.Text = total.ToString();
                dr.Close();
                cn.Close();
                //new Report.PrintInvoiceForm()
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }

        private void dgvSale_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string clNmn = dgvSale.Columns[e.ColumnIndex].Name;
            removeitem:
            if (clNmn == "Delete")
            {
                if (MessageBox.Show("Confirm that you want to remove this transaction!", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tbtrans WHERE transid LIKE '" + dgvSale.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("The transaction has been removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (clNmn == "Increase")
            {
                int i = chkqty(dgvSale.Rows[e.RowIndex].Cells[2].Value.ToString());
                if (int.Parse(dgvSale.Rows[e.RowIndex].Cells[4].Value.ToString()) < i)
                {
                    cn.Open();
                    cm = new SqlCommand("UPDATE tbTrans SET qty = qty + " + 1 + " WHERE transid LIKE '" + dgvSale.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);   
                    cm.ExecuteNonQuery();
                    cn.Close();
                }   else
                {
                    MessageBox.Show("The stock quantity is " + i + "!", "Not sufficient stock!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (clNmn == "Decrease")
            {
                if(int.Parse(dgvSale.Rows[e.RowIndex].Cells[4].Value.ToString()) == 1)
                {
                    clNmn = "Delete";
                    goto removeitem;
                }
                cn.Open();
                cm = new SqlCommand("UPDATE tbTrans SET qty = qty - " + 1 + " WHERE transid LIKE '" + dgvSale.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);

                cm.ExecuteNonQuery();
                cn.Close();
            }

            ShowTrans();
        }


        public int chkqty(string itemcode)
        {
            int i = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT itemqty FROM tbItem WHERE itemid LIKE '" + itemcode + "'", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
            return i;
        }

        private void guna2Button1_Click(object sender, EventArgs e)  /// SALEbtn
        {
            CustomerSale cs = new CustomerSale(this);
            cs.ShowDialog();

            if (MessageBox.Show("Do you want to check out this order?", "Sale", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                getTrans();
                main.SaleReport();
                for (int i = 0; i < dgvSale.Rows.Count; i++)
                {
                    cn.Open();
                    cm = new SqlCommand("UPDATE tbItem SET itemqty = itemqty - " + int.Parse(dgvSale.Rows[i].Cells[4].Value.ToString()) + " WHERE itemid LIKE " + dgvSale.Rows[i].Cells[2].Value.ToString() + "", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                }
                dgvSale.Rows.Clear();
            }

          

            dgvSale.Rows.Clear();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbltot_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
