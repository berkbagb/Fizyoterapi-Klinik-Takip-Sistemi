using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fizyotakip2
{


    public static class Shared
    {
        private static string servername = "(localdb)\\MSSQLLocalDB";
        private static string databaseName = "klinik";
        private static SqlConnection con;
       
        public static SqlConnection getConnection() //sql bağlantısı çekme kodu
        {


            con = new SqlConnection("Data source = "+servername+ "; Initial Catalog= "+databaseName+"; Integrated Security = true;");

            return con; //type çıktısı sqlconnection'dır

            
             
 


        }

        public static void UpdateDataGridView(DataGridView dgv, string tableName)
        {

            /*
                kullanım: UpdateDataGridView(dataGridView1, hastalar)
                sonuç: hastalar tablosundaki verileri datagridview1'e çeker



             */
           
            SqlDataAdapter adap = new SqlDataAdapter("SELECT * FROM " + tableName, con);
            DataTable dt = new DataTable();
            con.Open();
            
                adap.Fill(dt);
                dgv.DataSource = dt;

            con.Close();

        }

        public static bool TumAlanlarDolu(params string[] alanlar)
        {
            return alanlar.All(s => !string.IsNullOrWhiteSpace(s));
        }

        public static void ComboboxFill(ComboBox cmb, string tableName)
        {

            cmb.DataSource = null; // DataSource sıfırlanır (tedavitürleri için)
            cmb.Items.Clear();     // Items koleksiyonu temizlenir (hastalar ve Fizyoterapistler için)
            cmb.SelectedIndex = -1; // Seçili öğe sıfırlanır
            cmb.DisplayMember = ""; // DisplayMember sıfırlanır
            cmb.ValueMember = "";   // ValueMember sıfırlanır

            if (tableName == "hastalar")
            {
                con.Open();
                string query = "SELECT ID, İsim + ' ' + Soyisim AS HastaAd FROM hastalar";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int id = (int)dr["ID"];
                    string hastaAd = dr["HastaAd"].ToString();
                    cmb.Items.Add(new KeyValuePair<int, string>((int)dr["ID"], dr["HastaAd"].ToString()));
                    Console.WriteLine(hastaAd);
                    Console.WriteLine(id);
                }

                cmb.DisplayMember = "Value";
                cmb.ValueMember = "Key";
                con.Close();
            }
            else if (tableName == "tedavitürleri") 
            {
                con.Open();

                string query = "SELECT TedaviID, TedaviAdi FROM tedavitürleri";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

                Dictionary<int, string> tedaviler = new Dictionary<int, string>();
                while (dr.Read())
                {

                    tedaviler.Add(dr.GetInt32(0), dr.GetString(1)); //tedaviid - tedaviadi

                }

                cmb.DataSource = new BindingSource(tedaviler, null);

                cmb.DisplayMember = "Value";
                cmb.ValueMember = "Key";


                con.Close();
            }
            else if (tableName == "Fizyoterapistler")
            {
                con.Open();
                string query = "SELECT FizyoterapistID, name  FROM Fizyoterapistler";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {



                    cmb.Items.Add(new KeyValuePair<int, string>((int)dr["FizyoterapistID"], dr["name"].ToString()));
                }
                con.Close();
                cmb.DisplayMember = "Value";
                cmb.ValueMember = "Key";
            }


        }



        public static void RandevulariGetir(DataGridView dgv)
        {

            con.Open();
            string query = "SELECT Randevular.RandevuID, hastalar.İsim + ' ' + hastalar.Soyisim AS HastaAd, " +
                           "Fizyoterapistler.name AS FizyoterapistAd, " +
                           "Randevular.RandevuTarihi, Randevular.TedaviTipi, Randevular.Aciklama " +
                           "FROM Randevular " +
                           "INNER JOIN hastalar ON Randevular.HastaID = hastalar.ID " +
                           "INNER JOIN Fizyoterapistler ON Randevular.FizyoterapistID = Fizyoterapistler.FizyoterapistID";

            SqlDataAdapter adap = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            adap.Fill(dt);

            dgv.DataSource = dt;
            con.Close();

        }


    }


}
