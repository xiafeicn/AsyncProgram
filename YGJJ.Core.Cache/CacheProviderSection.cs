using System.Configuration;

namespace YGJJ.Core.Cache
{
    public class CacheProviderSection : ConfigurationSection
    {
        private readonly ConfigurationProperty _defaultProvider;
        private readonly ConfigurationProperty _providers;
        private readonly ConfigurationPropertyCollection _properties;
        private readonly ConfigurationProperty _enabled;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheProviderSection()
        {
            _defaultProvider = new ConfigurationProperty("defaultProvider", typeof(string), null);
            _providers = new ConfigurationProperty("providers", typeof(ProviderSettingsCollection), null);
            _properties = new ConfigurationPropertyCollection();
            _properties.Add(_providers);
            _properties.Add(_defaultProvider);
        }
         
        [ConfigurationProperty("defaultProvider")]
        public string DefaultProvider
        {
            get { return (string)base[_defaultProvider]; }
            set { base[_defaultProvider] = value; }
        }

        [ConfigurationProperty("providers", DefaultValue = "RedisProvider")]
        [StringValidator(MinLength = 1)]
        public ProviderSettingsCollection Providers
        {
            get { return (ProviderSettingsCollection)base[_providers]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
