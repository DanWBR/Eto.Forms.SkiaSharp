using System;
using Eto.Forms;

namespace Eto.Forms.Controls.SkiaSharp.Shared
{

    [Eto.Handler(typeof(ISKGLControl))]
    public class SKGLControl : Eto.Forms.Control
    {

        public interface ISKGLControl : Eto.Forms.Control.IHandler
        {
        }

    }
}