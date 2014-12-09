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
        //int picNum = 0;
        double rotateAngle = 0;
        BitmapImage bitmap = null;          //현재 보이는 이미지
        RotateTransform photoRotate = new RotateTransform(90);
        List<BitmapImage> bitmapList = new List<BitmapImage>();
        List<BitmapImage> resultBitmapList = new List<BitmapImage>();

        public PhotoInsertModule()
        {
            InitializeComponent();
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.bitmapList.Count == 1)
            {
                ImageBrush tempBrush = (ImageBrush)mainCanvas.Background;
                this.rotateAngle += 90;
                this.rotateAngle %= 360;
                if (bitmap != null)
                {
                    //tempBrush.Transform = new RotateTransform(rotateAngle, bitmap.PixelWidth*0.5, bitmap.PixelHeight*0.5);
                    TransformedBitmap myRotatedBitmapSource = new TransformedBitmap();

                    // BitmapSource objects like TransformedBitmap can only have their properties
                    // changed within a BeginInit/EndInit block.
                    myRotatedBitmapSource.BeginInit();

                    // Use the BitmapSource object defined above as the source for this BitmapSource.
                    // This creates a "chain" of BitmapSource objects which essentially inherit from each other.
                    myRotatedBitmapSource.Source = bitmap;

                    // Flip the source 90 degrees.
                    myRotatedBitmapSource.Transform = new RotateTransform(rotateAngle);
                    myRotatedBitmapSource.EndInit();
                    mainCanvas.Background = new ImageBrush(myRotatedBitmapSource);
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
                this.resultBitmapList.Clear();
                if (dlg.FileNames.Length + bitmapList.Count > 4)
                {
                    this.bitmapList.Clear();
                    
                }
                foreach (string selectedFileName in dlg.FileNames)
                {
                    //string selectedFileName = dlg.FileName;
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
                displayImageDecord();
                
                this.mainCanvas.Background = new ImageBrush(bitmap);
            }
        }
        private void displayImageDecord()
        {
            List<int> aspectRatioHeight = new List<int>();
            int i = 0;
            
            foreach(BitmapImage _tmp in bitmapList){
                BitmapImage insertbitmap = new BitmapImage();

                insertbitmap.BeginInit();
                insertbitmap.UriSource = _tmp.UriSource;

                if(_tmp.PixelWidth > _tmp.PixelHeight){
                    insertbitmap.DecodePixelHeight = (int)this.ActualHeight;
                    insertbitmap.DecodePixelWidth = (int)this.ActualWidth;
                }
                else{
                    insertbitmap.DecodePixelHeight = (int)this.ActualHeight;
                    insertbitmap.DecodePixelWidth = (int)this.ActualWidth / 2;
                    aspectRatioHeight.Add(i);
                }
                insertbitmap.EndInit();

                resultBitmapList.Add(insertbitmap);
                i++;
               
            }
            mergeImageDecord(aspectRatioHeight);
           
        }
        private void mergeImageDecord(List<int> _tList){
            int imgWidth = (int)this.ActualWidth;
            int imgHeight = (int)this.ActualHeight;
            DrawingVisual drawingVisual = new DrawingVisual();
            RenderTargetBitmap bmp;
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                
                if (resultBitmapList.Count == 1)
                {
                    drawingContext.DrawImage(resultBitmapList[0], new Rect(0, 0, imgWidth*2, imgHeight*2));
                }
                else
                {
                    
                    drawingContext.DrawImage(resultBitmapList[0], new Rect(0, 0, imgWidth, imgHeight));
                    drawingContext.DrawImage(resultBitmapList[1], new Rect(imgWidth, imgHeight, imgWidth, imgHeight));
                    if (resultBitmapList.Count >= 3)
                    {
                        drawingContext.DrawImage(resultBitmapList[2], new Rect(0, imgHeight,  imgWidth, imgHeight));
                    }
                    if (resultBitmapList.Count >= 4)
                    {
                        drawingContext.DrawImage(resultBitmapList[3], new Rect(imgWidth, 0, imgWidth, imgHeight));
                    }
                }

                
                
                
            }
            //*/
            bmp = new RenderTargetBitmap(imgWidth * 2, imgHeight * 2, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            bitmap = renderToBitmapImage(bmp);
        }
        /*/
        private void mergeImage(BitmapImage _inserBitmap)
        {

            // Draws the images into a DrawingVisual component
           RenderTargetBitmap bmp;
            switch (this.picNum)
            {
                case 1: // 이미지 2개로 만들 때
                case 2:
                    
                       bmp = mergeDrawVisual(_inserBitmap);
                       mainCanvas.Background = new ImageBrush();
                       renderToBitmapImage(bmp);
                       this.mainCanvas.Background = new ImageBrush(bitmap);
                       this.picNum++;

                    break;
                
                case 4: // 이미지 3개로 만들 때
                    break;


            }
           
        }
        */
        /*/
        private RenderTargetBitmap mergeDrawVisual(BitmapImage _inserBitmap)
        {
            int imgWidth = bitmap.PixelWidth + _inserBitmap.PixelWidth;
            int imgHeight = bitmap.PixelHeight + _inserBitmap.PixelHeight;
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(bitmap, new Rect(0, 0, imgWidth * 0.5, imgHeight));
                drawingContext.DrawImage(_inserBitmap, new Rect(imgHeight * 0.5, 0, imgWidth, imgHeight));

            }
            RenderTargetBitmap bmp = new RenderTargetBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }*/
        //*/
        private BitmapImage renderToBitmapImage(RenderTargetBitmap _temp)
        {
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(_temp));
            BitmapImage _tmpBitmap;
            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                _tmpBitmap = new BitmapImage();
                _tmpBitmap.BeginInit();
                _tmpBitmap.CacheOption = BitmapCacheOption.OnLoad;
                _tmpBitmap.StreamSource = stream;
                _tmpBitmap.EndInit();
            }
            return _tmpBitmap;
        }//*/
        private void mainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            fileOpen();
        }

        private void Canvas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            fileOpen();
            
            
        }

        private void Canvas_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            //초기화 함수 진행
            e.Handled = true;
            //this.mainCanvas.Background = new ImageBrush(new BitmapImage(new Uri("Resource//noImg.png")));
            this.bitmapList.Clear();
            this.resultBitmapList.Clear();
            this.mainCanvas.Background = new SolidColorBrush();

        }
    }
}
