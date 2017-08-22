using Eto.Forms.Controls.SkiaSharp.Shared;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eto.Forms.Controls.SkiaSharp.GTK
{
    public class SKControlHandler : Eto.GtkSharp.Forms.GtkControl<Gtk.EventBox, SKControl, SKControl.ICallback>, SKControl.ISKControl
    {

        private SKControl_GTK nativecontrol;

        public SKControlHandler()
        {
            nativecontrol = new SKControl_GTK();
            this.Control = nativecontrol;
        }

        public override Eto.Drawing.Color BackgroundColor { get; set; }
    }

    public class SKControl_GTK : Gtk.EventBox
    {
        public SKControl_GTK()
        {
            this.AddEvents((int)Gdk.EventMask.PointerMotionMask);
        }

         protected override bool OnExposeEvent(Gdk.EventExpose evnt)
        {

            var rect = Allocation;

            if (rect.Width > 0 && rect.Height > 0)
            {
                var area = evnt.Area;
                SKColorType ctype = SKColorType.Bgra8888;
                using (Cairo.Context cr = Gdk.CairoHelper.Create(base.GdkWindow))
                {
                    if (cr == null) { Console.WriteLine("Cairo Context is null"); }
                    using (var bitmap = new SKBitmap(rect.Width, rect.Height, ctype, SKAlphaType.Premul))
                    {
                        if (bitmap == null) { Console.WriteLine("Bitmap is null"); }
                        IntPtr len;
                        using (var skSurface = SKSurface.Create(bitmap.Info.Width, bitmap.Info.Height, ctype, SKAlphaType.Premul, bitmap.GetPixels(out len), bitmap.Info.RowBytes))
                        {
                            if (skSurface == null) { Console.WriteLine("skSurface is null"); }
                            skSurface.Canvas.Flush();
                            using (Cairo.Surface surface = new Cairo.ImageSurface(bitmap.GetPixels(out len), Cairo.Format.Argb32, bitmap.Width, bitmap.Height, bitmap.Width * 4))
                            {
                                surface.MarkDirty();
                                cr.SetSourceSurface(surface, 0, 0);
                                cr.Paint();
                            }
                        }
                    }
                }

            }

            return true;
        }

    }



}
