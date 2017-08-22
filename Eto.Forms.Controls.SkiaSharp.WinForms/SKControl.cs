using Eto.Forms;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Eto.Forms.Controls.SkiaSharp.WinForms
{
    public class SKControlHandler : Eto.WinForms.Forms.WindowsControl<System.Windows.Forms.Control, Shared.SKControl, Shared.SKControl.ICallback>, Shared.SKControl.ISKControl
    {

        private SKControl_WinForms nativecontrol;

        public SKControlHandler()
        {
            nativecontrol = new SKControl_WinForms();
            this.Control = nativecontrol;
        }
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

    public class SKControl_WinForms : SKControl
    {

        public new Action<SKSurface> PaintSurface;

        private System.Drawing.Bitmap bitmap;
        
        public SKControl_WinForms()
        {
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {

            base.OnPaint(e);

            // get the bitmap
            CreateBitmap();
            var data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            // create the surface
            var info = new SKImageInfo(Width, Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            using (var surface = SKSurface.Create(info, data.Scan0, data.Stride))
            {

                if (PaintSurface != null) PaintSurface.Invoke(surface);

                OnPaintSurface(new SKPaintSurfaceEventArgs(surface, info));

                surface.Canvas.Flush();
            }

            // write the bitmap to the graphics
            bitmap.UnlockBits(data);
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void CreateBitmap()
        {
            if (bitmap == null || bitmap.Width != Width || bitmap.Height != Height)
            {
                FreeBitmap();

                bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            }
        }

        private void FreeBitmap()
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }

    }
}
