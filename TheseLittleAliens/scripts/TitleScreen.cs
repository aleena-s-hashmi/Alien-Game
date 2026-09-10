using Godot;
using System.Threading.Tasks;

namespace TheseLittleAliens;

public partial class TitleScreen : Control
{
    private Button _playButton;
    private Button _levelsButton;
    private Button _settingsButton;
    private Label _comingSoonLabel;

    public override void _Ready()
    {
        _playButton = GetNode<Button>("%PlayButton");
        _levelsButton = GetNode<Button>("%LevelsButton");
        _settingsButton = GetNode<Button>("%SettingsButton");
        _comingSoonLabel = GetNode<Label>("%ComingSoonLabel");
        _comingSoonLabel.Visible = false;

        StyleButton(_playButton, new Color(0.35f, 0.75f, 0.45f));
        StyleButton(_levelsButton, new Color(0.35f, 0.55f, 0.85f));
        StyleButton(_settingsButton, new Color(0.6f, 0.6f, 0.65f));

        _playButton.Pressed += OnPlayPressed;
        // Levels/Settings aren't built yet - for now they just show a quick note.
        _levelsButton.Pressed += () => _ = ShowComingSoon("Levels");
        _settingsButton.Pressed += () => _ = ShowComingSoon("Settings");
    }

    private void OnPlayPressed()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GoToScene(GameManager.Level1Path);
        else
            GetTree().ChangeSceneToFile(GameManager.Level1Path);
    }

    private async Task ShowComingSoon(string what)
    {
        _comingSoonLabel.Text = $"{what} coming soon!";
        _comingSoonLabel.Visible = true;
        await ToSignal(GetTree().CreateTimer(1.4), SceneTreeTimer.SignalName.Timeout);
        _comingSoonLabel.Visible = false;
    }

    private void StyleButton(Button button, Color color)
    {
        var normal = new StyleBoxFlat
        {
            BgColor = color,
            CornerRadiusTopLeft = 16,
            CornerRadiusTopRight = 16,
            CornerRadiusBottomLeft = 16,
            CornerRadiusBottomRight = 16,
            ContentMarginLeft = 24,
            ContentMarginRight = 24,
            ContentMarginTop = 10,
            ContentMarginBottom = 10
        };
        var hover = (StyleBoxFlat)normal.Duplicate();
        hover.BgColor = color.Lightened(0.15f);
        var pressed = (StyleBoxFlat)normal.Duplicate();
        pressed.BgColor = color.Darkened(0.15f);

        button.AddThemeStyleboxOverride("normal", normal);
        button.AddThemeStyleboxOverride("hover", hover);
        button.AddThemeStyleboxOverride("pressed", pressed);
        button.AddThemeStyleboxOverride("focus", normal);
        button.AddThemeColorOverride("font_color", Colors.White);
        button.AddThemeFontSizeOverride("font_size", 24);
    }
}
