using TradingEngineServer.Orders;

namespace OrdersCS
{
    public interface IOrderCore
    {

        public long OrderId { get; }
        long OrderID { get; }
        public string Username { get; }
        public int SecurityId { get; }
        int SecurityID { get; }
        uint InitialQuantity { get; }
        uint CurrentQuantity { get; }

        CancelOrder ToCancelOrder();
    }
}