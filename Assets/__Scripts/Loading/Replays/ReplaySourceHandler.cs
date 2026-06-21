public static class ReplaySourceHandler
{
    public static ReplayStreamingSocket Stream { get; private set; }


    private static void HandleUIStateChanged(UIState newState)
    {
        // Avoid resetting when the map loader is currently loading
        // This is because we go back to map selection when loading a new map for the stream
        if (newState == UIState.MapSelection && !MapLoader.Loading)
        {
            Reset();
        }
    }


    public static void HandleStreamClosed(ReplayStreamingSocket closedStream)
    {
        if (closedStream != Stream)
        {
            return;
        }

        // Simply doing this *should* reset all state everywhere
        // (I don't really remember all this shit that well)
        UIStateManager.OnUIStateChanged -= HandleUIStateChanged;
        UIStateManager.CurrentState = UIState.MapSelection;
    }


    public static void SetStream(ReplayStreamingSocket stream)
    {
        if (Stream != null)
        {
            Reset();
        }

        Stream = stream;
        UIStateManager.OnUIStateChanged += HandleUIStateChanged;
    }


    public static void Reset()
    {
        Stream?.Dispose();
        Stream = null;
    }
}