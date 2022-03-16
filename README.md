This program is configured to listen to data from the Deribit ticker api, https://docs.deribit.com/#ticker-instrument_name-interval and save data to a local SQLite database.


Prerequisites:
  * The program writes data to a SQLite database. You must have Sqlite running locally, with a database matching the DB_STRING constant 
    (this is set to @"Data Source=C:\Users\saebert\Documents\db\tickerdata.db" in the code, but should be updated with your local db location).
  * By default, the program will create a table called 'ticker' if it does not already exist, and write the ticker data to this table as json.
  * An environment running .NET 5 to compile and run the application


Running the application:
  * From the application directory, first build with: `dotnet build`
  * To run the application, run: `dotnet run TickerPersister.cs <TICKER> <INTERVAL>`; for example: `dotnet run TickerPersister.cs BTC-PERPETUAL 100ms`
  * The program will continue reading and persiting data until stopped manually
