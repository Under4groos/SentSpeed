using MainWindow;
using SentSpeed.lib;
using SentSpeed.windows;
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
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace SentSpeed
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly TimerTick timerTick = new TimerTick();
        readonly TimerTick timerTick2 = new TimerTick();
        readonly Network network = new Network();      
        readonly Windinfo windinfo = new Windinfo();
        readonly NotifyIcon notify = new NotifyIcon();


        int debug_mode = 0;
        int count_error_ = 0;
        bool isOpenPu = false;
        public bool IsClosed
        {
            get; private set;
        }


        public MainWindow()
        {

            InitializeComponent();
            this.Title = "";
            this.Hide();
            windinfo.IsActiveWind = true;

            timerTick.Tick += TimerTick_Tick;
            timerTick.Time = 1000;
            
            timerTick.Start();

            timerTick2.Tick += TimerTick2_Tick;                     
            timerTick2.Start();

            notify.Icon = Properties.Resource.ico;
            notify.Visible = true;
            notify.Click += Notify_Click;

            for (int i = 0; i < network.NetworkInterfaces.Length; i++)
                cb.Items.Add(network.NetworkInterfaces[i].Name);
            cb.SelectedIndex = 1;
        }

        private void Notify_Click(object sender, EventArgs e)
        {
            if (IsClosed)
                return;

            this.Show();
        }

        private void TimerTick2_Tick(object sender, EventArgs e)
        {
            try
            {
                
                isOpenPu = ColorUnderCursor.Is(ColorUnderCursor.Get(2, 1075), ColorRGBbox.ColorRGB) && ColorUnderCursor.Is(ColorUnderCursor.Get(2, 853), ColorRGBbox.ColorRGB);
                
                
                int size_h = 0;
                if (isOpenPu)
                {
                    size_h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

                    windinfo.SetPos(
                        (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - windinfo.Width),
                        (int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - (size_h + windinfo.Height))
                        );

                    windinfo.Show();
                    windinfo.isVis = true;
                }
                else
                {
                    windinfo.Hide();
                    windinfo.isVis = false;
                }
                if (windinfo.IsActiveWind)
                    windinfo.Topmost = (bool)CBTopMost.IsChecked;
            }
            catch (Exception)
            {
                Err();
            }
            
        }
        public void Err()
        {
            count_error_++;
            if (count_error_ == 0)
                return;
            Cerr.Content = $"Errors: {count_error_}";
        }
      

        private void TimerTick_Tick(object sender, EventArgs e)
        {
            try
            {
                network.Update();
                string s = $"Upload:{network.GetUploadSpeed()}  Download:{network.GetDownloadSpeed()}";
                if (this.Visibility == Visibility.Visible)
                {
                    string s_1 = "Sent: " + network.ConvertTo(network.GetSentAndReceivedbytes().Item1, debug_mode);
                    string s_2 = "Received: " + network.ConvertTo(network.GetSentAndReceivedbytes().Item2, debug_mode);
                    Debug.Content = $"{s_1} {s_2}";
                    Debug2.Content = s;
                }
                notify.Text = $"{s}";
                if (windinfo.IsClosed == false)
                {
                    windinfo.SetText(s);
                }
            }
            catch (Exception)
            {
                Err();
            }
            

        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            network.IDInterfaces = cb.SelectedIndex;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            
        }

        private void Debug_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            debug_mode++;
            string s_1 = network.ConvertTo(network.GetSentAndReceivedbytes().Item1, debug_mode);
            string s_2 = network.ConvertTo(network.GetSentAndReceivedbytes().Item2, debug_mode);
            Debug.Content = $"Sent: {s_1} Received: {s_2}";
            if (debug_mode > 3)
                debug_mode = 0;
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsClosed = false;
        }
    }
}
