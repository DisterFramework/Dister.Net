namespace Dister.Net.Communication.Message
{
    internal enum MessageType
    {
        NoResponseRequest,
        ResponseRequest,
        Response,
        NullResponse,
        VariableSet,
        VariableGet,
        Enqueue,
        Dequeue,
        DictionaryGet,
        DictionarySet
    }
}
