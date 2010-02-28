using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AstroDeconvolution
{
    class SelectablePictureBox : PictureBox
    {
        public SelectablePictureBox()
        {
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                OnClick(EventArgs.Empty);
            }
            else
            {
                base.OnKeyPress(e);
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e); 
            Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (Focused) {
                Graphics grfx = pe.Graphics;
                grfx.DrawRectangle(new Pen(Brushes.Black, grfx.DpiX / 12), ClientRectangle);
            }
        }
    }
}
