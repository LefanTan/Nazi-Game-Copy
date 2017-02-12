using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    // Zero parameters
    public abstract class MemoryPool<TValue> : MemoryPoolBase<TValue>, IMemoryPool<TValue>
    {
        public TValue Spawn()
        {
            var item = GetInternal();
            Reinitialize(item);
            return item;
        }

        protected virtual void Reinitialize(TValue item)
        {
            // Optional
        }
    }

    // One parameter
    public abstract class MemoryPool<TParam1, TValue>
        : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TValue>
    {
        public TValue Spawn(TParam1 param)
        {
            var item = GetInternal();
            Reinitialize(item, param);
            return item;
        }

        protected virtual void Reinitialize(TValue item, TParam1 p1)
        {
            // Optional
        }
    }

    // Two parameters
    public abstract class MemoryPool<TParam1, TParam2, TValue>
        : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TValue>
    {
        public TValue Spawn(TParam1 param1, TParam2 param2)
        {
            var item = GetInternal();
            Reinitialize(item, param1, param2);
            return item;
        }

        protected abstract void Reinitialize(TValue item, TParam1 p1, TParam2 p2);
    }

    // Three parameters
    public abstract class MemoryPool<TParam1, TParam2, TParam3, TValue>
        : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TValue>
    {
        public TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            var item = GetInternal();
            Reinitialize(item, param1, param2, param3);
            return item;
        }

        protected abstract void Reinitialize(TValue item, TParam1 p1, TParam2 p2, TParam3 p3);
    }

    // Four parameters
    public abstract class MemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
        : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
    {
        public TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            var item = GetInternal();
            Reinitialize(item, param1, param2, param3, param4);
            return item;
        }

        protected abstract void Reinitialize(TValue item, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4);
    }

    // Five parameters
    public abstract class MemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        : MemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
    {
        public TValue Spawn(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            var item = GetInternal();
            Reinitialize(item, param1, param2, param3, param4, param5);
            return item;
        }

        protected abstract void Reinitialize(TValue item, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5);
    }
}
