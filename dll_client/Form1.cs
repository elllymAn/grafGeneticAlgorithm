using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using GraphsElements;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dll_client
{
    public partial class Form1 : Form
    {
        string graphData = "";
        string graphDataHistory = "";
        //создан заранее класс Node(хранит в себе информацию о вершине)
        private List<Node> _nodes = new List<Node>();
        private String alfabeth = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private bool _moving = false;
        private int deltaX = 0;
        private int deltaY = 0;
        private bool _edge_mode = false;
        private List<Node> _node_picked = new List<Node>();
        private List<Node> _nodes_draw = new List<Node>();
        private String inPut = ";";
        private bool isPainted = false;
        //private StringBuilder input = new StringBuilder(";",255);
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Включение режима ребер и изменение цвета кнопки
            if (!_edge_mode)
            {
                _edge_mode = true;
                button2.BackColor = Color.Aqua;
            }
            else
            {
                _edge_mode = false;
                button2.BackColor = Color.Khaki;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //создание графического элемента графа
            if (_nodes.Count() < 10)
            {
                if (_nodes.Count() != 0)
                    _nodes[_nodes.Count() - 1]._moveStatus = false;
                _nodes.Add(new Node(10, 10));
                _node_picked.Clear();
                pictureBox1.Invalidate();
            }
            else
            {
                MessageBox.Show("Больше вершин не влезет!!", "Предупреждение");
            }
            graphData = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //преобразование строки к формату приема в DLL
            String points = "";
            foreach (var node in _nodes)
            {
                points += node._name + ",";
            }

            if(points.Length > 0) points = points.Substring(0, points.Length - 1);
            if (graphData != "")
            {
                graphDataHistory = graphData;
                graphData = points + ";" + graphData;
            }
            else graphData = points + ";" + graphDataHistory;
            //вызов фукнции преобразования
            StringBuilder output = new StringBuilder(graphData, 1024);
            StringBuilder input = new StringBuilder(1024);
            StringBuilder richText = new StringBuilder(1024);
            graphData = "";
            geneticModify(output, input, richText, input.Capacity);
            inPut = input.ToString();
            richTextBox2.Text = richText.ToString();
           // MessageBox.Show(output.ToString(), "");

            MessageBox.Show("Все возможные варианты можно увидеть в ячейке", "Информация");//);
            // вызов функции отрисовки
            enterStack();
            pictureBox2.Invalidate();
            isPainted = true;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //отрисовка графа, созданного пользователем
            int i = 0;
            foreach (var node in this._nodes)
            {
                e.Graphics.DrawRectangle(Pens.Black, node._form);
                e.Graphics.FillRectangle(Brushes.White, node._form);
                using (Font font1 = new Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Point))
                {
                    e.Graphics.DrawString(alfabeth[i].ToString(), font1, Brushes.Black, node._form);
                }
                node._name = alfabeth[i].ToString();
                i++;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //логика работы при нажатии мыши
            if(!isPainted)
            {
                if (_edge_mode)
                {
                    //закраска выбранных вершин
                    if (_node_picked.Count() < 2)
                    {
                        foreach (var node in this._nodes)
                        {
                            //если мышка находится в вершине
                            if ((e.X < node._form.X + 50) && (e.X > node._form.X))
                            {
                                if ((e.Y < node._form.Y + 20) && (e.Y > node._form.Y))
                                {
                                    Graphics g = pictureBox1.CreateGraphics();
                                    g.FillRectangle(Brushes.Cyan, node._form);
                                    //index_picked.Add(node._form.X + node._form.Width / 2); index_picked.Add(node._form.Y + node._form.Height / 2);
                                    _node_picked.Add(node);
                                }
                            }
                        }
                    }
                    else if (_node_picked.Count() == 2)
                    {
                        //отрисовка между ними ребра
                        Graphics g = pictureBox1.CreateGraphics();
                        SolidBrush br1 = new SolidBrush(Color.Black);
                        Pen pn = new Pen(br1);
                        //отрисовка соединения
                        g.DrawLine(pn, _node_picked[0]._form.X + _node_picked[0]._form.Width / 2, _node_picked[0]._form.Y + _node_picked[0]._form.Height / 2,
                            _node_picked[1]._form.X + _node_picked[1]._form.Width / 2, _node_picked[1]._form.Y + _node_picked[1]._form.Height / 2);
                        //заполнение данных о создаваемой связи
                        this.graphData += _node_picked[0]._name + "-" + _node_picked[1]._name + ",";
                        //возвращение цвета выбранным вершинам
                        g.FillRectangle(Brushes.White, _node_picked[0]._form);
                        g.FillRectangle(Brushes.White, _node_picked[1]._form);
                        //отрисовка имени вершины, т.к. закраска покрывает ранее выданное имя
                        using (Font font1 = new Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            g.DrawString(_node_picked[0]._name, font1, Brushes.Black, _node_picked[0]._form);
                            g.DrawString(_node_picked[1]._name, font1, Brushes.Black, _node_picked[1]._form);
                        }
                        _node_picked.Clear();
                    }
                }
                else
                {
                    _nodes_draw.Clear();
                    //если не включен режим ребер, то при нажатии позволяет перемещать объект
                    if (_nodes.Count != 0)
                    {

                        if ((e.X < _nodes[_nodes.Count() - 1]._form.X + 50) && (e.X > _nodes[_nodes.Count() - 1]._form.X))
                        {
                            if ((e.Y < _nodes[_nodes.Count() - 1]._form.Y + 20) && (e.Y > _nodes[_nodes.Count() - 1]._form.Y))
                            {
                                _moving = true;
                                deltaX = e.X - _nodes[_nodes.Count() - 1]._form.X;
                                deltaY = e.Y - _nodes[_nodes.Count() - 1]._form.Y;
                            }
                        }
                    }
                }
            }
            else
            {
                pictureBox1.Invalidate();
                isPainted = false;
                _node_picked.Clear();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //отключение режима движения элемента
            _moving = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //мышка передает свои координаты для удобного распределения вершин в пределах выделенной "территории"
            int margin = 5;
            if (_moving)
            {
                _nodes[_nodes.Count() - 1]._form.X = Math.Max(margin, Math.Min(pictureBox1.Width - _nodes[_nodes.Count() - 1]._form.Width-margin, e.X - deltaX));
                _nodes[_nodes.Count() - 1]._form.Y = Math.Max(margin, Math.Min(pictureBox1.Height - _nodes[_nodes.Count() - 1]._form.Height-margin, e.Y - deltaY));
                pictureBox1.Invalidate();
            }
        }
        [DllImport(@"..\..\..\Debug\dll.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        static extern void geneticModify( StringBuilder output,StringBuilder input, StringBuilder richText, int outSize);
        private String[] parseResult(String type)
        {
            //парсинг приходящей строки
            if (type == "points")
                return ((inPut.Split('/'))[0]
                .Split(';'))[0]
                .Split(',');
            else
                return ((inPut.Split('/'))[0]
                .Split(';'))[1]
                .Split(',');
        }

        private void enterStack()
        {
            //создание элементов для преобразованного графа
            int y = 10;
            int margin = 10;
            String[] points = parseResult("points");
            foreach (String point in points)
            {
                if(point != "")
                {
                    Node elem = new Node(40, y)
                    {
                        _name = point
                    };
                    _nodes_draw.Add(elem);
                    y += 20 + margin;
                }
            }
        }
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            //рисуем вершины, созданные в enterStack()
            foreach (var node in this._nodes_draw)
            {
                e.Graphics.DrawRectangle(Pens.Black, node._form);
                e.Graphics.FillRectangle(Brushes.White, node._form);
                using (Font font1 = new Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Point))
                {
                    e.Graphics.DrawString(node._name, font1, Brushes.Black, node._form);
                }
            }
            //подключение графики для отрисовки ребер
            int movingRight = 5;
            Graphics g = pictureBox2.CreateGraphics();
            Point p1 = new Point(0,0); Point p2 = new Point(0,0);
            //получение данных о имеющихся ребрах
            String[] edges = parseResult("edge");
            edges = edges.Where(val => val != "").ToArray();
            //отрисовка имеющихся ребер
            foreach (String edge in edges)
            {
                String[] names = edge.Split('-');
                for (int i = 0; i < _nodes_draw.Count; i++)
                {
                    if (_nodes_draw[i]._name == names[0])
                    {
                        p1.Y = _nodes_draw[i]._y + _nodes_draw[i]._form.Height / 2;
                        p1.X = _nodes_draw[i]._x + _nodes_draw[i]._form.Width;
                    }
                    if (_nodes_draw[i]._name == names[1])
                    {
                        p2.Y = _nodes_draw[i]._y + _nodes_draw[i]._form.Height / 2;
                        p2.X = _nodes_draw[i]._x + _nodes_draw[i]._form.Width;
                    }
                }
                e.Graphics.DrawLine(Pens.Black, p1.X, p1.Y, p1.X + movingRight, p1.Y);
                e.Graphics.DrawLine(Pens.Black, p1.X + movingRight, p1.Y, p1.X + movingRight, p2.Y);
                e.Graphics.DrawLine(Pens.Black, p1.X + movingRight, p2.Y, p2.X, p2.Y);
                movingRight += 5;
            }
            _nodes_draw.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //нахождение возможных утечек
            string s = "";
            IntPtr ip;
            ip = Marshal.StringToCoTaskMemUni(s); // копирование управляемой строки в неупр.
            int len = s.Length;
            MemoryLeaks(ref ip, ref len);
            if (len > 0)
            {
                richTextBox1.Text = Marshal.PtrToStringUni(ip);
                Marshal.FreeCoTaskMem(ip);
            }
            MessageBox.Show("Возможные утечки также записаны в log.txt", "Информация");
        }
        [DllImport(@"..\..\..\Debug\dll.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static extern void MemoryLeaks(ref IntPtr s_array, ref int s_len);

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //если утечки были все-таки найдены
            MessageBox.Show("Утечки найдены!!!", "Предупреждение");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Это приложение позволяет нарисовать линейный граф с наименьшей длиной ребер", "Информация", MessageBoxButtons.OK);
            System.Windows.Forms.ToolTip t = new System.Windows.Forms.ToolTip();
            t.SetToolTip(button1, "Добавьте же вершину!");
            System.Windows.Forms.ToolTip t2 = new System.Windows.Forms.ToolTip();
            t2.SetToolTip(button2, "А соединять вершины интересно!");
            System.Windows.Forms.ToolTip t3 = new System.Windows.Forms.ToolTip();
            t3.SetToolTip(button3, "Теперь ваш граф станет плоским!");
            System.Windows.Forms.ToolTip t4 = new System.Windows.Forms.ToolTip();
            t4.SetToolTip(button4, "Проверим, все ли работает...");
            System.Windows.Forms.ToolTip t5 = new System.Windows.Forms.ToolTip();
            t5.SetToolTip(pictureBox1, "Тут вы можете нарисовать граф");
            System.Windows.Forms.ToolTip t6 = new System.Windows.Forms.ToolTip();
            t6.SetToolTip(pictureBox2, "Здесь уже увидете готовый:)");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _node_picked.Clear();
            _nodes.Clear();
            isPainted = false;
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
            _edge_mode = false;
            button2.BackColor = Color.Khaki;
            richTextBox2.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Help form2 = new Help();
            form2.ShowDialog();
        }

    }
}
