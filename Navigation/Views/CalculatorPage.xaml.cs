using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace Navigation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculatorPage : ContentPage
    {
        SKPaint blackFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black
        };

        SKPaint whiteStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        public CalculatorPage()
        {
            InitializeComponent();

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                canvasView.InvalidateSurface();
                return true;
            });
        }
        private void canvasView_paintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.CornflowerBlue);

            int width = e.Info.Width;
            int height = e.Info.Height;

            //Set transforms
            canvas.Translate(width / 2, height / 2);
            canvas.Scale(width / 280f);

            //Get DateTime
            DateTime dateTime = DateTime.Now;

            //Clock BG
            canvas.DrawCircle(0, 0, 100,blackFillPaint);

            //Hour hand
            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            whiteStrokePaint.StrokeWidth = 15;
            canvas.DrawLine(0, 0, 0, -50, whiteStrokePaint);
            canvas.Restore();
            //Minute hand
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            whiteStrokePaint.StrokeWidth = 10;
            canvas.DrawLine(0, 0, 0, -70, whiteStrokePaint);
            canvas.Restore();
            //Second hand
            canvas.Save();
            float seconds = dateTime.Second + dateTime.Millisecond / 1000f; 
            canvas.RotateDegrees(6 * seconds);
            whiteStrokePaint.StrokeWidth = 2;
            canvas.DrawLine(0, 10, 0, -80, whiteStrokePaint);
            canvas.Restore();

        }
    }
}