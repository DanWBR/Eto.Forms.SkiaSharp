using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp.Shared;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using SkiaSharp;
using SkiaSharp.Views.GlesInterop;
using SkiaSharp.Views.Mac;
using System;

namespace Eto.Forms.Controls.SkiaSharp.Mac
{
    public class SKGLControlHandler : Eto.Mac.Forms.MacView<NSView, SKGLControl, SKGLControl.ICallback>, SKGLControl.ISKGLControl
    {

        private SKGLControl_Mac nativecontrol;

        public SKGLControlHandler()
        {
            nativecontrol = new SKGLControl_Mac();
            this.Control = nativecontrol;
        }

        public override Eto.Drawing.Color BackgroundColor
        {
            get
            {
                return Eto.Drawing.Colors.White;
            }
            set
            {
                return;
            }
        }

        public override NSView ContainerControl
        {
            get
            {
                return Control;
            }
        }

        public override bool Enabled { get; set; }
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


    public class SKGLControl_Mac : SKGLView, Eto.Mac.Forms.IMacControl
    {

        public Action<SKSurface> PaintSurface;

        private NSTrackingArea trackarea;
        
        public SKGLControl_Mac() : base()
        {
            BecomeFirstResponder();
        }

        public override CGRect Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;
                UpdateTrackingAreas();
            }
        }

        public override CGRect Frame
        {
            get
            {
                return base.Frame;
            }

            set
            {
                base.Frame = value;
                UpdateTrackingAreas();
            }
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public override void UpdateTrackingAreas()
        {
            if (trackarea != null) { RemoveTrackingArea(trackarea); }
            trackarea = new NSTrackingArea(Frame, NSTrackingAreaOptions.ActiveWhenFirstResponder | NSTrackingAreaOptions.MouseMoved | NSTrackingAreaOptions.InVisibleRect, this, null);
            AddTrackingArea(trackarea);
        }

        public override void DrawRect(CGRect dirtyRect)
        {

            base.DrawRect(dirtyRect);
            
            var size = ConvertSizeToBacking(Bounds.Size);
            renderTarget.Width = (int)size.Width;
            renderTarget.Height = (int)size.Height;

            Gles.glClear(Gles.GL_STENCIL_BUFFER_BIT);

            using (var surface = SKSurface.Create(context, renderTarget))
            {

                if (PaintSurface != null) PaintSurface.Invoke(surface);

                surface.Canvas.Flush();
            }

            // flush the SkiaSharp contents to GL
            context.Flush();

            OpenGLContext.FlushBuffer();
        }


        public override void MouseMoved(NSEvent theEvent)
        {
            base.MouseMoved(theEvent);
        }

        public override void MouseDragged(NSEvent theEvent)
        {
            base.MouseDragged(theEvent);
        }

        public override void MouseUp(NSEvent theEvent)
        {
            base.MouseUp(theEvent);
        }

        public override void ScrollWheel(NSEvent theEvent)
        {
            base.MouseUp(theEvent);
        }

        public WeakReference WeakHandler { get; set; }

    }

}
