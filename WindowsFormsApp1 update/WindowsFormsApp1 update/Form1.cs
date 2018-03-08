using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1_update
{
    public partial class Form1 : Form
    {
        static string resultFileAddress, fileType, head1, head2, head3, head4, head5, head6;
        static bool fileAdd1_ok, fileAdd2_ok, fileAdd3_ok, resultName_ok, head1_ok, head2_ok, head3_ok, head4_ok, head5_ok, head6_ok;
        static byte[] head1_Byte, head2_Byte, head3_Byte, head4_Byte, head5_Byte, head6_Byte;

        struct DataLineStruct
        {
            public byte commenValue;
            public byte bbSecondValue;
            public List<Int64> positionList;
        }
        static List<DataLineStruct> dataLineListA, dataLineListB;

        static FileStream resultWriter, fileReader1, fileReader2, finalWriter;//, tempWriter;//, tempReader;
        static StreamWriter resultWriterForText;

        static Form2 form2;

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        //static byte[] tempStation = new byte[4];

        static byte fileAddressLength = 0;//文件最大地址位组数，最大为4，最小为1

        static bool searchLoop = true;

        static int judgeLength = 3;
        static uint cursorRange = 256;
        static uint cursorStep = 1;

        private void para_textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(para_textBox1.Text) > 0)
                judgeLength = Convert.ToInt32(para_textBox1.Text);
            else
                para_textBox1.Text = "3";
        }

        private void para_textBox2_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToUInt32(para_textBox2.Text) > 0)
                cursorRange = Convert.ToUInt32(para_textBox2.Text);
            else
                para_textBox2.Text = "256";
        }

        private void para_textBox3_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToUInt32(para_textBox3.Text) > 0)
                cursorStep = Convert.ToUInt32(para_textBox3.Text);
            else
                para_textBox3.Text = "1";
        }

        private void type_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fileType = type_comboBox1.SelectedItem.ToString();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load();

            if (fileInPut_textBox1.Text != null)
                fileAdd1_ok = true;
            if (fileInPut_textBox2.Text != null)
                fileAdd2_ok = true;
            if (fileOutPut_textBox3.Text != null)
                fileAdd3_ok = true;
            if (resultFileName_textBox.Text != null)
                resultName_ok = true;

            textBox10.Text = textBox4.Text + " " + textBox5.Text + " " + textBox6.Text + " " + textBox7.Text + " " + textBox8.Text + " " + textBox9.Text;

            string[] array = { ".bin", ".txt" };
            type_comboBox1.DataSource = array;
            type_comboBox1.SelectedIndex = 0;
            fileType = type_comboBox1.SelectedItem.ToString();
        }

        private void load()
        {
            head1 = "0";
            head2 = "0";
            head3 = "0";
            head4 = "0";
            head5 = "0";
            head6 = "0";
            para_textBox1.Enabled = false;
            para_textBox2.Enabled = false;
            para_textBox3.Enabled = false;
            fileAdd1_ok = false;
            fileAdd2_ok = false;
            fileAdd3_ok = false;
            resultName_ok = false;
            head1_ok = false;
            head2_ok = false;
            head3_ok = false;
            head4_ok = false;
            head5_ok = false;
            head6_ok = false;
            head1_Byte = new byte[4];//格式
            head2_Byte = new byte[4];//厂商代码
            head3_Byte = new byte[2];//目标版本
            head4_Byte = new byte[4];//升级包校验码
            head5_Byte = new byte[4];//升级后校验码
            head6_Byte = new byte[14];//保留
        }

        private void paraLock_Click(object sender, EventArgs e)
        {
            if(para_textBox1.Enabled == false)
            {
                para_textBox1.Enabled = true;
                para_textBox2.Enabled = true;
                para_textBox3.Enabled = true;
            }
            else
            {
                para_textBox1.Enabled = false;
                para_textBox2.Enabled = false;
                para_textBox3.Enabled = false;
            }
        }

        private void paraReset_Click(object sender, EventArgs e)
        {
            judgeLength = 3;
            cursorRange = 256;
            cursorStep = 1;
            para_textBox1.Text = judgeLength.ToString();
            para_textBox2.Text = cursorRange.ToString();
            para_textBox3.Text = cursorStep.ToString();
        }

        private void detailShow_Click(object sender, EventArgs e)
        {
            if(form2 == null)
            {
                form2 = new Form2();
                form2.textString = detailShow_textBox.Text; ;
                form2.showDetialShow();
            }
            form2.ShowDialog();

        }

        private void fileInPut1_Click(object sender, EventArgs e)//取文件地址1
        {
            openFileDialog1.Filter = "bin files (*.bin) | *.bin";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            fileAdd1_ok = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileInPut_textBox1.Text = openFileDialog1.FileName.ToString();
                if (fileInPut_textBox1.Text != null)
                    fileAdd1_ok = true;
            }
        }

        private void fileInPut2_Click(object sender, EventArgs e)//取文件地址2
        {
            openFileDialog1.Filter = "bin files (*.bin) | *.bin";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            fileAdd2_ok = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileInPut_textBox2.Text = openFileDialog1.FileName.ToString();
                if (fileInPut_textBox2.Text != null)
                    fileAdd2_ok = true;
            }
        }

        private void fileInPut_textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)//生成文件名字
        {
            resultName_ok = false;
            if(resultFileName_textBox.Text != null)
                resultName_ok = true;
        }

        private void fileOutPut_Click(object sender, EventArgs e)
        {
            fileAdd3_ok = false;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                fileOutPut_textBox3.Text = folderBrowserDialog1.SelectedPath + @"\";
                if (fileOutPut_textBox3.Text != null)
                    fileAdd3_ok = true;
            }
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            head1_ok = false;
            if(textBox4.Text == "0")
                textBox4.Text = "";
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox4.Text == "0")
                textBox4.Text = "";
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox4.Text.Length > 4 || textBox4.Text.Length <= 0)
            {
                errorProvider2.SetError(label1, "请输入1-4位字符");
            }
            else
            {
                head1 = textBox4.Text;
                errorProvider2.Clear();
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            head1_ok = false;
            if ((textBox4.Text.Length > 4) || (textBox4.Text.Length <= 0))
            {
                //errorProvider2.SetError(label1, "请输入1-4位字符");
                textBox4.Text = head1;
            }
            else
            {
                head1_ok = true;
                head1 = textBox4.Text;
                textBox10.Text = head1 + " " + head2 + " " + head3 + " " + head4 + " " + head5 + " " + head6;
            }
            errorProvider2.Clear();
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            head2_ok = false;
            if (textBox5.Text == "0")
                textBox5.Text = "";
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox5.Text == "0")
                textBox5.Text = "";
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox5.Text.Length > 4 || textBox5.Text.Length <= 0)
            {
                errorProvider2.SetError(label2, "请输入1-4位字符");
            }
            else
            {
                head2 = textBox5.Text;
                errorProvider2.Clear();
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            head2_ok = false;
            if ((textBox5.Text.Length > 4) || (textBox5.Text.Length <= 0))
            {
                textBox5.Text = head2;
            }
            else
            {
                head2_ok = true;
                head2 = textBox5.Text;
                textBox10.Text = head1 + " " + head2 + " " + head3 + " " + head4 + " " + head5 + " " + head6;
            }
            errorProvider2.Clear();
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            head3_ok = false;
            if (textBox6.Text == "0")
                textBox6.Text = "";
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox6.Text == "0")
                textBox6.Text = "";
        }

        private void textBox6_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox6.Text.Length > 2 || textBox6.Text.Length <= 0)
            {
                errorProvider2.SetError(label3, "请输入1-2位字符");
            }
            else
            {
                head3 = textBox6.Text;
                errorProvider2.Clear();
            }
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            head3_ok = false;
            if ((textBox6.Text.Length > 2) || (textBox6.Text.Length <= 0))
            {
                textBox6.Text = head3;
            }
            else
            {
                head3_ok = true;
                head3 = textBox6.Text;
                textBox10.Text = head1 + " " + head2 + " " + head3 + " " + head4 + " " + head5 + " " + head6;
            }
            errorProvider2.Clear();
        }

        private void textBox7_Click(object sender, EventArgs e)
        {
            head4_ok = false;
            if (textBox7.Text == "0")
                textBox7.Text = "";
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox7.Text == "0")
                textBox7.Text = "";
        }

        private void textBox7_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox7.Text.Length > 4 || textBox7.Text.Length <= 0)
            {
                errorProvider2.SetError(label4, "请输入1-4位字符");
            }
            else
            {
                head4 = textBox7.Text;
                errorProvider2.Clear();
            }
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            head4_ok = false;
            if ((textBox7.Text.Length > 4) || (textBox7.Text.Length <= 0))
            {
                errorProvider2.SetError(label4, "请输入1-4位字符");
                textBox7.Text = head4;
            }
            else
            {
                head4_ok = true;
                head4 = textBox7.Text;
                textBox10.Text = head1 + " " + head2 + " " + head3 + " " + head4 + " " + head5 + " " + head6;
            }
            errorProvider2.Clear();
        }

        private void textBox8_Click(object sender, EventArgs e)
        {
            head5_ok = false;
            if (textBox8.Text == "0")
                textBox8.Text = "";
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox8.Text == "0")
                textBox8.Text = "";
        }

        private void textBox8_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox8.Text.Length > 4 || textBox8.Text.Length <= 0)
            {
                errorProvider2.SetError(label5, "请输入1-4位字符");
            }
            else
            {
                head5 = textBox8.Text;
                errorProvider2.Clear();
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            head5_ok = false;
            if ((textBox8.Text.Length > 4) || (textBox8.Text.Length <= 0))
            {
                errorProvider2.SetError(label5, "请输入1-4位字符");
                textBox8.Text = head5;
            }
            else
            {
                head5_ok = true;
                head5 = textBox8.Text;
                textBox10.Text = head1 + " " + head2 + " " + head3 + " " + head4 + " " + head5 + " " + head6;
            }
            errorProvider2.Clear();
        }

        private void textBox9_Click(object sender, EventArgs e)
        {
            head6_ok = false;
            if (textBox9.Text == "0")
                textBox9.Text = "";
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox9.Text == "0")
                textBox9.Text = "";
        }

        private void textBox9_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox9.Text.Length > 14 || textBox9.Text.Length <= 0)
            {
                errorProvider2.SetError(label6, "请输入1-4位字符");
            }
            else
            {
                head6 = textBox9.Text;
                errorProvider2.Clear();
            }
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            head6_ok = false;
            if ((textBox9.Text.Length > 14) || (textBox9.Text.Length <= 0))
            {
                errorProvider2.SetError(label6, "请输入1-14位字符");
                textBox9.Text = head6;
            }
            else
            {
                head6_ok = true;
                head6 = textBox9.Text;
                textBox10.Text = head1 + " " + head2 + " " + head3 + " " + head4 + " " + head5 + " " + head6;
            }
            errorProvider2.Clear();
        }

        private void start_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            /*if (fileAdd1_ok == false)
                errorProvider1.SetError(label8, "请输入正确地址");
            if (fileAdd2_ok == false)
                errorProvider1.SetError(label9, "请输入正确地址");
            if (fileAdd3_ok == false)
                errorProvider1.SetError(label10, "请输入正确地址");
            if (resultName_ok == false)
                errorProvider1.SetError(label11, "请输入名称");
            if (head1_ok == false)
                errorProvider2.SetError(label1, "请输入");
            if (head2_ok == false)
                errorProvider2.SetError(label2, "请输入");
            if (head3_ok == false)
                errorProvider2.SetError(label3, "请输入");
            if (head4_ok == false)
                errorProvider2.SetError(label4, "请输入");
            if (head5_ok == false)
                errorProvider2.SetError(label5, "请输入");
            if (head6_ok == false)
                errorProvider2.SetError(label6, "请输入");*/
            if(fileAdd1_ok == true && fileAdd2_ok == true && fileAdd3_ok == true && resultName_ok == true)
            {
                start.Enabled = false;
                detailShow_textBox.Clear();
                //load();

                //头
                head1_Byte = Encoding.Default.GetBytes(textBox4.Text);
                head2_Byte = Encoding.Default.GetBytes(textBox5.Text);
                head3_Byte = Encoding.Default.GetBytes(textBox6.Text);
                head4_Byte = Encoding.Default.GetBytes(textBox7.Text);
                head5_Byte = Encoding.Default.GetBytes(textBox8.Text);
                head6_Byte = Encoding.Default.GetBytes(textBox9.Text);

                //结果地址
                fileType = type_comboBox1.SelectedItem.ToString();
                resultFileAddress = fileOutPut_textBox3.Text + resultFileName_textBox.Text + fileType;
                fileReader1 = new FileStream(fileInPut_textBox1.Text, FileMode.Open, FileAccess.Read);
                fileReader2 = new FileStream(fileInPut_textBox2.Text, FileMode.Open, FileAccess.Read);
                resultWriter = new FileStream(resultFileAddress, FileMode.Create, FileAccess.Write);
                finalWriter = resultWriter; //new FileStream(@"C:\Users\Master\Desktop\finalWriter.bin", FileMode.Create, FileAccess.Write);
                if (fileType == ".txt")
                {
                    resultWriterForText = new StreamWriter(resultWriter, Encoding.UTF8);
                    resultWriterForText.AutoFlush = true;
                }

                Console.WriteLine(@"格        式：{0}", textBox4.Text);
                detailShow_textBox.AppendText(string.Format(@"格        式：{0}", textBox4.Text) + "\r\n");
                Console.WriteLine(@"厂 商 代 码 ：{0}", textBox5.Text);
                detailShow_textBox.AppendText(string.Format(@"厂 商 代 码 ：{0}", textBox5.Text) + "\r\n");
                Console.WriteLine(@"目 标 版 本 ：{0}", textBox6.Text);
                detailShow_textBox.AppendText(string.Format(@"目 标 版 本 ：{0}", textBox6.Text) + "\r\n");
                Console.WriteLine(@"升级包校验码：{0}", textBox7.Text);
                detailShow_textBox.AppendText(string.Format(@"升级包校验码：{0}", textBox7.Text) + "\r\n");
                Console.WriteLine(@"升级后校验码：{0}", textBox8.Text);
                detailShow_textBox.AppendText(string.Format(@"升级后校验码：{0}", textBox8.Text) + "\r\n");
                Console.WriteLine(@"保        留：{0}\r\n", textBox9.Text);
                detailShow_textBox.AppendText(string.Format(@"保        留：{0}", textBox9.Text) + "\r\n\r\n");

                //写入头
                if (fileType == ".bin")
                {
                    for (int i = 0; i < head1_Byte.Length; i++)
                        resultWriter.WriteByte(head1_Byte[i]);
                    for (int i = head1_Byte.Length; i < 4; i++)
                        resultWriter.WriteByte(0);

                    for (int i = 0; i < head2_Byte.Length; i++)
                        resultWriter.WriteByte(head2_Byte[i]);
                    for (int i = head2_Byte.Length; i < 4; i++)
                        resultWriter.WriteByte(0);

                    for (int i = 0; i < head3_Byte.Length; i++)
                        resultWriter.WriteByte(head3_Byte[i]);
                    for (int i = head3_Byte.Length; i < 2; i++)
                        resultWriter.WriteByte(0);

                    for (int i = 0; i < head4_Byte.Length; i++)
                        resultWriter.WriteByte(head4_Byte[i]);
                    for (int i = head4_Byte.Length; i < 4; i++)
                        resultWriter.WriteByte(0);

                    for (int i = 0; i < head5_Byte.Length; i++)
                        resultWriter.WriteByte(head5_Byte[i]);
                    for (int i = head5_Byte.Length; i < 4; i++)
                        resultWriter.WriteByte(0);

                    for (int i = 0; i < head6_Byte.Length; i++)
                        resultWriter.WriteByte(head6_Byte[i]);
                    for (int i = head6_Byte.Length; i < 14; i++)
                        resultWriter.WriteByte(0);
                }
                else if(fileType == ".txt")
                {
                    for (int i = 0; i < head1_Byte.Length; i++)
                        resultWriterForText.Write(head1_Byte[i].ToString("x2") + " ");
                    for (int i = head1_Byte.Length; i < 4; i++)
                        resultWriterForText.Write("00" + " ");

                    for (int i = 0; i < head2_Byte.Length; i++)
                        resultWriterForText.Write(head2_Byte[i].ToString("x2") + " ");
                    for (int i = head2_Byte.Length; i < 4; i++)
                        resultWriterForText.Write("00" + " ");

                    for (int i = 0; i < head3_Byte.Length; i++)
                        resultWriterForText.Write(head3_Byte[i].ToString("x2") + " ");
                    for (int i = head3_Byte.Length; i < 2; i++)
                        resultWriterForText.Write("00" + " ");

                    for (int i = 0; i < head4_Byte.Length; i++)
                        resultWriterForText.Write(head4_Byte[i].ToString("x2") + " ");
                    for (int i = head4_Byte.Length; i < 4; i++)
                        resultWriterForText.Write("00" + " ");

                    for (int i = 0; i < head5_Byte.Length; i++)
                        resultWriterForText.Write(head5_Byte[i].ToString("x2") + " ");
                    for (int i = head5_Byte.Length; i < 4; i++)
                        resultWriterForText.Write("00" + " ");

                    for (int i = 0; i < head6_Byte.Length; i++)
                        resultWriterForText.Write(head6_Byte[i].ToString("x2") + " ");
                    for (int i = head6_Byte.Length; i < 14; i++)
                        resultWriterForText.Write("00" + " ");

                    resultWriterForText.WriteLine();
                }

                dataLineListA = new List<DataLineStruct>();
                dataLineListB = new List<DataLineStruct>();
                dataLineListA.Clear();
                dataLineListB.Clear();
                play();
                //AddSameElements();
                WriteListToFile();
                start.Enabled = true;
                fileReader1.Close();
                fileReader2.Close();
                resultWriter.Close();
                finalWriter.Close();
            }
        }

    private void play()
        {
            Console.WriteLine("Hello World!");

            if (fileReader1.Length > 0xFFFF)
                return;
            if (fileReader2.Length > 0xFFFF)
                return;

            long fileLength = Math.Max(fileReader1.Length, fileReader2.Length);
            if ((fileLength & 0xffffff00) == 0)
                fileAddressLength = 1;
            else if ((fileLength & 0xffff0000) == 0)
                fileAddressLength = 2;
            else if ((fileLength & 0xff000000) == 0)
                fileAddressLength = 3;
            else
                fileAddressLength = 4;

            //cursorRange = Convert.ToUInt32(para_textBox2.Text);

            if (fileType == ".bin")
                resultWriter.WriteByte(fileAddressLength);
            else if (fileType == ".txt")
            {
                resultWriterForText.Write(fileAddressLength.ToString("x2") + " ");
                resultWriterForText.WriteLine();
            }
                //resultWriterForText.Write(String.Format("{0:X2 }", fileAddressLength)); 

            bool bigLoop = true;
            while (bigLoop)//步步跟进//////////////////////////////////////////////////////////*************一样循环
            {
                int a = 0; int b = 0;
                searchLoop = true;
                a = fileReader1.ReadByte();
                b = fileReader2.ReadByte();

                if ((a == -1) && (b == -1))//末尾退出//////////////------------------------如果相同区域共同到底
                    break;

                if (a != b)//路径断的时候
                {
                    if (a == -1)
                        fileReader2.Position--;
                    if (b == -1)
                        fileReader1.Position--;
                    //记录各个重要节点
                    Int64 locationTailA = fileReader1.Position - 2;//上一个连路径的末尾
                    Int64 locationTailB = fileReader2.Position - 2;//上一个连路径的末尾
                    Int64 locationHeadHoldA = fileReader1.Position - 1;//记录查找时断路径区域的开头
                    Int64 locationHeadHoldB = fileReader2.Position - 1;//记录查找时断路径区域的开头

                    Int64 locationRootA = locationHeadHoldA;
                    Int64 locationRootB = locationHeadHoldB;//用于渐进的游标底

                    byte[] rootA = new byte[judgeLength];
                    byte[] rootB = new byte[judgeLength];//对照数组，游标底数组
                    byte[] cursorA = new byte[judgeLength];
                    byte[] cursorB = new byte[judgeLength];//游标数组

                    Int64 locationCursorA = locationRootA;
                    Int64 locationCursorB = locationRootB;

                    //root
                    while (searchLoop)//////////////////////////////////////////////////////////*************root循环
                    {
                        fileReader1.Position = locationRootA;
                        fileReader2.Position = locationRootB;
                        
                        int rootResultA = fileReader1.Read(rootA, 0, judgeLength);//读取、记录root
                        int rootResultB = fileReader2.Read(rootB, 0, judgeLength);

                        if ((rootResultA == 0) && (rootResultB == 0))//如果root到底
                        {
                            RecordAndQuit(locationHeadHoldA, locationHeadHoldB, locationRootA, locationRootB, 2);
                            bigLoop = false;
                            break;
                        }

                        Int64 activeA = 0; Int64 activeB = 0;

                        while ((Math.Max(activeA, activeB) < cursorRange) && (searchLoop == true))/////////*************游标循环
                        {
                            fileReader1.Position = locationRootA + activeA;
                            fileReader2.Position = locationRootB + activeB;
                            int resultA = fileReader1.Read(cursorA, 0, judgeLength);//得到cursor，等待比较
                            int resultB = fileReader2.Read(cursorB, 0, judgeLength);

                            if ((resultA == 0) && (resultB == 0))////////////////----------如果双到底
                            {
                                if (activeA > 0)
                                    locationRootB++;
                                if (activeB > 0)
                                    locationRootA++;
                                break;
                            }

                            bool AFind = ArrayEqul(cursorA, rootB, Math.Min(resultA, rootResultB));//删除
                            bool BFind = ArrayEqul(cursorB, rootA, Math.Min(resultB, rootResultA));//添加
                            if(activeA == 254)
                            {

                            }
                            if ((AFind == true) || (BFind == true))///////////---------如果找到路径（,）
                            {
                                if (AFind == true)
                                {
                                    locationRootA += activeA;//记录此时位置，即为下一个一样区域的开头
                                }

                                if (BFind == true)
                                {
                                    locationRootB += activeB;
                                }
                                //如果找到或者到末尾还未找到

                                RecordAndQuit(locationHeadHoldA, locationHeadHoldB, locationRootA, locationRootB, 1);//记录，做相应工作
                                
                                fileReader1.Position = locationRootA;//重置文件位置为一样区域开头，开始一样查找，找下一个不一样
                                fileReader2.Position = locationRootB;//文件重置查找位置

                                if (searchLoop == false)//说明找到了
                                    break;
                            }

                            if ((resultA > 0) && (rootResultA > 0))
                                activeA++;
                            if ((resultB > 0) && (rootResultB > 0))
                                activeB++;
                        }

                        if (Math.Max(activeA, activeB) >= cursorRange)
                        {
                            if ((activeA > 0) && (rootResultA > 0))
                                locationRootB++;
                            if ((activeB > 0) && (rootResultB > 0))
                                locationRootA++;
                        }
                        if (searchLoop == false)
                            break;
                    }
                }

                if (bigLoop == false)
                    break;
            }
            //resultWriter.Close();
            //while (true) ;
        }

        private void RecordAndQuit(Int64 theLocationHeadHoldA, Int64 theLocationHeadHoldB, Int64 theLocationRootA, Int64 theLocationRootB, int type)
        {
            Int64 lengthA = theLocationRootA - theLocationHeadHoldA;
            Int64 lengthB = theLocationRootB - theLocationHeadHoldB; //int i = 0;
            Int64 theLengthA = lengthA;
            Int64 theLengthB = lengthB;
            Int64 theHoldA = theLocationHeadHoldA;

            if ((lengthA > 0) && (lengthB > 0))////////////////////////////////////////////////////////////////////////////////////////////////替换
            {
                byte[] res = new byte[lengthB];
                fileReader2.Position = theLocationHeadHoldB;
                fileReader2.Read(res, 0, (int)lengthB);
                Console.Write(@"从:{0:x}(包括)到:{1:x}(不包括)的{2}个元素替换为{3}个元素：", theLocationHeadHoldA, theLocationRootA, lengthA, lengthB);
                detailShow_textBox.AppendText(string.Format(@"从:{0:x}(包括)到:{1:x}(不包括)的{2}个元素替换为{3}个元素：", theLocationHeadHoldA, theLocationRootA, lengthA, lengthB)); 
                for (int i = 0; i < lengthB; i++)
                {
                    Console.Write(@" {0:x}", res[i]);
                    detailShow_textBox.AppendText(string.Format(@" {0:x}", res[i]));
                }
                Console.WriteLine(@" ");
                detailShow_textBox.AppendText("\r\n");
                //写入文件
                if ((theLengthA == theLengthB) && (theLengthA < 3))
                {
                    if ((theLengthA == 1) && (theLengthB == 1))
                    {
                        bool found = false;
                        int i;
                        for(i = 0; i < dataLineListA.Count; i++)//逐项寻找A匹配项
                        {
                            if(dataLineListA[i].commenValue == res[0])
                            {
                                dataLineListA[i].positionList.Add(theLocationHeadHoldA);
                                found = true;
                                break;
                            }
                        }
                        if(found == false)//说明该项不存在，创建
                        {
                            DataLineStruct tempData = new DataLineStruct();
                            tempData.commenValue = res[0];
                            tempData.bbSecondValue = 0;
                            tempData.positionList = new List<long>();
                            tempData.positionList.Add(theLocationHeadHoldA);
                            dataLineListA.Add(tempData);
                        }
                    }
                        
                    else if ((theLengthA == 2) && (theLengthB == 2))
                    {
                        bool found = false;
                        int i;
                        for(i = 0; i < dataLineListB.Count; i++)
                        {
                            if(theLocationHeadHoldA == 58)
                            {

                            }
                            if((dataLineListB[i].commenValue == res[0]) && (dataLineListB[i].bbSecondValue == res[1]))
                            {
                                dataLineListB[i].positionList.Add(theLocationHeadHoldA);
                                found = true;
                                break;
                            }
                        }
                        if(found == false)
                        {
                            DataLineStruct tempData = new DataLineStruct();
                            tempData.commenValue = res[0];
                            tempData.bbSecondValue = res[1];
                            tempData.positionList = new List<long>();
                            tempData.positionList.Add(theLocationHeadHoldA);
                            dataLineListB.Add(tempData);
                        }
                    }
                }
                else
                {
                    if (fileType == ".bin")
                    {
                        //for (int i = 0; i < 2; i++)
                            resultWriter.WriteByte(0xff);//小节头
                        resultWriter.WriteByte(0x01);//类型符
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriter.WriteByte((byte)(theLocationHeadHoldA & 0xff));//写入theLocationHeadHoldA
                            theLocationHeadHoldA >>= 8;
                        }
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriter.WriteByte((byte)(lengthA & 0xff));//写入被替换长度
                            lengthA >>= 8;
                        }
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriter.WriteByte((byte)(lengthB & 0xff));//写入替换长度
                            lengthB >>= 8;
                        }
                        for (int i = 0; i < theLengthB; i++)//写入替换元素
                            resultWriter.WriteByte(res[i]);
                    }
                    else if (fileType == ".txt")
                    {
                        //for (int i = 0; i < 2; i++)
                            resultWriterForText.Write(0xff.ToString("x2") + " ");//小节头
                        resultWriterForText.Write(0x01.ToString("x2") + " ");//类型符
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriterForText.Write((theLocationHeadHoldA & 0xff).ToString("x2") + " ");//写入theLocationHeadHoldA
                            theLocationHeadHoldA >>= 8;
                        }
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriterForText.Write((lengthA & 0xff).ToString("x2") + " ");//写入被替换长度
                            lengthA >>= 8;
                        }
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriterForText.Write((lengthB & 0xff).ToString("x2") + " ");//写入替换长度
                            lengthB >>= 8;
                        }
                        for (int i = 0; i < theLengthB; i++)//写入替换元素
                            resultWriterForText.Write(res[i].ToString("x2") + " ");
                        resultWriterForText.WriteLine();
                    }
                } 
            }

            else if ((lengthA > 0) && (lengthB == 0))////////////////////////////////////////////////////////////////////////////////////////////删除
            {
                Console.WriteLine(@"删除:{0:x}（包括）到:{1:x}(不包括)后面长度为:{2}的元素", theLocationHeadHoldA, theLocationRootA, lengthA);
                detailShow_textBox.AppendText(string.Format(@"删除:{0:x}（包括）到:{1:x}(不包括)后面长度为:{2}的元素", theLocationHeadHoldA, theLocationRootA, lengthA) + "\r\n");
                {
                    //写入文件
                    if(fileType == ".bin")
                    {
                        //for (int i = 0; i < 2; i++)
                            resultWriter.WriteByte(0xff);//小节头
                        resultWriter.WriteByte(0x02);//类型符
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriter.WriteByte((byte)(theLocationHeadHoldA & 0xff));//写入theLocationHeadHoldA
                            theLocationHeadHoldA >>= 8;
                        }
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriter.WriteByte((byte)(lengthA & 0xff));//写入删除长度
                            lengthA >>= 8;
                        }
                    }
                    else if (fileType == ".txt")
                    {
                        //for (int i = 0; i < 2; i++)
                            resultWriterForText.Write(0xff.ToString("x2") + " ");//小节头
                        resultWriterForText.Write(0x02.ToString("x2") + " ");//类型符
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriterForText.Write((theLocationHeadHoldA & 0xff).ToString("x2") + " ");//写入theLocationHeadHoldA
                            theLocationHeadHoldA >>= 8;
                        }
                        for (int i = 0; i < fileAddressLength; i++)
                        {
                            resultWriterForText.Write((lengthA & 0xff).ToString("x2") + " ");//写入删除长度
                            lengthA >>= 8;
                        }
                        resultWriterForText.WriteLine();
                    }
                }
            }

            else if ((lengthA == 0) && (lengthB > 0))////////////////////////////////////////////////////////////////////////////////////////////插入
            {
                byte[] res = new byte[lengthB];
                fileReader2.Position = theLocationHeadHoldB;
                fileReader2.Read(res, 0, (int)lengthB);
                Console.Write(@"在{0:x}前增加数量为:{1}的元素：", theLocationHeadHoldA, lengthB);
                detailShow_textBox.AppendText(string.Format(@"在{0:x}前增加数量为:{1}的元素：", theLocationHeadHoldA, lengthB));
                for (int i = 0; i < lengthB; i++)
                {
                    Console.Write(@" {0:x}", res[i]);
                    detailShow_textBox.AppendText(string.Format(@" {0:x}", res[i]));
                }
                Console.WriteLine(@" ");
                detailShow_textBox.AppendText("\r\n");
                if(fileType == ".bin")
                {
                    //写入文件
                    //for (int i = 0; i < 2; i++)
                        resultWriter.WriteByte(0xff);//小节头
                    resultWriter.WriteByte(0x03);//类型符
                    for (int i = 0; i < fileAddressLength; i++)
                    {
                        resultWriter.WriteByte((byte)(theLocationHeadHoldA & 0xff));//写入theLocationHeadHoldA
                        theLocationHeadHoldA >>= 8;
                    }
                    for (int i = 0; i < fileAddressLength; i++)
                    {
                        resultWriter.WriteByte((byte)(lengthB & 0xff));//写入插入长度
                        lengthB >>= 8;
                    }
                    for (int i = 0; i < theLengthB; i++)//写入插入元素
                        resultWriter.WriteByte(res[i]);
                }
                else if(fileType == ".txt")
                {
                    //写入文件
                    //for (int i = 0; i < 2; i++)
                        resultWriterForText.Write(0xff.ToString("x2") + " ");//小节头
                    resultWriterForText.Write(0x03.ToString("x2") + " ");//类型符
                    for (int i = 0; i < fileAddressLength; i++)
                    {
                        resultWriterForText.Write((theLocationHeadHoldA & 0xff).ToString("x2") + " ");//写入theLocationHeadHoldA
                        theLocationHeadHoldA >>= 8;
                    }
                    for (int i = 0; i < fileAddressLength; i++)
                    {
                        resultWriterForText.Write((lengthB & 0xff).ToString("x2") + " ");//写入插入长度
                        lengthB >>= 8;
                    }
                    for (int i = 0; i < theLengthB; i++)//写入插入元素
                        resultWriterForText.Write(res[i].ToString("x2") + " ");
                    resultWriterForText.WriteLine();
                }
            }
            searchLoop = false;
        }

        /*static void AddSameElements()
        {
            tempWriter.Position = 0;
            Int64 readPosition = 0;
            byte[] positionTemp = new byte[2];
            while(tempWriter.Position < tempWriter.Length)
            {
                switch(tempWriter.ReadByte())
                {
                    case 0xaa:
                        {
                            readPosition = tempWriter.Position;//读取数据
                            tempWriter.Read(positionTemp, 0, 2);
                            readPosition += 2;
                            Int64 aaPosition = 0;
                            for(int j = 0; j < fileAddressLength; j++)
                                aaPosition |= (Int64)(positionTemp[j] << (8 * j));
                            tempWriter.Position = readPosition;
                            byte value = (byte)tempWriter.ReadByte();
                            readPosition = tempWriter.Position;

                            int i;//操作
                            for (i = 0; i < dataLineListA.Count; i++)
                            {
                                if (dataLineListA[i].commenValue == value)//如果存在元素相等，存入地址
                                {
                                    dataLineListA[i].positionList.Add(aaPosition);
                                    break;
                                }
                            }
                            if (i == dataLineListA.Count)//如果不存在，创建新元素
                            {
                                DataLineStruct tempStructA;
                                tempStructA.commenValue = value;
                                tempStructA.bbSecondValue = 0;
                                tempStructA.positionList = new List<long>();
                                tempStructA.positionList.Add(aaPosition);
                                dataLineListA.Add(tempStructA);
                            }
                        }
                        break;

                    case 0xbb:
                        {
                            readPosition = tempWriter.Position;
                            tempWriter.Read(positionTemp, 0, 2);
                            readPosition += 2;
                            Int64 bbPosition = 0;
                            for (int j = 0; j < fileAddressLength; j++)
                                bbPosition |= (Int64)(positionTemp[j] << (8 * j));
                            tempWriter.Position = readPosition;
                            byte value1 = (byte)tempWriter.ReadByte();
                            byte value2 = (byte)tempWriter.ReadByte();
                            readPosition = tempWriter.Position;

                            int i;
                            for (i = 0; i < dataLineListB.Count; i++)
                            {
                                if ((dataLineListB[i].commenValue == value1) && 
                                    (dataLineListB[i].bbSecondValue == value2))//如果存在元素相等，存入地址
                                {
                                    dataLineListB[i].positionList.Add(bbPosition);
                                    break;
                                }
                            }
                            if (i == dataLineListB.Count)//如果不存在，创建新元素
                            {
                                DataLineStruct tempStructB;
                                tempStructB.commenValue = value1;
                                tempStructB.bbSecondValue = value2;
                                tempStructB.positionList = new List<long>();
                                tempStructB.positionList.Add(bbPosition);
                                dataLineListA.Add(tempStructB);
                            }
                        }
                        break;

                    default:
                        //resultWriterForText.WriteLine("输出错误");
                        break;
                }
            }
        }*/

        static void WriteListToFile()
        {
            //finalWriter.WriteByte(fileAddressLength);

            if (fileType == ".bin")
            {
                //先写A链
                //finalWriter.WriteByte(0xaa);//a类标志头
                //finalWriter.WriteByte(0xaa);
                for (int i = 0; i < dataLineListA.Count; i++)//写入所有头值
                {
                    finalWriter.WriteByte(0xaa);
                    finalWriter.WriteByte(dataLineListA[i].commenValue);//写入头值

                    int positionCountA = dataLineListA[i].positionList.Count;//写入地址长度
                    for(int j = 0; j < fileAddressLength; j++)
                    {
                        finalWriter.WriteByte((byte)(positionCountA & 0xff));
                        positionCountA >>= 8;
                    }
                    for (int j = 0; j < dataLineListA[i].positionList.Count; j++)//写入所有地址
                    {
                        Int64 tempPosition = dataLineListA[i].positionList[j];
                        byte a = fileAddressLength;
                        while (a > 0)
                        {
                            finalWriter.WriteByte((byte)(tempPosition & 0xff));//左低右高
                            tempPosition >>= 8;
                            a--;
                        }
                    }
                }

                //再写B链
                //finalWriter.WriteByte(0xbb);//b类标志头
                //finalWriter.WriteByte(0xbb);
                for (int i = 0; i < dataLineListB.Count; i++)//写入所有头值
                {
                    finalWriter.WriteByte(0xbb);
                    finalWriter.WriteByte(dataLineListB[i].commenValue);//写入头值
                    finalWriter.WriteByte(dataLineListB[i].bbSecondValue);

                    int positionCountB = dataLineListB[i].positionList.Count;//写入地址长度
                    for (int j = 0; j < fileAddressLength; j++)
                    {
                        finalWriter.WriteByte((byte)(positionCountB & 0xff));
                        positionCountB >>= 8;
                    }
                    for (int j = 0; j < dataLineListB[i].positionList.Count; j++)//写入所有地址
                    {
                        Int64 tempPosition = dataLineListB[i].positionList[j];
                        byte b = fileAddressLength;
                        while (b > 0)
                        {
                            finalWriter.WriteByte((byte)(tempPosition & 0xff));//左低右高
                            tempPosition >>= 8;
                            b--;
                        }
                    }
                }
            }

            else if (fileType == ".txt")
            {
                resultWriterForText.WriteLine("aa aa ".ToString());//a类标志头

                for (int i = 0; i < dataLineListA.Count; i++)//写入所有头值
                {
                    //resultWriterForText.Write("aa ".ToString());
                    resultWriterForText.Write(dataLineListA[i].commenValue.ToString("x2") + " ");//写入头值

                    int positionCountA = dataLineListA[i].positionList.Count;//写入地址长度
                    for (int j = 0; j < fileAddressLength; j++)
                    {
                        resultWriterForText.Write((positionCountA & 0xff).ToString("x2") + " ");
                        positionCountA >>= 8;
                    }
                    for (int j = 0; j < dataLineListA[i].positionList.Count; j++)//写入所有地址
                    {
                        Int64 tempPosition = dataLineListA[i].positionList[j];
                        byte a = fileAddressLength;
                        while (a > 0)
                        {
                            resultWriterForText.Write((tempPosition & 0xff).ToString("x2") + " ");//左低右高
                            tempPosition >>= 8;
                            a--;
                        }
                    }
                    resultWriterForText.WriteLine();
                }

                resultWriterForText.WriteLine("bb bb ");//b类标志头
                for (int i = 0; i < dataLineListB.Count; i++)//写入所有头值
                {
                    //resultWriterForText.Write("bb ");
                    resultWriterForText.Write(dataLineListB[i].commenValue.ToString("x2") + " ");//写入头值
                    resultWriterForText.Write(dataLineListB[i].bbSecondValue.ToString("x2") + " ");

                    int positionCountB = dataLineListB[i].positionList.Count;//写入地址长度
                    for (int j = 0; j < fileAddressLength; j++)
                    {
                        resultWriterForText.Write((positionCountB & 0xff).ToString("x2") + " ");
                        positionCountB >>= 8;
                    }
                    for (int j = 0; j < dataLineListB[i].positionList.Count; j++)//写入所有地址
                    {
                        Int64 tempPosition = dataLineListB[i].positionList[j];
                        byte b = fileAddressLength;
                        while (b > 0)
                        {
                            resultWriterForText.Write((tempPosition & 0xff).ToString("x2") + " ");//左低右高
                            tempPosition >>= 8;
                            b--;
                        }
                    }
                    resultWriterForText.WriteLine();
                }
            }
            else
                Console.WriteLine(@"内部文件类型错误，尝试更改文件类型重新生成");
        }

        static bool ArrayEqul(byte[] a, byte[] b, int judgeCount)
        {
            if (judgeCount == 0)
                return false;
            bool boolTip = true;
            for (int i = 0; i < judgeCount; i++)
            {
                if (a[i] != b[i])
                    boolTip = false;
            }
            return boolTip;
        }
    }
}
