using System;
using System.ServiceModel.Web;



namespace PumoxTest
{

    class Program
    {
        private const string baseAddress = "http://localhost/";
        private readonly Context _context;
        static void Main(string[] args)
        {
            try
            {
                WebServiceHost myHost = new WebServiceHost(typeof(Service), new Uri(baseAddress));
                Console.WriteLine("starting service...");
                myHost.Open();
                Console.WriteLine("press anny key to close the service");
                Console.ReadLine();
                myHost.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
              
            
        }
    }
}
