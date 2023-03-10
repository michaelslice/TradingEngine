using OrdersCS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Rejects
{
    public class Rejection : IOrderCore
    {
        public Rejection(IOrderCore rejectedOrder, RejectionReason rejectionReason)
        {
            // PROPERTIES //
            RejectionReason = rejectionReason;
            
            // FIELDS // 
            _orderCore = rejectedOrder;
        }

        // PROPERTIES //
        public RejectionReason RejectionReason { get; private set; }

        private IOrderCore _orderCore;

        public long OrderID => _orderCore.OrderID;
        public string Username => _orderCore.Username;
        public int SecurityID => _orderCore.SecurityID;

        public long OrderId => orderCore.OrderId;

        public int SecurityId => orderCore.SecurityId;

        public uint InitialQuantity => orderCore.InitialQuantity;

        public uint CurrentQuantity => orderCore.CurrentQuantity;

        // FIELDS // 
        private readonly IOrderCore orderCore;

        CancelOrder IOrderCore.ToCancelOrder()
        {
            return orderCore.ToCancelOrder();
        }
    }
}
