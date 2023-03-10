using OrdersCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders 
{
    public class ModifyOrder : IOrderCore 
    {
        public ModifyOrder(IOrderCore orderCore,
            long modifyprice, uint modifyQuantity, bool isBuySide)
        {

            // PROPERTIES //
            Price = modifyprice;
            Quantity = modifyQuantity;
            IsBuySide = isBuySide;  

            // FIELDS // 
            _orderCore = orderCore;
        }
            // PROPERTIES // 
        public long Price { get; private set; } 
        public uint Quantity { get; private set; }    
        public bool IsBuySide { get; private set; }

        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;

        public long OrderID => _orderCore.OrderID;

        public uint InitialQuantity => _orderCore.InitialQuantity;

        public uint CurrentQuantity => _orderCore.CurrentQuantity;

        public int SecurityID => _orderCore.SecurityID;

        // METHODS // 
        public CancelOrder ToCancelOrder()
        {
            return new CancelOrder(this);
        }

        public Order ToNewOrder()
        {
            return new Order(this);
        }

        // FIELDS // 
        private readonly IOrderCore _orderCore;

    }
}
