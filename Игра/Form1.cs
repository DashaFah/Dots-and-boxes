using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Игра
{
    public partial class Form1 : Form
    {
        static Graphics g;
        static int numofcells=1;
        static bool igrok1 = true;
        static int CursorX;
        static int CursorY;
        bool Clicked;
        static int fieldsize = 0;
        static public int Schet1 = 0;
        static public int Schet2 = 0;


        public struct border
        {
            public bool coloured;
            public bool side;
            public bool Red;
        };

        public struct Catched
        {
            public bool closed;
            public bool cross;
        }

        public struct Cell
        {
            public Catched catched;
            public Point Coors;
            public border borderL;
            public border borderR;
            public border borderU;
            public border borderD;
        };

        static public List<Cell> AllCells = new List<Cell>();
        //class myPanel : Panel { public myPanel() { this.DoubleBuffered = true; } };
        public Form1()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("C:/Users/mvideo/Documents/Visual Studio 2015/Projects/Игра/Page.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.BackColor = Color.Transparent;
            pictureBox1.BackColor = Color.Transparent;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;           
            comboBox2.Items.Add("7*7");
            comboBox2.Items.Add("9*9");
            comboBox2.Items.Add("13*13");
            comboBox2.SelectedItem = "9*9";
            ActivePlayer();

            //myPanel Field = new myPanel();


        }




        void Field_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);

            Pen PenLines = new Pen(Color.Blue, 1.5f);
            //g = Field.CreateGraphics();
            int w = pictureBox1.Width;
            int h = pictureBox1.Height;
            int firstY = h / 2 - 10;
            int firstX = w / 2 - 10;
            for (int i = 0; i < Math.Ceiling((double)fieldsize/2); i++)
            {
                int X = firstX;
                for (int a = 0; a < numofcells; a++)
                {
                    int tempY = Convert.ToInt32(h / 2 - (double)fieldsize / 2 * 20);                
                    Point pos1 = new Point(X,   tempY + i * 20);
                    Point pos3 = new Point(X, tempY + (fieldsize - i) * 20);
                    Point pos2 = new Point(X + 20, tempY + i * 20);
                    Point pos4 = new Point(X + 20, tempY + (fieldsize - i) * 20);

                    Point central = new Point();
                    central = Zentrum(pos1, pos2, true);
                    int CellIndex = FindCell(central);
                    PenLines = DefineColor2(pos1, pos2, CellIndex);                   
                    g.DrawLine(PenLines, pos1, pos2);

                    central = Zentrum(pos3, pos4, true);
                    CellIndex = FindCell(central);
                    PenLines = DefineColor2(pos3, pos4, CellIndex);
                    g.DrawLine(PenLines, pos3, pos4);

                    X += 20;                  
                }
                numofcells +=2;
                firstX -= 20;
            }

            numofcells = 1;
            for (int i = 0; i < (fieldsize+1)/2; i++)
            {
                int Y = firstY;
                for (int a = 0; a < numofcells; a++)
                {
                    int tempX = Convert.ToInt32(w / 2 - (double)fieldsize / 2 * 20);
                    Point pos1 = new Point(tempX + i * 20, Y);
                    Point pos3 = new Point(tempX + (fieldsize - i) * 20, Y);
                    Point pos2 = new Point(tempX + i * 20, Y+20);
                    Point pos4 = new Point(tempX + (fieldsize - i) * 20, Y+20);

                    Point central = new Point();
                    central = Zentrum(pos1, pos2, true);
                    int CellIndex = FindCell(central);
                    PenLines = DefineColor2(pos1, pos2, CellIndex);
                    g.DrawLine(PenLines, pos1, pos2);

                    central = Zentrum(pos3, pos4, true);
                    CellIndex = FindCell(central);
                    PenLines = DefineColor2(pos3, pos4, CellIndex);
                    g.DrawLine(PenLines, pos3, pos4);

                    Y += 20;
                }
                numofcells += 2;
                firstY -= 20;
                
            }
            numofcells = 1;
            blockcells();         
            checkcellclosed();
            drawicon();
            if (Clicked == true)
            {
                Clicked = false;
                if (igrok1 == true) igrok1 = false;
                else igrok1 = true;
                ActivePlayer();
            }
            

            pictureBox1.Image = bmp;
        }

        void checkcellclosed()
        {
            for (int i = 0; i < AllCells.Count; i++ )
            {
                if (AllCells[i].catched.closed == false)
                {
                    if ((AllCells[i].borderD.coloured == true || AllCells[i].borderD.side == true)
                        && (AllCells[i].borderU.coloured == true || AllCells[i].borderU.side == true)
                        && (AllCells[i].borderL.coloured == true || AllCells[i].borderL.side == true) 
                        && (AllCells[i].borderR.coloured == true || AllCells[i].borderR.side == true
                        && !(AllCells[i].borderD.side == true && AllCells[i].borderU.side == true && AllCells[i].borderL.side == true && AllCells[i].borderR.side == true)))
                    {
                        if (igrok1 == true)
                        {
                            Cell ctemp = new Cell();
                            ctemp = AllCells[i];
                            ctemp.catched.closed = true;
                            ctemp.catched.cross = false;
                            AllCells[i] = ctemp;
                            Schet1++;
                            String SchetString1 = Convert.ToString(Schet1);
                            label5.Text = SchetString1;
                        }
                        else
                        {
                            Cell ctemp = new Cell();
                            ctemp = AllCells[i];
                            ctemp.catched.closed = true;
                            ctemp.catched.cross = true;
                            AllCells[i] = ctemp;
                            Schet2++;
                            String SchetString2 = Convert.ToString(Schet2);
                            label6.Text = SchetString2;
                        }
                        if (igrok1 == true) igrok1 = false;
                        else igrok1 = true;
                        ActivePlayer();
                        return;
                    }
                }
            }
        }

        void drawicon()
        {
            for (int i = 0; i < AllCells.Count; i++)
            {
                if (AllCells[i].catched.closed == true)
                {
                    if (AllCells[i].catched.cross == true)
                    {
                        Point cross11 = new Point(AllCells[i].Coors.X - 8, AllCells[i].Coors.Y - 8);
                        Point cross12 = new Point(AllCells[i].Coors.X + 8, AllCells[i].Coors.Y + 8);
                        Pen pen = new Pen(Color.Red, 2f);
                        g.DrawLine(pen, cross11, cross12);
                        Point cross21 = new Point(AllCells[i].Coors.X + 8, AllCells[i].Coors.Y - 8);
                        Point cross22 = new Point(AllCells[i].Coors.X - 8, AllCells[i].Coors.Y + 8);
                        g.DrawLine(pen, cross21, cross22);
                    }
                    else
                    {
                        Rectangle rect = new Rectangle(AllCells[i].Coors.X - 8, AllCells[i].Coors.Y - 8, 16, 16);
                        Pen pen = new Pen(Color.DarkGreen, 2f);
                        g.DrawEllipse(pen, rect);
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            if (comboBox2.Text == "9*9") fieldsize = 9;
            else if (comboBox2.Text == "13*13") fieldsize = 13;
                else if (comboBox2.Text == "7*7") fieldsize = 7;

            if (textBox1.Text != "") label1.Text = textBox1.Text;
            else label1.Text = "Игрок 1";
            if (textBox2.Text != "") label2.Text = textBox2.Text;
            else label2.Text = "Игрок 2";

            int firstX = pictureBox1.Width / 2;
            for (int i = 0; i < Math.Ceiling((double)fieldsize / 2); i++)
            {
                int X = firstX;
                for (int a = 0; a < numofcells; a++)
                {
                    int tempY = Convert.ToInt32(pictureBox1.Height / 2 - Math.Floor((double)fieldsize  / 2)*20);
                    Catched c1 = new Catched() { closed = false };
                    Cell cc = new Cell() { catched = c1 };
                    cc.Coors.X = X;
                    cc.Coors.Y = tempY + i * 20;
                    AllCells.Add(cc);
                    X += 20;
                }
                numofcells += 2;
                firstX -= 20;
            }
            numofcells = 1;

            firstX = pictureBox1.Width / 2;
            for (int i = 0; i < Math.Floor((double)fieldsize / 2); i++)
            {
                int X = firstX;
                for (int a = 0; a < numofcells; a++)
                {
                    int tempY = Convert.ToInt32(pictureBox1.Height / 2 + Math.Floor((double)fieldsize / 2)*20);
                    Cell cc = new Cell();
                    cc.Coors.X = X;
                    cc.Coors.Y = tempY - i * 20;
                    AllCells.Add(cc);
                    X += 20;
                }
                numofcells += 2;
                firstX -= 20;
            }
            numofcells = 1;
        }

        void ActivePlayer()
        {
            if (igrok1 == true)
            {
                label1.Font = new Font(label1.Font.Name, 20, FontStyle.Bold);
                label2.Font = new Font(label1.Font.Name, 14, FontStyle.Regular);
            }
            else
            {
                label1.Font = new Font(label1.Font.Name, 14, FontStyle.Regular);
                label2.Font = new Font(label1.Font.Name, 20, FontStyle.Bold);
            }
        }

        void blockcells()
        {
            for (int i = 0; i < AllCells.Count; i++)
            {
                if (AllCells[i].borderD.side == true && AllCells[i].borderU.side == true || AllCells[i].borderL.side == true && AllCells[i].borderR.side == true)
                {
                    Brush brush = Brushes.Blue;
                    g.FillRectangle(brush, AllCells[i].Coors.X - 10, AllCells[i].Coors.Y - 10, 20, 20);
                    Cell temp = AllCells[i];
                    Cell temp2 = new Cell();
                    if (AllCells[i].borderD.side == false)
                    {
                        temp.borderD.side = true;
                        temp2 = AllCells[i + 2];
                        temp2.borderU.side = true;
                        AllCells[i] = temp;
                        AllCells[i + 2] = temp2;
                    }
                    else if (AllCells[i].borderL.side == false)
                    {
                        temp.borderL.side = true;
                        temp2 = AllCells[i - 1];
                        temp2.borderR.side = true;
                        AllCells[i] = temp;
                        AllCells[i - 1] = temp2;
                    }
                    else if (AllCells[i].borderR.side == false)
                    {
                        temp.borderR.side = true;
                        temp2 = AllCells[i + 1];
                        temp2.borderL.side = true;
                        AllCells[i] = temp;
                        AllCells[i + 1] = temp2;
                    }
                    else if (AllCells[i].borderU.side == false)
                    {
                        temp.borderU.side = true;
                        temp2 = AllCells[i + 2];
                        temp2.borderD.side = true;
                        AllCells[i] = temp;
                        AllCells[i + 2] = temp2;
                    }
                }
            }
        }

        Pen DefineColor(Point p1, Point p2)
        {
            Pen pen = new Pen(Color.Blue);
            int avarageX = (p2.X - p1.X) / 2 + p1.X;
            int avarageY = (p2.Y - p1.Y) / 2 + p1.Y; //Это средние точки прямой
            double dist = Math.Sqrt(Math.Pow(avarageX - CursorX, 2) + Math.Pow((avarageY - CursorY), 2)); //Расстояние от точки клика до центра прямой

            if (Clicked == true) //Если перерисовывается из-за нажатия
            {
                int index = FindCell(Zentrum(p1, p2, true));
                int index2 = FindCell(Zentrum(p1, p2, false)); //индексы клеток в списке, которым принадлежит сторона
                if (dist < 5 * Math.Sqrt(2)) //если расстояние от точки клика до средней точки меньше вычисленного радиуса
                {
                    /* if (/*FindCell(Zentrum(p1, p2, true)) == -1 || FindCell(Zentrum(p1, p2, false)) == -1 || /*AllCells[FindCell(Zentrum(p1, p2, true))].borderL.side == true
                         || AllCells[FindCell(Zentrum(p1, p2, true))].borderR.side == true || AllCells[FindCell(Zentrum(p1, p2, true))].borderU.side == true
                         || AllCells[FindCell(Zentrum(p1, p2, true))].borderD.side == true*/
                    //   p1.Y == p2.Y) //Если с одной из сторон от линии клетки нет, значит она боковая
                    // {
                    if (FindCell(Zentrum(p1, p2, true)) == -1 || FindCell(Zentrum(p1, p2, false)) == -1)
                    {

                        Clicked = false;
                        if (igrok1 == true) igrok1 = false;
                        else igrok1 = true;
                        ActivePlayer();
                        return pen;//выходим из функции

                    }
                    else if ((AllCells[FindCell(Zentrum(p1, p2, true))].borderD.side == true && AllCells[FindCell(Zentrum(p1, p2, false))].borderU.side == true) || (AllCells[FindCell(Zentrum(p1, p2, true))].borderR.side == true && AllCells[FindCell(Zentrum(p1, p2, false))].borderL.side == true))
                    {
                        Clicked = false;
                        if (igrok1 == true) igrok1 = false;
                        else igrok1 = true;
                        ActivePlayer();
                        return pen;//выходим из функции
                    }
                    //}


                    //if (p1.Y == p2.Y && AllCells[FindCell(Zentrum(p1, p2, true))].borderD.coloured == false || p1.X == p2.X && AllCells[FindCell(Zentrum(p1, p2, true))].borderR.coloured == false)
                    //{
                    else {
                        if (igrok1 == true) //Если все клетки существуют, то цвет выбирается исходя из игрока
                        {
                            pen = new Pen(Color.DarkGreen, 1.5f);
                            //ActivePlayer();
                        }
                        else
                        {
                            pen = new Pen(Color.Red, 1.5f);
                            //ActivePlayer();
                        }
                        Clicked = false; //отключаем режим клика

                        if (p1.Y == p2.Y)//добавляем информацию о прямой в список клеток. Если линия горизонтальная
                        {
                            Point сent1 = new Point(avarageX, avarageY - 10);
                            Point сent2 = new Point(avarageX, avarageY + 10);

                            border b1 = new border() { coloured = false, side = false, Red = false };

                            {
                                if (pen.Color == Color.Blue) b1.coloured = false;
                                else
                                {
                                    b1.coloured = true;
                                    if (pen.Color == Color.Red) b1.Red = true;
                                    else b1.Red = false;
                                }
                            }

                            int cellIndex = FindCell(сent1);
                            Cell temp = AllCells[cellIndex];
                            temp.borderD = b1;
                            AllCells[cellIndex] = temp;

                            cellIndex = FindCell(сent2);
                            temp = AllCells[cellIndex];
                            temp.borderU = b1;
                            AllCells[cellIndex] = temp;
                        }
                        else //Если вертикальная
                        {
                            Point сent1 = new Point(avarageX - 10, avarageY);
                            Point сent2 = new Point(avarageX + 10, avarageY);

                            border b1 = new border() { coloured = false, side = false, Red = false };
                            if (pen.Color == Color.Blue) b1.coloured = false;
                            else
                            {
                                b1.coloured = true;
                                if (pen.Color == Color.Red) b1.Red = true;
                                else b1.Red = false;
                            }

                            int cellIndex = FindCell(сent1);
                            Cell temp = AllCells[cellIndex];
                            temp.borderR = b1;
                            AllCells[cellIndex] = temp;

                            cellIndex = FindCell(сent2);
                            temp = AllCells[cellIndex];
                            temp.borderL = b1;
                            AllCells[cellIndex] = temp;
                        }
                
                    }
                }
                else pen = new Pen(Color.Blue, 1.5f);//Если расстояние меньше, просто отрисуем сторону
            }
            //Если отрисовывается для показа возможных мест для постановки линии
            else {
                if (dist < 5 * Math.Sqrt(2)) //если расстояние меньше, покажем, что туда можно поставить
                {
                    if (p1.Y == p1.Y)
                    {
                        int index = FindCell(Zentrum(p1, p2, true));
                        int index2 = FindCell(Zentrum(p1, p2, false));
                        if (index != -1 && index2 != -1)
                        {
                            if ((AllCells[FindCell(Zentrum(p1, p2, true))].borderD.side == false && AllCells[FindCell(Zentrum(p1, p2, false))].borderU.side == false) || (AllCells[FindCell(Zentrum(p1, p2, true))].borderR.side == false && AllCells[FindCell(Zentrum(p1, p2, false))].borderL.side == false))
                                pen = new Pen(Color.Pink, 2f);
                        }
                    else pen = new Pen(Color.Blue, 1.5f);
                    }
                }
                else //иначе просто отрисуем сторону, а если она боковая, то внесём эти данные в список клеток
                {
                    pen = new Pen(Color.Blue, 1.5f);
                    int index = FindCell(Zentrum(p1, p2, true));
                    int index2 = FindCell(Zentrum(p1, p2, false));
                    Cell temp = new Cell();
                    if (index != -1 && index2 == -1)
                    {
                        temp = AllCells[index];
                        if (p1.Y == p2.Y) temp.borderD.side = true;
                        else temp.borderR.side = true;
                        AllCells[index] = temp;
                    }
                    else if ((index2 != -1 && index == -1)) 
                        {
                            temp = AllCells[index2];
                            if (p1.Y == p2.Y) temp.borderU.side = true;
                            else temp.borderL.side = true;
                            AllCells[index2] = temp;
                        }
                    else pen = new Pen(Color.Blue, 1.5f);
                }
            }
            return pen;
        }

        int FindCell(Point centr)
        {
            for (int i=0; i<AllCells.Count; i++)
            {
                Cell cc = AllCells[i];
                if (centr.X == cc.Coors.X && centr.Y == cc.Coors.Y)
                    return i;
            }
            return -1;
        }

        private void Field_MouseClick(object sender, MouseEventArgs e)
        {         
            CursorX = e.X;
            CursorY = e.Y;
            Clicked = true;
            Refresh();
            if (igrok1 == true) igrok1 = false;
            else igrok1 = true;
            ActivePlayer();
        }

        Point Zentrum(Point p1,Point p2, bool prev)
        {
            Point central;
            int avarageX = (p2.X - p1.X) / 2 + p1.X;
            int avarageY = (p2.Y - p1.Y) / 2 + p1.Y;
            if (prev == true)
            {
                if (p1.Y == p2.Y) central = new Point(avarageX, avarageY - 10);
                else central = new Point(avarageX - 10, avarageY);
            }
            else
            {
                if (p1.Y == p2.Y) central = new Point(avarageX, avarageY + 10);
                else central = new Point(avarageX + 10, avarageY);
            }
            return central;
        }

        Pen DefineColor2(Point p1, Point p2, int index)
        {
            Pen pen;
            if (index != -1)
            {
                if (p1.Y == p2.Y)
                {
                    if (AllCells[index].borderD.coloured == true)
                    {
                        if (AllCells[index].borderD.Red == true) pen = new Pen(Color.Red, 2f);
                        else pen = new Pen(Color.DarkGreen, 2f);
                    }
                    else pen = DefineColor(p1, p2);
                }
                else
                {
                    if (AllCells[index].borderR.coloured == true)
                    {
                        if (AllCells[index].borderR.Red == true) pen = new Pen(Color.Red, 2f);
                        else pen = new Pen(Color.DarkGreen, 2f);
                    }
                    else pen = DefineColor(p1, p2);//Если граница ещё не закрашена, то идём её закрашивать
                }
            }
            else pen = DefineColor(p1, p2); //Значит, она боковушка. И её надо просто закрасить чёрным. Всегда
            return pen;
        }

        bool checkside(Point p1, Point p2)
        {
            int index = FindCell(Zentrum(p1, p2, true));
            int index2 = FindCell(Zentrum(p1, p2, false));
            if (index == -1 || index2 == -1) return true;
            else return false;
        }

        

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            CursorX = e.X;
            CursorY = e.Y;
            Refresh();
        }

    }
}
