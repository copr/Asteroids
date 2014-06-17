using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public interface IGameRoom
    {
        double RoomWidth { get; }
        double RoomHeight { get; }

        IEnumerable<T> GetObjectsOfType<T>();
    }
}
