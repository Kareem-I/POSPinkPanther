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
using System.Data.Sql;

namespace MobileShopPOS
{
    public partial class ItemForm : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        SqlDataReader dr;
        DataTable dt = new DataTable();
        SqlDataAdapter adapt;

        bool check = false;

        public ItemForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            ShowItems();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ItemModule module = new ItemModule(this);
            module.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            cn.Open();
            adapt = new SqlDataAdapter("SELECT * FROM tbUser WHERE concat(ItemName,Category, QTY, Price) like '" + txtSearch.Text + "%'", cn);
            dt = new DataTable();
            adapt.Fill(dt);
            dgvItems.DataSource = dt;
            cn.Close();
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string clName = dgvItems.Columns[e.ColumnIndex].Name;

                if(clName == "Edit")
            {
                ItemModule module = new ItemModule(this);
                module.lblitemid.Text = dgvItems.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtItemname.Text = dgvItems.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.cbCategory.Text = dgvItems.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtqty.Text = dgvItems.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.txtprice.Text = dgvItems.Rows[e.RowIndex].Cells[5].Value.ToString();


                module.btnSave.Visible = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
                else if(clName == "Delete")
            {
                if(MessageBox.Show("Confirm that you want to remove this item!","Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tbItem WHERE itemid LIKE '" + dgvItems.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn); 
                    
                    
                    //"DELETE FROM supplier WHERE supplier_id =" + textBox1.Text;
                    cm.ExecuteNonQuery();

                    //cm = new SqlCommand("DELETE FROM tbItem WHERE itemid =" + dgvItems.Rows[e.RowIndex].Cells[0].Value.ToString());
                    cn.Close();
                    MessageBox.Show("Data has been removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            ShowItems();
        }

        public void ShowItems()
        {

            int i = 0;
            dgvItems.Rows.Clear();
            // "SELECT * FROM tbUser2 WHERE CONCAT(name,address,phone,role,dob) LIKE '%" + txtSearch.Text + "%'"       WHERE CONCAT(itemname,itemcategory,itemqty,itemprice) LIKE '%" + txtSearch.Text + "%'"
           cm = new SqlCommand("SELECT * FROM tbItem", cn);
            //adapt = SqlDataAdapter("SELECT * FROM tbItem", cn);
            cn.Open();
            //adapt = SqlDataAdapter("SELECT * FROM tbItem WHERE CONCAT(itemname,itemcategory,itemqty,itemprice) LIKE '%" + txtSearch.Text + "%'", cn);
            //dt = new DataTable();
            //adapt.Fill(dt);
            //dgvItems.DataSource = dt;
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvItems.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(),dr[4].ToString());

            }
            dr.Close();
            cn.Close();
        }
    }
}
