using System;
using Eto.Forms;

namespace Eto.Forms.Controls.SkiaSharp.Shared
{

    [Eto.Handler(typeof(ISKControl))]
    public class SKControl : Eto.Forms.Control
    {
        
        public interface ISKControl : Eto.Forms.Control.IHandler
        {
        }
        
    }
}


