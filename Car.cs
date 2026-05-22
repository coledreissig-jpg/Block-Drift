using System;
using System.Drawing;
using System.Collections.Generic;

namespace BlockDrift
{
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary, Mythical, Exotic }

    public class Car
    {
        public PointF Position;
        public PointF Velocity;
        public Rarity Rarity;
        public Color Color;
        public int Health = 10;
        private float boostTimer = 0;

        public static Car CreateDefaultPlayer(PointF pos)
        {
            return new Car { Position = pos, Rarity = Rarity.Common, Color = Color.Red };
        }

        public static Car CreateRandomNPC(Random rnd, Size screen)
        {
            var rar = (Rarity)rnd.Next(Enum.GetValues(typeof(Rarity)).Length);
            var c = new Car { Position = new PointF(rnd.Next(100, screen.Width - 100), rnd.Next(100, screen.Height - 200)), Rarity = rar, Color = RandomColor(rnd) };
            c.Velocity = new PointF(0, (float)(rnd.NextDouble() * -30 - 10));
            return c;
        }

        private static Color RandomColor(Random rnd)
        {
            return Color.FromArgb(255, rnd.Next(50,255), rnd.Next(50,255), rnd.Next(50,255));
        }

        public void Update(float dt, bool left, bool right, bool up, bool down, Size screen)
        {
            float speed = 120;
            if (boostTimer > 0) { speed *= 2; boostTimer -= dt; }

            var vx = 0f; var vy = 0f;
            if (left) vx -= speed; if (right) vx += speed; if (up) vy -= speed; if (down) vy += speed;

            Velocity = new PointF(vx, vy);
            Position = new PointF(Position.X + Velocity.X * dt, Position.Y + Velocity.Y * dt);

            // clamp
            Position = new PointF(Math.Max(20, Math.Min(screen.Width - 20, Position.X)), Math.Max(20, Math.Min(screen.Height - 20, Position.Y)));
        }

        public void Draw(Graphics g)
        {
            var rect = new RectangleF(Position.X - 18, Position.Y - 10, 36, 20);
            using var b = new SolidBrush(Color);
            g.FillRectangle(b, rect);
            g.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void ApplyBoost() { boostTimer = 1.0f; }
        public void DropOil() { /* placeholder */ }
        public void SwapPosition(List<Car> others, Random rnd)
        {
            if (others.Count == 0) return;
            var idx = rnd.Next(others.Count);
            var tmp = others[idx].Position;
            others[idx].Position = this.Position;
            this.Position = tmp;
        }
        public void Skydive() { Position = new PointF(Position.X, Position.Y - 150); }
        public void TakeDamage(int amount) { Health = Math.Max(0, Health - amount); }
    }
}
