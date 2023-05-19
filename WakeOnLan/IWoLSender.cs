namespace TeleWoL.WakeOnLan;

internal interface IWoLSender : IDisposable
{
    Task Send(byte[] data);
}
