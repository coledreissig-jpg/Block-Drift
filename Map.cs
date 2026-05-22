using System;
using System.Drawing;

namespace BlockDrift
{
    public enum MapPreset { City, Country, Alps, Suburbs, Snowy }

    public class Map
    {
        private int width, height;
        private Random rnd = new Random();

        public Map(int w, int h)
        {
            width = w; height = h;
        }

        public void Draw(Graphics g)
        {
            // simple voxel-like ground layers
            int block = 24;
            for (int y = 0; y < height; y += block)
            {
                for (int x = 0; x < width; x += block)
                {
                    var shade = 180 - (y / Math.Max(1, block));
                    var color = Color.FromArgb(Math.Max(20, shade), 100, 160);
                    g.FillRectangle(new SolidBrush(color), x, y, block - 2, block - 2);
                }
            }

            // draw a road
            var roadRect = new RectangleF(width * 0.15f, height * 0.55f, width * 0.7f, 80);
            g.FillRectangle(Brushes.DimGray, roadRect);
            // road damage
            for (int i = 0; i < 6; i++)
            {
                var rx = (float)(width * 0.15f + i * roadRect.Width / 6 + rnd.NextDouble() * 30 - 15);
                var ry = roadRect.Y + rnd.Next(-10, 20);
                g.FillEllipse(Brushes.Black, rx, ry, 24, 12);
            }
        }
    }
}
