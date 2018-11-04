using System;

namespace YGJJ.Core.Cache
{
    public class CacheParam : IComparable<CacheParam>
    {
        public string Name { get; set; }
        public string Value { get; set; }
       
        public int CompareTo(CacheParam other)
        {
            return String.Compare(this.Name, other.Name, StringComparison.Ordinal);
        }
    }
}
