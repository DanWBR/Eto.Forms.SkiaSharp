using System;
using Eto.Forms;

namespace Eto.Forms.Controls.SkiaSharp.Shared
{

    [Eto.Handler(typeof(ISKGLControl))]
    public class SKGLControl : Eto.Forms.Control
    {
        ISKGLControl Handler { get { return (ISKGLControl)base.Handler; } }

        public Action<global::SkiaSharp.SKSurface> PaintSurfaceAction
        {
            get
            {
                return Handler.PaintSurfaceAction;
            }
            set
            {
                Handler.PaintSurfaceAction = value;
            }
        }

        public interface ISKGLControl : Eto.Forms.Control.IHandler
        {
            Action<global::SkiaSharp.SKSurface> PaintSurfaceAction { get; set; }
        }

    }
}