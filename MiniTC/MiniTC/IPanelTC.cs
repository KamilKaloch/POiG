namespace MiniTC
{ 
    interface IPanelTC
    {
        string CurrentPath { get; }
        string[] AvailableDrives { get; }
        string[] AvailablePaths { get; }
    }
}