using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orderbook
{
    internal interface IMatchingOrderbook : IRetrievalOrderbook
    {
        MatchResult Match();
    }
}
