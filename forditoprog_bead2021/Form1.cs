using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace fordito_winFrom
{
    public partial class Form1 : Form
    {
        Stack<string> verem;
        public Form1()
        {
            InitializeComponent();
        }

        private void Start_click(object sender, EventArgs e)
        {
            this.init();

            //str-be elmentem az inputot szóköz nélkül
            string updatedStr = this.removeSpace(this.textBox1.Text);
            if (updatedStr.Length > 0 && (int)updatedStr[updatedStr.Length - 1] != 35)
            {
               
                result.Text = "Lemaradt #";
            }
            else
            {
                result.Items.Add("Kifejezés kiértékelése: ");
                string str = this.checkInput(updatedStr);
                if (str != "")
                {
                    result.Items.Add("Státusz: elfogad");
                }
                else
                {
                    result.Items.Add("Hiba a kiértékelésnél!");
                }
            }
        }
        private void ESC_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            defaultTable();
        }
        private void init()
        {
            this.result.Items.Clear();
            this.verem = new Stack<string>();
            this.verem.Push("#");
            this.verem.Push("E");

        }
        private string getVeremData() {
            string temp=string.Empty;
            List<string> tempverem = new List<string>();
            while (verem.Count > 0) {
                string veremItem = verem.Pop();
                temp += veremItem;
                tempverem.Add(veremItem);
            }
            for (int i = tempverem.Count-1; i >= 0; i--)
            {
                verem.Push(tempverem[i]);
            }
            return temp;
        }
        private int getRowIndex(string s)
        {
            int index = 0;
            while (index < this.rules.RowCount && rules.Rows[index].HeaderCell.Value.ToString().ToUpper() != s.ToUpper())
                index++;
            return this.rules.Rows[index].HeaderCell.RowIndex;
        }
        private int searchInputInHeader(char actChar)
        {
            int columnCount = this.rules.ColumnCount;
            for (int currentCol = 0; currentCol < columnCount; ++currentCol)
            {
                if ((int)this.getHeaderText(currentCol) == (int)actChar)
                    return currentCol;
            }
            //-1 mert nincs ilyen eleme
            return -1;
        }
        private char getHeaderText(int colIndex)
        {
            string headerText = this.rules.Columns[colIndex].HeaderText;
            try
            {
                return char.Parse(headerText);
            }
            catch(Exception e)
            {
                MessageBox.Show("Hiba" + e.Message);
                return ' ';
            }
        }
        private string RegEx(string filter, string input, string swap)
        {
            return new Regex(filter).Replace(input, swap);
        }
        private string removeSpace(string input)
        {
            return this.RegEx("\\s+", input, "");
        }
        private string checkInput(string updatedStr)
        {
            string updatedStrHelper = updatedStr;
            string ret = "";
            try
            {
                while (true)
                {
                    string s = this.verem.Pop();
                    if (s!="e")
                    {
                        if (s==updatedStrHelper[0].ToString())
                        {
                            if (!(s == "#"))
                                updatedStrHelper = this.removeAt(updatedStrHelper, 1);
                            else
                                break;
                        }
                        else
                        {
                            int index = this.searchInputInHeader(updatedStrHelper[0]);
                            string[] nextMoveAndNum = this.rules.Rows[this.getRowIndex(s)].Cells[index].Value.ToString().Split(',');
                            string strArrayHelper = nextMoveAndNum[0];
                            string moveNum = nextMoveAndNum[1];
                            this.listToStack(this.splitString(strArrayHelper));
                            
                            ret += moveNum;
                            string rules = getVeremData();
                            result.Items.Add(string.Format("({0}, {1}, {2})",updatedStrHelper, rules ,ret));
                        }
                    }
                }
                result.Items.Add("OK!");
                return ret;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "";
            }
        }
        private string removeAt(string data, int index)
        {
            string str = "";
            for (int indexWalker = index; indexWalker < data.Length; ++indexWalker)
                str += data[indexWalker].ToString();
            return str;
        }
        private void listToStack(List<string> items)
        {
            //jobbról balra járjuk be a listát LIFO miatt
            for (int index = items.Count - 1; index >= 0; --index)
                this.verem.Push(items[index]);
        }
        private List<string> splitString(string curStr)
        {
            List<string> dataList = new List<string>();
            for (int curPos = 0; curPos < curStr.Length; ++curPos)
            {
                if (curStr.Length == 1)
                    dataList.Add(curStr);
                else if ((int)curStr[curPos] == 39)
                {
                    List<string> dataListHelper = dataList;
                    int curPosHelper = curPos - 1;
                    dataListHelper[curPosHelper] = dataListHelper[curPosHelper] + "'";
                }
                else
                    dataList.Add(curStr[curPos].ToString());
            }
            return dataList;
        }

        private void defaultTable()
        {
            rules.Columns.Add("+", "+");
            rules.Columns.Add("*", "*");
            rules.Columns.Add("(", "(");
            rules.Columns.Add(")", ")");
            rules.Columns.Add("i", "i");
            rules.Columns.Add("#", "#");
            rules.Rows.Add("", "", "TE',1", "", "TE',1", "");
            rules.Rows.Add("+TE',2", "", "", "e,3", "", "e,3");
            rules.Rows.Add("", "", "FT,4", "", "FT',4", "");
            rules.Rows.Add("e,6", "*FT',5", "", "e,6", "", "e,6");
            rules.Rows.Add("", "", "(E),7", "", "i,8", "");
            rules.Rows.Add("pop", "", "", "", "", "");
            rules.Rows.Add("", "pop", "", "", "", "");
            rules.Rows.Add("", "", "pop", "", "", "");
            rules.Rows.Add("", "", "", "pop", "", "");
            rules.Rows.Add("", "", "", "", "pop", "");
            rules.Rows.Add("", "", "", "", "", "OK");
            //header értékek hozzárendelése a datagridview-hoz
            rules.Rows[0].HeaderCell.Value = "E";
            rules.Rows[1].HeaderCell.Value = "E'";
            rules.Rows[2].HeaderCell.Value = "T";
            rules.Rows[3].HeaderCell.Value = "T'";
            rules.Rows[4].HeaderCell.Value = "F";
            rules.Rows[5].HeaderCell.Value = "+";
            rules.Rows[6].HeaderCell.Value = "*";
            rules.Rows[7].HeaderCell.Value = "(";
            rules.Rows[8].HeaderCell.Value = ")";
            rules.Rows[9].HeaderCell.Value = "i";
            rules.Rows[10].HeaderCell.Value = "#";
            rules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
