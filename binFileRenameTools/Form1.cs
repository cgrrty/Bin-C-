using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
using System.IO;

namespace binFileRenameTools
{
    public partial class Form1 : Form
    {
        //定义变量
        bool IsFileOpened = false;
        string OriginalFilePath = string.Empty;

        UInt32 len1;
        UInt32 len2;

        public Form1()
        {
            InitializeComponent();
            //不支持拖入
            this.AllowDrop = true;
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            //MessageBox.Show(((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
            //richTextBox1.AppendText(((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
            IsFileOpened = true;
            OriginalFilePath = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            FileInfo MyFileInfo = new FileInfo(OriginalFilePath);
            if (string.Equals(MyFileInfo.Extension, ".bin") == true)
            {
                textBox1.Text = OriginalFilePath;
                richTextBox1.AppendText(OriginalFilePath + "\r\n");
            }
            else
            {
                IsFileOpened = false;
                len1 = (uint)richTextBox1.Text.Length;
                richTextBox1.AppendText("所选文件类型错误！\r\n");
                len2 = (uint)richTextBox1.Text.Length;
                richTextBox1.Select((int)len1, (int)(len2 - len1));
                richTextBox1.SelectionColor = Color.Red;
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                IsFileOpened = true;
                OriginalFilePath = openFileDialog1.FileName;
                textBox1.Text = OriginalFilePath;
                richTextBox1.AppendText(OriginalFilePath + "\r\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsFileOpened == false)
            {
                //MessageBox.Show("请先打开一个bin文件");
                len1 = (uint)richTextBox1.Text.Length;
                richTextBox1.AppendText("请先打开一个bin文件\r\n");
                len2 = (uint)richTextBox1.Text.Length;
                richTextBox1.Select((int)len1, (int)(len2 - len1));
                richTextBox1.SelectionColor = Color.Fuchsia;
            }
            else
            {
                long FileLength = 0;
                string newname = string.Empty;
                FileInfo MyFileInfo = new FileInfo(OriginalFilePath);
                FileLength = MyFileInfo.Length;
                Byte[] TempArray = new Byte[4];
                Byte[] NewNameArray = new Byte[64];
                string NewNameStr = string.Empty;
                UInt32 NameAddr;

                try
                {
                    Array.Copy(File.ReadAllBytes(OriginalFilePath), 0x1000, TempArray, 0, 4);
                    //NameAddr = (UInt32)(TempArray[3] << 24 + TempArray[2] << 16 + TempArray[1] << 8 + TempArray[0]);
                    //C# 方法真简单
                    NameAddr = BitConverter.ToUInt32(TempArray, 0);

                    if (checkBox1.Checked == true)
                    {
                        NameAddr = NameAddr - 0x08001000;
                    }
                    else
                    {
                        NameAddr = NameAddr - 0x0800a000;                       
                    }
                    Array.Copy(File.ReadAllBytes(OriginalFilePath), NameAddr, NewNameArray, 0, 64);
                    string strTemp = string.Empty;
                    strTemp = string.Copy(OriginalFilePath);
                    strTemp = strTemp.Substring(0, strTemp.LastIndexOf("\\") + 1);

                    NewNameStr = System.Text.Encoding.ASCII.GetString(NewNameArray);
                    NewNameStr = NewNameStr.Substring(0, NewNameStr.IndexOf("\0"));   
                    if (string.Equals(MyFileInfo.Name, NewNameStr)) //如果不需要重命名
                    {
                        len1 = (uint)richTextBox1.Text.Length;
                        richTextBox1.AppendText(NewNameStr);
                        richTextBox1.AppendText("文件名正确，不需要重命名\r\n");
                        len2 = (uint)richTextBox1.Text.Length;
                        richTextBox1.Select((int)len1, (int)(len2 - len1));
                        richTextBox1.SelectionColor = Color.Lime;
                    }
                    else
                    {
                        System.IO.File.Copy(OriginalFilePath, strTemp + NewNameStr);//拷贝文件 并重新命名
                        File.Delete(OriginalFilePath);//删除源文件                    
                        IsFileOpened = false;
                        textBox1.Text = string.Empty;
                        ///
                        len1 = (uint)richTextBox1.Text.Length;
                        richTextBox1.AppendText(NewNameStr);
                        richTextBox1.AppendText("  重命名成功\r\n");
                        len2 = (uint)richTextBox1.Text.Length;
                        richTextBox1.Select((int)len1, (int)(len2 - len1));
                        richTextBox1.SelectionColor = Color.Lime;
                    }
                }
                catch
                {
                    len1 = (uint)richTextBox1.Text.Length;
                    richTextBox1.AppendText("打开的文件错误！\r\n"); 
                    len2 = (uint)richTextBox1.Text.Length;
                    richTextBox1.Select((int)len1, (int)(len2 - len1));
                    richTextBox1.SelectionColor = Color.Black;                                       
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, System.EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
