using OrdersCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
    public class OrderCore : IOrderCore
    {
            
        public OrderCore(long orderId, string username, int securityID)
        {
            OrderID = orderId;
            Username = username;
            SecurityID = securityID;
        }

        public long OrderID { get; private set; }

        public long OrderId => throw new NotImplementedException();

        public int SecurityID { get; private set; }

        public int SecurityId => throw new NotImplementedException();

        public string Username { get; private set; }

        public uint InitialQuantity => throw new NotImplementedException();

        public uint CurrentQuantity => throw new NotImplementedException();

        CancelOrder IOrderCore.ToCancelOrder()
        {
            throw new NotImplementedException();
        }
    }
}
 