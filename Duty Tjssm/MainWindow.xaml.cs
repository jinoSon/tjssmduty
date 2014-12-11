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
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;

namespace Duty_Tjssm
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        clipboard dutyTextClip;
        GspConnector gCtest;
        BackgroundWorker _backgroundWorker;
        void createGspConnector(){
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerAsync();
        }

        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            gCtest = new GspConnector();
        }
        public MainWindow()
        {
            
            InitializeComponent();
            createGspConnector();
            this.Loaded += MainWindow_Loaded;
            dutyDataPicker.SelectedDateChanged += dutyDataPicker_SelectedDateChanged;
            dutyTimeCheckBox.Click += dutyTimeCheckBox_Click;
            btnNameinput.Click += btnNameinput_Click;
            btnMission.Click += btnMission_Click;
        }

        void btnMission_Click(object sender, RoutedEventArgs e)
        {
            this.diTxtMisson1.Text = this.txtMission1.Text;
            this.diTxtMisson2.Text = this.txtMission2.Text;
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
            this.dutyTextClip = new clipboard();
            insertTextBoxToClip();
        }
        void insertTextBoxToClip()
        {


            dutyTextClip.insertString("2층");
            dutyTextClip.insertTextBox(tbt1, "체단실");
            dutyTextClip.insertTextBox(tbt2, "2층 화장실");
            dutyTextClip.insertTextBox(tbt3, "도서실");
            dutyTextClip.insertTextBox(tbt4, "섹  터");
            dutyTextClip.insertTextBox(tbt5, "샤워실");

            dutyTextClip.insertString("3층");
            dutyTextClip.insertTextBox(tbt6, "탕비실");
            dutyTextClip.insertTextBox(tbt7, "휴게실");
            dutyTextClip.insertTextBox(tbt8, "3층 화장실");
            dutyTextClip.insertTextBox(tbt9, "대미나");
            dutyTextClip.insertTextBox(tbt10, "소미나");
            dutyTextClip.insertTextBox(tbt11, "수면실");

            dutyTextClip.insertString("복사기 사용량");
            dutyTextClip.insertTextBox(tbt12, "기준매수");
            dutyTextClip.insertTextBox(tbt13, "현재매수");


       
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
                this.diTxtWorst.Text = txtWorst.Text;
                this.diTxtBest.Text = txtBest.Text;
            }
            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)mainCanvas.Width, (int)mainCanvas.Height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(mainCanvas);
            SaveFileDialog dlg = new SaveFileDialog();
  
              dlg.Filter = "PNG|*.png";
              dlg.AddExtension = true;
  
              if (dlg.ShowDialog() == true)
              {
              FileStream stream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write);
              BitmapEncoder encoder = new PngBitmapEncoder();
  
              dlg.FileName.ToCharArray(dlg.FileName.Length - 3, 3);
  
              string upper = dlg.FileName.ToUpper();
              char[] format = upper.ToCharArray(dlg.FileName.Length - 3, 3);
              upper = new string(format);



              encoder.Frames.Add(BitmapFrame.Create(rtb));
  
              encoder.Save(stream);
              stream.Close();
              }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dutyTextClip.setClipboardText();
        }

        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            mainScroll.ScrollToTop();
        }

    }
}
