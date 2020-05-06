using Microsoft.Extensions.Logging;
using Moq;
using sm_coding_challenge.Services.CacheProvider;
using StackExchange.Redis;
using System;
using Xunit;

namespace sm_coding_challenge.Test
{
    public class CacheProviderTests
    {
        private Mock<IConnectionMultiplexer> _connectionMultiplexer;
        private Mock<ILogger<RedisCacheProvider>> _logger;

        [Fact]
        public void GetPlayerWithEmptyStringReturnsNull()
        {
            _connectionMultiplexer = new Mock<IConnectionMultiplexer>();
            _logger = new Mock<ILogger<RedisCacheProvider>>();
            RedisCacheProvider _cache = new RedisCacheProvider(_connectionMultiplexer.Object, _logger.Object);
            Assert.Null(_cache.GetPlayerAsync("").Result);
        }
    }
}
