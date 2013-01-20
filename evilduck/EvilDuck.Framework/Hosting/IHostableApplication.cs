namespace EvilDuck.Framework.Hosting
{
    public interface IHostableApplication
    {
        void Start();
        void Stop();
        RunAs RunAs { get; }
        string Describtion { get; }
        string ServiceName { get; }
        string DisplayName { get; }
    }
}