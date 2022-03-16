using System;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;
using Websocket.Client;
using System.Data.SQLite;

namespace ticker
{
    class TickerPersister
    {
        private static readonly string VERSION = "2.0";
        private static readonly string METHOD = "public/subscribe";
        private static readonly string ID = "42";
        private static readonly string URL = "wss://test.deribit.com/ws/api/v2";
        private static readonly string DB_STRING = @"Data Source=C:\Users\saebert\Documents\db\tickerdata.db";
        private static readonly string CREATE_TABLE_COMMAND = "CREATE TABLE IF NOT EXISTS ticker(data, text);";
        private static readonly string INSERT_DATA_COMMAND = "INSERT INTO ticker(data) VALUES('{0}');";
        private static readonly ManualResetEvent EXIT_EVENT = new ManualResetEvent(false);
        private static readonly JsonSerializerSettings JSON_SETTINGS = new JsonSerializerSettings() 
        {
            MissingMemberHandling = MissingMemberHandling.Error
        };
    
        static void Main(string[] args)
        {
            if (args.Length != 3) {
                Console.WriteLine("Symbol and duration must be specified as command-line arguments");
                return;
            }

            string instrument = args[1];
            string frequency = args[2];
            var requestMessage = new Request(VERSION, METHOD, ID, new Parameters(new List<string>{String.Format("ticker.{0}.{1}", instrument, frequency)}));
            string requestMessageJson = JsonConvert.SerializeObject(requestMessage);

            using var client = new WebsocketClient(new Uri(URL));
            using var dbConnection = new SQLiteConnection(DB_STRING);
        
            dbConnection.Open();
            createTableIfNecessary(dbConnection);

            client.MessageReceived.Subscribe(msg => processMessage(msg, dbConnection));
            client.Start();
            client.Send(requestMessageJson);

            EXIT_EVENT.WaitOne();
        }

        private static void createTableIfNecessary(SQLiteConnection connection) {
            var createTableCmd = new SQLiteCommand(CREATE_TABLE_COMMAND, connection);
            createTableCmd.ExecuteNonQuery();
        }

        private static void processMessage(ResponseMessage aMessage, SQLiteConnection aConnection) {
            try {
                // In the current setup, the tickerData object isn't actually used since we persist the json directly.
                // The reason for still parsing it is to ensure that we only persist the json for valid TickerData 
                // objects, not e.g. error messages from the server.
                //
                // The overall setup (persisting only the json of the message as opposed to a more detailed database with more columns)
                // was chosen mainly due to time constraints; in an ideal version of this I would expect to specify more columns in the database
                // for the fields of TickerData, and potentially only persist certain fields depending on the use case
                TickerData tickerData = JsonConvert.DeserializeObject<TickerData>(aMessage.Text, JSON_SETTINGS);

                var insertCommand = new SQLiteCommand(String.Format(INSERT_DATA_COMMAND, aMessage.Text), aConnection);
                insertCommand.ExecuteNonQuery();
            } catch (JsonException aException) {
                Console.WriteLine("Failed to process message: " + aMessage.Text + " Exception: " + aException.Message);
            }
        }
     }
}
