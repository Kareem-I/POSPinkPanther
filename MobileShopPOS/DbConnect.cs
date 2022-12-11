using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace MobileShopPOS
{
    class DbConnect
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        private string con;


            public string connection()
        {
            
            con = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\karee\Desktop\Ny mapp\MobileShopPOS\MobileShopPOS\POSMobileShop.mdf;Integrated Security=True";
            return con;
        }
    }
}




