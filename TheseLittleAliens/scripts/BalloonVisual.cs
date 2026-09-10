using Godot;

namespace TheseLittleAliens;

// Draws the balloon itself: a string, an egg-shaped body that grows with
// Fill, and a little burst effect when Popped is set.
public partial class BalloonVisual : Node2D
{
    [Export] public Color BalloonColor = new Color(0.95f, 0.4f, 0.55f);

    private const float MinRadius = 18f;
    private const float MaxRadius = 62f;

    private float _fill;
    public float Fill
    {
        get => _fill;
        set { _fill = Mathf.Clamp(value, 0f, 1f); QueueRedraw(); }
    }

    private bool _popped;
    public bool Popped
    {
        get => _popped;
        set { _popped = value; QueueRedraw(); }
    }

    public override void _Draw()
    {
        if (_popped)
        {
            DrawPopBurst();
            return;
        }

        float r = Mathf.Lerp(MinRadius, MaxRadius, _fill);

        // String
        DrawLine(new Vector2(0, r * 0.9f), new Vector2(0, r * 0.9f + 40), new Color(0.3f, 0.3f, 0.3f), 2f, true);

        // Balloon body (slightly egg-shaped ellipse)
        DrawColoredPolygon(BuildEllipsePoints(Vector2.Zero, r, r * 1.15f, 32), BalloonColor);

        // Highlight + knot
        DrawCircle(new Vector2(-r * 0.3f, -r * 0.35f), r * 0.22f, new Color(1, 1, 1, 0.5f));
        DrawCircle(new Vector2(0, r * 0.95f), r * 0.12f, BalloonColor.Darkened(0.15f));
    }

    private void DrawPopBurst()
    {
        for (int i = 0; i < 8; i++)
        {
            float angle = Mathf.Tau * i / 8f;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            DrawLine(dir * 10f, dir * 34f, BalloonColor, 4f, true);
        }
    }

    private static Vector2[] BuildEllipsePoints(Vector2 center, float rx, float ry, int segments)
    {
        var pts = new Vector2[segments];
        for (int i = 0; i < segments; i++)
        {
            float t = Mathf.Tau * i / segments;
            pts[i] = center + new Vector2(Mathf.Cos(t) * rx, Mathf.Sin(t) * ry);
        }
        return pts;
    }
}
