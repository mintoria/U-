using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace U盘防火墙
{
    public partial class Form2 : CCSkinMain
    {
        private 明管家 f1;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(明管家 f)
        {
            InitializeComponent();
            f1 = f;
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            bool temp=f1.del_virus();
            if(!temp)
            MessageBox.Show("成功删除所有病毒，建议手动删除AutoRun.inf文件，以绝后患");
            this.Close();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
