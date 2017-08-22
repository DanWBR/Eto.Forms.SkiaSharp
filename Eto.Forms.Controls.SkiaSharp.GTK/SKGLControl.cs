using Eto.Forms.Controls.SkiaSharp.Shared;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eto.Forms.Controls.SkiaSharp.GTK
{
    public class SKGLControlHandler : Eto.GtkSharp.Forms.GtkControl<OpenTK.GLWidget, SKGLControl, SKGLControl.ICallback>, SKGLControl.ISKGLControl
    {

        private SKGLControl_GTK nativecontrol;

        public SKGLControlHandler()
        {
            nativecontrol = new SKGLControl_GTK();
            this.Control = nativecontrol;
        }

        public override Eto.Drawing.Color BackgroundColor { get; set; }
    }

    public class SKGLControl_GTK : OpenTK.GLWidget
    {
        
        private GRContext grContext;
        private GRBackendRenderTargetDesc renderTarget;

        private float _lastTouchX;
        private float _lastTouchY;

        public SKGLControl_GTK(): base()
        {
            this.AddEvents((int)Gdk.EventMask.PointerMotionMask);
        }

        protected override void OnRenderFrame()
        {

            var rect = Allocation;

            // create the contexts if not done already
            if (grContext == null)
            {
                var glInterface = GRGlInterface.CreateNativeGlInterface();

                if (glInterface == null)
                {
                    Console.WriteLine("Error creating OpenGL ES interface. Check if you have OpenGL ES correctly installed and configured or change the PFD Renderer to 'Software (CPU)' on the Global Settings panel.", "Error Creating OpenGL ES interface");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                else
                {
                    grContext = GRContext.Create(GRBackend.OpenGL, glInterface);
                }

                try
                {
                    renderTarget = CreateRenderTarget();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating OpenGL ES render target. Check if you have OpenGL ES correctly installed and configured or change the PFD Renderer to 'Software (CPU)' on the Global Settings panel.\nError message:\n" + ex.ToString());
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }

            }

            if (grContext != null)
            {
                // update to the latest dimensions
                renderTarget.Width = rect.Width;
                renderTarget.Height = rect.Height;

                // create the surface
                using (var surface = SKSurface.Create(grContext, renderTarget))
                {

                    surface.Canvas.Clear(SKColors.White);
                                       
                    // start drawing
                    OnPaintSurface(new SKPaintGLSurfaceEventArgs(surface, renderTarget));

                    surface.Canvas.Flush();
                }
            }

        }

        public override void Dispose()
        {

            base.Dispose();

            // clean up
            if (grContext != null)
            {
                grContext.Dispose();
                grContext = null;
            }
        }

        public event EventHandler<SKPaintGLSurfaceEventArgs> PaintSurface;

        protected virtual void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
        {
            if (PaintSurface != null) PaintSurface.Invoke(this, e);
        }

        public static GRBackendRenderTargetDesc CreateRenderTarget()
        {

            int framebuffer, stencil, samples;
            Gles.glGetIntegerv(Gles.GL_FRAMEBUFFER_BINDING, out framebuffer);
            Gles.glGetIntegerv(Gles.GL_STENCIL_BITS, out stencil);
            Gles.glGetIntegerv(Gles.GL_SAMPLES, out samples);

            int bufferWidth = 0;
            int bufferHeight = 0;

            return new GRBackendRenderTargetDesc
            {
                Width = bufferWidth,
                Height = bufferHeight,
                Config = GRPixelConfig.Rgba8888,
                Origin = GRSurfaceOrigin.TopLeft,
                SampleCount = samples,
                StencilBits = stencil,
                RenderTargetHandle = (IntPtr)framebuffer,
            };
        }

    }

    public class SKPaintGLSurfaceEventArgs : EventArgs
    {
        public SKPaintGLSurfaceEventArgs(SKSurface surface, GRBackendRenderTargetDesc renderTarget)
        {
            Surface = surface;
            RenderTarget = renderTarget;
        }

        public SKSurface Surface { get; private set; }

        public GRBackendRenderTargetDesc RenderTarget { get; private set; }
    }


}
