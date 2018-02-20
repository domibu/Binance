using System;
using Kucoin.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Kucoin.Api
{
    /// <summary>
    /// A <see cref="IKucoinApiUser"/> provider allowing for the application of <see cref="KucoinApiOptions"/>.
    /// </summary>
    public sealed class KucoinApiUserProvider : IKucoinApiUserProvider
    {
        #region Private Fields

        private readonly IServiceProvider _services;

        private readonly IOptions<KucoinApiOptions> _options;

        #endregion Private Fields

        #region Constructors

        public KucoinApiUserProvider(IServiceProvider services, IOptions<KucoinApiOptions> options = null)
        {
            Throw.IfNull(services, nameof(services));

            _services = services;
            _options = options;
        }

        #endregion Constructors

        #region Public Methods

        public IKucoinApiUser CreateUser(string apiKey, string apiSecret = null)
        {
            return new KucoinApiUser(apiKey, apiSecret, _services.GetService<IApiRateLimiter>(), _options);
        }

        #endregion Public Methods
    }
}
