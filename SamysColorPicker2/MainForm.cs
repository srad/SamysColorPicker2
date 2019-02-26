using System;
using System.Drawing;
using System.Windows.Forms;

namespace SamysColorPicker2
{
    public partial class MainForm : Form
    {
        private Graphics ScreenCapture;
        private Bitmap ScreenBitmap;

        public MainForm()
        {
            InitializeComponent();
        }

        private void SamysNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            CaptureScreen();
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            Screenshot.Image = ScreenBitmap;
        }

        private void CaptureScreen()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            ScreenBitmap = new Bitmap(bounds.Width, bounds.Height);
            ScreenCapture = Graphics.FromImage(ScreenBitmap);
            ScreenCapture.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ScreenCapture != null) { ScreenCapture.Dispose(); }
            if (ScreenBitmap != null) { ScreenBitmap.Dispose(); }
        }

        private void Pick(object sender, MouseEventArgs e)
        {
            if (ScreenBitmap != null && e.Button == MouseButtons.Left)
            {
                var color = GetColorAt(e.Location);
                ColorValue.Text = HexConverter(color);
                StatusBar.BackColor = color;
            }
        }

        private Color GetColorAt(Point point)
        {
            float stretch_X = ScreenBitmap.Width / (float)Screenshot.Width;
            float stretch_Y = ScreenBitmap.Height / (float)Screenshot.Height;
            var color = ScreenBitmap.GetPixel((int)(point.X * stretch_X), (int)(point.Y * stretch_Y));

            return color;
        }

        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void Copy(object sender, MouseEventArgs e)
        {
            Clipboard.SetText(ColorValue.Text);
        }

        private void CopyTextBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ColorValue.Text))
            {
                Clipboard.SetText(ColorValue.Text);
            }
        }
    }
}
