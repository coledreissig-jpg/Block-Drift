using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlockDrift
{
    public class MainForm : Form
    {
        private Game game;
        private Timer timer;
        private bool left, right, up, down;
        private Button btnStart, btnCollection, btnMap, btnSettings;
        private bool inGame = false;

        public MainForm()
        {
            Text = "Block Drift";
            ClientSize = new Size(1000, 700);
            DoubleBuffered = true;

            game = new Game(ClientSize);

            InitializeUI();

            timer = new Timer { Interval = 33 };
            timer.Tick += Timer_Tick;
            timer.Start();

            KeyDown += MainForm_KeyDown;
            KeyUp += MainForm_KeyUp;
            Paint += MainForm_Paint;
            Resize += MainForm_Resize;
        }

        private void InitializeUI()
        {
            btnStart = new Button { Text = "Start", Location = new Point(20, 20), Size = new Size(120, 40) };
            btnCollection = new Button { Text = "Collection", Location = new Point(150, 20), Size = new Size(120, 40) };
            btnMap = new Button { Text = "Map: Choose", Location = new Point(280, 20), Size = new Size(120, 40) };
            btnSettings = new Button { Text = "⚙", Location = new Point(ClientSize.Width - 60, 20), Size = new Size(40, 40) };

            btnStart.Click += (s, e) => { StartGame(); };
            btnCollection.Click += (s, e) => { MessageBox.Show("Collection: (demo)\nShows owned cars and rarities.", "Collection"); };
            btnMap.Click += (s, e) => { ChooseMap(); };
            btnSettings.Click += (s, e) => { ShowSettings(); };

            Controls.Add(btnStart);
            Controls.Add(btnCollection);
            Controls.Add(btnMap);
            Controls.Add(btnSettings);
        }

        private void ShowSettings()
        {
            var res = MessageBox.Show("Leave game?", "Settings", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes) Close();
        }

        private void ChooseMap()
        {
            var maps = string.Join("\n", Enum.GetNames(typeof(MapPreset)));
            var choice = MessageBox.Show("Maps:\n" + maps + "\n(Choose via console for demo)", "Maps");
        }

        private void StartGame()
        {
            inGame = true;
            game.Reset();
            Invalidate();
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            btnSettings.Location = new Point(ClientSize.Width - 60, 20);
            game.ScreenSize = ClientSize;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (inGame)
            {
                game.Update(0.033f, left, right, up, down);
            }
            Invalidate();
        }

        private void MainForm_Paint(object? sender, PaintEventArgs e)
        {
            if (!inGame)
            {
                DrawHome(e.Graphics);
            }
            else
            {
                game.Draw(e.Graphics);
            }
        }

        private void DrawHome(Graphics g)
        {
            g.Clear(Color.FromArgb(30, 30, 30));
            using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("Block Drift", new Font("Arial", 48, FontStyle.Bold), Brushes.White, new RectangleF(0, 120, ClientSize.Width, 80), sf);
            g.DrawString("Voxels · Cars · Events · Abilities", new Font("Arial", 14), Brushes.LightGray, new RectangleF(0, 190, ClientSize.Width, 40), sf);
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) left = true;
            if (e.KeyCode == Keys.Right) right = true;
            if (e.KeyCode == Keys.Up) up = true;
            if (e.KeyCode == Keys.Down) down = true;
            if (e.KeyCode == Keys.Space) game.UsePowerUp(PowerUpType.Boost);
            if (e.KeyCode == Keys.D) game.UsePowerUp(PowerUpType.Oil);
            if (e.KeyCode == Keys.C) game.UsePowerUp(PowerUpType.Swap);
            if (e.KeyCode == Keys.K) game.UsePowerUp(PowerUpType.Skydive);
        }

        private void MainForm_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) left = false;
            if (e.KeyCode == Keys.Right) right = false;
            if (e.KeyCode == Keys.Up) up = false;
            if (e.KeyCode == Keys.Down) down = false;
        }
    }
}
