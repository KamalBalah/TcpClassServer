using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpClassServer
{
    public partial class Server : Form
    {
        TcpListener tcp;
        List<TcpClient> clints = new List<TcpClient>();
        public Server()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Unicode.GetBytes(txtToSend.Text.Trim());
            foreach (TcpClient c in clints)
            {
                c.GetStream().Write(data, 0, data.Length);
                c.GetStream().Close();
            }
        }

        private void Server_Load(object sender, EventArgs e)
        {
            tcp = new TcpListener(new IPEndPoint(IPAddress.Any, 3090));
            tcp.Start();
            new System.Threading.Thread(
                () =>
                {
                    TcpClient clint = tcp.AcceptTcpClient();
                    clints.Add(clint);
                }
                ).Start();

            new System.Threading.Thread(

                ()=>{
                    UdpClient udp = new UdpClient(3050);
                    byte[] l = new byte[1024];

                    IPEndPoint ipe = null;
                   l= udp.Receive(ref ipe);
                    listBox1.Items.Add(Encoding.Unicode.GetString(l));
                }
                    ).Start();
                
                }
    }
}
