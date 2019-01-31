using System.IO;
using System.Linq;

namespace unbox.console.client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0) {
                var csvfilename = args[0];
                var lines = File.ReadAllLines(csvfilename);
                var records = lines.Skip(1).Select(line => line.Split(';'));
                
            }
        }
        
        
    }
}