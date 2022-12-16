using System;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BankTask
{
    public partial class ComparisonOfDepositsPage : Window
    {
        public double srokkredits;
        public double summavklada;

        public ComparisonOfDepositsPage(string stabilityValue, string optimalValue, string standartValue, double stabotvet, double stabotvet1, double stabotvet2, double srok, double totalSumma)
        {
            InitializeComponent();
            //обработка данных после получения
            tbl_stabilitydohod.Text = stabilityValue;
            tbl_optimaldohod.Text = optimalValue;
            tbl_standartdohod.Text = standartValue;
            srokkredits = srok;
            summavklada = totalSumma;

            tbl_stabilitysumma.Text = Convert.ToDecimal(stabotvet).ToString("#,##0 Руб.");
            tbl_optimalsumma.Text = Convert.ToDecimal(stabotvet1).ToString("#,##0 Руб.");
            tbl_standartsumma.Text = Convert.ToDecimal(stabotvet2).ToString("#,##0 Руб.");
        }

        private void bt_vkladfour_Click(object sender, RoutedEventArgs e)
        {
            //создаём объект для скрина и путь сохранения для передачи в метод по созданию скрина
            UIElement element = gd_screen as UIElement;
            Uri path = new Uri(@"C:\Users\kolpa\Downloads\screenshot.png");
            CaptureScreen(element, path);
        }
        public void CaptureScreen(UIElement source, Uri destination)
        {
            try
            {
                double Height, renderHeight, Width, renderWidth;

                Height = renderHeight = source.RenderSize.Height;
                Width = renderWidth = source.RenderSize.Width;

                // Спецификация для целевого растрового изображения, такого как пиксель ширины / высоты и т.д.
                RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
                //создает визуальную кисть UIElement
                VisualBrush visualBrush = new VisualBrush(source);

                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    //рисуем изображение элемента
                    drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(0, 0), new Point(Width, Height)));
                }
                //обрататываем изображение
                renderTarget.Render(drawingVisual);
                //PNG кодировщик для создания файла PNG
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));

                using (FileStream stream = new FileStream(destination.LocalPath, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(stream);
                }

                PdfDocument doc = new PdfDocument();//Создём объект pdf-документа
                PdfPage page = doc.Pages.Add();//Добавляем новую странницу документа               
                PdfGraphics graphics = page.Graphics;//Создаём графический объект в pdf            
                PdfBitmap image = new PdfBitmap(@"C:\Users\kolpa\Downloads\screenshot.png");//Выгружаем картинку на диск             
                graphics.DrawImage(image, 0, 0);//Заполняем графисечкий объект в pdf
                                                
                doc.Save(@"C:\Users\kolpa\Downloads\screenshot1.pdf");//Сохраняем изменения
                doc.Close(true);//Закрываем документ
            }
            catch (Exception e)//Обработка всех исключений
            {
                MessageBox.Show(e.ToString());
            }
        }

        // Пепредача данных для формирования объекта, берём соответсвующие данные в зависимости от выбранной услуги
        private void bt_vkladthree_Click(object sender, RoutedEventArgs e)
        {
            nextPage(tbl_stability.Text, tbl_stabilitydohod.Text, tbl_stabilitystavka.Text, tbl_stabilitysumma.Text);
        }

        private void btn_vkladone_Click(object sender, RoutedEventArgs e)
        {
            nextPage(tbl_optimal.Text, tbl_optimaldohod.Text, tbl_optimalstavka.Text, tbl_optimalsumma.Text);
        }

        private void bt_vkladtwo_Click(object sender, RoutedEventArgs e)
        {
            nextPage(tbl_standart.Text, tbl_standartdohod.Text, tbl_standartstavka.Text, tbl_standartsumma.Text);
        }

        public void nextPage(string name, string dohod, string stavka, string summa )
        {
            AuthorizationPanel form = new AuthorizationPanel(name, dohod, stavka, summa, srokkredits, summavklada);
            form.Show();
        }
    }
}
