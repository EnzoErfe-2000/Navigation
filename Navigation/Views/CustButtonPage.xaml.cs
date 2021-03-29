using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PancakeView;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace Navigation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomButtonPage : ContentPage
    {
        SKPaint primaryFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Red,
            IsAntialias = true
        };

        SKPaint secondFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black,
            IsAntialias = true
        };

        SKPath btnPath = new SKPath();

        public CustomButtonPage()
        {
            InitializeComponent();

            //Btn path
            btnPath.MoveTo(-50, 0);
            btnPath.ArcTo(50, 60, 90, SKPathArcSize.Large, SKPathDirection.Clockwise, 50, 0);
            btnPath.ArcTo(1, 10, 90, SKPathArcSize.Small, SKPathDirection.CounterClockwise, -50, 0);
            btnPath.Close();
        }

        private void canvasView_paintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            int width = e.Info.Width;
            int height = e.Info.Height;

            canvas.Translate(width / 2, height / 2);
            canvas.Scale(Math.Min(width / 210f, height / 520f));

            //canvas.DrawPath(btnPath, primaryFillPaint);
            canvas.DrawCircle(0, 0, 100, primaryFillPaint);
            canvas.Save();
            //canvas.DrawPath(btnPath, secondFillPaint);
            canvas.Restore();

            for (int i =0; i<4; i++)
            {
                canvas.Save();
                switch (i)
                {
                    case 0:
                        canvas.Translate(0, -120);
                        break;
                    case 1:
                        canvas.Translate(120, 0);
                        break;
                    case 2:
                        canvas.Translate(0, 120);
                        break;
                    case 3:
                        canvas.Translate(-120, 0);
                        break;
                    default :
                        break;
                }
                canvas.RotateDegrees(90 * i);
                canvas.DrawPath(btnPath, secondFillPaint);
                canvas.Restore();
            }
        }
            async void GoToHome(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        
    }
}