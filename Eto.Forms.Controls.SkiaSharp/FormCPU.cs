using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms.Controls.SkiaSharp.Shared;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.Tests
{
    class FormCPU: Form
    {

        public FormCPU():base()
        {
            Init();            
        }

        void Init()
        {
            
            var skcontrol = new SKControl();

            skcontrol.PaintSurfaceAction = ((surface) => Tests.PaintStuff(surface));

            Title = "SKControl Demo (CPU Renderer)";

            ClientSize = new Drawing.Size(500, 500);

            Content = skcontrol;
        
        }

    }
}
