using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SentSpeed.Controls
{
    /// <summary>
    /// Логика взаимодействия для UIColor.xaml
    /// </summary>
    public partial class UIColor : UserControl
    {
        public UIColor()
        {
            InitializeComponent();
            TextBox_text.Text = "31, 31, 31";
        }
        public System.Drawing.Color ColorRGB = System.Drawing.Color.FromArgb(31, 31, 31);
        int mouse_left_pos = 0;

        private void TextBox_text_TextChanged(object sender, TextChangedEventArgs e)
        {
            mouse_left_pos = TextBox_text.SelectionStart;
            TextBox_text.Text = TextBox_text.Text.Replace(" ", "").Replace("\n", "");
            TextBox_text.SelectionStart = mouse_left_pos;    
        }

        private void TextBox_text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            string text = TextBox_text.Text.Trim();
            string[] split = text.Split(new char[] { ',' });


            
            if (split.Length != 3)
                return;
            /////////////////////////
            int R = 0, G = 0, B = 0;

            if (int.TryParse(split[0], out R) && int.TryParse(split[1], out G) && int.TryParse(split[2], out B))
            {

                ColorRGB = System.Drawing.Color.FromArgb(R, G, B);
                //MessageBox.Show($"{ColorRGB}");
            }
        }
    }
}
