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
    public partial class UserForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        string title = "MobilePOS System";
        SqlDataReader dr;

    
        public UserForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            ShowUser();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModule module = new UserModule(this);
            module.ShowDialog();
        }

        #region Method
        public void ShowUser()
        {
            
            int i = 0;
            dgvUser.Rows.Clear();
           // cm = new SqlCommand( "SELECT * FROM tbUser WHERE CONCAT(name,address,phone,role,dob) LIKE '%" + txtSearch.Text + "%'", cn);
            cm = new SqlCommand("SELECT * FROM tbUser", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dgvUser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(),dr[2].ToString(),dr[3].ToString(),dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString());
               
            }
            dr.Close();
            cn.Close();
        }

        public void YT()                            // WHERE CONCAT(name, address, phone, role, dob) LIKE '%" + txtSearch.Text + "%'"
        {
            cn.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT * FROM tbUser", cn);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);
            dgvUser.DataSource = dtbl;
            dgvUser.AutoGenerateColumns = false;
            cn.Close();
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colNmn = dgvUser.Columns[e.ColumnIndex].Name;

            if(colNmn == "Edit")
            {
                UserModule module = new UserModule(this);
                module.lbluid.Text = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtname.Text = dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtAddress.Text = dgvUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtPhone.Text = dgvUser.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.cbRole.Text = dgvUser.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.dtBirth.Text = dgvUser.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.txtPassword.Text = dgvUser.Rows[e.RowIndex].Cells[7].Value.ToString();

                module.btnSave.Visible = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if (colNmn == "Delete")
            {
                if(MessageBox.Show("Confirm if you want to delete this from the database?", "DELETE?",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tbUser WHERE id LIKE '" + dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Datainfo has been removed from the database!");
                }
            }

            ShowUser();
        }

        //public void Test()
        //{
        //    SqlCommand cmd = new SqlCommand("SELECT * FROM tbUser WHERE name= @name");
        //    cmd.Connection = cn;
        //    //cmd.CommandText "SELECT * FROM tbUser WHERE name= @name  and password= @pwd";
        //    dgvUser.Rows.Add("@name");

        //    //cmd.Parameters.Add("@username", SqlDbType.VarChar, 40).Value = textBox1.Text;
        //    //cmd.Parameters.Add("@pwd", SqlDbType.NVarChar, 40).Value = textBox2.Text;
        //}

        #endregion Method



        //public void FillGrid()
        //{
        //    cn.Open();
        //    string query = "SELECT * FROM tbUser2 WHERE CONCAT(name,address,phone,role,dob) LIKE '%" + txtSearch.Text + "%'";
        //    SqlDataAdapter da = new SqlDataAdapter(query, cn);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    dgvUser.DataSource = dt;
        //    cn.Close();


    }

}
