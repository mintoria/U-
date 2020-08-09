using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CCWin;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Web.UI.Design.WebControls.WebParts;
using System.Reflection.Emit;

namespace U盘防火墙
{
    public partial class 明管家 : CCSkinMain
    {

        public string Value;
        public string pan;
        public bool exist;
        public bool e_usb = false;
        public string kind = "";//病毒种类
        public int number = 0;//病毒个数
        public List<string>site=new List<string>();//病毒位置
        public bool virus_exist = false;
        public 明管家()
        {
            InitializeComponent();
        }
       
        //获取可执行文件的路径
        public static string file_route()
        {
            string file = Application.ExecutablePath;
            return file;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }
       
        protected override void WndProc(ref Message m)//wndProc是默认的消息处理函数
        {
            // 发生设备变动
            const int WM_DEVICECHANGE = 0x0219;
            // 系统检测到一个新设备
            const int DBT_DEVICEARRIVAL = 0x8000;
            // 系统完成移除一个设备
            const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
            // 逻辑卷标
            const int DBT_DEVTYP_VOLUME = 0x00000002;
            //关闭窗体信息
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    switch (m.WParam.ToInt32())
                    {
                        case DBT_DEVICEARRIVAL:
                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                e_usb = true;
                                DEV_BROADCAST_VOLUME vol;
                                vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(
                                 m.LParam, typeof(DEV_BROADCAST_VOLUME));
                                skinLabel3.Text ="发现可移动设备："+ IO(vol.dbcv_unitmask.ToString("x"));
                                MessageBox.Show("U盘已插入");
                                pan = IO(vol.dbcv_unitmask.ToString("x"));
                                exist=search_inf();
                                if (exist)
                                    skinLabel5.Text = "存在";
                                else
                                    skinLabel5.Text = "不存在";
                            }
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:
                            e_usb = false;
                            MessageBox.Show("U盘已拔出");
                            skinLabel3.Text = "未检测到可移动设备";
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        //判断AutoRun.inf是否存在
        public bool search_inf()
        {
   
            bool result = false;

            if (File.Exists(pan + "/AutoRun.inf"))
            { result = true; }
            return result;
        }
        //转换盘符格式
        private string IO(string v)
        {
            switch (v)
            {
                case "1":
                    Value = "A:";
                    break;
                case "2":
                    Value = "B:";
                    break;
                case "4":
                    Value = "C:";
                    break;
                case "8":
                    Value = "D:";
                    break;
                case "10":
                    Value = "E:";
                    break;
                case "20":
                    Value = "F:";
                    break;
                case "40":
                    Value = "G:";
                    break;
                case "80":
                    Value = "H:";
                    break;
                case "100":
                    Value = "I:";
                    break;
                case "200":
                    Value = "J:";
                    break;
                case "400":
                    Value = "K:";
                    break;
                case "800":
                    Value = "L:";
                    break;
                case "1000":
                    Value = "M:";
                    break;
                case "2000":
                    Value = "N:";
                    break;
                case "4000":
                    Value = "O:";
                    break;
                case "8000":
                    Value = "P:";
                    break;
                case "10000":
                    Value = "Q:";
                    break;
                case "20000":
                    Value = "R:";
                    break;
                case "40000":
                    Value = "S:";
                    break;
                case "80000":
                    Value = "T:";
                    break;
                case "100000":
                    Value = "U:";
                    break;
                case "200000":
                    Value = "V:";
                    break;
                case "400000":
                    Value = "W:";
                    break;
                case "800000":
                    Value = "X:";
                    break;
                case "1000000":
                    Value = "Y:";
                    break;
                case "2000000":
                    Value = "Z:";
                    break;
                default: break;
            }
            return Value;
        }


        private void skinLabel3_Click(object sender, EventArgs e)
        {

        }
        //打开文件
        private void skinButton1_Click_1(object sender, EventArgs e)
        {
            if (!exist)
                MessageBox.Show("文件不存在！");
            else
                Process.Start("notepad.exe", pan + "/AutoRun.inf");
        }
        //删除文件
        private void skinButton2_Click(object sender, EventArgs e)
        {
            if (!exist)
                MessageBox.Show("文件不存在！");
            else
               if (MessageBox.Show("确认删除？", "此删除不可恢复", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.Delete(pan + "/AutoRun.inf");
                exist = search_inf();
                if (!exist)//判断是否删除成功
                {
                    skinLabel5.Text = "不存在";
                }
                else
                    MessageBox.Show("删除失败！");
            }
            
        }
        // 添加双击托盘图标事件（双击显示窗口）
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示    
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                notifyIcon1.Visible = false;
            }
        }

        private void 明管家_Load(object sender, EventArgs e)
        {

        }
        //关闭程序确认
        private void CloseBox_Click(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
            else
            {
                e.Cancel = true;
            }
        }
        //最小化到托盘
        private void SizeChanged_Click(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

     

        private void button1_Click(object sender, EventArgs e)
        {
            if (e_usb)//判断是否插入可移动设备
                System.Diagnostics.Process.Start(pan + "/");
            else
                MessageBox.Show("未检测到可移动设备");
        }
        //设置开机自启动

        private void skinCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool start = skinCheckBox1.Checked;
            string path = file_route();//获取exe文件路径
            string name = "U盘防火墙";
            RunWhenStart(start, name, path);
        }
        //注册表编辑
        public static void RunWhenStart(bool Started, string name, string path)
        {
            Microsoft.Win32.RegistryKey HKLM = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (Started == true)
            {
                try
                {
                    Run.SetValue(name, path);//添加到注册表
                    HKLM.Close();
                }
                catch { }
            }
            else
            {
                try
                {
                    Run.DeleteValue(name);//注册表中删除
                    HKLM.Close();
                }
                catch { }
            }
        }
        //托盘菜单：打开主界面
        private void 打开主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Hide();
            }
            else
            { 
                this.Show();
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
        }
        //托盘菜单：退出程序
        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //杀毒
       
        [DllImport("kernel32")]
        //扫描inf文件
        private static extern bool GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        public void auto_scan()
        {

            StringBuilder strValue_open = new StringBuilder(256);
            StringBuilder strValue_ocommand = new StringBuilder(256);
            StringBuilder strValue_ecommand = new StringBuilder(256);
            StringBuilder strValue_shellexecute = new StringBuilder(256);
            GetPrivateProfileString("AuToRun", "open", "error", strValue_open, strValue_open.Capacity, pan + "autorun.inf");
           
            GetPrivateProfileString("AuToRun", "Shell\\open\\Command", "error", strValue_ocommand, strValue_ocommand.Capacity, pan + "autorun.inf");
            //Shell\explore\Command
            GetPrivateProfileString("AuToRun", "Shell\\explore\\Command", "error", strValue_ecommand, strValue_ecommand.Capacity, pan + "autorun.inf");
            GetPrivateProfileString("AuToRun", "shellexecute", "error", strValue_shellexecute, strValue_shellexecute.Capacity, pan + "autorun.inf");
            //String s = new String(sb.toString().getBytes("ios8859-1"),"UTF-8");
            if (strValue_open.ToString()!="error")
            {
                if(File.Exists(pan+"/"+strValue_open.ToString()))
                {
                    
                    string result=judge_virus(strValue_open.ToString());
                    kind = kind  +" "+result ;
                    string wz= pan + "/" + strValue_open.ToString();
                    site.Add(wz); ;
                    number++;
                    label1.Text = kind;
                    
                    virus_exist = true;
                }
                else
                {
                    //MessageBox.Show("未找到病毒文件！"+ strValue_open);
                }
                //判断病毒文件是否存在，如存在是什么类型的病毒文件
            }
            if (strValue_ocommand.ToString() != "error")
            {
                if (File.Exists(pan + "/" + strValue_ocommand.ToString()))
                {
                    string result=judge_virus(strValue_ocommand.ToString());
                    kind = kind + " " + result;
                    string wz = pan + "/" + strValue_ocommand.ToString();
                    site.Add(wz); ;
                    //label3.Text = site;
                    label1.Text = kind;
                    number++;
                    virus_exist = true;
                }
                else
                {
                    //MessageBox.Show("未找到病毒文件！");
                }
                //判断病毒文件是否存在，如存在是什么类型的病毒文件
            }
            if (strValue_ecommand.ToString() != "error")
            {
                if (File.Exists(pan + "/" + strValue_ecommand.ToString()))
                {
                    string result = judge_virus(strValue_ecommand.ToString());
                    kind = kind + " " + result;
                    string wz = pan + "/" + strValue_ecommand.ToString();
                    site.Add(wz);
                    //label3.Text = site;
                    label1.Text = kind;
                    number++;
                    virus_exist = true;
                }
                else
                {
                    //MessageBox.Show("未找到病毒文件！");
                }
                //判断病毒文件是否存在，如存在是什么类型的病毒文件
            }
            if (strValue_shellexecute.ToString() != "error")
            {
                if (File.Exists(pan + "/" + strValue_shellexecute.ToString()))
                {
                    string result=judge_virus(strValue_shellexecute.ToString());
                    kind = kind + " " + result;
                    string wz = pan + "/" + strValue_shellexecute.ToString();
                    site.Add(wz); ;
                   // label3.Text = site;
                    label1.Text = kind;
                    number++;
                    virus_exist = true;
                }
                else
                {
                    //MessageBox.Show("未找到病毒文件！");
                }
                //判断病毒文件是否存在，如存在是什么类型的病毒文件
            }
            //string exe = pan + strValue_open.ToString();//盘符+启动exe文件名。
            //string autorun = pan + "autorun.inf";//盘符+启动项文件名。
            //FileInfo fi1 = new FileInfo(exe);//判断病毒文件是否存在
            //FileInfo fi2 = new FileInfo(autorun);
            ////删除这两个文件
            //fi1.Delete();
            //fi2.Delete();

        }
        //判断病毒种类：vbs\exe\bat\pif\scr\com
        public string judge_virus(string str_v)
        {
            string values = null;
            if (str_v.Contains(".exe"))
                values = "EXE";
            else if (str_v.Contains(".vbs"))
                values = "VBS";
            else if (str_v.Contains(".bat"))
                values = "BAT";
            else if (str_v.Contains(".pif"))
                values = "PIF";
            else if (str_v.Contains(".scr"))
                values = "SCR";
            else if (str_v.Contains(".com"))
                values = "COM";
            else
                values = "未知病毒";
            return values;

        }
        //病毒删除
        public bool del_virus()
        {
            foreach(string et in site)
            {
                if(File.Exists(et))
                File.Delete(et);
            }
            bool temp = false;
            foreach(string et in site)
            {
                if(File.Exists(et))
                {
                    temp = true;
                    break;
                }
            }
            if(!temp)
            {
                virus_exist = false;
            }
                
            return temp;
        }
        //扫描按钮触发
        private void skinButton3_Click(object sender, EventArgs e)
        {
            if(e_usb)
            {
                if (exist)//autorun.inf存在
                {
                    kind = "";
                    number = 0;
                    site.Clear();
                    auto_scan();//扫描
                    label2.Text = number.ToString();
                    if (virus_exist)
                    {
                        
                        string text = "";
                        foreach(string et in site)
                        {
                            text = text + et + ";";
                        }
                        label3.Text = text;
                        Form2 form = new Form2(this); //显示第二个窗体
                        form.ShowDialog();

                    }
                    else
                    {
                        label4.Text = "未发现病毒";
                        label1.Text = "--";
                        label2.Text = "--";
                        label3.Text = "--";
                    }
                       
                }
                else
                {
                    MessageBox.Show("AutoRun.inf文件不存在！");
                }
            }
            else
            {
                MessageBox.Show("请插入可移动设备");
            }
            
        }

        private void skinLabel7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            skinPanel2.Visible = false;
            skinPanel1.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            skinPanel2.Visible = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}

