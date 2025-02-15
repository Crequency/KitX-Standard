using System.Collections.Concurrent;
using KitX.Shared.CSharp.Device;

namespace Kscript.CSharp.Utils
{
    /// <summary>
    /// 设备信息缓存管理器
    /// </summary>
    public class DeviceCache : IDisposable
    {
        private class CacheEntry<T>
        {
            public T Value { get; set; }
            public DateTime Expiration { get; set; }
            public DateTime CreatedAt { get; } = DateTime.UtcNow;
            private int _accessCount;

            public bool IsExpired => DateTime.UtcNow > Expiration;

            public int GetAccessCount()
            {
                return Interlocked.CompareExchange(ref _accessCount, 0, 0);
            }

            public void IncrementAccess()
            {
                Interlocked.Increment(ref _accessCount);
            }
        }

        private readonly ConcurrentDictionary<string, CacheEntry<IEnumerable<DeviceInfo>>> _cache = new();
        private readonly TimeSpan _defaultExpiration;
        private readonly Timer _cleanupTimer;
        private volatile bool _isDisposed;

        /// <summary>
        /// 初始化设备缓存
        /// </summary>
        /// <param name="defaultExpiration">默认的缓存过期时间</param>
        /// <param name="cleanupInterval">缓存清理的时间间隔</param>
        public DeviceCache(
            TimeSpan? defaultExpiration = null,
            TimeSpan? cleanupInterval = null)
        {
            _defaultExpiration = defaultExpiration ?? TimeSpan.FromMinutes(1);
            var interval = cleanupInterval ?? TimeSpan.FromMinutes(5);
            
            _cleanupTimer = new Timer(
                CleanupCallback,
                null,
                interval,
                interval
            );
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        public void Set(string key, IEnumerable<DeviceInfo> value, TimeSpan? expiration = null)
        {
            var entry = new CacheEntry<IEnumerable<DeviceInfo>>
            {
                Value = value,
                Expiration = DateTime.UtcNow.Add(expiration ?? _defaultExpiration)
            };

            _cache.AddOrUpdate(key, _ => entry, (_, __) => entry);
        }

        /// <summary>
        /// 尝试获取缓存项
        /// </summary>
        public IEnumerable<DeviceInfo>? Get(string key)
        {
            var entry = _cache.GetOrAdd(key, _ => null);
            if (entry == null || entry.IsExpired)
            {
                _cache.TryRemove(key, out _);
                return null;
            }

            entry.IncrementAccess();
            return entry.Value;
        }

        /// <summary>
        /// 检查缓存项是否存在且有效
        /// </summary>
        public bool IsValid(string key)
        {
            var entry = _cache.GetOrAdd(key, _ => null);
            return entry != null && !entry.IsExpired;
        }

        /// <summary>
        /// 获取缓存项，如果不存在或已过期则使用提供的函数获取新值
        /// </summary>
        public async Task<IEnumerable<DeviceInfo>> GetOrAdd(
            string key,
            Func<Task<IEnumerable<DeviceInfo>>> valueFactory,
            TimeSpan? expiration = null)
        {
            var existingValue = Get(key);
            if (existingValue != null)
                return existingValue;

            var newValue = await valueFactory();
            Set(key, newValue, expiration);
            return newValue;
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        public bool Remove(string key)
        {
            return _cache.TryRemove(key, out _);
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public CacheStatistics GetStatistics()
        {
            var now = DateTime.UtcNow;
            var entries = _cache.ToArray();

            return new CacheStatistics
            {
                TotalItems = entries.Length,
                ExpiredItems = entries.Count(x => x.Value.IsExpired),
                TotalAccesses = entries.Sum(x => x.Value.GetAccessCount()),
                AverageAccessesPerItem = entries.Length > 0
                    ? entries.Average(x => x.Value.GetAccessCount())
                    : 0,
                OldestItemAge = entries.Length > 0
                    ? (now - entries.Min(x => x.Value.CreatedAt))
                    : TimeSpan.Zero,
                AverageItemAge = entries.Length > 0
                    ? TimeSpan.FromTicks((long)entries.Average(x => 
                        (now - x.Value.CreatedAt).Ticks))
                    : TimeSpan.Zero
            };
        }

        private void CleanupCallback(object? state)
        {
            if (_isDisposed)
                return;

            var expiredKeys = _cache
                .Where(kvp => kvp.Value.IsExpired)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _cache.TryRemove(key, out _);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _cleanupTimer.Dispose();
            Clear();
        }
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {
        public int TotalItems { get; set; }
        public int ExpiredItems { get; set; }
        public int TotalAccesses { get; set; }
        public double AverageAccessesPerItem { get; set; }
        public TimeSpan OldestItemAge { get; set; }
        public TimeSpan AverageItemAge { get; set; }
    }
}