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

namespace dll_client
{
    public partial class Form1 : Form
    {
        string graphData = "";
        string graphDataHistory = "";
        private List<Node> _nodes = new List<Node>();
        private String alfabeth = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private bool _moving = false;
        private int deltaX = 0;
        private int deltaY = 0;
        private bool _edge_mode = false;
        private List<Node> _node_picked = new List<Node>();
        private List<Node> _nodes_draw = new List<Node>();
        private String inPut = ";";
        //private StringBuilder input = new StringBuilder(";",255);
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!_edge_mode)
            {
                _edge_mode = true;
                button2.BackColor = Color.Aqua;
            }
            else
            {
                _edge_mode = false;
                button2.BackColor = Color.White;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show("We can't visualize more vertex's");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
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

            StringBuilder output = new StringBuilder(graphData, 1024);
            StringBuilder input = new StringBuilder(1024);
            graphData = "";
            geneticModify(output, input, input.Capacity);
            inPut = input.ToString();

           // MessageBox.Show(output.ToString(), "");
            MessageBox.Show("you can see all variants of graphs in log.txt", "Info");//);

            enterStack();
            pictureBox2.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int i = 0;
            foreach (var node in this._nodes)
            {
                e.Graphics.DrawRectangle(Pens.Black, node._form);
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
            if (_edge_mode)
            {

                if (_node_picked.Count() < 2)
                {
                    foreach (var node in this._nodes)
                    {
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
                    Graphics g = pictureBox1.CreateGraphics();
                    SolidBrush br1 = new SolidBrush(Color.Black);
                    Pen pn = new Pen(br1);
                    g.DrawLine(pn, _node_picked[0]._form.X + _node_picked[0]._form.Width / 2, _node_picked[0]._form.Y + _node_picked[0]._form.Height / 2,
                        _node_picked[1]._form.X + _node_picked[1]._form.Width / 2, _node_picked[1]._form.Y + _node_picked[1]._form.Height / 2);
                    this.graphData += _node_picked[0]._name + "-" + _node_picked[1]._name + ",";
                    g.FillRectangle(Brushes.White, _node_picked[0]._form);
                    g.FillRectangle(Brushes.White, _node_picked[1]._form);
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
                if(_nodes.Count != 0)
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

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _moving = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int margin = 5;
            if (_moving)
            {
                _nodes[_nodes.Count() - 1]._form.X = Math.Max(margin, Math.Min(pictureBox1.Width - _nodes[_nodes.Count() - 1]._form.Width-margin, e.X - deltaX));
                _nodes[_nodes.Count() - 1]._form.Y = Math.Max(margin, Math.Min(pictureBox1.Height - _nodes[_nodes.Count() - 1]._form.Height-margin, e.Y - deltaY));
                pictureBox1.Invalidate();
            }
        }
        [DllImport(@"..\..\..\Release\dll.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        static extern void geneticModify( StringBuilder output,StringBuilder input, int outSize);
        private String[] parseResult(String type)
        {
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
            foreach (var node in this._nodes_draw)
            {
                e.Graphics.DrawRectangle(Pens.Black, node._form);
                using (Font font1 = new Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Point))
                {
                    e.Graphics.DrawString(node._name, font1, Brushes.Black, node._form);
                }
            }
            int movingRight = 5;
            Graphics g = pictureBox2.CreateGraphics();
            Point p1 = new Point(0,0); Point p2 = new Point(0,0);
            String[] edges = parseResult("edge");
            edges = edges.Where(val => val != "").ToArray();
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
    }
}
