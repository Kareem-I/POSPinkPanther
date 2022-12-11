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
    public partial class TranshistoryForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        SqlDataReader dr;
        MainForm main;
        TransactionForm trans; 

        public TranshistoryForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            ListTrans();
        }


        public void ListTrans()
        {
            dgvTranshis.Rows.Clear();
          
            
                dgvTranshis.Rows.Clear();
                int i = 0;
                double total = 0;


        
                cm = new SqlCommand("SELECT transid,transno,itemcode, itemname, qty, price, total, cu.fullname ,employee FROM tbTrans as trans LEFT JOIN tbCustomer cu ON trans.cid = cu.cid", cn);
                //cm = new SqlCommand("SELECT top 1 transno FROM tbTrans", cn);
                cn.Open();
               dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvTranshis.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                    total += double.Parse(dr[5].ToString());
                }
               
                dr.Close();
                cn.Close();
           
            }

        private void dgvTranshis_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string clNmn = dgvTranshis.Columns[e.ColumnIndex].Name;
           
            if (clNmn == "Delete")
            {
                if (MessageBox.Show("Confirm that you want to remove this transaction!", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tbtrans WHERE transid LIKE '" + dgvTranshis.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("The transaction has been removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            ListTrans();
        }
    }
    }

