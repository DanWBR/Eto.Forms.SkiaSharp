using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using Eto;
using System.IO;
using Eto.Forms.Controls.SkiaSharp.Shared;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.Tests
{
    class Tests
    {

        [STAThread]
        static void Main()
        {

            //initialize OpenTK

            OpenTK.Toolkit.Init();
            
            //initialize Eto

            Eto.Platform platform = null;

            if (RunningPlatform() == Platform.Windows)
            {
                platform = new Eto.Wpf.Platform();
                platform.Add<SKControl.ISKControl>(() => new WPF.SKControlHandler());
                platform.Add<SKGLControl.ISKGLControl>(() => new WPF.SKGLControlHandler());
            }
            else if (RunningPlatform() == Platform.Linux)
            {
                platform = new Eto.GtkSharp.Platform();
                platform.Add<SKControl.ISKControl>(() => new GTK.SKControlHandler());
                platform.Add<SKGLControl.ISKGLControl>(() => new GTK.SKGLControlHandler());
            }
            else if ( RunningPlatform() ==  Platform.Mac)
            {
                platform = new Eto.Mac.Platform();
                platform.Add<SKControl.ISKControl>(() => new Mac.SKControlHandler());
                platform.Add<SKGLControl.ISKGLControl>(() => new Mac.SKGLControlHandler());
            }
            
            // test CPU drawing (SKControl)

            //new Application(platform).Run(new FormCPU());


            // test OpenGL drawing (SKGLControl)

            new Application(platform).Run(new FormGL());
            
        }

        public static void PaintStuff(SKSurface surface)
        {

            // CLEARING THE SURFACE

            // get the canvas that we can draw on
            var canvas = surface.Canvas;
            // clear the canvas / view
            canvas.Clear(SKColors.White);


            // DRAWING SHAPES

            // create the paint for the filled circle
            var circleFill = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColors.Blue
            };
            // draw the circle fill
            canvas.DrawCircle(100, 100, 40, circleFill);

            // create the paint for the circle border
            var circleBorder = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeWidth = 5
            };
            // draw the circle border
            canvas.DrawCircle(100, 100, 40, circleBorder);


            // DRAWING PATHS

            // create the paint for the path
            var pathStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Green,
                StrokeWidth = 5
            };

            // create a path
            var path = new SKPath();
            path.MoveTo(160, 60);
            path.LineTo(240, 140);
            path.MoveTo(240, 60);
            path.LineTo(160, 140);

            // draw the path
            canvas.DrawPath(path, pathStroke);


            // DRAWING TEXT

            // create the paint for the text
            var textPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColors.Orange,
                TextSize = 80
            };
            // draw the text (from the baseline)
            canvas.DrawText("SkiaSharp", 60, 160 + 80, textPaint);

        }

        public enum Platform
        {
            Windows,
            Linux,
            Mac
        }

        public static Platform RunningPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    if (Directory.Exists("/Applications") & Directory.Exists("/System") & Directory.Exists("/Users") & Directory.Exists("/Volumes"))
                    {
                        return Platform.Mac;
                    }
                    else
                    {
                        return Platform.Linux;
                    }
                case PlatformID.MacOSX:
                    return Platform.Mac;
                default:
                    return Platform.Windows;
            }
        }

    }
}
