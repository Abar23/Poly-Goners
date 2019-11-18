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
    }

    public IDungeonGenerationState Update()
    {
        return null;
    }
}
