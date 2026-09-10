using Godot;

namespace TheseLittleAliens;

// A simple procedural night-sky backdrop shared by every screen so the
// project doesn't depend on any external image files. Swap this out for
// real art later if you want - nothing else references it directly.
public partial class SpaceBackground : Node2D
{
    [Export] public Vector2 ViewportSize = new Vector2(1280, 720);
    [Export] public int StarCount = 60;

    private Vector2[] _stars = System.Array.Empty<Vector2>();
    private float[] _starSizes = System.Array.Empty<float>();

    public override void _Ready()
    {
        var rng = new RandomNumberGenerator();
        rng.Seed = 42; // fixed seed so stars don't jump around on reload

        _stars = new Vector2[StarCount];
        _starSizes = new float[StarCount];
        for (int i = 0; i < StarCount; i++)
        {
            _stars[i] = new Vector2(
                rng.RandfRange(0, ViewportSize.X),
                rng.RandfRange(0, ViewportSize.Y));
            _starSizes[i] = rng.RandfRange(1.0f, 2.6f);
        }

        ZIndex = -100;
        QueueRedraw();
    }

    public override void _Draw()
    {
        Color top = new Color(0.16f, 0.10f, 0.34f);
        Color bottom = new Color(0.40f, 0.20f, 0.52f);
        const int bands = 24;
        for (int i = 0; i < bands; i++)
        {
            float t0 = (float)i / bands;
            float t1 = (float)(i + 1) / bands;
            Color c = top.Lerp(bottom, t0);
            DrawRect(new Rect2(0, ViewportSize.Y * t0, ViewportSize.X, ViewportSize.Y * (t1 - t0) + 1f), c, true);
        }

        for (int i = 0; i < _stars.Length; i++)
        {
            DrawCircle(_stars[i], _starSizes[i], new Color(1, 1, 1, 0.85f));
        }
    }
}
