﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warship
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        private string Chars = " loxh";
        private string Title = " ABCDEFGHIJ";
        int[] Ships = {4, 3, 3, 2, 2, 2, 1, 1, 1, 1};

        char[,] HomeMap = new char[12, 12];
        char[,] EnemyMap = new char[12, 12];

        public void GenerateMap(char[,] Map)
        {
            //Filling array with spaces
            
            for(int i = 0; i < 11; i++)
                for (int j = 0; j < 11; j++)
                    Map[i, j] = Chars[0];

            for (int ShipLengthIndex = 0; ShipLengthIndex < Ships.Length; )
            {
                int ShipLength = Ships[ShipLengthIndex];
                //choosing orientation of new ship
                int Orientation = rnd.Next(2); //0 - horizontal, 1 - vertical

                //choosing left top ship coordinates, keeping inside array
                int X = rnd.Next(1, 10 - Orientation * 3);
                int Y = rnd.Next(1, 10 - (1 - Orientation) * 3);

                bool free = true;
                for (int i = 0; i < ShipLength; i++)
                    if (Map[X + i * Orientation, Y + i * (1 - Orientation)] != Chars[0])
                        free = false;

                //filling ship cells on the map, moving down or right according to orientation
                if (free)
                {
                    for (int i = -1; i < ShipLength + 1; i++)
                    for (int j = -1; j <= 1; j++)
                        Map[X + i * Orientation + j * (1 - Orientation),
                            Y + j * Orientation + i * (1 - Orientation)] = Chars[4];

                    for (int i = 0; i < ShipLength; i++)
                        Map[X + i * Orientation, Y + i * (1 - Orientation)] = Chars[2];

                    ShipLengthIndex++;
                }
            }

            //delete waves
            for(int i = 0; i < 11; i++)
                for(int j = 1; j < 11; j++)
                    if (Map[i, j] == Chars[4])
                        Map[i, j] = Chars[0];

        }

        //showing map on the form
        public void ShowMap(char[,] Map, TableLayoutPanel tlp)
        {
            for(int i = 1; i < 11; i++)
                for (int j = 1; j < 11; j++)
                {
                    Label label = tlp.Controls[i * 11 + j] as Label;
                    label.Text = Convert.ToString(Map[i, j]);
                    label.ForeColor = label.BackColor;
                }


        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HomeShips.Controls.Clear();
            for (int i = 0; i < 11; i++) 
                for (int j = 0; j < 11; j++)
                {
                    Label homeShip = new Label();
                    homeShip.Dock = DockStyle.Fill;
                    homeShip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    homeShip.Font = new System.Drawing.Font("Wingdings", 22);
                    homeShip.Text = Convert.ToString(i) + Convert.ToString(j);
                    homeShip.Text = " ";
                    HomeShips.Controls.Add(homeShip, j, i);
                    homeShip.Click += Label_Click_Home;
                    
                    
                    Label enemyShip = new Label();
                    enemyShip.Dock = DockStyle.Fill;
                    enemyShip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    enemyShip.Font = new System.Drawing.Font("Wingdings", 22);
                    enemyShip.Text = " ";
                    enemyShip.Text = Chars.Substring(0, 1);
                    EnemyShips.Controls.Add(enemyShip, j, i);
                    enemyShip.Click += Label_Click_Enemy;
                }
            
            for (int i = 1; i < 11; i++)
            {
                Label enemyShip = EnemyShips.Controls[i*11] as Label;
                enemyShip.Font = new System.Drawing.Font("Arial", 12);
                enemyShip.Text = Convert.ToString(i);

            }

            for (int j = 1; j < 11; j++)
            {
                Label enemyShip = EnemyShips.Controls[j] as Label;
                enemyShip.Font = new System.Drawing.Font("Arial", 12);
                enemyShip.Text = Convert.ToString(Title[j]);

            }
            
            for (int i = 1; i < 11; i++)
            {
                Label homeShip = HomeShips.Controls[i*11] as Label;
                homeShip.Font = new System.Drawing.Font("Arial", 12);
                homeShip.Text = Convert.ToString(i);

            }
            
            for (int j = 1; j < 11; j++)
            {
                Label homeShip = HomeShips.Controls[j] as Label;
                homeShip.Font = new System.Drawing.Font("Arial", 12);
                homeShip.Text = Convert.ToString(Title[j]);

            }

            GenerateMap(HomeMap);
            GenerateMap(EnemyMap);
            ShowMap(HomeMap, HomeShips);
            ShowMap(EnemyMap, EnemyShips);


        }

        private void Label_Click_Home(object sender, EventArgs e)
        {
            Label a = new Label();
            a = sender as Label;

            switch ((string) a.Text)
            {
                case " ":
                    a.Text = Convert.ToString(Chars[1]);
                    a.ForeColor = Color.Black;
                    break;
                case "o":
                    a.Text = Convert.ToString(Chars[3]);
                    a.ForeColor = Color.Black;
                    break;
            }

        }
        private void Label_Click_Enemy(object sender, EventArgs e)
        {
            Label a = new Label();
            a = sender as Label;
            switch ((string)a.Text)
            {
                case " ": a.Text = Convert.ToString(Chars[1]);
                    a.ForeColor = Color.Black;
                    break;
                case "o": a.Text = Convert.ToString(Chars[3]);
                    a.ForeColor = Color.Black;
                    break;
            }

        }
    }
}
