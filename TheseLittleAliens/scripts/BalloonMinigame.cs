using Godot;

namespace TheseLittleAliens;

public partial class BalloonMinigame : Control
{
    // --- Tunables: nudge these in the Inspector to change how it feels ---
    [Export] public float PumpAmount = 0.09f;          // air added per space press
    [Export] public float DecayRate = 0.12f;            // air lost per second
    [Export] public float SuccessSecondsNeeded = 2.5f;  // time in the green zone needed to win
    [Export] public float PopCooldown = 0.7f;           // pause after a pop before trying again

    private MeterBar _meterBar;
    private BalloonVisual _balloon;
    private Label _statusLabel;
    private ProgressBar _successBar;
    private Control _winPanel;

    private float _fill;
    private float _successProgress;
    private bool _popped;
    private float _popTimer;
    private bool _won;

    public override void _Ready()
    {
        _meterBar = GetNode<MeterBar>("%MeterBar");
        _balloon = GetNode<BalloonVisual>("%BalloonVisual");
        _statusLabel = GetNode<Label>("%ZoneStatusLabel");
        _successBar = GetNode<ProgressBar>("%SuccessBar");
        _winPanel = GetNode<Control>("%WinPanel");

        var backButton = GetNode<Button>("%BackToTitleButton");
        backButton.Pressed += OnBackToTitlePressed;
    }

    public override void _Process(double delta)
    {
        if (_won) return;

        float dt = (float)delta;

        if (_popped)
        {
            _popTimer -= dt;
            if (_popTimer <= 0f) ResetAfterPop();
            return;
        }

        // ui_accept is bound to Space (and Enter) by default in every Godot project.
        if (Input.IsActionJustPressed("ui_accept"))
        {
            if (_fill >= 0.999f)
            {
                // Already full - one more press is greedy, and that's what pops it.
                TriggerPop();
                return;
            }

            _fill = Mathf.Min(1f, _fill + PumpAmount);
            PulseBalloon();
        }

        _fill = Mathf.Max(0f, _fill - DecayRate * dt);

        _meterBar.Fill = _fill;
        _balloon.Fill = _fill;

        bool inZone = _meterBar.IsInGreenZone;
        if (inZone)
            _statusLabel.Text = "Just right, keep it up!";
        else if (_fill < _meterBar.GreenZoneMin)
            _statusLabel.Text = "Needs more air...";
        else
            _statusLabel.Text = "Careful, that's a lot of air!";

        if (inZone)
        {
            _successProgress = Mathf.Min(SuccessSecondsNeeded, _successProgress + dt);
            _successBar.Value = _successProgress / SuccessSecondsNeeded * 100.0;

            if (_successProgress >= SuccessSecondsNeeded)
            {
                WinLevel();
            }
        }
    }

    private void PulseBalloon()
    {
        var tween = CreateTween();
        tween.TweenProperty(_balloon, "scale", new Vector2(1.15f, 1.15f), 0.05);
        tween.TweenProperty(_balloon, "scale", Vector2.One, 0.08);
    }

    private void TriggerPop()
    {
        _popped = true;
        _popTimer = PopCooldown;
        _balloon.Popped = true;
        _successProgress = 0f;
        _successBar.Value = 0.0;
        _statusLabel.Text = "Oops, it popped! Try again...";
    }

    private void ResetAfterPop()
    {
        _popped = false;
        _fill = 0f;
        _balloon.Popped = false;
        _balloon.Fill = 0f;
        _meterBar.Fill = 0f;
        _statusLabel.Text = "Needs more air...";
    }

    private void WinLevel()
    {
        _won = true;
        _winPanel.Visible = true;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Level1Complete = true;
        }
    }

    private void OnBackToTitlePressed()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GoToScene(GameManager.TitleScreenPath);
        else
            GetTree().ChangeSceneToFile(GameManager.TitleScreenPath);
    }
}
