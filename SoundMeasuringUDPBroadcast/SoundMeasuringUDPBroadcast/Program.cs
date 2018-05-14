using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SoundMeasuringUDPBroadcast
{
    class Program
    {
        static void Main(string[] args)
        {
            var pm = new Program();
            // laver en udpclient der kan læse på en specifik port
            UdpClient udpReceiver = new UdpClient(9877);

            //Tillader os at læse datagrammer fra hvilkensom helts ip-addresse, på port 7000
            IPAddress ip = IPAddress.Parse("192.168.6.193");
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(ip, 9877);




            try
            {
                Console.WriteLine("Server is online");
                while (true)
                {


                    Byte[] receiveBytes = udpReceiver.Receive(ref RemoteIpEndPoint);
                    Console.WriteLine("Received data");

                    string receivedData = Encoding.ASCII.GetString(receiveBytes);
                    Console.WriteLine("Data processed");
                    Console.WriteLine("Recorded Data" + receivedData);
                    if (receivedData.Equals("STOP.Secret")) throw new Exception("Receiver stopped");

                    pm.InsertDataInDatabase(receivedData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

        }

        

        public int InsertDataInDatabase(string temperature)
        {
            //int id = 0;
            //int ID = id++;
            var Date = DateTime.Now.ToShortTimeString();
            if (Date == null || temperature == null)
            {
                throw new ArgumentException("Cannot insert null");
            }

            var connectionstring =
                "Server=tcp:davids-sql-server.database.windows.net,1433;Initial Catalog=Davids sql server;Persist Security Info=False;User ID=davidmalmberg;Password=Dak/Tha12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (var connection = new SqlConnection(connectionstring))
            {
                var sqlQuery = "INSERT INTO [dbo].[measurments] (Date,Temperature) VALUES(@Date, @Temperature)";
                using (var command = new SqlCommand(sqlQuery,connection))
                {
                    command.Parameters.AddWithValue("@Date", Date);
                    command.Parameters.AddWithValue("@Temperature", double.Parse(temperature));
                    //command.Parameters.AddWithValue("@Id", ID);

                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    if (result < 0)
                    {
                        throw new ArgumentException("Nothing inserted");
                    }
                    return result;
                }
            }

        }

        public int NextId(int id)
        {
            return id + 1;
        }
        
    }
}
