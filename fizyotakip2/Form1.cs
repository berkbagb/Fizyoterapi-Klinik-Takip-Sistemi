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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace fizyotakip2
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string id = "-1";
        //SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog = klinik; Integrated Security=true;");
        //SqlCommand cmd;
        //SqlDataAdapter adap;

        SqlConnection con = Shared.getConnection();

        public void comboboxfill()
        {
            Shared.ComboboxFill(comboBox1, "hastalar");
            Shared.ComboboxFill(comboBox2, "Fizyoterapistler");
            Shared.ComboboxFill(cmbHasta, "hastalar");
            //Shared.ComboboxFill(cmbFizyoterapist, "Fizyoterapisler");
            Shared.ComboboxFill(cmbFizyoterapist, "Fizyoterapistler");
            Shared.ComboboxFill(cmbtedavituru, "tedavitürleri");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboboxfill();
            //Shared.RandevulariGetir(dataGridView2);
            Shared.UpdateDataGridView(dataGridView1, "hastalar");
            Shared.UpdateDataGridView(dataGridView2, "Randevular");
            Shared.UpdateDataGridView(dataGridView3, "Tedaviler");

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


        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
            comboBox1.Text = row.Cells["HastaName"].Value.ToString();
            comboBox2.Text = row.Cells["FizyoterapistName"].Value.ToString();
            dateTimePicker2.Value = (DateTime)row.Cells["RandevuTarihi"].Value;
            textBox4.Text = row.Cells["TedaviTipi"].Value.ToString();
            richTextBox1.Text = row.Cells["Aciklama"].Value.ToString() ;
            /*

            textBox1.Text = row.Cells["İsim"].Value.ToString();
            textBox2.Text = row.Cells["Soyisim"].Value.ToString();
            textBox3.Text = row.Cells["Telefon"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(row.Cells["Dogum"].Value.ToString());
            metroComboBox1.Text = row.Cells["Cinsiyet"].Value.ToString();*/

            id = row.Cells["RandevuID"].Value.ToString();
        }

        public string tedaviID = "-1";
        private void dataGridView3_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            cmbHasta.Text = "";
            cmbFizyoterapist.Text = "";
            cmbtedavituru.Text = "";
            /*
            cmbHasta.Text = "";
            cmbFizyoterapist.Text = "";
            cmbtedavituru.Text = "";
            DataGridViewRow row = dataGridView3.Rows[e.RowIndex];
            string hastanem = row.Cells["HastaName"].Value.ToString();
            cmbHasta.SelectedText = hastanem;

            cmbFizyoterapist.SelectedText = row.Cells["FizyoterapistName"].Value.ToString();
            cmbtedavituru.SelectedText = row.Cells["TedaviTuru"].Value.ToString();
            decimal miktar;
            if (decimal.TryParse(row.Cells["SeansSayisi"].Value.ToString(), out miktar))
            {
                // NumericUpDown kontrolünün değerini ayarla
                seanscounter.Value = miktar;
            }
            DateTime tarih;
            if (DateTime.TryParse(row.Cells["BaslangicTarihi"].Value.ToString(), out tarih))
            {
                // DateTimePicker kontrolünün değerini ayarla
                dateTimePicker3.Value = tarih;
            }
            DateTime btarih;
            if (DateTime.TryParse(row.Cells["BitisTarihi"].Value.ToString(), out btarih))
            {
                // DateTimePicker kontrolünün değerini ayarla
                dateTimePicker4.Value = btarih;
            }
            textBox5.Text = row.Cells["Aciklama"].Value.ToString();
            tedaviID = row.Cells["TedaviID"].Value.ToString();
            Console.WriteLine("jashdasjdhasdjh");*/
            int rowIndex = e.RowIndex;

            // Satırın geçerli olup olmadığını kontrol et
            if (rowIndex >= 0)
            {
                // Seçilen satırdaki hücrelerden verileri al
                int selectedHastaID = (int)dataGridView3.Rows[rowIndex].Cells["HastaID"].Value; // HastaID sütun adı
                string selectedHastaName = dataGridView3.Rows[rowIndex].Cells["HastaName"].Value.ToString(); // HastaName sütun adı
                string fizyoterapistName = dataGridView3.Rows[rowIndex].Cells["FizyoterapistName"].Value.ToString(); // FizyoterapistName sütun adı
                string selectedTedaviAdi = dataGridView3.Rows[rowIndex].Cells["TedaviTuru"].Value.ToString(); // TedaviTuru sütun adı
                int seansSayisi = (int)dataGridView3.Rows[rowIndex].Cells["SeansSayisi"].Value; // SeansSayisi sütun adı
                DateTime baslangicTarihi = (DateTime)dataGridView3.Rows[rowIndex].Cells["BaslangicTarihi"].Value; // BaslangicTarihi sütun adı
                DateTime bitisTarihi = (DateTime)dataGridView3.Rows[rowIndex].Cells["BitisTarihi"].Value; // BitisTarihi sütun adı
                string aciklama = dataGridView3.Rows[rowIndex].Cells["Aciklama"].Value.ToString(); // Aciklama sütun adı

                // Verileri ilgili kontrollerde göster
                cmbHasta.SelectedItem = new KeyValuePair<int, string>(selectedHastaID, selectedHastaName);
                cmbFizyoterapist.SelectedText = dataGridView3.Rows[rowIndex].Cells["FizyoterapistName"].Value.ToString();
                cmbtedavituru.SelectedItem = selectedTedaviAdi; // Eğer KeyValuePair değilse, doğrudan atama yapabilirsiniz
                seanscounter.Value = seansSayisi;
                dateTimePicker3.Value = baslangicTarihi;
                dateTimePicker4.Value = bitisTarihi;
                textBox5.Text = aciklama;
                tedaviID = dataGridView3.Rows[rowIndex].Cells["TedaviID"].Value.ToString();
            }
        }
        

        //hasta kayıt sekmesi başı

        private void kayitEkle_Click(object sender, EventArgs e)
        {
            //hasta kayıt
            string name = textBox1.Text;
            string surname = textBox2.Text;
            string phone = textBox3.Text;
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string age = metroComboBox1.Text;
            bool check = Shared.TumAlanlarDolu(name, surname, phone, date, age);
            if (check)
            {
                // Tüm alanlar dolu
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO hastalar(İsim, Soyisim, Telefon, Dogum, Cinsiyet) VALUES (@p1, @p2, @p3, @p4, @p5)", con);
                cmd.Parameters.AddWithValue("@p1", name);
                cmd.Parameters.AddWithValue("@p2", surname);
                cmd.Parameters.AddWithValue("@p3", phone);
                cmd.Parameters.AddWithValue("@p4", date);
                cmd.Parameters.AddWithValue("@p5", age);
                cmd.ExecuteNonQuery();
                con.Close();
                Shared.UpdateDataGridView(dataGridView1, "hastalar");
                MessageBox.Show("Hasta kayıt başarıyla tamamlandı");
                comboboxfill();

            }
            else
            {
                MessageBox.Show("Tüm alanlar doldurulmalıdır!");
            }


        }

        private void updatebutton_Click(object sender, EventArgs e)
        {
            //Hasta kaydı güncelleme

            string name = textBox1.Text;
            string surname = textBox2.Text;
            string phone = textBox3.Text;
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string age = metroComboBox1.Text;
            bool check = Shared.TumAlanlarDolu(name, surname, phone, date, age);
            if (check)
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE hastalar SET İsim = @name, Soyisim = @surname, Telefon = @phone, Dogum = @date, Cinsiyet = @age WHERE ID = @id", con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                con.Close();
                Shared.UpdateDataGridView(dataGridView1, "hastalar");
                MessageBox.Show("" + name + " " + surname + ", isimli hastanın Hasta Kaydı başarıyla güncellendi!");
                comboboxfill();
            }
            else
                MessageBox.Show("Tüm alanlar doldurulmalıdır!");
        }
       


        private void deletebutton_Click(object sender, EventArgs e)
        {
            //hasta kaydı silme
            if (id != "-1") //eğer hasta id -1 değilse bir hasta seçilmiş demektir
            {

                DialogResult dr = MessageBox.Show("" + textBox1.Text + ", isimli hastaya dair bütün kayıtlar silinecektir bu işlemi onaylıyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                {
                    return; //eğer eveti seçmezse işlem iptali
                }

                con.Open();

                    SqlCommand cmd = new SqlCommand("BEGIN TRANSACTION;" +
                        "DELETE FROM tedaviler WHERE HastaID = @p1;" +
                        "DELETE FROM Randevular WHERE HastaID = @p1;" +
                        "DELETE FROM hastalar WHERE id = @p1;" +
                        "COMMIT;", con
                        );

                    cmd.Parameters.AddWithValue("@p1", id);

                    cmd.ExecuteNonQuery();

                con.Close();
                comboboxfill();
                MessageBox.Show("İlgili hastaya dair tüm kayıtlar silinmiştir!");
                Shared.UpdateDataGridView(dataGridView1, "hastalar");
            }
        }
        //hasta kayıt sekmesi sonu

        private void randevuaddbtn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null) 
            {
                MessageBox.Show("Hasta/Fizyoterapist seçimleri boş bırakılamaz!");
                return;
            }

            int hastaID = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
            string fizyoname = ((KeyValuePair<int, string>)comboBox2.SelectedItem).Value;
            int fizyoID = ((KeyValuePair<int, string>)comboBox2.SelectedItem).Key;
            string tarih = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string tur = textBox4.Text;
            string reason = richTextBox1.Text;
            string hastaName = ( (KeyValuePair<int, string>)comboBox1.SelectedItem ).Value;

            bool check = Shared.TumAlanlarDolu(tur, reason);
            if (check)
            {
                //eğer bütün her şey doluysa bura çalışacak(comboboxlardan seçim yapıldıysa, tarih, tedavi türü ve açıklamalar da girildiyse)
                con.Open();
                /*
                 1: hastaid
                2: fizyoid
                3: fizyoname
                4: hastaname
                5: tarih
                6: tedavitip
                7:aciklama
                 */

                SqlCommand cmd = new SqlCommand("INSERT INTO randevular(HastaID, FizyoterapistID, FizyoterapistName, HastaName, RandevuTarihi, TedaviTipi, Aciklama) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7)", con);
                cmd.Parameters.AddWithValue("@p1", hastaID);
                cmd.Parameters.AddWithValue("@p2", fizyoID);//id yerine name eklicek düzelt
                cmd.Parameters.AddWithValue("@p3", fizyoname);
                cmd.Parameters.AddWithValue("@p4", hastaName);
                cmd.Parameters.AddWithValue("@p5", tarih);
                cmd.Parameters.AddWithValue("@p6", tur);
                cmd.Parameters.AddWithValue("@p7", reason);
                cmd.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("INSERT INTO tedavitürleri (TedaviAdi) VALUES(@p1)", con);
                cmd2.Parameters.AddWithValue("@p1", tur);
                cmd2.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("Randevu başarıyla oluşturuldu!");
                //Shared.RandevulariGetir(dataGridView2);
                Shared.UpdateDataGridView(dataGridView2, "Randevular");
                comboboxfill();


            }
            else
            {
                MessageBox.Show("lütfen tüm alanları doldurunuz");
            }



        }

        private void RandevuDeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0) 
            {

                int rID = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["RandevuID"].Value);

                con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Randevular WHERE RandevuID = @p1", con);
                cmd.Parameters.AddWithValue("@p1", rID);
                cmd.ExecuteNonQuery();

                con.Close();
                Shared.RandevulariGetir(dataGridView2);
                MessageBox.Show("Randevu Silme işlemi başarıyla tamamlandı!");
                comboboxfill();


            }
        }
                    //randevu bitişi\\

        //------------------------------------------------\\

                    // tedavi başlangıç \\
        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {//tedavi ekle

            int selectedHastaID = ((KeyValuePair<int, string>)cmbHasta.SelectedItem).Key;
            string selectedHastaName = ((KeyValuePair<int, string>)cmbHasta.SelectedItem).Value;
            string FizyoterapistName = ( (KeyValuePair<int, string>)cmbFizyoterapist.SelectedItem ).Value;
            var tedavi = (KeyValuePair<int, string>)cmbtedavituru.SelectedItem;
            string selectedTedaviAdi = tedavi.Value;
            int seans = Convert.ToInt32(seanscounter.Value) ;
            DateTime baslangic = dateTimePicker3.Value;
            DateTime bitis = dateTimePicker4.Value;
            string aciklama = textBox5.Text;

            
            if (selectedHastaID != -1) //eğer hasta id -1 değilse yani seçim yapıldıysa
            {
                if (FizyoterapistName != "") //eğer fizyoterapist adı varsa
                {
                    if (cmbtedavituru.Text != "") //eğer tedavi seçildiyse
                    {
                        if (seans != 0) //eğer seans sayısı sıfırdan büyükse
                        { 
                        
                            if (baslangic != bitis && bitis > baslangic) //eğer baslangiç ve bitis esit değilse ve de bitis baslangiçtan büyükse
                            {
                                if (aciklama != "")//eğer açıklama doluysa
                                {
                                    //her şey tam tedavi eklenebilir burda

                                    string query = "INSERT INTO Tedaviler(HastaID, HastaName, FizyoterapistName, TedaviTuru, SeansSayisi, BaslangicTarihi, BitisTarihi, Aciklama) VALUES(@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)";
                                    con.Open();

                                    SqlCommand cmd = new SqlCommand(query, con);
                                    cmd.Parameters.AddWithValue("@p1", selectedHastaID);
                                    cmd.Parameters.AddWithValue("@p2", selectedHastaName);
                                    cmd.Parameters.AddWithValue("@p3", FizyoterapistName);
                                    cmd.Parameters.AddWithValue("@p4", selectedTedaviAdi);
                                    cmd.Parameters.AddWithValue("@p5", seans);
                                    cmd.Parameters.AddWithValue("@p6", baslangic);
                                    cmd.Parameters.AddWithValue("@p7", bitis);
                                    cmd.Parameters.AddWithValue("@p8", aciklama);
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("" + selectedHastaName + ", adlı kişinin tedavi kaydı başarıyla tamamlanmıştır!");


                                    con.Close();
                                    Shared.UpdateDataGridView(dataGridView3, "Tedaviler");
                                    comboboxfill();



                                }
                                else
                                { MessageBox.Show("Lütfen bir açıklama giriniz"); return; }
                            }
                            else
                            { MessageBox.Show("Bitiş tarihi, başlangıç tarihinden önce veya eşit olamaz!"); return; }

                        }
                        else
                        { MessageBox.Show("Seans sayısı sıfıra eşit olamaz"); return; }
                    }
                    else
                    { MessageBox.Show("Bir tedavi türü seçiniz!"); return; }
                }
                else
                { MessageBox.Show("Lütfen listeden bir fizyoterapist seçiniz!"); return; }
            }
            else
            { MessageBox.Show("Lütfen listeden bir hasta seçiniz!"); return; }


        }


        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            //tedavi güncelle



            int selectedHastaID = ((KeyValuePair<int, string>)cmbHasta.SelectedItem).Key;
            string selectedHastaName = ((KeyValuePair<int, string>)cmbHasta.SelectedItem).Value;
            string FizyoterapistName = cmbFizyoterapist.Text; //((KeyValuePair<int, string>)cmbFizyoterapist.SelectedItem).Value;
            var tedavi = (KeyValuePair<int, string>)cmbtedavituru.SelectedItem;
            string selectedTedaviAdi = tedavi.Value;
            int seans = Convert.ToInt32(seanscounter.Value);
            DateTime baslangic = dateTimePicker3.Value;
            DateTime bitis = dateTimePicker4.Value;
            string aciklama = textBox5.Text;
            bool check = Shared.TumAlanlarDolu(selectedHastaName, FizyoterapistName, selectedTedaviAdi);
            if (check)
            {
                con.Open();
                string query = "UPDATE Tedaviler SET HastaName=@p1, FizyoterapistName=@p2, TedaviTuru=@p3, SeansSayisi=@p4, BaslangicTarihi = @p5, BitisTarihi =@p6, Aciklama = @p7  WHERE TedaviID = @tedaviid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@p1", selectedHastaName);
                cmd.Parameters.AddWithValue("@p2", FizyoterapistName);
                cmd.Parameters.AddWithValue("@p3", selectedTedaviAdi);
                cmd.Parameters.AddWithValue("@p4", seans);
                cmd.Parameters.AddWithValue("@p5", baslangic);
                cmd.Parameters.AddWithValue("@p6", bitis);
                cmd.Parameters.AddWithValue("@p7", aciklama);
                cmd.Parameters.AddWithValue("@tedaviid", tedaviID);

                cmd.ExecuteNonQuery();
                con.Close();
                Shared.UpdateDataGridView(dataGridView3, "Tedaviler");
                MessageBox.Show("" + selectedHastaName + ", isimli hastanın Randevu Kaydı başarıyla güncellendi!");
                comboboxfill();
            }



        }

        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            string selectedHastaName = ((KeyValuePair<int, string>)cmbHasta.SelectedItem).Value;
            //tedavi sil
            if (tedaviID != "-1") 
            {
                

                
                
                    con.Open();

                    SqlCommand cmd = new SqlCommand("BEGIN TRANSACTION;" +
                        "DELETE FROM tedaviler WHERE TedaviID = @p1;" +
                        "DELETE FROM Randevular WHERE HastaID = @p1;" +
                        
                        "COMMIT;", con
                        );

                    cmd.Parameters.AddWithValue("@p1", tedaviID);

                    cmd.ExecuteNonQuery();

                    con.Close();
                MessageBox.Show("" + selectedHastaName + ", isimli hastanın Randevu Kaydı başarıyla güncellendi!");
                comboboxfill();
                Shared.UpdateDataGridView(dataGridView3, "Tedaviler");

                

            }
            /*
            
            if (id != "-1") //eğer hasta id -1 değilse bir hasta seçilmiş demektir
            {

                DialogResult dr = MessageBox.Show("" + textBox1.Text + ", isimli hastaya dair bütün kayıtlar silinecektir bu işlemi onaylıyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                {
                    return; //eğer eveti seçmezse işlem iptali
                }

                con.Open();

                    SqlCommand cmd = new SqlCommand("BEGIN TRANSACTION;" +
                        "DELETE FROM tedaviler WHERE HastaID = @p1;" +
                        "DELETE FROM Randevular WHERE HastaID = @p1;" +
                        "DELETE FROM hastalar WHERE id = @p1;" +
                        "COMMIT;", con
                        );

                    cmd.Parameters.AddWithValue("@p1", id);

                    cmd.ExecuteNonQuery();

                con.Close();

                MessageBox.Show("İlgili hastaya dair tüm kayıtlar silinmiştir!");
                Shared.UpdateDataGridView(dataGridView1, "hastalar");
            }

             */

        }
    }
}
