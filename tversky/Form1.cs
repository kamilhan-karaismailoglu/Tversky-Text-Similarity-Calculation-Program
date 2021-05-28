using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tversky
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < hesaplama_türü.Items.Count; i++) // Yanlızca bir checkboxı seçilir yapar
            {
                if (i != e.Index)
                    hesaplama_türü.SetItemChecked(i, false);
            }

            if (hesaplama_türü.SelectedIndex == 1 || hesaplama_türü.SelectedIndex == 2)  // 2. ve 3. checkbox seçili ise
            {
                textBox3.Enabled = false;   // 3. textbox kapatılır
                textBox4.Enabled = false;   // 4. textbox kapatılır
            }
            else
            {
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( hesaplama_türü.SelectedIndex == -1)     // arama türünü seçilmesini sorgular
            {
                MessageBox.Show("Lütfen Hesaplama Türünü Seçiniz");
            }
            else
            {
                if (hesaplama_türü.SelectedIndex == 1)
                {
                    textBox3.Text = 1.ToString();
                    textBox4.Text = 1.ToString();
                }
                if (hesaplama_türü.SelectedIndex == 2)
                {
                    textBox3.Text = (0.5).ToString();
                    textBox4.Text = (0.5).ToString();
                }
                if (textbox1.Text == "" || textbox2.Text == "" || textbox1.Text == null || textbox2.Text == null ||
                    textbox1.Text.Length <2 || textbox2.Text.Length < 2 ||
                    textBox3.Text == "" || textBox4.Text == "" || textBox3.Text == null || textBox4.Text == null)
                {
                    if (textbox1.Text.Length < 2 || textbox2.Text.Length < 2 )
                    {
                        MessageBox.Show("Girilen Metinler En Az 2 Karakterli Olmalıdır");
                    }
                    if (textBox3.Text == "" || textBox4.Text == "" || textBox3.Text == null || textBox4.Text == null)
                    {
                        MessageBox.Show("Lütfen Metin Ağırlıklarını Giriniz");
                    }
                }
                else
                {
                    string metin1 = textbox1.Text;
                    string metin2 = textbox2.Text;

                    List<string> bigram1 = new List<string>();  // bigramlar için liste oluşturur
                    List<string> bigram2 = new List<string>();

                    for (int i = 1; i < metin1.Length; i++)     // 1. metni bigrama çevirir ve bigram1 listesine ekler
                    {
                        if (metin1[i].ToString() != " " && bigram1.Contains(metin1[i - 1].ToString() + metin1[i].ToString()) == false)
                        {
                            bigram1.Add(metin1[i - 1].ToString() + metin1[i].ToString());
                        }
                        else if (metin1[i].ToString() == " " && bigram1.Contains("_" + metin1[i + 1].ToString()) == false && bigram1.Contains(metin1[i - 1].ToString() + "_") == false)
                        {
                            bigram1.Add(metin1[i - 1].ToString() + "_");
                            bigram1.Add("_" + metin1[i + 1].ToString());
                            i++;
                        }
                    }

                    for (int i = 1; i < metin2.Length; i++)      // 2. metni bigrama çevirir ve bigram2 listesine ekler
                    {
                        if (metin2[i].ToString() != " " && bigram2.Contains(metin2[i - 1].ToString() + metin2[i].ToString()) == false) // metin 2
                        {
                            bigram2.Add(metin2[i - 1].ToString() + metin2[i].ToString());
                        }
                        else if (metin2[i].ToString() == " " && bigram2.Contains("_" + metin2[i + 1].ToString()) == false && bigram2.Contains(metin2[i - 1].ToString() + "_") == false)
                        {
                            bigram2.Add(metin2[i - 1].ToString() + "_");
                            bigram2.Add("_" + metin2[i + 1].ToString());
                            i++;
                        }
                    }

                    List<string> kesisim = new List<string>();      // kesişim kümesi için liste oluşturur

                    foreach (var item in bigram1)   // aynı elemanları sorgular ve kesişim listesine ekler
                    {
                        foreach (var item2 in bigram2)
                        {
                            if (item == item2)
                            {
                                kesisim.Add(item);
                            }
                        }
                    }

                    var fark1 = bigram1.Except(bigram2).ToList();   // bigram1 listesinin bigram2 listesinden farkını fark1 listesine ekler
                    var fark2 = bigram2.Except(bigram1).ToList();    // bigram2 listesinin bigram1 listesinden farkını fark2 listesine ekler

                    int kesisimsayı = kesisim.Count;    // kesişim kümesinin eleman sayısı
                    int f1 = fark1.Count;               // bigram1 fark bigram2 kümesinin eleman sayısı
                    int f2 = fark2.Count;               // bigram2 fark bigram1 kümesinin eleman sayısı

                    double katsayı1;
                    double katsayı2;

                    if (hesaplama_türü.SelectedIndex == 0)     // hesaplama türü tversky seçilirse
                    {
                        katsayı1 = Convert.ToDouble(textBox3.Text);
                        katsayı2 = Convert.ToDouble(textBox4.Text);
                    }
                    else if (hesaplama_türü.SelectedIndex == 1) // hesaplama türü jaccard seçilirse
                    {
                        katsayı1 = 1;
                        katsayı2 = 1;
                    }
                    else    // hesaplama türü Sorensen-dice seçilirse
                    {
                        katsayı1 = 0.5;
                        katsayı2 = 0.5;
                    }

                    double formul = kesisimsayı / (kesisimsayı + (katsayı1 * f1) + (katsayı2 * f2));    // formülü hesaplar

                    MessageBox.Show("Girilen Metinler Arasındaki Benzerlik Oranı = " + formul.ToString());   // sonucu ekrana yazar
                }
            }
                
        }
    }
}
