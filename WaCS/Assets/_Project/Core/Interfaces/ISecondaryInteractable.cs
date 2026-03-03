using _Project.Gameplay.Player;

public interface ISecondaryInteractable
{
    bool CanSecondaryInteract(PlayerContext context);
    string GetSecondaryPrompt();
    void SecondaryInteract(PlayerContext context);
}
