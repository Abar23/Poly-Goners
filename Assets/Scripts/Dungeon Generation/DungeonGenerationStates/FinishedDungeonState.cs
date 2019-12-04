public class FinishedDungeonState : IDungeonGenerationState
{
    public FinishedDungeonState(DungeonGenerator generator)
    {
        generator.BottomRooms.Clear();
        generator.TopRooms.Clear();
        generator.LeftRooms.Clear();
        generator.RightRooms.Clear();
        generator.StartRooms.Clear();
        generator.Shops.Clear();
        generator.GoalRooms.Clear();
        generator.DeadEnds.Clear();

        generator.GetOnGenerationFinished().Invoke();
        DungeonCompletionTracker.GetInstance().UpdateLevelText(generator);

        if (generator.GetAnimator() != null)
        {
            generator.GetAnimator().SetTrigger("Enter");
        }
    }

    public IDungeonGenerationState Update()
    {
        return null;
    }
}
