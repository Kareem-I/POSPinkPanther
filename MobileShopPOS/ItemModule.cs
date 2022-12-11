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
    public partial class ItemModule : Form

    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlCommand cmp = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        bool check = false;
        ItemForm item;
        public ItemModule(ItemForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            item = form;
            cbCategory.SelectedIndex = 0;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               Checker();
                if (check)
                {


                    if (MessageBox.Show("Confirm that you want to register this item.", "Item Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbItem (itemname,itemcategory,itemqty,itemprice)VALUES(@itemname,@itemcategory,@itemqty,@itemprice)", cn);
                    
                        cm.Parameters.AddWithValue("@itemid", lblitemid.Text);
                            cm.Parameters.AddWithValue("@itemname", txtItemname.Text);
                            cm.Parameters.AddWithValue("@itemcategory", cbCategory.Text);
                            cm.Parameters.AddWithValue("@itemprice", double.Parse(txtprice.Text));

                        //Tjänster har inga kvantiteter
                        if(txtqty.Text == "")
                        {
                            cm.Parameters.AddWithValue("@itemqty", DBNull.Value);
                        }
                        else
                        {
                            cm.Parameters.AddWithValue("@itemqty", int.Parse(txtqty.Text));
                        }
                    



                    

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();

                        MessageBox.Show("Item has been registered!", title);
                        Clear();
                        item.ShowItems();

                     
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


                    if (MessageBox.Show("Confirm that you want to update this item.", "Item updated!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbItem SET itemname=@itemname, itemcategory=@itemcategory, itemqty=@itemqty, itemprice=@itemprice WHERE itemid=@itemid", cn);
                        cm.Parameters.AddWithValue("@itemid", lblitemid.Text);
                        cm.Parameters.AddWithValue("@itemname", txtItemname.Text);
                        cm.Parameters.AddWithValue("@itemcategory", cbCategory.Text);
                        //cm.Parameters.AddWithValue("@itemqty", int.Parse(txtqty.Text));
                        //Tjänster har inga kvantiteter
                        if (txtqty.Text == "")
                        {
                            cm.Parameters.AddWithValue("@itemqty", DBNull.Value);
                        }
                        else
                        {
                            cm.Parameters.AddWithValue("@itemqty", int.Parse(txtqty.Text));
                        }
                        cm.Parameters.AddWithValue("@itemprice", double.Parse(txtprice.Text));
                      

                      

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();


                        MessageBox.Show("Item has been updated!", title);
                        item.ShowItems();
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
        public void Checker()
        {
            //Kollar så att namn och roll är ifyllda


            if (txtItemname.Text == "" || txtprice.Text == "" || txtqty.Text == "" && cbCategory.Text != "Service")
            {
                MessageBox.Show("Please fill in all required field(s)!");
                return;
            }

            check = true;
        }

        public void Clear()
        {
            txtItemname.Clear();
            txtprice.Clear();
            txtqty.Clear();
            cbCategory.SelectedIndex = 0;
            

           // btnUpdate.Visible = false;

        }

        private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar!=','))
            {
                e.Handled = true;
            }
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCategory.Text == "Service")
            {
                qtylbl.Visible = false;
                txtqty.Visible = false;
            }

            else
            {
                qtylbl.Visible = true;
                txtqty.Visible = true;
            }
        }
    }
}
