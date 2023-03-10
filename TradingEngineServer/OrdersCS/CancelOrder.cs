using OrdersCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
    internal class CancelOrder : IOrderCore
    {
    
        public CancelOrder(IOrderCore orderCore)
        {
            // FIELDS //

            _orderCore = orderCore;
        }

        // PROPERTIES 

        public long OrderID => _orderCore.OrderID;
        public string Username => _orderCore.Username;
        public int SecurityID => _orderCore.SecurityId;

        public long OrderId => throw new NotImplementedException();

        public int SecurityId => throw new NotImplementedException();

        public uint InitialQuantity => throw new NotImplementedException();

        public uint CurrentQuantity => throw new NotImplementedException();

        // FIELDS //

        private readonly IOrderCore _orderCore;

        public CancelOrder ToCancelOrder()
        {
            return _orderCore.ToCancelOrder();
        }
    }
}
