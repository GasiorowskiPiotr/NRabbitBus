using System;
using System.Configuration;

namespace EvilDuck.Framework.Configuration
{
    public abstract class Configuration<TSection> where TSection : ConfigurationSection
    {
        private static readonly Lazy<TSection> CurrentSection =
            new Lazy<TSection>(() =>
                                   {
                                       try
                                       {
                                           return (TSection) ConfigurationManager.GetSection(typeof (TSection).Name);
                                       }
                                       catch (Exception)
                                       {
                                           return null;
                                       }
                                       
                                   });

        public static TSection Current
        {
            get { return CurrentSection.Value; }
        }
    }
}
