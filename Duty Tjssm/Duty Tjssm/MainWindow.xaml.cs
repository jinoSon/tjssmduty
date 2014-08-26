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
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace Duty_Tjssm
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        GspConnector gCtest;

        public MainWindow()
        {
            InitializeComponent();
            createGspConnector();
            this.Loaded += MainWindow_Loaded;
            dutyDataPicker.SelectedDateChanged += dutyDataPicker_SelectedDateChanged;
            dutyTimeCheckBox.Click += dutyTimeCheckBox_Click;
            btnNameinput.Click += btnNameinput_Click;
        }

        void btnNameinput_Click(object sender, RoutedEventArgs e)
        {
            gCtest.writeData(dutyDataPicker.SelectedDate.Value, (bool)dutyTimeCheckBox.IsChecked, txtBest.Text, txtWorst.Text);
            MessageBox.Show("설정되었습니다.");
        }

        void dutyTimeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            UpdateBW();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.dutyDataPicker.Loaded += dutyDataPicker_Loaded;
          
        }

        void dutyDataPicker_Loaded(object sender, RoutedEventArgs e)
        {
            dutyDataPicker.SelectedDate = DateTime.Now;
            UpdateBW();
        }

        void dutyDataPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateBW();
        }

        void UpdateBW()
        {
           

            string[] tempStringArry = gCtest.loadData(dutyDataPicker.SelectedDate.Value, (bool)dutyTimeCheckBox.IsChecked);
            if (tempStringArry == null)
            {
                txtBest.Text = "";
                txtWorst.Text = "";
            }
            else
            {
                txtBest.Text = tempStringArry[0];
                txtWorst.Text = tempStringArry[1];
            }
            
        }

        void createGspConnector()
        {
            gCtest = new GspConnector();
            


        }

    }
}
