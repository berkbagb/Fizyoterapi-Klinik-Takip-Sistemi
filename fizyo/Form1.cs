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
using System.Xml.Linq;

namespace fizyo
{
    public partial class Form1 : Form
    {

       
        public Form1()
        {
            InitializeComponent();



        }

        public string id = "-1";
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = klinik; Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adap;
        
        private void addButton_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string surname = textBox2.Text;
            string phone = textBox3.Text;
            string dogum = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string age = metroComboBox1.Text;

            if (name.Length > 0 && surname.Length > 0 && phone.Length > 0 && dogum.Length > 0) 
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO hastalar(İsim, Soyisim, Telefon, Dogum, Cinsiyet) VALUES (@p1, @p2, @p3, @p4, @p5)", con);
                cmd.Parameters.AddWithValue("@p1", name);
                cmd.Parameters.AddWithValue("@p2", surname);
                cmd.Parameters.AddWithValue("@p3", phone);
                cmd.Parameters.AddWithValue("@p4", dogum);
                cmd.Parameters.AddWithValue("@p5", age);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Hasta başarıyla veritabanına eklendi.");
                data();
            }


        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            dataGridView1.Height = (this.Height / 2);
        }

        private void data()
        {
            adap = new SqlDataAdapter("Select * From hastalar", con);
            DataTable dt = new DataTable();
            con.Open();
            adap.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            data();
            fizyoGetir(comboBox2);
            fizyoGetir(cmbFizyoterapist);
            RandevulariGetir();
            HastalariGetir(comboBox1);
            HastalariGetir(cmbHasta);
            tedavigetir();
            TedavileriYukle();
        }

        private void updatebutton_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string surname = textBox2.Text;
            string phone = textBox3.Text;
            string dogum = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string age = metroComboBox1.Text;

            if (name.Length > 0 && surname.Length > 0 && phone.Length > 0 && dogum.Length > 0)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Update hastalar set İsim = @p1, Soyisim = @p2, Telefon = @p3, Dogum = @p4, Cinsiyet = @p5", con);
                cmd.Parameters.AddWithValue("@p1", name);
                cmd.Parameters.AddWithValue("@p2", surname);
                cmd.Parameters.AddWithValue("@p3", phone);
                cmd.Parameters.AddWithValue("@p4", dogum);
                cmd.Parameters.AddWithValue("@p5", age);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Hasta bilgileri güncellendi!");
                data();
            }
        }

        private void deletebutton_Click(object sender, EventArgs e)
        {
            if (id != "-1")
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM hastalar WHERE id = @p1", con);
                cmd.Parameters.AddWithValue("@p1", id);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Kayıt başarıyla silindi!");
                data();
            }
        }
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            textBox1.Text = row.Cells["İsim"].Value.ToString();
            textBox2.Text = row.Cells["Soyisim"].Value.ToString();
            textBox3.Text = row.Cells["Telefon"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(row.Cells["Dogum"].Value.ToString());
            metroComboBox1.Text = row.Cells["Cinsiyet"].Value.ToString();
            id = row.Cells["id"].Value.ToString();
           
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void HastalariGetir(ComboBox cmb)
        {


            con.Open();
            string query = "SELECT ID, İsim + ' ' + Soyisim AS HastaAd FROM hastalar";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                cmb.Items.Add(new KeyValuePair<int, string>((int)dr["ID"], dr["HastaAd"].ToString()));
            }

            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Key";
            con.Close();

        }

        private void tedavigetir()
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

            cmbtedavituru.DataSource = new BindingSource(tedaviler, null);

            cmbtedavituru.DisplayMember = "Value";
            cmbtedavituru.ValueMember = "Key";


            con.Close();


        }
        private void fizyoGetir(ComboBox cmb)
        {


            con.Open();
            string query = "SELECT FizyoterapistID, Ad + ' ' + Soyad AS FizyoterapistAd FROM Fizyoterapistler";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                cmb.Items.Add(new KeyValuePair<int, string>((int)dr["FizyoterapistID"], dr["FizyoterapistAd"].ToString()));
            }
            con.Close();
            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Key";

        }

        private void RandevulariGetir()
        {
            
                con.Open();
                string query = "SELECT Randevular.RandevuID, hastalar.İsim + ' ' + hastalar.Soyisim AS HastaAd, " +
                               "Fizyoterapistler.Ad + ' ' + Fizyoterapistler.Soyad AS FizyoterapistAd, " +
                               "Randevular.RandevuTarihi, Randevular.TedaviTipi, Randevular.Aciklama " +
                               "FROM Randevular " +
                               "INNER JOIN hastalar ON Randevular.HastaID = hastalar.ID " +
                               "INNER JOIN Fizyoterapistler ON Randevular.FizyoterapistID = Fizyoterapistler.FizyoterapistID";

                SqlDataAdapter adap = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adap.Fill(dt);

                dataGridView2.DataSource = dt;
            con.Close();
            
        }

        private void randevuaddbtn_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Lütfen hasta ve fizyoterapist seçiniz!");
                return;
            }

            int hastaID = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
            int fizyoID = ((KeyValuePair<int, string>)comboBox2.SelectedItem).Key;
            string tür = textBox4.Text;
            string reason = richTextBox1.Text;
            string tarih = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            con.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO Randevular (HastaID, FizyoterapistID, RandevuTarihi, TedaviTipi, Aciklama) VALUES(@p1, @p2, @p3, @p4,@p5)", con);
            cmd.Parameters.AddWithValue("@p1", hastaID);
            cmd.Parameters.AddWithValue("@p2", fizyoID);
            cmd.Parameters.AddWithValue("@p3", tarih);
            cmd.Parameters.AddWithValue("@p4", tür);
            cmd.Parameters.AddWithValue("@p5", reason);
            cmd.ExecuteNonQuery();

            SqlCommand cmd2 = new SqlCommand("INSERT INTO tedavitürleri (TedaviAdi) VALUES(@p1)", con);
            cmd2.Parameters.AddWithValue("@p1", tür);

            cmd2.ExecuteNonQuery();
            MessageBox.Show("Randevu başarıyla oluşturuldu!");
            con.Close();
            RandevulariGetir();

        }

        private void RandevuDeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {

                int randevuID = Convert.ToInt32( dataGridView2.SelectedRows[0].Cells["RandevuID"].Value);

                con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Randevular WHERE RandevuID = @p1", con);
                cmd.Parameters.AddWithValue("@p1", randevuID);
                cmd.ExecuteNonQuery();
                con.Close();
                RandevulariGetir();
                MessageBox.Show("Randevu silme işlemi başarıyla tamamlandı!");

            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }


       

        //tedavi kısmının comboboxları tamamlandı sadece geriye serverside işlemler kaldı 

        private void materialRaisedButton1_Click(object sender, EventArgs e) //tedavi ekle
        {

            //bütün componentslerden veriyi çekip sql e işlicen


            int secilihastaID = ((KeyValuePair<int, string>)cmbHasta.SelectedItem).Key;
            int secilifizyoterapistID = ((KeyValuePair<int, string>)cmbFizyoterapist.SelectedItem).Key;
            string secilitedavi = cmbtedavituru.SelectedItem.ToString();
            int seanssayisi = Convert.ToInt32( seanscounter.Value);
            DateTime baslangic = dateTimePicker3.Value;
            DateTime bitis = dateTimePicker4.Value;
            string tedavireason = textBox5.Text;

            if (secilihastaID > 0)
            {

                if (secilifizyoterapistID > 0)
                {
                    if (secilitedavi != "")
                    {
                        if (seanssayisi > 0)
                        {

                            if (baslangic != bitis && bitis > baslangic)
                            {
                                if (tedavireason != "")
                                {
                                    //işlemlerin asıl başladığı mevzu

                                    string query = "INSERT INTO Tedaviler(HastaID, FizyoterapistID, TedaviTuru, SeansSayisi, BaslangicTarihi, BitisTarihi, Aciklama) VALUES(@p1, @p2, @p3, @p4, @p5, @p6, @p7)";
                                    con.Open();

                                    SqlCommand cmd = new SqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@p1", secilihastaID);
                                    cmd.Parameters.AddWithValue("@p2", secilifizyoterapistID);
                                    cmd.Parameters.AddWithValue("@p3", secilitedavi);
                                    cmd.Parameters.AddWithValue("@p4", seanssayisi);
                                    cmd.Parameters.AddWithValue("@p5", baslangic);
                                    cmd.Parameters.AddWithValue("@p6", bitis);
                                    cmd.Parameters.AddWithValue("@p7", tedavireason);

                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Tedavi başarıyla eklendi");

                                    con.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Lütfen bir açıklama giriniz!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Bitiş tarihi, başlangıç tarihinden önce veya eşit olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }
                        else
                        { MessageBox.Show("Seans sayısı sıfır olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }


                    }
                    else { MessageBox.Show("Bir tedavi türü seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
                else { MessageBox.Show("Bir fizyoterapist seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            }
            else { MessageBox.Show("Listeden bir hasta seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
           
        }
        /*
        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {

            int secilihastaID = ((KeyValuePair<int, string>)cmbHasta.SelectedItem).Key;
            int secilifizyoterapistID = ((KeyValuePair<int, string>)cmbFizyoterapist.SelectedItem).Key;
            string secilitedavi = cmbtedavituru.SelectedItem.ToString();
            int seanssayisi = Convert.ToInt32(seanscounter.Value);
            DateTime baslangic = dateTimePicker3.Value;
            DateTime bitis = dateTimePicker4.Value;
            string tedavireason = textBox5.Text;

            if (secilihastaID > 0)
            {

                if (secilifizyoterapistID > 0)
                {
                    if (secilitedavi != "")
                    {
                        if (seanssayisi > 0)
                        {

                            if (baslangic != bitis && bitis < baslangic)
                            {
                                if (tedavireason != "")
                                {
                                    //işlemlerin asıl başladığı mevzu

                                    string query = "UPDATE Tedaviler HastaID=@p1, FizyoterapistID = @p2, TedaviTuru = @p3, SeansSayisi = @p4, BaslangicTarihi = @p5, BitisTarihi = @p6, Aciklama = @p7 WHERE TedaviID = @p8";
                                    con.Open();

                                    SqlCommand cmd = new SqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@p1", secilihastaID);
                                    cmd.Parameters.AddWithValue("@p2", secilifizyoterapistID);
                                    cmd.Parameters.AddWithValue("@p3", secilitedavi);
                                    cmd.Parameters.AddWithValue("@p4", seanssayisi);
                                    cmd.Parameters.AddWithValue("@p5", baslangic);
                                    cmd.Parameters.AddWithValue("@p6", bitis);
                                    cmd.Parameters.AddWithValue("@p7", tedavireason);
                                    cmd.Parameters.AddWithValue("@p8", tedavireason);

                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Tedavi başarıyla eklendi");

                                    con.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Lütfen bir açıklama giriniz!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Bitiş tarihi, başlangıç tarihinden önce veya eşit olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }
                        else
                        { MessageBox.Show("Seans sayısı sıfır olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }


                    }
                    else { MessageBox.Show("Bir tedavi türü seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
                else { MessageBox.Show("Bir fizyoterapist seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

            }
            else { MessageBox.Show("Listeden bir hasta seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
        */
        private void TedavileriYukle()
        {
          
                con.Open();

            string query = "SELECT T.TedaviID, H.İsim + ' ' + H.Soyisim AS HastaAdi, F.Ad, ' ' + F.Soyad AS FizyoterapistAdi, T.TedaviTuru, T.SeansSayisi, T.BaslangicTarihi, T.BitisTarihi, T.Aciklama FROM Tedaviler T INNER JOIN hastalar H ON T.HastaID = H.ID INNER JOIN Fizyoterapistler F ON T.FizyoterapistID = F.FizyoterapistID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView3.DataSource = dt;
            con.Close();
            
        }
    }
}
