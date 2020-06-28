using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
namespace DecyptDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //The key and IV must be the same values that were used
            //to encrypt the stream.
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
            try
            {
                //Initialize a TCPListener on port 11000
                //using the current IP address.
                TcpListener tcpListen = new TcpListener(IPAddress.Any, 11000);

                //Start the listener.
                tcpListen.Start();

                //Check for a connection every five seconds.
                while (!tcpListen.Pending())
                {
                    Console.WriteLine("Still listening. Will try in 5 seconds.");
                    Thread.Sleep(5000);
                }

                //Accept the client if one is found.
                TcpClient tcp = tcpListen.AcceptTcpClient();

                //Create a network stream from the connection.
                NetworkStream netStream = tcp.GetStream();

                //Create a new instance of the RijndaelManaged class
                // and decrypt the stream.
                RijndaelManaged rmCrypto = new RijndaelManaged();

                //Create a CryptoStream, pass it the NetworkStream, and decrypt
                //it with the Rijndael class using the key and IV.
                CryptoStream cryptStream = new CryptoStream(netStream,
                   rmCrypto.CreateDecryptor(key, iv),
                   CryptoStreamMode.Read);

                //Read the stream.
                StreamReader sReader = new StreamReader(cryptStream);

                //Display the message.
                Console.WriteLine("The decrypted original message: {0}", sReader.ReadToEnd());

                //Close the streams.
                sReader.Close();
                netStream.Close();
                tcp.Close();
            }
            //Catch any exceptions.
            catch
            {
                Console.WriteLine("The Listener Failed.");
            }

        }
    }
}
