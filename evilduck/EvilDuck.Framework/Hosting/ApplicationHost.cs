using Topshelf;

namespace EvilDuck.Framework.Hosting
{
    public class ApplicationHost
    {
        public static void Run<TService>() where TService : class, IHostableApplication, new()
        {
            var conf = HostFactory.New(
                x =>
                {
                    var application = new TService();
                    x.Service<TService>(
                        s =>
                        {
                            s.ConstructUsing(f => application);
                            s.WhenStarted(ts => ts.Start());
                            s.WhenStopped(ts => ts.Stop());
                        });

                    var runAs = application.RunAs;
                    runAs.Configure(x);

                });

            conf.Run();
        }
    }
}
