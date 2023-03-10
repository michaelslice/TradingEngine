using OrdersCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngineServer.Orders
{
    public class Order : IOrderCore
    {
    
        public Order(IOrderCore orderCore, long price, uint quantity,bool isBuySide)
        {
            //  PROPERTIES //
            Price = price;
            IsBuySide = isBuySide;
            InitialQuantity = quantity;
            CurrentQuantity = quantity;

            
            // FIELDS //
            _orderCore = orderCore;
        }

        public Order(ModifyOrder modifyOrder) : 
            this(modifyOrder, modifyOrder.Price, modifyOrder.Quantity, modifyOrder.IsBuySide)
        {

        }                

        // PROPERTIES  // 
        public long Price { get; private set; }
        public uint InitialQuantity { get; private set; }
        public uint CurrentQuantity { get; private set; }
        public bool IsBuySide { get; private set; }
        public long OrderID => _orderCore.OrderID;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;

        public long OrderId => throw new NotImplementedException();

        public int SecurityID => _orderCore.SecurityID;

        // METHODS //

        public void IncreaseQuantity(uint quantityDelta)
        {
            CurrentQuantity +=quantityDelta;
        }

        public void DecreaseQuantity(uint quantityDelta)
        {
            if (quantityDelta > CurrentQuantity)
                throw new InvalidOperationException($"Quantiy Delta > Current Quantity for OrderID={OrderID}");
            CurrentQuantity -=quantityDelta;            
        }

        CancelOrder IOrderCore.ToCancelOrder()
        {
            return _orderCore.ToCancelOrder();
        }

        // FIELDS //
        private readonly IOrderCore _orderCore;
    }
}
