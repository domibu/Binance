using System;
using System.Threading;
using System.Threading.Tasks;

namespace BinanceConsoleApp.Controllers
{
    internal class Ping : IHandleCommand
    {
        public async Task<bool> HandleAsync(string command, CancellationToken token = default)
        {
            if (!command.Equals("ping", StringComparison.OrdinalIgnoreCase))
                return false;

            var isBinanceSuccessful = await Program.BinanceApi.PingAsync(token);
            var isKucoinSuccessful = await Program.KucoinApi.PingAsync(token);

            lock (Program.ConsoleSync)
            {
                Console.WriteLine($"  BinancePing: {(isBinanceSuccessful ? "SUCCESSFUL" : "FAILED")}");
                Console.WriteLine($"  KucoinPing: {(isKucoinSuccessful ? "SUCCESSFUL" : "FAILED")}");
                Console.WriteLine();
            }

            return true;
        }
    }
}
