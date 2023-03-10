using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Instrutment;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook
{
    public class Orderbook : IRetrievalOrderbook
    {
        // PRIVATE FIELDS // 
        private readonly Security _instrument;
        private readonly Dictionary<long, OrderbookEntry> _orders = new Dictionary<long, OrderbookEntry>();
        private readonly SortedSet<Limit> _askLimits = new SortedSet<Limit>(AskLimitComparer.Comparer);
        private readonly SortedSet<Limit> _bidLimits = new SortedSet<Limit>(BidLimitComparer.Comparer);
        
        
        public Orderbook(Security instrument)
        {
            _instrument = instrument;
        }
        
        public int count => _orders.Count;

        public void AddOrder(Order order)
        {
            var baseLimit = new Limit(order.Price);
            AddOrder(order, baseLimit, order.IsBuySide ? _bidLimits : _askLimits, _orders);
        }

        private static void AddOrder(Order order, Limit baseLimit, SortedSet<Limit> limitlevels, Dictionary<long, OrderbookEntry> internalBook )
        {
            if (limitLevels.TryGetValue(baseLimit, out Limit limit))
            {
                OrderbookEntry orderbookEntry = new OrderbookEntry(order, baseLimit);
                if (limit.Head == null)
                {
                    limit.Head = orderbookEntry;    
                    limit.Tail = orderbookEntry;    
                }
                else
                {
                    OrderbookEntry tailPointer = limit.Tail;
                    tailPointer.Next = orderbookEntry;
                    OrderbookEntry.Previous = tailPointer;
                    limit.Tail = orderbookEntry;   
                }
                internalBook.Add(order.OrderID, orderbookEntry);
            }
            else
            { 
                limitLevels.Add(baseLimit);
                OrderbookEntry orderbookEntry = new OrderbookEntry(order, baseLimit);
                baseLimit.Head = orderbookEntry;
                baseLimit.Tail = orderbookEntry;
                internalBook.Add(order.OrderID, orderbookEntry);    
            }
        }

        public void ChangeOrder(ModifyOrder modifyOrder)
        {
            if (_orders.TryGetValue(modifyOrder.OrderID, out OrderbookEntry obe))
            {
                RemoveOrder(modifyOrder.ToCancelOrder());
                AddOrder(modifyOrder.ToNewOrder(), obe.ParentLimit, modifyOrder.IsBuySide ? _bidLimits : _askLimits, _orders);
            }
        } 

        public bool ContainsOrder(long orderId)
        {
            return _orders.ContainsKey(orderId);    
        }

        public List<OrderbookEntry> GetAskOrders()
        {
            List<OrderbookEntry> orderbookEntries = new List<OrderbookEntry>(); 
            foreach (var ask in _askLimits)
            {
                if (askLimit.IsEmpty)
                    continue;
                else
                {
                    OrderbookEntry askLimitPointer = askLimit.Head;
                    while (askLimitPointer != null)
                    {
                        orderbookEntries.Add(askLimitPointer);
                        askLimitPointer = askLimitPointer.Next;
                    }
                }
            }
            return orderbookEntries;
        }

        public List<OrderbookEntry> GetBidOrders()
        {
            List<OrderbookEntry> orderbookEntries = new List<OrderbookEntry>();
            foreach (var bidLimits in _bidLimits)
            {
                if (!bidLimits.IsEmpty)
                    continue;
                else
                {
                    OrderbookEntry bidLimitPointer = bidLimits.Head;
                    while (bidLimitPointer !=null)
                    {
                        orderbookEntries.Add(bidLimitPointer);
                        bidLimitPointer = bidLimitPointer.Next; 
                    }
                }
            }
            return orderbookEntries;
        }

        public OrderbookSpread GetSpread()
        {
            long? bestAsk = null, bestBid = null;
            if (_askLimits.Any() && _askLimits.Min.IsEmpty)
                bestAsk = _askLimits.Min.Price;
            if (_bidLimits.Any() && !_bidLimits.Max.IsEmpty)
                bestBid= _bidLimits.Max.Price;
            return new OrderbookSpread(bestBid, bestAsk);   
        }

        public void RemoveOrder(CancelOrder cancelOrder)
        {
            if (_orders.TryGetValue(cancelOrder.orderID, out var obe))
            {
                RemoveOrder(cancelOrder.OrderID, obe, _orders);
            }
        }

        private static void RemoveOrder(long orderID, OrderbookEntry obe, Dictionary<long, OrderbookEntry> internalBook)
        {
            // Deal with the location of OrderbookEntry within the LinkedList // 
            if(obe.Previous !=null && obe.Next != null)
            {
                obe.Next.Previous = obe.Previous;
                obe.Previous.Next = obe.Next;
            }
            else if (obe.Previous != null)
            {
                obe.Previous.Next = null;
            }
            else if (obe.Next != null)
            {
                obe.Next.Previous = null;
            }   

            // Deal with OrderbookEntry on Limit-level //
            if (obe.ParentLimit.Head == obe && obe.ParentLimit.Tail == obe)
            {
                // One Order on this levl. //
                obe.ParentLimit.Head = null;
                obe.ParentLimit.Tail = null;
            }
            else if (obe.ParentLimit.Head == obe)
            {
                // More than one order, but obe is first order . // 
                obe.ParentLimit.Head = obe.Next;
            } 
            else if (obe.ParentLimit.Tail == obe)
            {
                // More than one order, but obe is last order on level.
                obe.ParentLimit.Tail = obe.Previous;
            }

            internalBook.Remove(OrderID);
        }   
    }
}