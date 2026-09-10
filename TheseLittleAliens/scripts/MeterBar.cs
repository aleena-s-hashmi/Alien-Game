using Godot;

namespace TheseLittleAliens;

// Draws the vertical inflation meter: a background track, a highlighted
// "green zone" band, a red "danger" band near the top, and a rising fill.
// Everything is drawn relative to this node's own origin, so just move
// the node to place the bar anywhere on screen.
public partial class MeterBar : Node2D
{
    [Export] public float BarWidth = 46f;
    [Export] public float BarHeight = 320f;
    [Export] public float GreenZoneMin = 0.45f;
    [Export] public float GreenZoneMax = 0.68f;

    private float _fill;
    public float Fill
    {
        get => _fill;
        set
        {
            _fill = Mathf.Clamp(value, 0f, 1f);
            QueueRedraw();
        }
    }

    public bool IsInGreenZone => _fill >= GreenZoneMin && _fill <= GreenZoneMax;

    public override void _Draw()
    {
        var bgRect = new Rect2(0, 0, BarWidth, BarHeight);
        DrawRect(bgRect, new Color(0.15f, 0.15f, 0.2f, 0.9f), true);

        // Green target zone
        float zoneTopY = BarHeight * (1f - GreenZoneMax);
        float zoneBottomY = BarHeight * (1f - GreenZoneMin);
        DrawRect(new Rect2(0, zoneTopY, BarWidth, zoneBottomY - zoneTopY), new Color(0.35f, 0.85f, 0.4f, 0.5f), true);

        // Danger band near the very top - a warning that popping is close
        DrawRect(new Rect2(0, 0, BarWidth, BarHeight * 0.08f), new Color(0.9f, 0.3f, 0.3f, 0.35f), true);

        // Fill
        if (_fill > 0f)
        {
            float fillTopY = BarHeight * (1f - _fill);
            Color fillColor = new Color(1f, 0.85f, 0.3f);
            if (IsInGreenZone) fillColor = new Color(0.4f, 0.95f, 0.5f);
            else if (_fill > 0.92f) fillColor = new Color(1f, 0.4f, 0.35f);
            DrawRect(new Rect2(2, fillTopY, BarWidth - 4, BarHeight - fillTopY - 2), fillColor, true);
        }

        // Outline drawn last so it stays crisp on top of the fill/zone colors
        DrawRect(bgRect, new Color(1, 1, 1, 0.6f), false, 3f);
    }
}
