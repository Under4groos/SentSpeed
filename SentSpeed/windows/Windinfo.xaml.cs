using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SentSpeed.windows
{
    /// <summary>
    /// Логика взаимодействия для Windinfo.xaml
    /// </summary>
    public partial class Windinfo : Window
    {
        #region Dll Used
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);
        #endregion
        #region 

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private bool IsActiveWind_ = true;
        public bool IsActiveWind
        {
            get
            {
                return IsActiveWind_;
            }
            set
            {
                IsActiveWind_ = value;
                if (IsActiveWind_)
                {
                    this.Show();
                }
                {
                    this.Hide();
                }
            }
        }
        
        public bool IsClosed
        {
            get; private set;
        }


        #endregion
        public static void HideFromAltTab(System.Windows.Window w)
        {
            IntPtr Handle = new WindowInteropHelper(w).Handle;
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }
        public bool isVis = false;
        public Windinfo()
        {
            InitializeComponent();
            
        }
        public void SetPos(int x , int y)
        {
            this.Left = x;
            this.Top = y;
            //HideFromAltTab(this);
        }
        public void SetText(string s)
        {
            LabelText.Content = s;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HideFromAltTab(this);
            IsClosed = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
            
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }
    }
}
