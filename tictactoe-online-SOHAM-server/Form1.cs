using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace tictactoe_online_SOHAM_server
{
    public partial class Form1 : Form
    {
        public Socket server;
        public Socket[] sockets;
        public IPEndPoint servip;
        public byte[] buffer;
        public StringBuilder msg;
        public string message;
        public int[] play;
        public int turn;
        public int moves;


       
        public int checkrows(int val)
        {
            if ((play[0] * play[1] * play[2]) == val)
            {
                if (play[0] == 1)
                    return 0;
                if (play[1] == 1)
                    return 1;
                if (play[2] == 1)
                    return 2;
                return val;
            }

            else if ((play[3] * play[4] * play[5]) == val)
            {
                if (play[3] == 1)
                    return 3;
                if (play[4] == 1)
                    return 4;
                if (play[5] == 1)
                    return 5;
                return val;
            }
            if ((play[6] * play[7] * play[8]) == val)
            {
                if (play[6] == 1)
                    return 6;
                if (play[7] == 1)
                    return 7;
                if (play[8] == 1)
                    return 8;
                return val;
            }

            return -1;
        
        }

        public int checkcol(int val)
        {
            if ((play[0] * play[3] * play[6]) == val)
            {
                if (play[0] == 1)
                    return 0;
                if (play[3] == 1)
                    return 3;
                if (play[6] == 1)
                    return 6;
                return val;
            }

            else if ((play[1] * play[4] * play[7]) == val)
            {
                if (play[1] == 1)
                    return 1;
                if (play[4] == 1)
                    return 4;
                if (play[7] == 1)
                    return 7;
                return val;
            }
            if ((play[2] * play[5] * play[8]) == val)
            {
                if (play[2] == 1)
                    return 2;
                if (play[5] == 1)
                    return 5;
                if (play[8] == 1)
                    return 8;
                return val;
            }

            return -1;

        }

        public int checkdia(int val)
        {
            if ((play[0] * play[4] * play[8]) == val)
            {
                if (play[0] == 1)
                    return 0;
                if (play[4] == 1)
                    return 4;
                if (play[8] == 1)
                    return 8;
                return val;
            }

            else if ((play[2] * play[3] * play[6]) == val)
            {
                if (play[2] == 1)
                    return 2;
                if (play[3] == 1)
                    return 3;
                if (play[6] == 1)
                    return 6;
                return val;
            }
           
            return -1;

        }
        
        
        public int win(int who)                               // returns 1 if won
        {
            if (who == 2)
            {
                if (((checkrows(8) == 8) || (checkcol(8) == 8)) || (checkdia(8) == 8))
                    return 1;
                else
                    return 0;
            }
            else
            {
                if (((checkrows(27) == 27) || (checkcol(27) == 27)) || (checkdia(27) == 27))
                    return 1;
                else
                    return 0;
            }
        }

        
        
        public void string2int()
        {
            for (int i = 0; i < 9; i++)
                play[i] = Int32.Parse(""+message[i]);
        
        }

      
        
        public void game()
        {
            string temp="000000000";
            while (moves > 0)
            {

                if (turn == 1)
                {
                    
                    sockets[0].Receive(buffer);
                    temp = Encoding.ASCII.GetString(buffer);
                    if (temp.CompareTo("000000000") != 0)
                    {
                        moves--;
                        this.message = Encoding.ASCII.GetString(buffer);
                      //  MessageBox.Show("RECIEVED FROM PLAYER 1 : " + message);
                        this.string2int();
                        checkwin();
                        if (moves == 0)
                            finish(0);
                        // MessageBox.Show("SENDING TO PLAYER 2 : 000000000");
                        sockets[1].Send(Encoding.ASCII.GetBytes("000000000"));
                        sockets[1].Send(buffer);
                        this.turn = 2;
                        
                        temp = "000000000";
                        
                    }
                    
                    }

                if (turn == 2)
                {
                    sockets[1].Receive(buffer);
                    temp = Encoding.ASCII.GetString(buffer);
                    if (temp.CompareTo("000000000") != 0)
                    {
                        moves--;
                        this.message = Encoding.ASCII.GetString(buffer);
                     //   MessageBox.Show("RECIEVED FROM PLAYER 2 : " + message);
                        this.string2int();
                        checkwin();
                        if (moves == 0)
                            finish(0);

                        //MessageBox.Show("SENDING TO PLAYER 1 : 000000000"); 
                        sockets[0].Send(Encoding.ASCII.GetBytes("000000000"));
                        sockets[0].Send(buffer);
                        this.turn = 1;
                        
                        temp = "000000000";
                        
                    }
                }
            
            }

            if (moves == 0)
                finish(0);
        
        
        
        }


        public void checkwin()
        {
            if (win(2) == 1)
                finish(1);
            else if (win(3) == 1)
                finish(2);
            else
                return;
        }

        public void finish(int what)
        {
            if (what == 0)
            {
                sockets[0].Send(Encoding.ASCII.GetBytes("595959595"));   // draw
                sockets[0].Send(buffer);
                sockets[1].Send(Encoding.ASCII.GetBytes("595959595"));
                sockets[1].Send(buffer);
                label4.Text = "GAME IS A DRAW ";
                Application.DoEvents();
            
            }

            if (what == 1)
            {
                
                sockets[1].Send(Encoding.ASCII.GetBytes("999999999"));
                sockets[1].Send(buffer);
                sockets[0].Send(Encoding.ASCII.GetBytes("555555555"));
                sockets[0].Send(buffer);

                label4.Text = "PLAYER 1 WINS ";
                Application.DoEvents();
            
            }

            if (what == 2)
            {
                sockets[0].Send(Encoding.ASCII.GetBytes("999999999"));
                sockets[0].Send(buffer);
                sockets[1].Send(Encoding.ASCII.GetBytes("555555555"));
                sockets[1].Send(buffer);

                label4.Text = "PLAYER 2 WINS ";
                Application.DoEvents();
            
            
            }
        }
       
        
        
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("WELCOME TO TICTACTOE-Online-SERVER designed by Soham Chakraborty.  Please give the SOCKET INFORMATION and Click RUN to Begin ");
            buffer = new byte[9];
            this.turn = 0;
            play = new int[9];
            for (int i = 0; i < 9; i++)
                play[i] = 1;
            sockets = new Socket[2];
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            msg = new StringBuilder(9);
            this.moves = 9;
        
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int nop = 0;
            servip = new IPEndPoint(IPAddress.Parse(textBox1.Text), Int32.Parse(textBox2.Text));
            server.Bind(servip);
            label4.Text = "SERVER STARTED SUCCESSFULLY ";
            Application.DoEvents();
            server.Listen(2);
            
            sockets[0] = server.Accept();
            nop++;
            label2.Text = "PLAYER 1 has JOINED ";
            sockets[0].Send(Encoding.ASCII.GetBytes("000000000"));
            label3.Text = "WAITING FOR PLAYER 2 to JOIN ";
            this.turn = 1;
            Application.DoEvents();

            sockets[1] = server.Accept();
            label3.Text = "PLAYER 2 has JOINED ";
            sockets[1].Send(Encoding.ASCII.GetBytes("999999999"));
            label4.Text = "GAME IS ABOUT TO COMMENCE ";
            Application.DoEvents();

            sockets[0].Send(Encoding.ASCII.GetBytes("111111111"));
            label4.Text = "GAME STARTED ";
            Application.DoEvents();
            game();
        
       
        
        }
    }
}