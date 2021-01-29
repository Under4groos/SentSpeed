using SentSpeed.lib;
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
    /// Логика взаимодействия для UICheckBox.xaml
    /// </summary>
    public partial class UICheckBox : UserControl
    {
        public UICheckBox()
        {
            InitializeComponent();
            RegistryAutorun.AppName = "Test";
        }
        bool isActive
        {
            get; set;
        } = false;
 
        public void setText(string s)
        {
            LabelText.Content = s;
        }
        public void setColor()
        {
            if (isActive)
            {
                border.Background = Brushes.Green;
            }
            {
                border.Background = Brushes.Red;
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setColor();
        }

      
 


        private void LabelText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isActive = !isActive;
            setColor();
            MessageBox.Show($"{isActive}");
        }
    }
}
