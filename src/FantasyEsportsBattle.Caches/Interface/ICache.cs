using System;
using System.Collections.Generic;
using System.Text;

namespace FantasyEsportsBattle.Caches.Interface
{
    public interface ICache<CacheObject>
    {
        ICollection<CacheObject> Values { get; }
        Action<CacheObject> OnItemAdded { get; set; }
        Action<CacheObject> OnItemRemoved { get; set; }
        bool AddItem(CacheObject item);
        bool BulkAddItem(ICollection<CacheObject> items);
        bool RemoveItem(CacheObject item);
        bool BulkRemoveItem(ICollection<CacheObject> items);
    }
}
