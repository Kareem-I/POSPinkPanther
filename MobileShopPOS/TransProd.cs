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
    public partial class TransProd : Form
    {
        public string employee;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        SqlDataReader dr;
        TransactionForm tranf;




        public TransProd(TransactionForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            tranf = form;
            ShowItems();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

       

        public void ShowItems()
        {

            int i = 0;
            //itemid,itemname, itemcategory, itemqty,itemprice
            dgvTrans.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbItem", cn);
          
            cn.Open();
           dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvTrans.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());

            }
            dr.Close();
            cn.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)   //btnSubmit
        {

            foreach (DataGridViewRow dr in dgvTrans.Rows)
            {
                bool box = Convert.ToBoolean(dr.Cells["Add"].Value);      
                if (box)
                {
                    try
                    {


                        cm = new SqlCommand("INSERT INTO tbTrans(transno, itemcode, itemname, qty, price, employee) VALUES (@transno, @itemcode, @itemname, @qty, @price, @employee)", cn);
                        cm.Parameters.AddWithValue("@transno", tranf.lbltrans.Text);
                        cm.Parameters.AddWithValue("@itemcode", dr.Cells[1].Value.ToString());
                        cm.Parameters.AddWithValue("@itemname", dr.Cells[2].Value.ToString());
                        cm.Parameters.AddWithValue("@qty", 1);
                        cm.Parameters.AddWithValue("@price", Convert.ToDouble(dr.Cells[5].Value.ToString()));
                        cm.Parameters.AddWithValue("@employee", employee);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();

                    }
                    catch (Exception ex)
                    {
                        cn.Close();
                        MessageBox.Show(ex.Message, title);
                    }



                }
            }

           tranf.ShowTrans();
            this.Dispose();

        }

        private void TransProd_Load(object sender, EventArgs e)
        {

        }
    }
}
