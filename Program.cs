using Raylib_cs;
using System;

Raylib.InitWindow(800, 600, "Dibujo bob");
Raylib.SetTargetFPS(60);

Color[] colors = new Color[]
{
    new Color(255, 255, 0, 255),
    new Color(26, 64, 133, 255),
    new Color(90, 78, 46, 255),
    new Color(225, 252, 200, 255),
    new Color(240, 96, 29, 255)
};

Color colorSeleccionado = Color.RayWhite;

int indexA = -1;
int indexB = -1;

int w = 100;
int h = 100;
int y = 450;


int sliderX = 730;
int sliderY = 80;
int sliderW = 30;
int sliderH = 300;
float alphaValue = 255f;     
bool draggingAlpha = false;

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RayWhite);

    System.Numerics.Vector2 mouse = Raylib.GetMousePosition();

  
    for (int i = 0; i < colors.Length; i++)
    {
        int x = 50 + i * 150;
        Rectangle rec = new Rectangle(x, y, w, h);

        if (Raylib.CheckCollisionPointRec(mouse, rec))
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                indexA = i;
                colorSeleccionado = colors[i];
            }

            if (Raylib.IsMouseButtonPressed(MouseButton.Right))
            {
                indexB = i;
            }
        }
    }

  
    Rectangle sliderHit = new Rectangle(sliderX - 10, sliderY - 10, sliderW + 20, sliderH + 20);

    if (Raylib.IsMouseButtonPressed(MouseButton.Left) && Raylib.CheckCollisionPointRec(mouse, sliderHit))
        draggingAlpha = true;

    if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        draggingAlpha = false;

    if (draggingAlpha)
    {
        float t = (mouse.Y - sliderY) / (float)sliderH;   
        t = Math.Clamp(t, 0f, 1f);
        alphaValue = (1f - t) * 255f;                     
    }

    byte alphaByte = (byte)Math.Round(alphaValue);

 
    Color cA = indexA >= 0 ? colors[indexA] : Color.RayWhite;
    Color cB = indexB >= 0 ? colors[indexB] : Color.RayWhite;

    int bigW = 600;
    int bigH = 250;
    int bigX = (800 - bigW) / 2;
    int bigY = 20;

    Raylib.DrawRectangle(bigX, bigY, bigW, bigH, cB);
    Raylib.DrawRectangle(bigX, bigY, bigW, bigH, new Color(cA.R, cA.G, cA.B, alphaByte));
    Raylib.DrawRectangleLines(bigX, bigY, bigW, bigH, Color.Black);

    float af = alphaByte / 255f;
    byte mr = (byte)Math.Round(cA.R * af + cB.R * (1f - af));
    byte mg = (byte)Math.Round(cA.G * af + cB.G * (1f - af));
    byte mb = (byte)Math.Round(cA.B * af + cB.B * (1f - af));
    Color blended = new Color(mr, mg, mb, (byte)255);
    string blendedHex = $"#{blended.R:X2}{blended.G:X2}{blended.B:X2}";

    Raylib.DrawRectangle(bigX + 10, bigY + 10, 60, 60, cA);
    Raylib.DrawRectangleLines(bigX + 10, bigY + 10, 60, 60, Color.Black);
    Raylib.DrawRectangle(bigX + 80, bigY + 10, 60, 60, cB);
    Raylib.DrawRectangleLines(bigX + 80, bigY + 10, 60, 60, Color.Black);


    Raylib.DrawRectangle(bigX + 5, bigY + bigH - 58, 420, 53, new Color(255, 255, 255, 210));
    Raylib.DrawText($"A sobre B   alpha {alphaByte} ({alphaByte / 255f * 100f:0}%)",
                    bigX + 12, bigY + bigH - 52, 20, Color.Black);
    Raylib.DrawText($"resultado: {blendedHex}",
                    bigX + 12, bigY + bigH - 28, 20, Color.Black);

   
    if (indexA >= 0 && indexB >= 0)
    {
        Color gA = colors[indexA];
        Color gB = colors[indexB];

        int gradCount = 10;
        int gradW = 70;
        int gradH = 70;
        int gradY = 300;
        int totalW = gradCount * gradW;
        int gradX = (800 - totalW) / 2;

        for (int i = 0; i < gradCount; i++)
        {
            float t = i / (float)(gradCount - 1);

            byte r = (byte)(gA.R + (gB.R - gA.R) * t);
            byte g = (byte)(gA.G + (gB.G - gA.G) * t);
            byte b = (byte)(gA.B + (gB.B - gA.B) * t);

            Color c = new Color(r, g, b, (byte)255);
            int gx = gradX + i * gradW;

            Raylib.DrawRectangle(gx, gradY, gradW, gradH, c);
            Raylib.DrawRectangleLines(gx, gradY, gradW, gradH, Color.Black);

            string hex = $"#{c.R:X2}{c.G:X2}{c.B:X2}";
            Raylib.DrawText(hex, gx + 2, gradY + gradH + 2, 10, Color.Black);
        }
    }

    for (int i = 0; i < sliderH; i++)
    {
        float t = i / (float)(sliderH - 1);
        float a = 1f - t;        // 1 arriba -> 0 abajo
        byte r = (byte)Math.Round(cA.R * a + cB.R * (1f - a));
        byte g = (byte)Math.Round(cA.G * a + cB.G * (1f - a));
        byte b = (byte)Math.Round(cA.B * a + cB.B * (1f - a));

        Raylib.DrawRectangle(sliderX, sliderY + i, sliderW, 1, new Color(r, g, b, (byte)255));
    }
    Raylib.DrawRectangleLines(sliderX, sliderY, sliderW, sliderH, Color.Black);

   
    int knobY = sliderY + (int)Math.Round((1f - alphaValue / 255f) * sliderH);
    Raylib.DrawRectangle(sliderX - 8, knobY - 4, sliderW + 16, 8, Color.Black);
    Raylib.DrawRectangle(sliderX - 6, knobY - 2, sliderW + 12, 4, Color.White);

    Raylib.DrawText("Alpha", sliderX - 8, sliderY - 28, 20, Color.Black);
    Raylib.DrawText($"{alphaByte}", sliderX - 5, sliderY + sliderH + 10, 20, Color.Black);
    Raylib.DrawText($"{alphaByte / 255f * 100f:0}%", sliderX - 5, sliderY + sliderH + 32, 20, Color.Black);

   
    for (int i = 0; i < colors.Length; i++)
    {
        int x = 50 + i * 150;
        Rectangle rec = new Rectangle(x, y, w, h);

        Raylib.DrawRectangleRec(rec, colors[i]);

        Color border = Raylib.CheckCollisionPointRec(mouse, rec)
            ? Color.Black
            : Color.Gray;
        Raylib.DrawRectangleLinesEx(rec, 2, border);

        string hex = $"#{colors[i].R:X2}{colors[i].G:X2}{colors[i].B:X2}";
        Raylib.DrawText(hex, x, y + h + 10, 20, Color.Black);

        if (i == indexA && i == indexB)
        {
            Raylib.DrawText("A", x + 5,  y + 5, 30, Color.Black);
            Raylib.DrawText("B", x + 40, y + 5, 30, Color.Black);
        }
        else if (i == indexA)
        {
            Raylib.DrawText("A", x + 5, y + 5, 30, Color.Black);
        }
        else if (i == indexB)
        {
            Raylib.DrawText("B", x + 5, y + 5, 30, Color.Black);
        }
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();