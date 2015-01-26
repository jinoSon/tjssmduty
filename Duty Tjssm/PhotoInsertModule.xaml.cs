using System;
using System.Collections.Generic;
using System.IO;
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

namespace Duty_Tjssm
{
    /// <summary>
    /// PhotoInsertModule.xaml에 대한 상호 작용 논리ddd
    /// </summary>
    public partial class PhotoInsertModule : UserControl
    {
        double rotateAngle = 0;
        RotateTransform photoRotate = new RotateTransform(90);
        List<BitmapImage> bitmapList = new List<BitmapImage>();

        public PhotoInsertModule()
        {
            InitializeComponent();
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.bitmapList.Count == 1)
            {
                this.rotateAngle += 90;
                this.rotateAngle %= 360;
                if (bitmapList[0] != null)
                {
                    TransformedBitmap myRotatedBitmapSource = new TransformedBitmap();

                    // BitmapSource objects like TransformedBitmap can only have their properties
                    // changed within a BeginInit/EndInit block.
                    myRotatedBitmapSource.BeginInit();

                    // Use the BitmapSource object defined above as the source for this BitmapSource.
                    // This creates a "chain" of BitmapSource objects which essentially inherit from each other.
                    myRotatedBitmapSource.Source = bitmapList[0];

                    // Flip the source 90 degrees.
                    myRotatedBitmapSource.Transform = new RotateTransform(rotateAngle);
                    myRotatedBitmapSource.EndInit();
                    img1.Source = myRotatedBitmapSource;
                }
                else
                {
                    fileOpen();
                }
            }
            e.Handled = true;
        }

        private void fileOpen()
        {
            rotateAngle = 0;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.FileName = "Images";
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPEG |*.jpg|  Png |*.png| GIF |*.gif";
            dlg.Multiselect = true;

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                if (dlg.FileNames.Length + bitmapList.Count > 4)
                {
                    reFreshImage();
                }
                foreach (string selectedFileName in dlg.FileNames)
                {
                    bool _check = true;
                    foreach (BitmapImage _bmp in bitmapList)
                    {
                        if (_bmp.UriSource.Equals(new Uri(selectedFileName)))
                        {
                            _check = false;
                            break;
                        }

                    }
                    if (_check)
                    {
                        BitmapImage insertbitmap = new BitmapImage();
                        insertbitmap.BeginInit();
                        insertbitmap.UriSource = new Uri(selectedFileName);
                        insertbitmap.EndInit();
                        this.bitmapList.Add(insertbitmap);
                    }
                }
                mergeImageDecord();
            }
        }

        private void mergeImageDecord()
        {
            if (bitmapList.Count == 1)
            {
                img1.Source = bitmapList[0];
            }
            else
            {
                img1.Width = 169;
                img1.Height = 108;
                img1.Source = bitmapList[0];
                img2.Stretch = Stretch.Uniform;
                img2.Source = bitmapList[1];
                if (bitmapList.Count >= 3)
                {
                    img3.Stretch = Stretch.Uniform;
                    img3.Source = bitmapList[2];
                }
                if (bitmapList.Count >= 4)
                {
                    img4.Stretch = Stretch.Uniform;
                    img4.Source = bitmapList[3];
                }
            }
        }

        //private BitmapImage renderToBitmapImage(RenderTargetBitmap _temp)
        //{
        //    var bitmapEncoder = new PngBitmapEncoder();
        //    bitmapEncoder.Frames.Add(BitmapFrame.Create(_temp));
        //    BitmapImage _tmpBitmap;
        //    using (var stream = new MemoryStream())
        //    {
        //        bitmapEncoder.Save(stream);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        _tmpBitmap = new BitmapImage();
        //        _tmpBitmap.BeginInit();
        //        _tmpBitmap.CacheOption = BitmapCacheOption.OnLoad;
        //        _tmpBitmap.StreamSource = stream;
        //        _tmpBitmap.EndInit();
        //    }
        //    return _tmpBitmap;
        //}

        private void mainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            fileOpen();
        }
        private void Canvas_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            //초기화 함수 진행            
            e.Handled = true;
            reFreshImage();
        }

        private void reFreshImage()
        {
            this.bitmapList.Clear();
            img1.Width = 338;
            img1.Height = 217;
            img1.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resource/noImg.png"));
            img2.Stretch = Stretch.Fill;
            img3.Stretch = Stretch.Fill;
            img4.Stretch = Stretch.Fill;
            img2.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resource/Noimage.png")); ;
            img3.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resource/Noimage.png")); ;
            img4.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resource/Noimage.png")); ;
        }
    }
}
