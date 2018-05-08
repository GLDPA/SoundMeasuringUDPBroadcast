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
            // laver en udpclient der kan læse på en specifik port
            UdpClient udpReceiver = new UdpClient(7000);

            //Tillader os at læse datagrammer fra hvilkensom helts ip-addresse, på port 7000
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 7000);




            try
            {
                while (true)
                {


                    Byte[] receiveBytes = udpReceiver.Receive(ref RemoteIpEndPoint);

                    string receivedData = Encoding.ASCII.GetString(receiveBytes);
                    if (receivedData.Equals("STOP.Secret")) throw new Exception("Receiver stopped");


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            public int InsertData(string temprature)
            {
                var time = DateTime.Now.ToShortTimeString();
            }
            var sqlQuery = "INSERT INTO [dbo].[Measurments](ID, DECIBEL, DATE) VALUES (@ID, @DECIBEL, @DATE)";

            using (var connection = new SqlConnection(connectionString))
            {

            }


        }


    }
}
