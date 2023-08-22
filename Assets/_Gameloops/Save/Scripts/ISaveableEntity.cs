using System;
using System.Collections.Generic;

namespace Gameloops.Save
{
    public interface ISaveableEntity<T>
    {
        Action OnSave { get; set; }
        void SetEntity(T entity);
        T GetEntity();
        T GetEntityDefault();
    }
}