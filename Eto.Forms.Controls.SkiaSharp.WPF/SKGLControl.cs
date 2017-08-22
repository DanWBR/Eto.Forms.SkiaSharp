using Eto.Forms.Controls.SkiaSharp.WinForms;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace Eto.Forms.Controls.SkiaSharp.WPF
{
    public class SKGLControlHandler : Eto.Wpf.Forms.WpfFrameworkElement<FrameworkElement, Shared.SKGLControl, Shared.SKGLControl.ICallback>, Shared.SKGLControl.ISKGLControl
    {

        private SKGLControl_WPF nativecontrol;

        public SKGLControlHandler()
        {
            nativecontrol = new SKGLControl_WPF();

            // Create the winforms control

            nativecontrol.WinFormsControl = new WinForms.SKGLControl_WinForms();

            this.Control = nativecontrol;
        }

        public override Eto.Drawing.Color BackgroundColor { get; set; }

        public Action<SKSurface> PaintSurfaceAction
        {
            get
            {
                return nativecontrol.WinFormsControl.PaintSurface;
            }
            set
            {
                nativecontrol.WinFormsControl.PaintSurface = value;
            }
        }


    }
    
    public class SKGLControl_WPF : System.Windows.Controls.Grid
    {

        public SKGLControl_WinForms WinFormsControl;

        public SKGLControl_WPF()
        {
            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Create the interop host control.
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            
            WinFormsControl.WPFHost = true;

            // Assign the winforms control as the host control's child.
            host.Child = WinFormsControl;

            // Add the interop host control to the Grid
            // control's collection of child controls.
            this.Children.Add(host);

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            WinFormsControl.Invalidate();
        }

    }

}
