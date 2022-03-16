using System;

namespace ticker
{
    public class TickerData
    {
        public Params @params { get; set; }
        public string method { get; set; }
        public string jsonrpc { get; set; }
    }
    public class Stats
    {
        public double volume_usd { get; set; }
        public double volume { get; set; }
        public double price_change { get; set; }
        public double low { get; set; }
        public double high { get; set; }
    }

    public class Data
    {
        public long timestamp { get; set; }
        public Stats stats { get; set; }
        public string state { get; set; }
        public double settlement_price { get; set; }
        public double open_interest { get; set; }
        public double min_price { get; set; }
        public double max_price { get; set; }
        public double mark_price { get; set; }
        public double last_price { get; set; }
        public string instrument_name { get; set; }
        public double index_price { get; set; }
        public double funding_8h { get; set; }
        public double estimated_delivery_price { get; set; }
        public double current_funding { get; set; }
        public double best_bid_price { get; set; }
        public double best_bid_amount { get; set; }
        public double best_ask_price { get; set; }
        public double best_ask_amount { get; set; }
    }

    public class Params
    {
        public Data data { get; set; }
        public string channel { get; set; }
    }
}    