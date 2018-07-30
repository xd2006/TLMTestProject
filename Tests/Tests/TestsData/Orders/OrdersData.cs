
namespace Tests.TestsData.Orders
{
    using System.Collections.Generic;

    public class OrdersData
    {
        public static Dictionary<string, int> Customers =>
            new Dictionary<string, int> { { "BMW", 1 }, { "Audi", 2 }, { "Opel", 3 } };

    }
}
