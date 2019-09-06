
public abstract class Command<TReceiver> : ICommand
{
    /// <summary>
    /// Defines the generic receriver of the command
    /// </summary>
    protected TReceiver receiver;

    /// <summary>
    /// Constructs a command using a generic receiver
    /// </summary>
    /// <param name="receiver">The generic receiver object</param>
    protected Command(TReceiver receiver)
    {
        this.receiver = receiver;
    }

    /// <summary>
    /// Executes set action of the given receiver
    /// </summary>
    public abstract void Execute();
}

abstract class PlayerCommand : Command<Player>
{
    protected PlayerCommand(Player player) : base(player)
    {
    }
}
