using System;
using System.Collections.Generic;
using System.Drawing;

namespace BlockDrift
{
    public class Game
    {
        public Size ScreenSize { get; set; }
        private Map map;
        private Car player;
        private List<Car> npcs = new List<Car>();
        private Random rnd = new Random();
        private float timeOfDay = 12f; // 0-24
        private string weather = "Clear";

        public Game(Size screenSize)
        {
            ScreenSize = screenSize;
            Reset();
        }

        public void Reset()
        {
            map = new Map(ScreenSize.Width, ScreenSize.Height);
            player = Car.CreateDefaultPlayer(new PointF(ScreenSize.Width / 2f, ScreenSize.Height - 120));
            npcs.Clear();
            for (int i = 0; i < 6; i++) npcs.Add(Car.CreateRandomNPC(rnd, ScreenSize));
            RandomizeWeather();
        }

        private void RandomizeWeather()
        {
            var choices = new[] { "Clear", "Rain", "Windy" };
            weather = choices[rnd.Next(choices.Length)];
            timeOfDay = (float)(rnd.NextDouble() * 24.0);
        }

        public void UsePowerUp(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.Boost: player.ApplyBoost(); break;
                case PowerUpType.Oil: player.DropOil(); break;
                case PowerUpType.Swap: player.SwapPosition(npcs, rnd); break;
                case PowerUpType.Skydive: player.Skydive(); break;
            }
        }

        public void Update(float dt, bool left, bool right, bool up, bool down)
        {
            // simple time progression
            timeOfDay += dt * 0.2f;
            if (timeOfDay > 24) timeOfDay -= 24;

            // weather rare events
            if (rnd.Next(5000) == 0) // meteor shower
            {
                // placeholder: spawn damage
            }

            // update player
            player.Update(dt, left, right, up, down, ScreenSize);

            // update npcs
            foreach (var c in npcs) c.Update(dt, false, false, false, false, ScreenSize);

            // collisions simple
            foreach (var c in npcs)
            {
                if (Distance(c.Position, player.Position) < 40)
                {
                    player.TakeDamage(1);
                }
            }
        }

        private float Distance(PointF a, PointF b)
        {
            var dx = a.X - b.X; var dy = a.Y - b.Y; return (float)Math.Sqrt(dx*dx+dy*dy);
        }

        public void Draw(Graphics g)
        {
            // background based on time
            Color bg = GetSkyColor();
            g.Clear(bg);

            // draw map voxels
            map.Draw(g);

            // draw npcs
            foreach (var c in npcs) c.Draw(g);

            // draw player
            player.Draw(g);

            // HUD
            DrawHUD(g);
        }

        private Color GetSkyColor()
        {
            if (timeOfDay < 6 || timeOfDay > 20) return Color.FromArgb(18, 24, 48);
            if (timeOfDay < 9) return Color.FromArgb(120, 170, 240);
            if (timeOfDay < 17) return Color.FromArgb(135, 206, 235);
            return Color.FromArgb(200, 150, 220);
        }

        private void DrawHUD(Graphics g)
        {
            using var f = new Font("Consolas", 12);
            g.DrawString($"Weather: {weather}", f, Brushes.White, 10, 10);
            g.DrawString($"Time: {Math.Round(timeOfDay)}:00", f, Brushes.White, 10, 28);
            g.DrawString($"Player HP: {player.Health}", f, Brushes.White, 10, 46);
        }
    }
}
