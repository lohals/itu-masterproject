namespace MasterProject.PatchEngine.PatchTasks
{
    public interface IPatchTask
    {
        void Patch(TargetDocument targetDocument, ChangeDocument[] changes);
    }
}