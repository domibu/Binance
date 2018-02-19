using System;

namespace Kucoin
{
    public interface IChronological
    {
        #region Properties

        /// <summary>
        /// Get the time (UTC).
        /// </summary>
        DateTime Time { get; }

        #endregion Properties
    }
}
