using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp.Shared;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using SkiaSharp;
using SkiaSharp.Views.Mac;
using System;

namespace Eto.Forms.Controls.SkiaSharp.Mac
{
    public class SKControlHandler : Eto.Mac.Forms.MacView<NSView, SKControl, SKControl.ICallback>, SKControl.ISKControl
    {

        private SKControl_Mac nativecontrol;

        public SKControlHandler()
        {
            nativecontrol = new SKControl_Mac();
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


    public class SKControl_Mac : NSView, Eto.Mac.Forms.IMacControl
    {

        public Action<SKSurface> PaintSurface;

        private NSTrackingArea trackarea;
        
        public float _lastTouchX;
        public float _lastTouchY;

        private SKDrawable drawable;

        public SKControl_Mac()
        {
            drawable = new SKDrawable();
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
                     
            var ctx = NSGraphicsContext.CurrentContext.GraphicsPort;

            // create the skia context
            SKImageInfo info;

            var surface = drawable.CreateSurface(Bounds, 1.0f, out info);

            if (PaintSurface != null) PaintSurface.Invoke(surface);

            // draw the surface to the context
            drawable.DrawSurface(ctx, Bounds, info, surface);

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
            base.ScrollWheel(theEvent);
        }

        public WeakReference WeakHandler { get; set; }

    }

}
