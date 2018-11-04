using System;
using System.Configuration.Provider;

namespace YGJJ.Core.Cache
{
    public class CacheProviderCollection : ProviderCollection
    {
        /// <summary>
        /// 向集合中添加提供程序。
        /// </summary>
        /// <param name="provider">要添加的提供程序。</param>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is CacheProvider))
                throw new ArgumentException("provider参数类型必须是CacheProvider.");

            base.Add(provider);
        }
    }
}
