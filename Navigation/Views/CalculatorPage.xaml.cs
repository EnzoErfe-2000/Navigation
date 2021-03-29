using System;
using System.IO;
using System.Reflection;
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
            Color = SKColors.Black,
            IsAntialias = true
        };

        SKPaint whiteStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 2,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        SKPaint whiteFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.White,
            IsAntialias = true
        };

        SKPaint greenFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.PaleGreen,
            IsAntialias = true
        };
        SKPaint blackStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 20,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };

        SKPaint grayFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Gray
            //IsAntialias = true
        };

        SKPaint backgroundFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill
        };

        SKPath catEarPath = new SKPath();
        SKPath catEyePath = new SKPath();
        SKPath catPupilPath = new SKPath();
        SKPath catTailPath = new SKPath();

        SKPath hourHandPath = SKPath.ParseSvgPathData(
            "M 0 -60 C 0 -30 20 -30 5 -20 L 5 0 C 5 7.5 -5 7.5 -5 0 L -5 -20 C -20 -30 0 -30 0 -60");

        SKPath minuteHandPath = SKPath.ParseSvgPathData(
            "M 0 -80 C 0 -75 0 -70 2.5 -60 L 2.5 0 C 2.5 5 -2.5 5 -2.5 0 L -2.5 -60 C 0 -70 0 -75 0 -80");

        public CalculatorPage()
        {
            InitializeComponent();

            //Cat Ear Path
            catEarPath.MoveTo(0, 0);
            catEarPath.LineTo(0, 75);
            catEarPath.LineTo(100, 75);
            catEarPath.Close();

            //Cat eye path
            catEyePath.MoveTo(0,0);
            catEyePath.ArcTo(50, 50,0,SKPathArcSize.Small, SKPathDirection.Clockwise,50,0);
            catEyePath.ArcTo(50, 50, 0, SKPathArcSize.Small, SKPathDirection.Clockwise, 0, 0);
            catEyePath.Close();

            //Eye pupil path
            catPupilPath.MoveTo(25, -5);
            catPupilPath.ArcTo(6, 6, 0, SKPathArcSize.Small, SKPathDirection.Clockwise, 25, 5);
            catPupilPath.ArcTo(6, 6, 0, SKPathArcSize.Small, SKPathDirection.Clockwise, 25, -5);
            catPupilPath.Close();

            //Tail path
            catTailPath.MoveTo(0, 100);
            catTailPath.CubicTo(50, 200, 0, 250, -50, 200);

            //Create Shader
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("Navigation.WoodGrain.png"))
            using (SKManagedStream skStream = new SKManagedStream(stream))
            using (SKBitmap bitmap = SKBitmap.Decode(skStream))
            using (SKShader shader = SKShader.CreateBitmap(bitmap, SKShaderTileMode.Mirror, SKShaderTileMode.Mirror))
            {
                backgroundFillPaint.Shader = shader;
            }

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

            //canvas.Clear(SKColors.CornflowerBlue);
            canvas.DrawPaint(backgroundFillPaint);


            int width = e.Info.Width;
            int height = e.Info.Height;

            //Set transforms
            canvas.Translate(width / 2, height / 2);
            canvas.Scale(Math.Min(width / 210f, height / 520f));

            //Get DateTime
            DateTime dateTime = DateTime.Now;

            //Clock BG
            canvas.DrawCircle(0, 0, 100,blackFillPaint);

            //Head
            canvas.DrawCircle(0, -160, 75, blackFillPaint);

            //Draw eyes and ears
            for (int i =0; i<2; i++)
            {
                canvas.Save();
                canvas.Scale(2 * i - 1, 1);

                canvas.Save();
                canvas.Translate(-65, -255);
                canvas.DrawPath(catEarPath, blackFillPaint);
                canvas.Restore();

                canvas.Save();
                canvas.Translate(10, -170);
                canvas.DrawPath(catEyePath, greenFillPaint);
                canvas.DrawPath(catPupilPath, blackFillPaint);
                canvas.Restore();

                //Draw Whiskers
                canvas.DrawLine(10, -120, 100, -100, whiteStrokePaint);
                canvas.DrawLine(10, -125, 100, -120, whiteStrokePaint);
                canvas.DrawLine(10, -130, 100, -140, whiteStrokePaint);
                canvas.DrawLine(10, -135, 100, -160, whiteStrokePaint);

                canvas.Restore();
            }

            //Move Tail
            float t = (float)Math.Sin((dateTime.Second % 2 + dateTime.Millisecond / 1000.0) * Math.PI);
            catTailPath.Reset();
            catTailPath.MoveTo(0, 100);
            SKPoint point1 = new SKPoint(-50 * t, 200);
            SKPoint point2 = new SKPoint(0, 250 - Math.Abs(50 * t));
            SKPoint point3 = new SKPoint(50 * t, 250 - Math.Abs(75 * t));
            catTailPath.CubicTo(point1, point2, point3);

            //Draw tail
            canvas.DrawPath(catTailPath, blackStrokePaint);

            //Hour & Minute marks
            for (int angle=0; angle<360; angle += 6)
            {
                canvas.DrawCircle(0, -90, angle % 30 == 0 ? 4 : 2, whiteFillPaint);
                canvas.RotateDegrees(6);
            }

            //Hour hand
            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            //whiteStrokePaint.StrokeWidth = 15;
            //canvas.DrawLine(0, 0, 0, -50, whiteStrokePaint);
            canvas.DrawPath(hourHandPath, grayFillPaint);
            canvas.DrawPath(hourHandPath, whiteStrokePaint);
            canvas.Restore();

            //Minute hand
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            //whiteStrokePaint.StrokeWidth = 10;
            //canvas.DrawLine(0, 0, 0, -70, whiteStrokePaint);
            canvas.DrawPath(minuteHandPath, grayFillPaint);
            canvas.DrawPath(minuteHandPath, whiteStrokePaint);
            canvas.Restore();

            //Second hand
            canvas.Save();
            float seconds = dateTime.Second + dateTime.Millisecond / 1000f; 
            canvas.RotateDegrees(6 * seconds);
            //whiteStrokePaint.StrokeWidth = 2;
            canvas.DrawLine(0, 10, 0, -80, whiteStrokePaint);
            canvas.Restore();

        }
    }
}