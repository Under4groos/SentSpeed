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
        bool StatusTimer = true;

        public bool IsClosed
        {
            get; private set;
        }
        bool isActive_autorun
        {
            get; set;
        } = false;

        public MainWindow()
        {

            InitializeComponent();

            this.Title = "";
            this.Hide();            
            this.Icon = IBintmIco.ToImageSource(Properties.Resource.ico);

            windinfo.IsActiveWind = true;

            timerTick.Tick += TimerTick_Tick;
            timerTick.Time = 1000; // 1 сек
            timerTick2.Tick += TimerTick2_Tick;                     

            notify.Icon = Properties.Resource.ico;
            notify.Visible = true;
            notify.Click += Notify_Click;

            for (int i = 0; i < network.NetworkInterfaces.Length; i++)
                cb.Items.Add(network.NetworkInterfaces[i].Name);
            cb.SelectedIndex = 1;

            TimerSet(StatusTimer);
            //if (RegistryAutorun.IsAdm())
            if (RegistryAutorun.isItem())
            {
                isActive_autorun = true;
                setStatusAutorun();
            }


        }
        public void TimerSet(bool b)
        {
            switch (b)
            {
                case true:
                    timerTick.Start();
                    timerTick2.Start();
                    Grid1.Visibility = Visibility.Visible;
                    Grid2.Visibility = Visibility.Hidden;
                    break;
                case false:

                    timerTick.Stop();
                    timerTick2.Stop();
                    Grid1.Visibility = Visibility.Hidden;
                    Grid2.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void Notify_Click(object sender, EventArgs e)
        {
            if (IsClosed)
                return;
            switch (this.Visibility)
            {
                case Visibility.Visible:
                    this.Hide();
                    break;
                case Visibility.Hidden:
                    this.Show();
                    break;
                case Visibility.Collapsed:
                    break;
                default:
                    this.Show();
                    break;
            }
            
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
                //string s = $"Upload:{network.GetUploadSpeed()}  Download:{network.GetDownloadSpeed()}";
                          
                if (this.Visibility == Visibility.Visible)
                {
                    //string s_1 = "Sent: " + network.ConvertTo(network.GetSentAndReceivedbytes().Item1, debug_mode);
                    //string s_2 = "Received: " + network.ConvertTo(network.GetSentAndReceivedbytes().Item2, debug_mode);
                    //Debug.Content = $"{s_1} {s_2}";

                    Debug.Content = network.GetbytesSentAndReceived(debug_mode);
                    Debug2.Content = network.GetUploadAndDownloadSpeed("\n");
                    Debug3.Content = network.GetIncomingPacketsWithErrors();
                }
                notify.Text = network.GetUploadAndDownloadSpeed();
                if (windinfo.IsClosed == false)
                {
                    windinfo.SetText(network.GetUploadAndDownloadSpeed(" "));
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
            if(e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            
        }

        private void Debug_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            debug_mode++;
            //string s_1 = network.ConvertTo(network.GetSentAndReceivedbytes().Item1, debug_mode);
            //string s_2 = network.ConvertTo(network.GetSentAndReceivedbytes().Item2, debug_mode);
            //Debug.Content = $"Sent: {s_1} Received: {s_2}";

            Debug.Content = network.GetbytesSentAndReceived(debug_mode);
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

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            StatusTimer = !StatusTimer;
            TimerSet(StatusTimer);
        }

        private void AutorunCh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isActive_autorun = !isActive_autorun;
            setStatusAutorun();
        }

        public void setStatusAutorun()
        {
            switch (isActive_autorun)
            {
                case true:
                    AutorunCh.Background = System.Windows.Media.Brushes.Green;
                    RegistryAutorun.add();
                    break;
                case false:
                    AutorunCh.Background = System.Windows.Media.Brushes.Red;
                    RegistryAutorun.remove();
                    break;
                default:
                    break;
            }
            AutorunCh.Content = $"Autorun: {isActive_autorun}";
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            Process.GetCurrentProcess().Kill();
        }
    }
}
