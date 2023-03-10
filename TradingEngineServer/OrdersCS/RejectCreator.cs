using OrdersCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Rejects;

namespace TradingEngineServer.Orders
{
    public sealed class RejectCreator
    {   
        public static Rejection GenerateOrderCoreRejection(IOrderCore rejectedOrder, RejectionReason rejectionReason)
        {
            return new Rejection(rejectedOrder, rejectionReason);
        }
    }
}
