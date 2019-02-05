using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PS3Lib;

namespace Advanced_Warfare_Proper_ACS_MrNiato
{
    public partial class Form1 : Form
    {
        static PS3API PS3 = new PS3API(SelectAPI.ControlConsole);
        public uint Call(uint func_address, params object[] parameters)
        {
            int length = parameters.Length;
            for (uint i = 0; i < length; i++)
            {
                if (parameters[i] is int)
                {
                    byte[] array = BitConverter.GetBytes((int)parameters[i]);
                    Array.Reverse(array);
                    PS3.SetMemory(0x10050008 + (i * 4), array);
                }
                else if (parameters[i] is uint)
                {
                    byte[] buffer2 = BitConverter.GetBytes((uint)parameters[i]);
                    Array.Reverse(buffer2);
                    PS3.SetMemory(0x10050008 + (i * 4), buffer2);
                }
                else if (parameters[i] is string)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(Convert.ToString(parameters[i]) + "\0");
                    PS3.SetMemory(0x1005002c + (i * 0x150), buffer);
                    uint num3 = 0x1005002c + (i * 0x150);
                    byte[] buffer4 = BitConverter.GetBytes(num3);
                    Array.Reverse(buffer4);
                    PS3.SetMemory(0x10050008 + (i * 4), buffer4);
                }
            }
            for (int j = 9 - length; j <= 9; j++)
            {
                PS3.SetMemory((uint)(0x10050008 + (j * 4)), new byte[4]);
            }
            byte[] bytes = BitConverter.GetBytes(func_address);
            Array.Reverse(bytes);
            PS3.SetMemory(0x10050000, bytes);
            Thread.Sleep(20);
            byte[] buffer6 = new byte[4];
            byte[] memory = GetMemory(0x10050004, 4);
            Array.Reverse(memory);
            return BitConverter.ToUInt32(memory, 0);
        }

        private void ClonePlayer(int client)
        {
            Call(0x1f6388, new object[] { client });
        }
        private uint function_address = 0x7aa050;
        public byte[] GetMemory(uint address, int length)
        {
            byte[] buffer = new byte[length];
            PS3.GetMemory(address, buffer);
            return buffer;
        }

        public byte[] GetMemory5(uint Address, byte[] length)
        {
            PS3.GetMemory(Address, length);
            return length;
        }
        public uint Get_func_address()
        {
            for (uint i = 0x7a8e48; i < 0x1000000; i += 4)
            {
                byte[] length = new byte[9];
                byte[] buffer2 = GetMemory5(i, length);
                if ((((buffer2[0] == 0x42) && (buffer2[1] == 200)) && ((buffer2[2] == 0) && (buffer2[3] == 0))) && (((buffer2[4] == 0x40) && (buffer2[5] == 0)) && ((buffer2[6] == 0) && (buffer2[7] == 0))))
                {
                    return (i + 8);
                }
            }
            return 0;
        }
        public int Init()
        {
            function_address = Get_func_address();
            if (function_address == 0)
            {
                return -1;
            }
            Enable_RPC();
            return 0;
        }
        public void Enable_RPC()
        {
            PS3.SetMemory(function_address, new byte[] { 0x4e, 0x80, 0, 0x20 });
            byte[] buffer = new byte[] { 
                0x7c, 8, 2, 0xa6, 0xf8, 1, 0, 0xd0, 0xdb, 0x21, 0, 0x88, 0xdb, 0x41, 0, 0x90, 
                0xdb, 0x61, 0, 0x98, 0xdb, 0x81, 0, 160, 0xdb, 0xa1, 0, 0xa8, 0xdb, 0xc1, 0, 0xb0, 
                0xdb, 0xe1, 0, 0xb8, 0xfb, 0xa1, 0, 0x70, 0xfb, 0xc1, 0, 120, 0xfb, 0xe1, 0, 0x80, 
                0x3d, 0x60, 0x10, 5, 0x80, 0x6b, 0, 0, 0x2c, 3, 0, 0, 0x41, 130, 0, 60, 
                0x80, 0x6b, 0, 8, 0x80, 0x8b, 0, 12, 0x80, 0xab, 0, 0x10, 0x80, 0xcb, 0, 20, 
                0x80, 0xeb, 0, 0x18, 0x81, 11, 0, 0x1c, 0x81, 0x2b, 0, 0x20, 0x81, 0x4b, 0, 0x24, 
                0x81, 0x6b, 0, 40, 0x48, 0, 0, 0x4d, 60, 0x80, 0x10, 5, 0x80, 100, 0, 4, 
                0x38, 160, 0, 0, 0x90, 0xa4, 0, 0, 0xe8, 1, 0, 0xd0, 0x7c, 8, 3, 0xa6, 
                0xcb, 0x21, 0, 0x88, 0xcb, 0x41, 0, 0x90, 0xcb, 0x61, 0, 0x98, 0xcb, 0x81, 0, 160, 
                0xcb, 0xa1, 0, 0xa8, 0xcb, 0xc1, 0, 0xb0, 0xcb, 0xe1, 0, 0xb8, 0xeb, 0xa1, 0, 0x70, 
                0xeb, 0xc1, 0, 120, 0xeb, 0xe1, 0, 0x80, 0x38, 0x21, 0, 0xc0, 0x4e, 0x80, 0, 0x20, 
                0x3d, 0x80, 0x10, 5, 0x81, 140, 0, 0, 0x7c, 8, 2, 0xa6, 0xf8, 1, 0, 0x20, 
                0x7d, 0x89, 3, 0xa6, 0x4e, 0x80, 4, 0x21, 0xe8, 1, 0, 0x20, 0x7c, 8, 3, 0xa6, 
                0x4e, 0x80, 0, 0x20, 0x60, 0, 0, 0
             };
            PS3.SetMemory(function_address + 4, buffer);
            PS3.SetMemory(0x10050000, new byte[0xaac]);
            PS3.SetMemory(function_address, new byte[] { 0xf8, 0x21, 0xff, 0x41 });
        }
        static bool IsConnected = false;
        public struct Stats
        {
            public static UInt32
                Kills = 0x26FC90C,
                Deaths = 0x26FC948,
                Gibs = 0x26FC91C,
                Nades = 0x26FC934,
                Perks = 0x26FC918,
                Doors = 0x26FC938,
                Downs = 0x26FC910,
                Headshots = 0x26FC884,
                Miles = 0x26FC93C,
                Hits = 0x26FC944,
                BlueEyes = 0x026FCA8F;
        }
        public struct AntiBan
        {
            public static UInt32
                 antiban1 = 0x0052041C,
                 antiban2 = 0x005300E8,
                 antiban3 = 0x005300D4,
                 antiban4 = 0x0050A5A8,
                 antiban5 = 0x0050A438,
                 antiban6 = 0x0050A3B4,
                 antiban7 = 0x0050A3BC,
                 antiban8 = 0x0050A3C4, 
                 antiban9 = 0x0050A5F8,
                 antiban10 = 0x005079D8,
                 antiban11 = 0x0050C194,
                 antiban12 = 0x0050C1FC,
                 antiban13 = 0x0050C220,
                 antiban14 = 0x0050C22C,
                 antiban15 = 0x0069E834;
        }
        public static void SetStats(uint Offset, decimal Value)
        {
            byte[] buffer = BitConverter.GetBytes(Convert.ToInt32(Value.ToString()));
            PS3.SetMemory(Offset, buffer);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void xylosButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void aetherButton1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void aetherButton3_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                if (aetherButton3.Text == "Blue Eyes [OFF]")
                {
                    byte[] mrniato = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                    PS3.SetMemory(Stats.BlueEyes, mrniato);
                    aetherButton3.Text = "Blue Eyes [ON]";
                }
                else
                {
                    byte[] mrniato = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    PS3.SetMemory(Stats.BlueEyes, mrniato);
                    aetherButton3.Text = "Blue Eyes [OFF]";
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void xylosButton1_Click_1(object sender, EventArgs e)
        {
            SetStats(Stats.Kills, numericUpDown1.Value);
        }

        public static uint SV_GameSendServerCommand = 0x349f6c;
        private void connectUsingCCAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS3.ChangeAPI(SelectAPI.TargetManager);
            try
            {
                if (PS3.ConnectTarget() && PS3.AttachProcess())
                {
                    IsConnected = true;
                    PS3.Extension.WriteString(0x8E3290, "^5MrNiato's ^2BO2 Zombie ^6Tool ^1Reloaded");
                    byte[] buffer1 = new byte[4];
                    buffer1[0] = 0x60;
                    PS3.SetMemory(0x37FEC, buffer1);
                    Enable_RPC();
                    MessageBox.Show("Playstation 3 Linked !", "Success !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("An error has occurred", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error has occurred", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void connectUsingTMAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS3.ChangeAPI(SelectAPI.ControlConsole);
            try
            {
                if (PS3.ConnectTarget() && PS3.AttachProcess())
                {
                    IsConnected = true;
                    PS3.Extension.WriteString(0x8E3290, "^5MrNiato's ^2BO2 Zombie ^6Tool ^1Reloaded");
                    byte[] buffer1 = new byte[4];
                    buffer1[0] = 0x60;
                    PS3.SetMemory(0x37FEC, buffer1);
                    Enable_RPC();
                    MessageBox.Show("Playstation 3 Linked !", "Success !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("An error has occurred", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error has occurred", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void disconnectConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                if (PS3.GetCurrentAPI() == SelectAPI.TargetManager)
                {
                    PS3.TMAPI.DisconnectTarget();
                    MessageBox.Show("Playstation 3 Disconnected via TMAPI !", "Success !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (PS3.GetCurrentAPI() == SelectAPI.ControlConsole)
                {
                    PS3.CCAPI.DisconnectTarget();
                    MessageBox.Show("Playstation 3 Disconnected via CCAPI !", "Success !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aetherButton1_Click_1(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer = new byte[4];
                PS3.GetMemory(0x26FC90C, buffer);
                numericUpDown1.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC884, buffer);
                numericUpDown2.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC910, buffer);
                numericUpDown3.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC948, buffer);
                numericUpDown4.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC938, buffer);
                numericUpDown5.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC918, buffer);
                numericUpDown6.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC940, buffer);
                numericUpDown7.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC934, buffer);
                numericUpDown8.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC914, buffer);
                numericUpDown9.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC93C, buffer);
                numericUpDown10.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC91C, buffer);
                numericUpDown11.Value = BitConverter.ToInt32(buffer, 0);
                PS3.GetMemory(0x26FC944, buffer);
                numericUpDown12.Value = BitConverter.ToInt32(buffer, 0);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                PS3.SetMemory(0x50B61C, new byte[] { 0x48, 0x00 });
                PS3.SetMemory(0x50BA74, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(0x547DD4, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(0x548148, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(0x50A38C, new byte[] { 0x2C, 0x1B, 0x00, 0x99 });
                PS3.SetMemory(AntiBan.antiban1, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban2, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban3, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban4, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban5, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban6, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban7, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban8, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban9, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban10, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban11, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban12, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban13, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban14, new byte[] { 0x60, 0x00, 0x00, 0x00 });
                PS3.SetMemory(AntiBan.antiban15, new byte[] { 0x60, 0x00, 0x00, 0x00 });
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aetherButton5_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                PS3.SetMemory(0x26FC90C, BitConverter.GetBytes((int)numericUpDown1.Value));
                PS3.SetMemory(0x26FC884, BitConverter.GetBytes((int)numericUpDown2.Value));
                PS3.SetMemory(0x26FC910, BitConverter.GetBytes((int)numericUpDown3.Value));
                PS3.SetMemory(0x26FC948, BitConverter.GetBytes((int)numericUpDown4.Value));
                PS3.SetMemory(0x26FC938, BitConverter.GetBytes((int)numericUpDown5.Value));
                PS3.SetMemory(0x26FC918, BitConverter.GetBytes((int)numericUpDown6.Value));
                PS3.SetMemory(0x26FC940, BitConverter.GetBytes((int)numericUpDown7.Value));
                PS3.SetMemory(0x26FC934, BitConverter.GetBytes((int)numericUpDown8.Value));
                PS3.SetMemory(0x26FC914, BitConverter.GetBytes((int)numericUpDown9.Value));
                PS3.SetMemory(0x26FC93C, BitConverter.GetBytes((int)numericUpDown10.Value));
                PS3.SetMemory(0x26FC91C, BitConverter.GetBytes((int)numericUpDown11.Value));
                PS3.SetMemory(0x26FC944, BitConverter.GetBytes((int)numericUpDown12.Value));
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] GRADE = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
                PS3.SetMemory(0x026FC894, GRADE);
                byte[] GRADE1 = new byte[] { 0x0A, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                PS3.SetMemory(0x026FCA87, GRADE1);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] GRADE = new byte[] { 0x0F };
                PS3.SetMemory(0x026FC896, GRADE);
                byte[] GRADE1 = new byte[] { 0x02, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                PS3.SetMemory(0x026FCA87, GRADE1);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] GRADE = new byte[] { 0x0F };
                PS3.SetMemory(0x026FC896, GRADE);
                byte[] GRADE1 = new byte[] { 0x0A, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                PS3.SetMemory(0x026FCA87, GRADE1);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] GRADE = new byte[] { 0x0F };
                PS3.SetMemory(0x026FC896, GRADE);
                byte[] GRADE1 = new byte[] { 0x15, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                PS3.SetMemory(0x026FCA87, GRADE1);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] GRADE = new byte[] { 0x0F };
                PS3.SetMemory(0x026FC896, GRADE);
                byte[] GRADE1 = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                PS3.SetMemory(0x026FCA87, GRADE1);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void aetherButton4_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                if (aetherButton4.Text == "Flash Prestige [OFF]")
                {
                    timer1.Start();
                    aetherButton4.Text = "Flash Prestige [ON]";
                }
                else
                {
                    timer1.Stop();
                    aetherButton4.Text = "Flash Prestige [OFF]";
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }
        private int Countername;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Interval = 300;
            {
                Countername++;
                switch (Countername)
                {
                    case 1:
                        PS3.SetMemory(0x026FC896, new byte[] { 0x0F });
                        PS3.SetMemory(0x026FCA87, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                        break;

                    case 2:
                        byte[] GRADE2 = new byte[] { 0x0F };
                        PS3.SetMemory(0x026FC896, GRADE2);
                        byte[] GRADE3 = new byte[] { 0x02, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                        PS3.SetMemory(0x026FCA87, GRADE3);
                        break;

                    case 3:
                        byte[] GRADE4 = new byte[] { 0x0F };
                        PS3.SetMemory(0x026FC896, GRADE4);
                        byte[] GRADE5 = new byte[] { 0x0A, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                        PS3.SetMemory(0x026FCA87, GRADE5);
                        break;

                    case 4:
                        byte[] GRADE6 = new byte[] { 0x0F };
                        PS3.SetMemory(0x026FC896, GRADE6);
                        byte[] GRADE7 = new byte[] { 0x15, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                        PS3.SetMemory(0x026FCA87, GRADE7);
                        break;

                    case 5:
                        PS3.SetMemory(0x026FC896, new byte[] { 0x0F });
                        PS3.SetMemory(0x026FCA87, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
                        break;
                }
                if (Countername == 6) Countername = 0;
                {
                    timer1.Start();
                }
            }
        }

        private void aetherButton2_Click(object sender, EventArgs e)
        {
            Random randNum = new Random();
            numericUpDown1.Value = randNum.Next(0, 50000);
            numericUpDown2.Value = randNum.Next(0, 25000);
            numericUpDown3.Value = randNum.Next(0, 50000);
            numericUpDown4.Value = randNum.Next(0, 25000);
            numericUpDown5.Value = randNum.Next(0, 15000);
            numericUpDown6.Value = randNum.Next(0, 15000);
            numericUpDown7.Value = randNum.Next(0, 15000);
            numericUpDown8.Value = randNum.Next(0, 15000);
            numericUpDown9.Value = randNum.Next(0, 15000);
            numericUpDown10.Value = randNum.Next(0, 15000);
            numericUpDown11.Value = randNum.Next(0, 15000);
            numericUpDown12.Value = randNum.Next(0, 15000);
        }

        private void aetherButton6_Click(object sender, EventArgs e)
        {
            Random randNum = new Random();
            numericUpDown1.Value = randNum.Next(50000, 250000);
            numericUpDown2.Value = randNum.Next(50000, 250000);
            numericUpDown3.Value = randNum.Next(50000, 250000);
            numericUpDown4.Value = randNum.Next(50000, 250000);
            numericUpDown5.Value = randNum.Next(50000, 250000);
            numericUpDown6.Value = randNum.Next(10000, 250000);
            numericUpDown7.Value = randNum.Next(10000, 250000);
            numericUpDown8.Value = randNum.Next(10000, 250000);
            numericUpDown9.Value = randNum.Next(10000, 250000);
            numericUpDown10.Value = randNum.Next(10000, 250000);
            numericUpDown11.Value = randNum.Next(10000, 250000);
            numericUpDown12.Value = randNum.Next(10000, 250000);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            PS3.SetMemory(0x01cd6553, new byte[] { 0x00 }); 
            PS3.SetMemory(0x01cd5f53, new byte[] { 0x01 }); 
            PS3.SetMemory(0x01cd69d3, new byte[] { 0x01 });
            PS3.SetMemory(0x19A95C0, new byte[] { 0x01 });
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                timer2.Stop();
                PS3.SetMemory(0x01cd6553, new byte[] { 0x01 });
                PS3.SetMemory(0x01cd5f53, new byte[] { 0x06 });
                PS3.SetMemory(0x01cd69d3, new byte[] { 0x06 });
                PS3.SetMemory(0x19A95C0, new byte[] { 0x06 });
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                timer2.Start();
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void xylosCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (xylosCheckBox1.Checked)
                {
                    byte[] buffer2 = new byte[4];
                    buffer2[0] = 0x60;
                    buffer = buffer2;
                    PS3.SetMemory(0xf9e54, buffer);
                }
                else
                {
                    buffer = new byte[] { 0x48, 80, 110, 0xf5 };
                    PS3.SetMemory(0xf9e54, buffer);
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }
        public static string get_player_name(uint client)
        {
            string getnames = PS3.Extension.ReadString(0x178646c + client * 0x5808);
            return getnames;
        }
        private void xylosButton1_Click_2(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                Players.Items.Clear();
                for (uint i = 0; i < 0x12; i++)
                {
                    Players.Items.Add(get_player_name(i));
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void enableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                for (int i = 0; i < 0x12; i++)
                {
                    int pedID = (Players.SelectedIndex);
                    PS3.SetMemory((0x1780f43 + (uint)Players.SelectedIndex * 0x6200), new byte[] { 0x05 });
                    Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" God Mode [^2ON^7]" });
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void disableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                for (int i = 0; i < 0x12; i++)
                {
                    int pedID = (Players.SelectedIndex);
                    PS3.SetMemory((0x1780f43 + (uint)Players.SelectedIndex * 0x6200), new byte[] { 0x04 });
                    Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" God Mode [^1OFF^7]"});
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void unlimitedAmmoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                for (int i = 0; i < 0x12; i++)
                {
                    int pedID = (Players.SelectedIndex);
                    PS3.SetMemory((uint)(0x1781356 + (uint)Players.SelectedIndex  * 0x5808), new byte[] { 0xff  });
                    PS3.SetMemory((uint)(0x1781352 + (uint)Players.SelectedIndex  * 0x5808), new byte[] { 0xff  });
                    PS3.SetMemory((uint)(0x178135e + (uint)Players.SelectedIndex  * 0x5808), new byte[] { 0xff });
                    PS3.SetMemory((uint)(0x1781363 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xff });
                    Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" Unlimited Ammo [^2ON^7]" });
                    timer3.Start();
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1781356 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xff });
                PS3.SetMemory((uint)(0x1781352 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xff });
                PS3.SetMemory((uint)(0x178135e + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xff });
                PS3.SetMemory((uint)(0x1781363 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xff });
            }
        }

        private void maxMoneyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1786501 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 15, 0x42, 0x3f });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" Max Money Given !" });
            }
        }

        private void freezeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void enableToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x17865bf + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xff });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" Freeze [^2ON^7]" });
            }
        }

        private void disableToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                byte[] buffer = new byte[1];
                PS3.SetMemory((uint)(0x17865bf + (uint)Players.SelectedIndex * 0x5808), buffer);
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" Freeze ^1disable" });
            }
        }

        private void enableToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                byte[] buffer = new byte[1];
                PS3.SetMemory((uint)(0x1786363 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 3 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" Fake Lag [^2ON^7]" });
            }
        }

        private void disableToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                byte[] buffer = new byte[1];
                PS3.SetMemory((uint)(0x1786363 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 2 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" Fake Lag ^1Disable" });
            }
        }

        private void enableToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1780fac + (uint)Players.SelectedIndex * 0x5808), new byte[] { 1 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" Third Person [^2ON^7]" });
            }
        }

        private void disableToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1780fac + (uint)Players.SelectedIndex * 0x5808), new byte[] { 1 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \" Third Person [^1OFF^7]" });
            }
        }

        private void allPerksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1781424 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 5, 0x55, 0x4f });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^5All Perks ^2Given !" });
            }
        }

        private void xylosButton2_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"[{+gostand}] ^5Welcome to ^2MrNiato^1's Modded Lobby ! [{+activate}] \n[{+breath_sprint}] ^3www.ihax.fr  [{+melee}]\n[{+smoke}] ^6www.allcodrecovery.com [{+frag}]\n[{togglemenu}] ^7wwww.boutique.h7k3r.fr [{togglescores}]" });
                Thread.Sleep(1500);
                Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"[{+actionslot 4}] ^2Made By ^5MrNiato [{+actionslot 4}] \n[{+actionslot 1}] ^2Faceboook : ^1Guillaume MrNiato[{+actionslot 2}] " });
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void enableToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1786418 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x40 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"Speed x2 [^2ON^7]" });
            }
        }

        private void disableToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1786418 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x3f });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"Speed x2 [^1OFF^7]" });
            }
        }

        private void enableToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1781025 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x01 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"Invisible [^2ON^7]" });
            }
        }

        private void disableToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1781025 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x00 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"Invisible [^1OFF^7]" });
            }
        }

        private void enableToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x178672b + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x01 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"Last Stand [^2ON^7]" });
            }
        }

        private void disableToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x178672b + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x00 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"Last Stand [^1OFF^7]" });
            }
        }

        private void enableToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1780f42 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x10 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"CheckBox On Screen [^2ON^7]" });
            }
        }

        private void disableToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1780f42 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x00 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"CheckBox On Screen [^1OFF^7]" });
            }
        }

        private void killToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1780f58 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 70 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^1You've been killed !" });
            }
        }

        private void kickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x178108f + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xff });
            }
        }

        private void enableToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x178102b + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x10 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"HUD Disable Mode [^2ON^7]" });
            }
        }

        private void disableToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x178102b + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x00 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"HUD Disable Mode [^1OFF^7]" });
            }
        }

        private void xylosCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (xylosCheckBox3.Checked)
                {
                    byte[] buffer2 = new byte[4];
                    buffer2[0] = 0x2c;
                    buffer2[1] = 4;
                    buffer = buffer2;
                    PS3.SetMemory(0x5f0bb0, buffer);
                }
                else
                {
                    buffer = new byte[] { 0x2c, 4, 0, 2 };
                    PS3.SetMemory(0x5f0bb0, buffer);
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (xylosCheckBox2.Checked)
                {
                    buffer = new byte[] { 1 };
                    PS3.SetMemory(0xef68f, buffer);
                }
                else
                {
                    buffer = new byte[1];
                    PS3.SetMemory(0xef68f, buffer);
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        
        private void timer8_Tick(object sender, EventArgs e)
        {
            timer8.Start();
            timer8.Interval = 100;
            Countername++;
            switch (Countername)
            {
                case 1:
                    {
                        byte[] buffer = new byte[] { 0x4f, 0, 0, 0, 0x3f, 0x80, 0, 0, 0, 0x3f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        break;
                    }
                case 2:
                    {
                        byte[] buffer2 = new byte[] { 0x6f, 0x80, 0, 0, 0x3f, 0x80, 0, 0, 0x6f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer2);
                        break;
                    }
                case 3:
                    {
                        byte[] buffer3 = new byte[] { 0x6f, 0x80, 0, 0, 0x4f, 0x80, 0, 0, 0x1f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer3);
                        break;
                    }
                case 4:
                    {
                        byte[] buffer4 = new byte[] { 0x4f, 0x80, 0, 0, 0x41, 240, 0, 0, 0x1f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer4);
                        break;
                    }
                case 5:
                    {
                        byte[] buffer5 = new byte[] { 0x7f, 0x80, 0, 0, 0x41, 0x90, 0, 0, 0x5f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer5);
                        break;
                    }
                case 6:
                    {
                        byte[] buffer6 = new byte[] { 0x1f, 0xff, 0, 0, 0x4f, 0, 0, 0, 0x4f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer6);
                        break;
                    }
                case 7:
                    {
                        byte[] buffer7 = new byte[] { 0x3d, 0, 0, 0, 0x4f, 0x80, 0, 0, 0x3f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer7);
                        break;
                    }
            }
            if (Countername == 8)
            {
                Countername = 0;
            }
            timer8.Start();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (comboBox1.Text.Equals("BLANC"))
                {
                    timer8.Stop();
                    buffer = new byte[] { 0x4f, 0xff, 0, 0, 0x4f, 0x80, 0, 0, 0x4f, 0x80 };
                    PS3.SetMemory(0x1cc14f8, buffer);
                    Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : BLANC" });
                }
                else if (comboBox1.Text.Equals("BLEU SICK"))
                {
                    timer8.Stop();
                    buffer = new byte[] { 0x7f, 0x80, 0, 0, 0x41, 0x40, 0, 0, 0x4f, 0x80 };
                    PS3.SetMemory(0x1cc14f8, buffer);
                    Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^5 BLEU SICK" });
                }
                else
                {
                    byte[] buffer2;
                    if (comboBox1.Text.Equals("BLEU"))
                    {
                        timer8.Stop();
                        buffer2 = new byte[10];
                        buffer2[8] = 0x40;
                        buffer = buffer2;
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^4 BLEU" });
                    }
                    else if (comboBox1.Text.Equals("CYAN FLUO"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x1f, 0xff, 0, 0, 0x4f, 0, 0, 0, 0x4f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^5 CYAN BLEU" });
                    }
                    else if (comboBox1.Text.Equals("NOIR"))
                    {
                        timer8.Stop();
                        buffer2 = new byte[10];
                        buffer = buffer2;
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^8 NOIR" });
                    }
                    else if (comboBox1.Text.Equals("ORANGE"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x4f, 0x80, 0, 0, 0x40, 240, 0, 0, 0x1f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^3 ORANGE" });
                    }
                    else if (comboBox1.Text.Equals("ROSE & BLANC"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x4f, 0x80, 0, 0, 0x41, 0x90, 0, 0, 0x5f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : BLANC & ^6ROSE" });
                    }
                    else if (comboBox1.Text.Equals("ROSE FLUO"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x6f, 0x80, 0, 0, 0x3f, 0x80, 0, 0, 0x6f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^6 ROSE FLUO" });
                    }
                    else if (comboBox1.Text.Equals("ROUGE FLUO"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x4f, 0, 0, 0, 0x3f, 0x80, 0, 0, 0x3f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^1 ROUGE FLUO" });
                    }
                    else if (comboBox1.Text.Equals("VERT"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x1f, 0x80, 0, 0, 0x41, 240, 0, 0, 0x1f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^2 VERT" });
                    }
                    else if (comboBox1.Text.Equals("VERT FLUO"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x3d, 0, 0, 0, 0x4f, 0x80, 0, 0, 0x3f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^2 VERT FLUO" });
                    }
                    else if (comboBox1.Text.Equals("VOLCAN"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x4f, 0x80, 0, 0, 0x41, 240, 0, 0, 0x1f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^3 VOLCAN" });
                    }
                    else if (comboBox1.Text.Equals("JAUNE FLUO"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x6f, 0x80, 0, 0, 0x4f, 0x80, 0, 0, 0x1f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^3 JAUNE FLUO" });
                    }
                    else if (comboBox1.Text.Equals("DISCO MOD"))
                    {
                        timer8.Start();
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^2D^5I^3S^1C^5O" });
                    }
                    else if (comboBox1.Text.Equals("NORMAL"))
                    {
                        timer8.Stop();
                        buffer = new byte[] { 0x3f, 0x80, 0, 0, 0x3f, 0x80, 0, 0, 0x3f, 0x80 };
                        PS3.SetMemory(0x1cc14f8, buffer);
                        Call(SV_GameSendServerCommand, new object[] { 0, 0, "; \"^7 Couleur : ^2 Normal" });
                    }
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (comboBox2.Text.Equals("BLANC"))
                {
                    buffer = new byte[] { 0x5f, 0xff };
                    PS3.SetMemory(0x1cc2998, buffer);
                }
                else if (comboBox2.Text.Equals("NOIR"))
                {
                    buffer = new byte[2];
                    PS3.SetMemory(0x1cc2998, buffer);
                }
                else if (comboBox2.Text.Equals("NORMAL"))
                {
                    buffer = new byte[] { 0x3f, 0x80 };
                    PS3.SetMemory(0x1cc2998, buffer);
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                if (comboBox3.Text.Equals("x10"))
                {
                    byte[] buffer = new byte[] { 0x99 };
                    PS3.SetMemory(0x1cc52d9, buffer);
                }
                else
                {
                    byte[] buffer2;
                    if (comboBox3.Text.Equals("x20"))
                    {
                        buffer2 = new byte[] { 0xa9 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("x30"))
                    {
                        byte[] buffer3 = new byte[] { 0xb9 };
                        PS3.SetMemory(0x1cc52d9, buffer3);
                    }
                    else if (comboBox3.Text.Equals("x40"))
                    {
                        byte[] buffer4 = new byte[] { 0xc9 };
                        PS3.SetMemory(0x1cc52d9, buffer4);
                    }
                    else if (comboBox3.Text.Equals("x50"))
                    {
                        buffer2 = new byte[] { 0xd9 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("x60"))
                    {
                        buffer2 = new byte[] { 0xe9 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("x70"))
                    {
                        buffer2 = new byte[] { 0xf9 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("x80"))
                    {
                        buffer2 = new byte[] { 0xfb };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("x90"))
                    {
                        buffer2 = new byte[] { 0xfc };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("x100"))
                    {
                        buffer2 = new byte[] { 0xff };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-100"))
                    {
                        buffer2 = new byte[1];
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-90"))
                    {
                        buffer2 = new byte[] { 0x12 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-80"))
                    {
                        buffer2 = new byte[] { 0x22 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-70"))
                    {
                        buffer2 = new byte[] { 50 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-60"))
                    {
                        buffer2 = new byte[] { 0x42 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-50"))
                    {
                        buffer2 = new byte[] { 0x52 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-40"))
                    {
                        buffer2 = new byte[] { 0x62 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-30"))
                    {
                        buffer2 = new byte[] { 0x70 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-20"))
                    {
                        buffer2 = new byte[] { 120 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("-10"))
                    {
                        buffer2 = new byte[] { 0x80 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                    else if (comboBox3.Text.Equals("NORMAL"))
                    {
                        buffer2 = new byte[] { 130 };
                        PS3.SetMemory(0x1cc52d9, buffer2);
                    }
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void enableToolStripMenuItem10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                PS3.SetMemory((uint)(0x1781470 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff });
                PS3.SetMemory((uint)(0x1cb2af2 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0, 2, 0, 0, 0, 0, 0x30, 0x23, 0xd7, 10, 0xd0, 1, 0x7d, 0x60 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"Rapid Fire [^2ON^7]" });
            }
        }
        byte[] buffer;
        private void disableToolStripMenuItem10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x12; i++)
            {
                int pedID = (Players.SelectedIndex);
                buffer = new byte[8];
                PS3.SetMemory((uint)(0x1781470 + (uint)Players.SelectedIndex * 0x5808), buffer);
                PS3.SetMemory((uint)(0x1cb2af2 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0, 2, 0, 0, 0, 0, 0x3f, 0x40, 0, 0, 0xd0, 1, 0x7d, 0x60 });
                Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"Rapid Fire [^1OFF^7]" });
            }
        }

        private void xylosCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (xylosCheckBox4.Checked)
                {
                    buffer = new byte[] { 5 };
                    PS3.SetMemory(0x1780f43, buffer);
                    PS3.SetMemory(0x1780f43 + 0x5808, buffer);
                    PS3.SetMemory(0x1780f43 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1780f43 + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"God Mode [^2ON^7] For ^5All !" });
                }
                else
                {
                    buffer = new byte[] { 4 };
                    PS3.SetMemory(0x1780f43, buffer);
                    PS3.SetMemory(0x1780f43 + 0x5808, buffer);
                    PS3.SetMemory(0x1780f43 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1780f43 + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"God Mode [^1OFF^7] For ^5All !" });
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                if (xylosCheckBox5.Checked)
                {
                    timer4.Start();
                    buffer = new byte[] { 0xff };
                    PS3.SetMemory(0x1781356, buffer);
                    PS3.SetMemory(0x1781356 + 0x5808, buffer);
                    PS3.SetMemory(0x1781356 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781356 + 0x5808 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781352, buffer);
                    PS3.SetMemory(0x1781352 + 0x5808, buffer);
                    PS3.SetMemory(0x1781352 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781352 + 0x5808 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x178135e, buffer);
                    PS3.SetMemory(0x178135e + 0x5808, buffer);
                    PS3.SetMemory(0x178135e + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x178135e + 0x5808 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781363, buffer);
                    PS3.SetMemory(0x1781363 + 0x5808, buffer);
                    PS3.SetMemory(0x1781363 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781363 + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"Max Ammo [^2ON^7] For ^5All !" });
                }
                else
                {
                    timer4.Stop();
                    buffer = new byte[] { 0x10 };
                    PS3.SetMemory(0x1781356, buffer);
                    PS3.SetMemory(0x1781356 + 0x5808, buffer);
                    PS3.SetMemory(0x1781356 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781356 + 0x5808 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781352, buffer);
                    PS3.SetMemory(0x1781352 + 0x5808, buffer);
                    PS3.SetMemory(0x1781352 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781352 + 0x5808 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x178135e, buffer);
                    PS3.SetMemory(0x178135e + 0x5808, buffer);
                    PS3.SetMemory(0x178135e + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x178135e + 0x5808 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781363, buffer);
                    PS3.SetMemory(0x1781363 + 0x5808, buffer);
                    PS3.SetMemory(0x1781363 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781363 + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"Max Ammo [^1OFF^7] For ^5All !" });
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            buffer = new byte[] { 0xff };
            PS3.SetMemory(0x1781356, buffer);
            PS3.SetMemory(0x1781356 + 0x5808, buffer);
            PS3.SetMemory(0x1781356 + 0x5808 + 0x5808, buffer);
            PS3.SetMemory(0x1781356 + 0x5808 + 0x5808 + 0x5808, buffer);
            PS3.SetMemory(0x1781352, buffer);
            PS3.SetMemory(0x1781352 + 0x5808, buffer);
            PS3.SetMemory(0x1781352 + 0x5808 + 0x5808, buffer);
            PS3.SetMemory(0x1781352 + 0x5808 + 0x5808 + 0x5808, buffer);
            PS3.SetMemory(0x178135e, buffer);
            PS3.SetMemory(0x178135e + 0x5808, buffer);
            PS3.SetMemory(0x178135e + 0x5808 + 0x5808, buffer);
            PS3.SetMemory(0x178135e + 0x5808 + 0x5808 + 0x5808, buffer);
            PS3.SetMemory(0x1781363, buffer);
            PS3.SetMemory(0x1781363 + 0x5808, buffer);
            PS3.SetMemory(0x1781363 + 0x5808 + 0x5808, buffer);
            PS3.SetMemory(0x1781363 + 0x5808 + 0x5808 + 0x5808, buffer);
        }

        private void xylosCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (xylosCheckBox6.Checked)
                {
                    buffer = new byte[] { 15, 0x42, 0x40 };
                    PS3.SetMemory(0x1786501, buffer);
                    PS3.SetMemory(0x1786501 + 0x5808, buffer);
                    PS3.SetMemory(0x1786501 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1786501 + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"Max Points [^2ON^7] For ^5All !" });
                }
                else
                {
                    byte[] buffer2 = new byte[3];
                    buffer2[1] = 1;
                    buffer2[2] = 0xf4;
                    buffer = buffer2;
                    PS3.SetMemory(0x1786501, buffer);
                    PS3.SetMemory(0x1786501 + 0x5808, buffer);
                    PS3.SetMemory(0x1786501 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1786501 + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"Max Points [^1OFF^7] For ^5All !" });
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosCheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (xylosCheckBox7.Checked)
                {
                    buffer = new byte[] { 5, 0x55, 0x4f };
                    PS3.SetMemory(0x1781424, buffer);
                    PS3.SetMemory(0x1781424 + 0x5808, buffer);
                    PS3.SetMemory(0x1781424 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1781424 + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"All Perks [^2ON^7] For ^5All !" });
                }
                else
                {
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"Can't Remove All Perks !" });
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosCheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (xylosCheckBox8.Checked)
                {
                    buffer = new byte[] { 0x40 };
                    PS3.SetMemory(0x1786418, buffer);
                    PS3.SetMemory(0x1786418 + 0x5808, buffer);
                    PS3.SetMemory(0x1786418 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1786418 + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"Speed x2 [^2ON^7] For ^5All !" });
                }
                else
                {
                    buffer = new byte[] { 0x3f};
                    PS3.SetMemory(0x1786418, buffer);
                    PS3.SetMemory(0x1786418 + 0x5808, buffer);
                    PS3.SetMemory(0x1786418 + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1786418 + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"Speed x2 [^1OFF^7] For ^5All !" });
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosCheckBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] buffer;
                if (xylosCheckBox9.Checked)
                {
                    buffer = new byte[] { 0x01 };
                    PS3.SetMemory(0x1780fac, buffer);
                    PS3.SetMemory(0x1780fac + 0x5808, buffer);
                    PS3.SetMemory(0x1780fac + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1780fac + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"Third Person [^2ON^7] For ^5All !" });
                }
                else
                {
                    buffer = new byte[] { 0x00 };
                    PS3.SetMemory(0x1780fac, buffer);
                    PS3.SetMemory(0x1780fac + 0x5808, buffer);
                    PS3.SetMemory(0x1780fac + 0x5808 + 0x5808, buffer);
                    PS3.SetMemory(0x1780fac + 0x5808 + 0x5808 + 0x5808, buffer);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"Third Person [^1OFF^7] For ^5All !" });
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                if (comboBox8.Text.Equals("PUNCH"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x1781193 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xa7 });
                        PS3.SetMemory((0x17811cb + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xa7 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Camo : ^5PUNCH" });
                    }
                }
                else if (comboBox8.Text.Equals("INVISIBLE"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x1781193 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xa8 });
                        PS3.SetMemory((0x17811cb + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xa8 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Camo : ^2Invisible" });
                    }
                }
                else if (comboBox8.Text.Equals("BLANC"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x1781193 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xa9 });
                        PS3.SetMemory((0x17811cb + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0xa9 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Camo : ^2Blank" });
                    }
                }
                else
                {
                    if (comboBox8.Text.Equals("NORMAL"))
                    {
                        for (int i = 0; i < 0x12; i++)
                        {
                            int pedID = (Players.SelectedIndex);
                            PS3.SetMemory((0x1781193 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 160 });
                            PS3.SetMemory((0x17811cb + (uint)Players.SelectedIndex * 0x5808), new byte[] { 160 });
                            Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Camo : ^2Default" });
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                if (comboBox4.Text.Equals("Taser"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x17810f7 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x47 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Taser ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("Knife Hunt"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x17810f7 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 100 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Knife Hunt ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("Balistik"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x17810f7 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x49 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Balistik ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("Shield"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x17810f7 + (uint)Players.SelectedIndex * 0x5808), new byte[] { 70 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Shield ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("Default Weapons"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x178118f + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x01 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Default Weapons ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("RPG"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x178118f + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x40 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2RPG ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("No Weapons"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x178118f + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x65 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2No Weapons ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("HAMR"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x178118f + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x29 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2HAMR ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("Galil"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x178118f + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x26 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2HAMR ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("RPD"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x178118f + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x28 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2RPD ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("AK74u"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x178118f + (uint)Players.SelectedIndex * 0x5808), new byte[] { 2 });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2AK74u ^7Gived !" });
                    }
                }
                if (comboBox4.Text.Equals("Ray Gun Mark II"))
                {
                    for (int i = 0; i < 0x12; i++)
                    {
                        int pedID = (Players.SelectedIndex);
                        PS3.SetMemory((0x178118f + (uint)Players.SelectedIndex * 0x5808), new byte[] { 0x4c });
                        Call(SV_GameSendServerCommand, new object[] { i, 0, "; \"^2Ray Gun Mark II ^7Gived !" });
                    }
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton3_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                if (xylosButton3.Text == "Deaths Barrier [ON]")
                {
                    byte[] buffer = new byte[] { 4 };
                    PS3.SetMemory(0x1786348, buffer);
                    byte[] buffer2 = new byte[] { 4 };
                    PS3.SetMemory(0x178bb50, buffer2);
                    byte[] buffer3 = new byte[] { 4 };
                    PS3.SetMemory(0x1791358, buffer3);
                    byte[] buffer4 = new byte[] { 4 };
                    PS3.SetMemory(0x1796b60, buffer4);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"^1Death Barrier ^5Disabled !" });
                    xylosButton3.Text = "Deaths Barrier [OFF]";
                }
                else
                {
                    byte[] buffer5 = new byte[1];
                    byte[] buffer = buffer5;
                    PS3.SetMemory(0x1786348, buffer);
                    buffer5 = new byte[1];
                    byte[] buffer2 = buffer5;
                    PS3.SetMemory(0x178bb50, buffer2);
                    buffer5 = new byte[1];
                    byte[] buffer3 = buffer5;
                    PS3.SetMemory(0x1791358, buffer3);
                    buffer5 = new byte[1];
                    byte[] buffer4 = buffer5;
                    PS3.SetMemory(0x1796b60, buffer4);
                    Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"^1Death Barrier ^2Enabled be carefull !" });
                    xylosButton3.Text = "Deaths Barrier [ON]";
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }

        private void xylosButton4_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 0x45, 0xef };
                PS3.SetMemory(0x5be0b4, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }

        private void xylosButton5_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] JUMP = new byte[] { 0x47, 0xFF };
                PS3.SetMemory(0x5be0b4, JUMP);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }       
        }

        private void xylosButton6_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] JUMP = new byte[] { 0x49, 0xFF };
                PS3.SetMemory(0x5be0b4, JUMP);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }       
        }

        private void xylosButton7_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] JUMP = new byte[] { 0x50, 0x01 };
                PS3.SetMemory(0x5be0b4, JUMP);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }       
        }

        private void xylosButton8_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] JUMP = new byte[] { 0x42, 0x9F };
                PS3.SetMemory(0x5be0b4, JUMP);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }       
        }

        private void xylosButton13_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] GRAVITY = new byte[] { 0x44, 0x00 };
                PS3.SetMemory(0x1caf9d8, GRAVITY);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        byte[] buffer2;
        private void xylosButton12_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer2 = new byte[2];
                buffer2[0] = 0x43;
                buffer = buffer2;
                PS3.SetMemory(0x1caf9d8, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton11_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 0x42, 0x48 };
                PS3.SetMemory(0x1caf9d8, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton10_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 0x40, 0x48 };
                PS3.SetMemory(0x1caf9d8, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton9_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 0x44, 0x48 };
                PS3.SetMemory(0x1caf9d8, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton18_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 1 };
                PS3.SetMemory(0x1ca4e7a, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton17_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 2 };
                PS3.SetMemory(0x1ca4e7a, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton16_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 3 };
                PS3.SetMemory(0x1ca4e7a, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton15_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 8 };
                PS3.SetMemory(0x1ca4e7a, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton14_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[1];
                PS3.SetMemory(0x1ca4e7a, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton23_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] TIMESCALE = new byte[] { 0x3F, 0x00 };
                PS3.SetMemory(0x01CB7BF8, TIMESCALE);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton22_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] TIMESCALE = new byte[] { 0x3F, 0xFF };
                PS3.SetMemory(0x01CB7BF8, TIMESCALE);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void aetherGroupBox7_Click(object sender, EventArgs e)
        {

        }

        private void xylosButton21_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] TIMESCALE = new byte[] { 0x3F, 0x80 };
                PS3.SetMemory(0x01CB7BF8, TIMESCALE);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton19_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                byte[] bytes = BitConverter.GetBytes(Convert.ToInt32(numericUpDown13.Value.ToString()));
                PS3.SetMemory(0x160b093, bytes);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void xylosButton27_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                PS3.SetMemory(0x26fca80, BitConverter.GetBytes((int)numericUpDown14.Value));
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosButton26_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 0xd6, 0xd6, 0x99, 0x99 };
                PS3.SetMemory(0x26fca80, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosButton25_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 0xd6 };
                PS3.SetMemory(0x26fca80, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosButton24_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 200 };
                PS3.SetMemory(0x26fca80, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosButton20_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                buffer = new byte[] { 100 };
                PS3.SetMemory(0x26fca80, buffer);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosButton28_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                PS3.Extension.WriteString(0x26c0658, xylosTextBox1.Text);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void Flash_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            int Color = random.Next(0, 7);
            byte[] Code1 = Encoding.ASCII.GetBytes("^" + Color + DateTime.Now.ToLongTimeString());
            Array.Resize(ref Code1, Code1.Length + 1);
            PS3.SetMemory(0x26c0658, Code1);
        }

        private void xylosButton29_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                PS3.Extension.WriteString(0x178646c, xylosTextBox2.Text);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosButton30_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                PS3.Extension.WriteString(0x178bc74, xylosTextBox3.Text);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosButton31_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                PS3.Extension.WriteString(0x179147c, xylosTextBox4.Text);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosButton32_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                PS3.Extension.WriteString(0x1796c84, xylosTextBox5.Text);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosButton33_Click(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                PS3.Extension.WriteString(0x178646c, xylosTextBox6.Text);
                PS3.Extension.WriteString(0x178bc74, xylosTextBox6.Text);
                PS3.Extension.WriteString(0x179147c, xylosTextBox6.Text);
                PS3.Extension.WriteString(0x1796c84, xylosTextBox6.Text);
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void xylosCheckBox10_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void xylosCheckBox11_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Clock_Tick(object sender, EventArgs e)
        {
        }

        private void xylosButton34_Click(object sender, EventArgs e)
        {
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] buffer;
            byte[] buffer2;
            byte[] buffer3;
            byte[] buffer4;
            if (IsConnected == true)
            {
                if (comboBox5.Text.Equals("DINER"))
                {
                    buffer = new byte[] { 0xc5, 0x8b, 0x47, 0x15, 0xc5, 0xdb, 0x4a, 0xeb, 0xc2, 0x89, 0x1a, 0x69 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0xc5, 0x8b, 0x47, 0x15, 0xc5, 0xdb, 0x4a, 0xeb, 0xc2, 0x89, 0x1a, 0x69 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0xc5, 0x8b, 0x47, 0x15, 0xc5, 0xdb, 0x4a, 0xeb, 0xc2, 0x89, 0x1a, 0x69 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0xc5, 0x8b, 0x47, 0x15, 0xc5, 0xdb, 0x4a, 0xeb, 0xc2, 0x89, 0x1a, 0x69 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("FERME"))
                {
                    buffer = new byte[] { 0x45, 0xd7, 0x55, 0xfc, 0xc5, 0xb2, 0x8e, 0x7c, 0xc2, 0x7a, 0x18, 0x1b };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0x45, 0xd7, 0x55, 0xfc, 0xc5, 0xb2, 0x8e, 0x7c, 0xc2, 0x7a, 0x18, 0x1b };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0x45, 0xd7, 0x55, 0xfc, 0xc5, 0xb2, 0x8e, 0x7c, 0xc2, 0x7a, 0x18, 0x1b };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0x45, 0xd7, 0x55, 0xfc, 0xc5, 0xb2, 0x8e, 0x7c, 0xc2, 0x7a, 0x18, 0x1b };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("NACH DER UNTOTEN"))
                {
                    buffer = new byte[] { 70, 0x59, 0x2e, 0x6a, 0xc4, 0x84, 0xe1, 0x77, 0xc3, 0x3d, 0xe0 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 70, 0x59, 0x2e, 0x6a, 0xc4, 0x84, 0xe1, 0x77, 0xc3, 0x3d, 0xe0 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 70, 0x59, 0x2e, 0x6a, 0xc4, 0x84, 0xe1, 0x77, 0xc3, 0x3d, 0xe0 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 70, 0x59, 0x2e, 0x6a, 0xc4, 0x84, 0xe1, 0x77, 0xc3, 0x3d, 0xe0 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("POWER"))
                {
                    buffer = new byte[] { 70, 0x3d, 0xe0, 0xe0, 70, 3, 0x39, 0x1c, 0xc4, 0x3b, 0xd8 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 70, 0x3d, 0xe0, 0xe0, 70, 3, 0x39, 0x1c, 0xc4, 0x3b, 0xd8 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 70, 0x3d, 0xe0, 0xe0, 70, 3, 0x39, 0x1c, 0xc4, 0x3b, 0xd8 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 70, 0x3d, 0xe0, 0xe0, 70, 3, 0x39, 0x1c, 0xc4, 0x3b, 0xd8 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("PACK A PUNCH"))
                {
                    buffer = new byte[] { 0x44, 0x90, 0xf9, 9, 0xc3, 0x2f, 0x4f, 0x2f, 0xc3, 0x97, 240 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0x44, 0x90, 0xf9, 9, 0xc3, 0x2f, 0x4f, 0x2f, 0xc3, 0x97, 240 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0x44, 0x90, 0xf9, 9, 0xc3, 0x2f, 0x4f, 0x2f, 0xc3, 0x97, 240 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0x44, 0x90, 0xf9, 9, 0xc3, 0x2f, 0x4f, 0x2f, 0xc3, 0x97, 240 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("BANK"))
                {
                    buffer = new byte[] { 0x43, 0xef, 9, 0x44, 0x43, 0xdf, 0xa5, 0xbd, 0xc2, 0x1f, 0x80 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0x43, 0xef, 9, 0x44, 0x43, 0xdf, 0xa5, 0xbd, 0xc2, 0x1f, 0x80 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0x43, 0xef, 9, 0x44, 0x43, 0xdf, 0xa5, 0xbd, 0xc2, 0x1f, 0x80 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0x43, 0xef, 9, 0x44, 0x43, 0xdf, 0xa5, 0xbd, 0xc2, 0x1f, 0x80 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("FRIGO"))
                {
                    buffer = new byte[] { 70, 0, 50, 0x4d, 0xc5, 0xd3, 0xcd, 0x13, 0x42, 0xea, 0x40 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 70, 0, 50, 0x4d, 0xc5, 0xd3, 0xcd, 0x13, 0x42, 0xea, 0x40 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 70, 0, 50, 0x4d, 0xc5, 0xd3, 0xcd, 0x13, 0x42, 0xea, 0x40 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 70, 0, 50, 0x4d, 0xc5, 0xd3, 0xcd, 0x13, 0x42, 0xea, 0x40 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("SEMTEX"))
                {
                    buffer = new byte[] { 0x44, 0x65, 0x7d, 0x85, 0xc4, 0xb6, 0x7c, 0xd5, 0xc2, 0x2d, 0xd7, 0x80 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0x44, 0x65, 0x7d, 0x85, 0xc4, 0xb6, 0x7c, 0xd5, 0xc2, 0x2d, 0xd7, 0x80 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0x44, 0x65, 0x7d, 0x85, 0xc4, 0xb6, 0x7c, 0xd5, 0xc2, 0x2d, 0xd7, 0x80 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0x44, 0x65, 0x7d, 0x85, 0xc4, 0xb6, 0x7c, 0xd5, 0xc2, 0x2d, 0xd7, 0x80 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("BUCHERON"))
                {
                    buffer = new byte[] { 0x45, 160, 30, 0x56, 0x45, 0xc2, 0xd8, 0x55, 0xc2, 0x7f, 0x80 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0x45, 160, 30, 0x56, 0x45, 0xc2, 0xd8, 0x55, 0xc2, 0x7f, 0x80 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0x45, 160, 30, 0x56, 0x45, 0xc2, 0xd8, 0x55, 0xc2, 0x7f, 0x80 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0x45, 160, 30, 0x56, 0x45, 0xc2, 0xd8, 0x55, 0xc2, 0x7f, 0x80 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("POING TASER"))
                {
                    buffer = new byte[] { 0xc5, 0xc6, 0x51, 7, 0xc5, 0xf6, 0xa4, 0x72, 0x43, 20, 0x20 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0xc5, 0xc6, 0x51, 7, 0xc5, 0xf6, 0xa4, 0x72, 0x43, 20, 0x20 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0xc5, 0xc6, 0x51, 7, 0xc5, 0xf6, 0xa4, 0x72, 0x43, 20, 0x20 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0xc5, 0xc6, 0x51, 7, 0xc5, 0xf6, 0xa4, 0x72, 0x43, 20, 0x20 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox5.Text.Equals("TUNEL"))
                {
                    buffer = new byte[] { 0xc6, 0x31, 0x59, 0xf4, 0xc4, 0x36, 0x68, 0xee, 0x43, 0x40, 0x20 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0xc6, 0x31, 0x59, 0xf4, 0xc4, 0x36, 0x68, 0xee, 0x43, 0x40, 0x20 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0xc6, 0x31, 0x59, 0xf4, 0xc4, 0x36, 0x68, 0xee, 0x43, 0x40, 0x20 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0xc6, 0x31, 0x59, 0xf4, 0xc4, 0x36, 0x68, 0xee, 0x43, 0x40, 0x20 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] buffer;
            byte[] buffer2;
            byte[] buffer3;
            byte[] buffer4;
            if (IsConnected == true)
            {
                if (comboBox6.Text.Equals("Juggernot"))
                {
                    buffer = new byte[] { 0x44, 0x77, 110, 0x81, 0xc4, 0xb1, 0xb0, 0x62, 0x43, 0, 0x20 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0x44, 0x77, 110, 0x81, 0xc4, 0xb1, 0xb0, 0x62, 0x43, 0, 0x20 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0x44, 0x77, 110, 0x81, 0xc4, 0xb1, 0xb0, 0x62, 0x43, 0, 0x20 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0x44, 0x77, 110, 0x81, 0xc4, 0xb1, 0xb0, 0x62, 0x43, 0, 0x20 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox6.Text.Equals("Speed cola"))
                {
                    buffer = new byte[] { 0xc5, 0xaf, 0x51, 0x76, 0xc5, 0xf6, 0xcb, 7, 0x3e };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0xc5, 0xaf, 0x51, 0x76, 0xc5, 0xf6, 0xcb, 7, 0x3e };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0xc5, 0xaf, 0x51, 0x76, 0xc5, 0xf6, 0xcb, 7, 0x3e };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0xc5, 0xaf, 0x51, 0x76, 0xc5, 0xf6, 0xcb, 7, 0x3e };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox6.Text.Equals("Double Hits"))
                {
                    buffer = new byte[] { 0x45, 0xfb, 50, 0x4e, 0xc5, 0x93, 0x19, 0x19, 0x43, 0x84, 0x10 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0x45, 0xfb, 50, 0x4e, 0xc5, 0x93, 0x19, 0x19, 0x43, 0x84, 0x10 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0x45, 0xfb, 50, 0x4e, 0xc5, 0x93, 0x19, 0x19, 0x43, 0x84, 0x10 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0x45, 0xfb, 50, 0x4e, 0xc5, 0x93, 0x19, 0x19, 0x43, 0x84, 0x10 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox6.Text.Equals("QUICK REVIVE"))
                {
                    buffer = new byte[] { 0xc5, 0xd1, 0xa6, 6, 0x45, 0x9f, 0x59, 0xe1, 0xc2, 0x5c, 0x74, 0x18 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0xc5, 0xd1, 0xa6, 6, 0x45, 0x9f, 0x59, 0xe1, 0xc2, 0x5c, 0x74, 0x18 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0xc5, 0xd1, 0xa6, 6, 0x45, 0x9f, 0x59, 0xe1, 0xc2, 0x5c, 0x74, 0x18 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0xc5, 0xd1, 0xa6, 6, 0x45, 0x9f, 0x59, 0xe1, 0xc2, 0x5c, 0x74, 0x18 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox6.Text.Equals("R.I.P"))
                {
                    buffer = new byte[] { 70, 0x29, 0x5b, 0x35, 70, 1, 150, 0x5d, 0xc3, 0xcb, 240 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 70, 0x29, 0x5b, 0x35, 70, 1, 150, 0x5d, 0xc3, 0xcb, 240 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 70, 0x29, 0x5b, 0x35, 70, 1, 150, 0x5d, 0xc3, 0xcb, 240 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 70, 0x29, 0x5b, 0x35, 70, 1, 150, 0x5d, 0xc3, 0xcb, 240 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
                else if (comboBox6.Text.Equals("MARATHON"))
                {
                    buffer = new byte[] { 0x44, 0xf1, 0xe3, 0x42, 0x43, 250, 0x30, 0xb6, 0xc2, 0x56, 0x10, 0xd0 };
                    PS3.SetMemory(0x1780f50, buffer);
                    buffer2 = new byte[] { 0x44, 0xf1, 0xe3, 0x42, 0x43, 250, 0x30, 0xb6, 0xc2, 0x56, 0x10, 0xd0 };
                    PS3.SetMemory(0x1786758, buffer2);
                    buffer3 = new byte[] { 0x44, 0xf1, 0xe3, 0x42, 0x43, 250, 0x30, 0xb6, 0xc2, 0x56, 0x10, 0xd0 };
                    PS3.SetMemory(0x178bf60, buffer3);
                    buffer4 = new byte[] { 0x44, 0xf1, 0xe3, 0x42, 0x43, 250, 0x30, 0xb6, 0xc2, 0x56, 0x10, 0xd0 };
                    PS3.SetMemory(0x1791768, buffer4);
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void enableToolStripMenuItem11_Click(object sender, EventArgs e)
        {
        }

        private void disableToolStripMenuItem11_Click(object sender, EventArgs e)
        {
            
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"[{+gostand}] ^5Welcome to ^2MrNiato^1's Modded Lobby ! [{+activate}] \n[{+breath_sprint}] ^3www.ihax.fr  [{+melee}]\n[{+smoke}] ^6www.allcodrecovery.com [{+frag}]\n[{togglemenu}] ^7wwww.boutique.h7k3r.fr [{togglescores}]" });
            Thread.Sleep(1500);
            Call(SV_GameSendServerCommand, new object[] { -1, 0, "; \"[{+actionslot 4}] ^2Made By ^5MrNiato [{+actionslot 4}] \n[{+actionslot 1}] ^2Faceboook : ^1Guillaume MrNiato[{+actionslot 2}]" });
        }

        private void xylosCheckBox10_CheckedChanged_1(object sender, EventArgs e)
        {
            if (IsConnected == true)
            {
                if (xylosCheckBox10.Checked)
                {
                    timer5.Start();
                }
                else
                {
                    timer5.Stop();
                }
            }
            else
            {
                MessageBox.Show("Playstation 3 Is Not Connected !!", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }        
    }
}
