using Godot;

namespace TheseLittleAliens;

// Autoloaded singleton (see project.godot [autoload]). Handles switching
// scenes and remembers simple progress, so a future "Levels" screen has
// something to read from.
public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    public const string TitleScreenPath = "res://scenes/TitleScreen.tscn";
    public const string Level1Path = "res://scenes/Level1_Balloon.tscn";

    public bool Level1Complete = false;

    public override void _EnterTree()
    {
        Instance = this;
    }

    public void GoToScene(string scenePath)
    {
        GetTree().ChangeSceneToFile(scenePath);
    }
}
