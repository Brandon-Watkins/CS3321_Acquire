using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    abstract class StockContainer
    {
        private Dictionary<Ecorporation, StockPile> stocks;


        public void addStock(StockPile s)
        {

        }

        public StockPile removeStock(Ecorporation corp, int qty)
        {
            return new StockPile();
        }

        public int count(ECorporation corp)
        {
            return 0;
        }
    }
}
