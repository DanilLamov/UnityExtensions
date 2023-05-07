using System.Collections.Generic;

namespace Lamov.UnityExtensions.Runtime
{
    public static class CollectionExtensions
    {
        public static bool ListEquals<T>(this List<T> a, List<T> b) 
        {
            if (a == null) return b == null;
            if (b == null || a.Count != b.Count) return false;
            for (var i = 0; i < a.Count; i++) if (!Equals(a[i], b[i])) return false;
            
            return true;
        }
    }
}