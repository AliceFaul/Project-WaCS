using _Project.Gameplay.Player;

public interface IInteractable
{
    void Interact(PlayerContext context);
    bool CanInteract(PlayerContext context);
    string GetPrompt();
}