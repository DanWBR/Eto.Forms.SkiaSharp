using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace Eto.Forms.Controls.SkiaSharp.WPF
{
    public class SKControlHandler : Eto.Wpf.Forms.WpfFrameworkElement<FrameworkElement, Shared.SKControl, Shared.SKControl.ICallback>, Shared.SKControl.ISKControl
    {

        private SKControl_WPF nativecontrol;

        public SKControlHandler()
        {
            nativecontrol = new SKControl_WPF();
            this.Control = nativecontrol;
        }

        public override Eto.Drawing.Color BackgroundColor { get; set; }
        public Action<SKSurface> PaintSurfaceAction
        {
            get
            {
                return nativecontrol.PaintSurface;
            }
            set
            {
                nativecontrol.PaintSurface = value;
            }
        }

    }
        
    public class SKControl_WPF : SKElement
    {

        public new Action<SKSurface> PaintSurface;

        private WriteableBitmap bitmap;
        

        public SKControl_WPF()
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {

            base.OnRender(drawingContext);

            int width, height;
            double dpiX = 1.0;
            double dpiY = 1.0;
            if (IgnorePixelScaling)
            {
                width = (int)ActualWidth;
                height = (int)ActualHeight;
            }
            else
            {
                var m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
                dpiX = m.M11;
                dpiY = m.M22;
                width = (int)(ActualWidth * dpiX);
                height = (int)(ActualHeight * dpiY);
            }

            var info = new SKImageInfo(width, height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

            // reset the bitmap if the size has changed
            if (bitmap == null || info.Width != bitmap.PixelWidth || info.Height != bitmap.PixelHeight)
            {
                bitmap = new WriteableBitmap(width, height, dpiX, dpiY, PixelFormats.Pbgra32, null);
            }

            // draw on the bitmap
            bitmap.Lock();
            using (var surface = SKSurface.Create(info, bitmap.BackBuffer, bitmap.BackBufferStride))
            {
                if (PaintSurface != null) PaintSurface.Invoke(surface);
                OnPaintSurface(new SKPaintSurfaceEventArgs(surface, info));
                surface.Canvas.Flush();
            }

            // draw the bitmap to the screen
            bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            bitmap.Unlock();
            drawingContext.DrawImage(bitmap, new Rect(0, 0, ActualWidth, ActualHeight));
        }

   }

}
