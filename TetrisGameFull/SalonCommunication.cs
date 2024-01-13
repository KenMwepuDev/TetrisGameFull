using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SimpleTCP;

namespace TetrisGameFull
{
    public class SalonCommunication
    {
        private Form1 _formInstance;
        private SimpleTcpServer server;
        private SimpleTcpClient client;
        private int Port = 6603;

        public SalonCommunication(Form1 formInstance)
        {
            _formInstance = formInstance;
        }

        public IPAddress CreateSalon(IPAddress ipAdress = null)
        {
            server = new SimpleTcpServer();
            IPAddress iP = ipAdress == null ? GetWifiIPv4Address() : ipAdress;

            server.Start(iP, Port);
            server.ClientConnected += NewClientConnected;
            server.DataReceived += NewMessageReceiced;

            return iP;
        }

        public void CloseSalon()
        {
            if (server != null && server.IsStarted)
                server.Stop();
        }

        public void ConnectToSalon(string ipAdress)
        {
            client = new SimpleTcpClient();
            client.Connect(ipAdress, Port);
        }

        private void NewClientConnected(object sender, TcpClient e)
        {
            System.Windows.Forms.MessageBox.Show("Connected");
            _formInstance.FLP_BoardsContener.Controls.Add(new System.Windows.Forms.Button() { Text = "Click Me" });
        }

        private void NewMessageReceiced(object sender, Message e)
        {
            Console.WriteLine($"Message from mobile : {e.MessageString}");
        }

        private IPAddress GetWifiIPv4Address()
        {
            var wifiInterface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211);

            if (wifiInterface != null)
            {
                var ipv4Address = wifiInterface.GetIPProperties().UnicastAddresses
                    .FirstOrDefault(address => address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                if (ipv4Address != null)
                {
                    return ipv4Address.Address;
                }
            }

            return null;
        }



        public List<IPAddress> GetAllIpAdress()
        {
            List<IPAddress> allIpAdress = new List<IPAddress>();
            // Obtenir toutes les interfaces réseau disponibles
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                // Ignorer les interfaces loopback et les interfaces sans adresse IP
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Tunnel ||
                    networkInterface.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                // Obtenir les adresses IP associées à l'interface
                UnicastIPAddressInformationCollection ipAddresses = networkInterface.GetIPProperties().UnicastAddresses;

                foreach (var ipAddress in ipAddresses)
                {
                    if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        //Console.WriteLine($"{networkInterface.Description}: {ipAddress.Address}");
                        allIpAdress.Add(ipAddress.Address);
                    }
                }
            }

            return allIpAdress;
        }
    }
}
