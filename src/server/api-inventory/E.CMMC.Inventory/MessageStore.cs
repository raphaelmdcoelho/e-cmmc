public static class MessageStore
{
    private static readonly List<string> _messages = new List<string>();

    public static void AddMessage(string message)
    {
        _messages.Add(message);
        // Optionally, you might want to limit the size of this list
    }

    public static IEnumerable<string> GetAllMessages()
    {
        return _messages;
    }

    public static string GetLatestMessage()
    {
        return _messages.LastOrDefault();
    }
}
