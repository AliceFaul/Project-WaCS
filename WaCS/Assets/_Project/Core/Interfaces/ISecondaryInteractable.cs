using _Project.Gameplay.Player;

public interface ISecondaryInteractable
{
    void SecondaryInteract(PlayerContext context);
    bool CanSecondaryInteract(PlayerContext context);
    string GetSecondaryPrompt();
}
