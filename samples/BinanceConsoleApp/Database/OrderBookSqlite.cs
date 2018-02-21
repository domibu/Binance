using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace BinanceConsoleApp.Database
{
    public class OrderBookSqlite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed("Timestamp", 1)]
        public DateTime Timestamp { get; set; }
        public string Market { get; set; }
        [Indexed("Market", 2)]
        public string Symbol { get; set; }
        public int DataType { get; set; }
        public string Data { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
    }
}
