using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System;

namespace SpyTool
{
    public class PicButton : PictureBox
    {
        protected bool m_bMouseOn;

        public PicButton() {
            this.Size = new Size(21, 21);
        }

        protected override void OnPaint(PaintEventArgs pe) {
            Graphics g = pe.Graphics;
            if (m_bMouseOn)
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(125, 0, 0, 0))) {
                    g.FillRectangle(sb, this.ClientRectangle);
                }
            base.OnPaint(pe);
        }

        protected override void OnMouseEnter(EventArgs e) {
            m_bMouseOn = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            m_bMouseOn = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }
    }
}
