using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ticker
{
    public class Request 
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }
        public string id { get; set; }

        [JsonProperty("params")]
        public Parameters parameters { get; set; }

        public Request(string aJsonPrc, string aMethod, string aId, Parameters aParams) {
            jsonrpc = aJsonPrc;
            method = aMethod;
            id = aId;
            parameters = aParams;
        }
    }

    public class Parameters
    {
        [JsonProperty("channels")]
        List<String> channels { get; set; }

        public Parameters(List<String> aChannels) {
            channels = aChannels;
        }
    }
}    