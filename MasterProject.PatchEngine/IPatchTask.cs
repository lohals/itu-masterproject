namespace MasterProject.PatchEngine
{
    public interface IPatchTask
    {
        void Patch(TargetDocument targetDocument, ChangeDocument[] changes);
    }
}