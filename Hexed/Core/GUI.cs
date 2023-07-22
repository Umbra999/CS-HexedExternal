using GameOverlay.Drawing;
using GameOverlay.Windows;
using Hexed.Extensions;
using Hexed.SDK.Base;
using Hexed.Wrappers;
using System.Drawing;
using System.Numerics;

namespace Hexed.Core
{
    internal class GUI
    {
        private static Font Consolas;
        private static SolidBrush TextBackground;
        private static SolidBrush DefaultText;

        private static void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            try
            {
                Graphics gfx = e.Graphics;

                gfx.ClearScene(gfx.CreateSolidBrush(0, 0, 0, 0));

                if (NativeMethods.GetForegroundWindow() != GameManager.Memory.Process.MainWindowHandle || !ConnectionManager.IsInWorld()) return;

                //gfx.DrawCrosshair(DefaultText, gfx.Width / 2, gfx.Height / 2, 4, 1, CrosshairStyle.Cross);

                DrawEnemies(gfx);
            }
            catch (Exception ex) { Logger.LogError(ex); }
        }

        private static void _window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
        {
            Graphics gfx = e.Graphics;

            if (e.RecreateResources) return;

            Consolas = gfx.CreateFont("Consolas", 14);
            TextBackground = gfx.CreateSolidBrush(0, 0, 0, 90);
            DefaultText = gfx.CreateSolidBrush(255, 255, 255);
        }

        public static void Run()
        {
            Graphics gfx = new()
            {
                //VSync = true,
                UseMultiThreadedFactories = true,
            };

            StickyWindow _window = new(GameManager.Memory.Process.MainWindowHandle, gfx)
            {
                IsTopmost = true,
                IsVisible = true,
                Title = "Hexed",
                MenuName = "Hexed",
                ClassName = "Hexed",
                ParentWindowHandle = GameManager.Memory.Process.MainWindowHandle,
            };

            _window.DrawGraphics += _window_DrawGraphics;
            _window.SetupGraphics += _window_SetupGraphics;

            _window.Create();
        }

        private static void DrawEnemies(Graphics gfx)
        {
            if (!Config.ESP) return;

            BasePlayer pLocal = PlayerManager.GetLocalPlayer();
            if (pLocal == null) return;

            foreach (BasePlayer Player in PlayerManager.GetPlayers())
            {
                if (Player.Address == pLocal.Address) continue;
                if (Player.IsDormant()) continue;
                if (Player.GetHealth() < 1) continue;
                if (Player.GetTeam() == pLocal.GetTeam()) continue;

                Vector3 WorldPlayerPos = Player.GetPosition();
                Vector3 WorldEyePos = Player.GetEyePos();

                Vector2 Player2DPos = Wrappers.Math.WorldToScreen(new Vector3(WorldPlayerPos.X, WorldPlayerPos.Y, WorldPlayerPos.Z - 5), new Vector2(gfx.Width, gfx.Height));
                Vector2 Player2DHeadPos = Wrappers.Math.WorldToScreen(new Vector3(WorldEyePos.X, WorldEyePos.Y, WorldEyePos.Z + 10), new Vector2(gfx.Width, gfx.Height));

                float BoxHeight = Player2DPos.Y - Player2DHeadPos.Y;
                float BoxWidth = BoxHeight / 2 * 1.25f;

                System.Drawing.Color HealthColor = Wrappers.Math.MapHealthToColor(Player.GetHealth());
                SolidBrush color = gfx.CreateSolidBrush(HealthColor.R, HealthColor.G, HealthColor.B);

                DrawOutlineBox(gfx, Player2DPos.X - (BoxWidth / 2), Player2DHeadPos.Y, BoxWidth, BoxHeight, color);
            }
        }

        private static void DrawOutlineBox(Graphics gfx, float x, float y, float width, float height, SolidBrush Color, float thiccness = 1)
        {
            gfx.OutlineRectangle(Color, Color, x, y, x + width, y + height, thiccness);
        }
    }
}
