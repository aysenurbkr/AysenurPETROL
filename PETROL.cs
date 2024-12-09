using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
 
        }

        // Form yüklendiğinde veritabanı bağlantısını kontrol et
        private void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-U1IT6GI8;Initial Catalog=petrol_uygulaması;Integrated Security=True;Encrypt=False";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Bağlantı başarılı!");
                    PersonelAdlariniGetir(connection);  // Personel isimlerini al
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı hatası: " + ex.Message);
            }
        }

        // Veritabanından personel adlarını alıp ComboBox'a ekler
        private void PersonelAdlariniGetir(SqlConnection conn)
        {
            try
            {
                string query = "SELECT AdSoyad FROM Personel";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmbPersonel.Items.Add(reader["AdSoyad"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri çekme hatası: " + ex.Message);
            }
        }

        // Personel seçildiğinde maaş ve mesai hesaplaması yapılır
        private void cmbPersonel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string secilenPersonel = cmbPersonel.SelectedItem.ToString();
            string connectionString = "Data Source=LAPTOP-U1IT6GI8;Initial Catalog=petrol_uygulaması;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    HesaplaMaaşMesai(conn, secilenPersonel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı bağlantısı hatası: " + ex.Message);
                }
            }
        }

        // Maaş ve mesai hesaplaması yapılır
        private void HesaplaMaaşMesai(SqlConnection conn, string personelAd)
        {
            try
            {
                string query = "SELECT Maas, MesaiSaatleri FROM Personel WHERE AdSoyad = @personelAd";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@personelAd", personelAd);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal maaş = Convert.ToDecimal(reader["Maas"]);
                    decimal mesaiSaat = Convert.ToDecimal(reader["MesaiSaatleri"]);

                    decimal mesaiÜcreti = 1000;  // Saatlik mesai ücreti
                    decimal toplamMaaş = maaş + (mesaiSaat * mesaiÜcreti);
                    lblSonuc.Text = "Toplam Maaş ve Mesai Ücreti: " + toplamMaaş.ToString("C2");
                }
                else
                {
                    MessageBox.Show("Veri bulunamadı!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Maaş hesaplama hatası: " + ex.Message);
            }
        }

        // Hesapla butonuna tıklanırsa, maaş ve mesai hesaplanır
        private void btnHesapla_Click(object sender, EventArgs e)
        {
            if (cmbPersonel.SelectedItem != null)
            {
                string secilenPersonel = cmbPersonel.SelectedItem.ToString();
                string connectionString = "Data Source=LAPTOP-U1IT6GI8;Initial Catalog=petrol_uygulaması;Integrated Security=True;Encrypt=False";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        HesaplaMaaşMesai(conn, secilenPersonel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanı bağlantısı hatası: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir personel seçin.");
            }
            lblSonuc.Text = "Maaş ve Mesai hesaplanıyor...";
            Console.WriteLine(lblSonuc.Text); // Konsola yazdırma

        }

        private void lblSonuc_Click(object sender, EventArgs e)
        {

        }
    }
}
