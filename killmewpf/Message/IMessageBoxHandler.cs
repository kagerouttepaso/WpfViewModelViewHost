using MessagePipe;
using System.Windows;

namespace killmewpf.Message
{
    public interface IMessageBoxHandler : IAsyncRequestHandler<string, MessageBoxResult?> { }
}
