using Microsoft.Maui.Graphics;

namespace ThogaiPay.Controls;

/// <summary>
/// Draws a login card with an asymmetric wave top, rounded lower corners and a subtle outline.
/// Shadow is intentionally omitted here because ICanvas shadow APIs vary across MAUI versions.
/// Add elevation using a platform-supported wrapper or Border.Shadow if required.
/// </summary>
public sealed class LoginWaveCardDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float width = dirtyRect.Width;
        float height = dirtyRect.Height;

        if ( width <= 0 || height <= 0 )
            return;

        float waveHeight = Math.Min(46f, height * 0.105f);
        float bottomRadius = Math.Min(28f, width * 0.075f);

        var path = new PathF();

        path.MoveTo(0, waveHeight * 0.72f);

        // Left shoulder
        path.CurveTo(
            width * 0.015f, waveHeight * 0.10f,
            width * 0.075f, 0,
            width * 0.19f, waveHeight * 0.08f);

        // Shallow central wave
        path.CurveTo(
            width * 0.34f, waveHeight * 0.18f,
            width * 0.52f, waveHeight * 0.90f,
            width * 0.70f, waveHeight * 0.74f);

        // Right shoulder
        path.CurveTo(
            width * 0.84f, waveHeight * 0.63f,
            width * 0.93f, waveHeight * 0.42f,
            width, waveHeight * 0.58f);

        // Rounded bottom corners
        path.LineTo(width, height - bottomRadius);
        path.QuadTo(width, height, width - bottomRadius, height);
        path.LineTo(bottomRadius, height);
        path.QuadTo(0, height, 0, height - bottomRadius);
        path.Close();

        canvas.FillColor = Color.FromArgb("#FFFEFE");
        canvas.FillPath(path);

        canvas.StrokeColor = Color.FromArgb("#E0E9F8");
        canvas.StrokeSize = 1.2f;
        canvas.DrawPath(path);
    }
}
