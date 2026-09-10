using Godot;

namespace TheseLittleAliens;

// A simple, cute, fully procedural alien - no image files required.
// Set BodyColor per-instance in the scene to get a little crowd of them,
// each one bobs and "breathes" gently so the title screen feels alive.
public partial class Alien : Node2D
{
    [Export] public Color BodyColor = new Color(0.45f, 0.85f, 0.45f);
    [Export] public bool Animate = true;

    private Vector2 _basePosition;
    private float _time;
    private const float BodyRadius = 40f;

    public override void _Ready()
    {
        _basePosition = Position;
        _time = (float)GD.RandRange(0.0, 10.0); // desync the bob/breathe cycle between aliens
        QueueRedraw();
    }

    public override void _Process(double delta)
    {
        if (!Animate) return;

        _time += (float)delta;
        Position = _basePosition + new Vector2(0, Mathf.Sin(_time * 2.0f) * 4f);
        float breathe = 1f + Mathf.Sin(_time * 1.4f) * 0.03f;
        Scale = new Vector2(breathe, breathe);
    }

    public override void _Draw()
    {
        float r = BodyRadius;
        Vector2 center = Vector2.Zero;
        Color outline = BodyColor.Darkened(0.25f);
        Color dark = new Color(0.1f, 0.1f, 0.15f);

        // Body
        DrawCircle(center, r, BodyColor);
        DrawArc(center, r, 0, Mathf.Tau, 48, outline, 3f, true);

        // Antennae
        Vector2 antBase1 = center + new Vector2(-r * 0.35f, -r * 0.85f);
        Vector2 antBase2 = center + new Vector2(r * 0.35f, -r * 0.85f);
        Vector2 antTip1 = antBase1 + new Vector2(-8, -20);
        Vector2 antTip2 = antBase2 + new Vector2(8, -20);
        DrawLine(antBase1, antTip1, outline, 4f, true);
        DrawLine(antBase2, antTip2, outline, 4f, true);
        DrawCircle(antTip1, 5f, Colors.White);
        DrawCircle(antTip2, 5f, Colors.White);

        // Eyes
        float eyeOffsetX = r * 0.38f;
        float eyeOffsetY = -r * 0.05f;
        float eyeRadius = r * 0.34f;
        Vector2 eyeL = center + new Vector2(-eyeOffsetX, eyeOffsetY);
        Vector2 eyeR = center + new Vector2(eyeOffsetX, eyeOffsetY);

        DrawCircle(eyeL, eyeRadius, Colors.White);
        DrawCircle(eyeR, eyeRadius, Colors.White);
        DrawCircle(eyeL + new Vector2(eyeRadius * 0.25f, eyeRadius * 0.25f), eyeRadius * 0.5f, dark);
        DrawCircle(eyeR + new Vector2(eyeRadius * 0.25f, eyeRadius * 0.25f), eyeRadius * 0.5f, dark);
        DrawCircle(eyeL + new Vector2(eyeRadius * 0.05f, -eyeRadius * 0.1f), eyeRadius * 0.15f, Colors.White);
        DrawCircle(eyeR + new Vector2(eyeRadius * 0.05f, -eyeRadius * 0.1f), eyeRadius * 0.15f, Colors.White);

        // Smile
        Vector2 mouthCenter = center + new Vector2(0, r * 0.42f);
        DrawArc(mouthCenter, r * 0.28f, Mathf.Pi * 0.15f, Mathf.Pi * 0.85f, 16, dark, 3f, true);

        // Feet nubs
        DrawCircle(center + new Vector2(-r * 0.4f, r * 0.85f), r * 0.18f, outline);
        DrawCircle(center + new Vector2(r * 0.4f, r * 0.85f), r * 0.18f, outline);
    }
}
