using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            UdpClient udpReceiver = new UdpClient(7000);

            //Tillader os at læse datagrammer fra hvilkensom helts ip-addresse, på port 7000
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 7000);




            try
            {
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

        

        public int InsertDataInDatabase(string temprature)
        {
            var time = DateTime.Now.ToShortTimeString();
            if (time == null || temprature == null)
            {
                throw new ArgumentException("Cannot insert null");
            }

            var connectionstring =
                "Server=tcp:davids-sql-server.database.windows.net,1433;Initial Catalog=Davids sql server;Persist Security Info=False;User ID=davidmalmberg;Password=Dak/Tha12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (var connection = new SqlConnection(connectionstring))
            {
                var sqlQuery = "INSERT INTO [dbo].[measurments] (Time,Temperatur, Humidity) VALUES(@Time, @Temperatur, @Humidity)";
                using (var command = new SqlCommand(sqlQuery,connection))
                {
                    command.Parameters.AddWithValue("@Time", time);
                    command.Parameters.AddWithValue("@Temprature", temprature);
                    //command.Parameters.AddWithValue("@Humidity", humidity);

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
        
    }
}
