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
using System.Media;

namespace Promodoro_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public enum TimeState
    {
        promodoro,
        shortBreak,
        longBreak
    }

    public partial class MainWindow : Window
    {
        private System.Windows.Forms.Timer timer = null;
        private int minutes;
        private int seconds;
        private bool bStop;
        private TimeState state;
        private SoundPlayer alarmSound;

        public MainWindow()
        {
            InitializeComponent();
            // timer
            timer = new System.Windows.Forms.Timer();
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            //
            // fix button size
            this.Fix_Button_Size();
            //
            // initial value
            this.SizeChanged += Windows_Resize;
            Update_Button_Select(TimeState.promodoro);
            alarmSound = new SoundPlayer(Properties.Resources.alarmSound);
            minutes = 25;
            seconds = 0;
            //
        }

        private void Fix_Button_Size()
        {
            btn_promodoro.Width = this.Width / 3;
            btn_shbreak.Width = this.Width / 3;
            btn_lgbreak.Width = this.Width / 3;
        }

        private void Update_Button_Select(TimeState state)
        {
            this.state = state;

            btn_promodoro.Background = new SolidColorBrush(Colors.BlueViolet);
            btn_shbreak.Background = new SolidColorBrush(Colors.BlueViolet);
            btn_lgbreak.Background = new SolidColorBrush(Colors.BlueViolet);

            switch (state)
            {
                case TimeState.promodoro:
                    btn_promodoro.Background = new SolidColorBrush(Colors.OrangeRed);
                    break;
                case TimeState.shortBreak:
                    btn_shbreak.Background = new SolidColorBrush(Colors.OrangeRed);
                    break;
                case TimeState.longBreak:
                    btn_lgbreak.Background = new SolidColorBrush(Colors.OrangeRed);
                    break;
            }
        }

        private void Update_Time_Text()
        {
            tx_Minutes.Text = tx_Seconds.Text = "";
            if (minutes < 10)
            {
                tx_Minutes.Text = "0";
            }
            tx_Minutes.Text += minutes.ToString();

            if (seconds < 10)
            {
                tx_Seconds.Text = "0";
            }
            tx_Seconds.Text += seconds.ToString();
        }

        private void Windows_Resize(object sender, EventArgs e)
        {
            this.Fix_Button_Size();
        }

        private void btn_promodoro_Click(object sender, RoutedEventArgs e)
        {
            Update_Button_Select(TimeState.promodoro);
            btn_Reset_Click(sender, e);
            btn_Start_Click(sender, e);
        }

        private void btn_shbreak_Click(object sender, RoutedEventArgs e)
        {
            Update_Button_Select(TimeState.shortBreak);
            btn_Reset_Click(sender, e);
            btn_Start_Click(sender, e);
        }

        private void btn_lgbreak_Click(object sender, RoutedEventArgs e)
        {
            Update_Button_Select(TimeState.longBreak);
            btn_Reset_Click(sender, e);
            btn_Start_Click(sender, e);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (seconds == 0 && minutes == 0)
            {
                timer.Enabled = false;
                bStop = true;
                alarmSound.Play();
            }
            else
            {
                if (seconds < 1)
                {
                    seconds = 59;
                    if (minutes != 0)
                    {
                        minutes -= 1;
                    }
                }
                else
                    seconds -= 1;
            }

            Update_Time_Text();
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            if (minutes == 0 && seconds == 0)
            {
                btn_Reset_Click(sender, e);
            }
            timer.Enabled = true;
            this.bStop = false;
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            timer.Enabled = false;
            this.bStop = true;
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            this.bStop = true;
            timer.Enabled = false;
            switch (this.state)
            {
                case TimeState.promodoro:
                    minutes = 25;
                    break;
                case TimeState.shortBreak:
                    minutes = 5;
                    break;
                case TimeState.longBreak:
                    minutes = 10;
                    break;
            }

            seconds = 0;
            Update_Time_Text();
        }
    }
}
