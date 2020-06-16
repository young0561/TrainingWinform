using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonTestApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        // 버튼 클리시 이벤트 발생시키는 메소드

     /*   버튼에는 popup, standard, flat 등 문구가 적혀 있다. 해당 버튼을 눌렀을 경우 
        label에 문구가 들어가는 표시를 하기 위한 코드 이다.*/
        private void BtnFlat_Click(object sender, EventArgs e)
        {
            
            LblButtonStyle.Text = FlatStyle.Flat.ToString();

        }

        private void BtnPopup_Click(object sender, EventArgs e)
        {
            LblButtonStyle.Text = FlatStyle.Popup.ToString();
        }

        private void BtnStandard_Click(object sender, EventArgs e)
        {
            LblButtonStyle.Text = FlatStyle.Standard.ToString();
        }

        private void BtnSystem_Click(object sender, EventArgs e)
        {
            LblButtonStyle.Text = FlatStyle.System.ToString();
        }


        
        private void MainForm_Load(object sender, EventArgs e)
        {
            LblButtonStyle.Text = "결과 표시.";
        }
    }
}
