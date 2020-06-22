using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BookRentalShop20
{
    public partial class BookForm : MetroForm
    {
        string strConnString = "Data Source=127.0.0.1;Initial Catalog=BookRentalshopDB;Persist Security Info=True;User ID=sa;Password=p@ssw0rd!";
        string mode = "";
        public BookForm()
        {
            InitializeComponent();
        }

        private void DivForm_Load(object sender, EventArgs e)
        {
            dataTP.CustomFormat = "yyyy-MM-dd";
            dataTP.Format = DateTimePickerFormat.Custom;
            UpdateData(); // 데이터그리드 DB 데이터 로딩하기
            UPdateCboDivision();
        }

        private void UPdateCboDivision()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                conn.Open();
                string strQuery = @"SELECT Division, Names
                                    from divtbl";

                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                Dictionary<string, string> temps = new Dictionary<string, string>();

                while (reader.Read())
                {
                    temps.Add(reader[0].ToString(), reader[1].ToString());

                }

                CboDi.DataSource = new BindingSource(temps, null);
                CboDi.DisplayMember = "value";
                CboDi.ValueMember = "Key";
                CboDi.SelectedIndex = -1;

            }
        }

        private void UpdateData()
        {
            // throw new NotImplementedException();
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                conn.Open(); // DB열기
                string strQuery = "SELECT Idx ,Author ,d.Names AS 'DivNames', b.Division ,b.Names " +
                                  "       , ReleaseDate, ISBN " +
                                  "       , REPLACE(CONVERT(VARCHAR, CAST(Price AS MONEY), 1), '.00', '') AS Price " +
                                  "  FROM bookstbl AS b " +
                                  " INNER JOIN divtbl AS d " +
                                  "    ON b.Division = d.Division ";
                // SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(strQuery, conn);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "bookstbl");

                GrdMemTbl.DataSource = ds;
                GrdMemTbl.DataMember = "bookstbl";
            }
        }

        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow data = GrdMemTbl.Rows[e.RowIndex];

                TxtIdx.Text = data.Cells[0].Value.ToString();
                TxtAuthor.Text = data.Cells[1].Value.ToString();
                TxtTitle.Text = data.Cells[4].Value.ToString();
                TxtIsbn.Text = data.Cells[6].Value.ToString();
                TxtPrice.Text = data.Cells[7].Value.ToString();

             
                dataTP.Value = DateTime.Parse(data.Cells[5].Value.ToString());

                TxtIdx.ReadOnly = true;
                TxtIdx.BackColor = Color.Beige;

                CboDi.SelectedValue = data.Cells[3].Value;

                mode = "UPDATE"; // 수정은 UPDATE
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {


            ClearTextControls();

            mode = "INSERT"; // 신규는 INSERT
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtAuthor.Text))
            {
                MetroMessageBox.Show(this, "빈값은 저장할 수없습니다.", "경고",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveProcess();
            UpdateData();
            
            ClearTextControls();
        }

        private void ClearTextControls()
        {
            TxtIdx.Text = TxtAuthor.Text = "";
            TxtIdx.ReadOnly = false;
            TxtIdx.BackColor = Color.White;
            TxtIdx.Focus();
        }

        private void SaveProcess()
        {
            if (string.IsNullOrEmpty(mode))
            {
                MetroMessageBox.Show(this, "신규버튼을 누르고 데이터를 저장하십시오", "경고",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Db저장프로세스
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string strQuery = "";

                if (mode == "UPDATE")
                {
                    strQuery = " UPDATE dbo.bookstbl " +
                               "    SET Author = @Author " +
                               "      , Division = @Division " +
                               "      , Names = @Names " +
                               "      , ReleaseDate = @ReleaseDate " +
                               "      , ISBN = @ISBN " +
                               "      , Price = @Price " +
                               "  WHERE Idx = @Idx ";
                }
                else if (mode == "INSERT")
                {
                    strQuery = "INSERT INTO dbo.bookstbl " +
                                "                 (Author " +
                                "                , Division " +
                                "                , Names " +
                                "                , ReleaseDate " +
                                "                , ISBN " +
                                "                , Price) " +
                                "     VALUES " +
                                "                (@Author " +
                                "               , @Division " +
                                "               , @Names " +
                                "               , @ReleaseDate " +
                                "               , @ISBN " +
                                "               , @Price) ";
                }
                cmd.CommandText = strQuery;
                if (mode == "UPDATE")
                {
                    SqlParameter parmIdx = new SqlParameter("@Idx", SqlDbType.Int);    // DB에 저장된 스키마를 따라서
                    parmIdx.Value = TxtIdx.Text;
                    cmd.Parameters.Add(parmIdx);
                }

                SqlParameter parmAuthor = new SqlParameter("@Author", SqlDbType.VarChar, 45);    // DB에 저장된 스키마를 따라서
                parmAuthor.Value = TxtAuthor.Text;
                cmd.Parameters.Add(parmAuthor);

                SqlParameter parmDivision = new SqlParameter("@Division", SqlDbType.Char, 4);    // DB에 저장된 스키마를 따라서
                parmDivision.Value = CboDi.SelectedValue;
                cmd.Parameters.Add(parmDivision);

                SqlParameter parmNames = new SqlParameter("@Names", SqlDbType.VarChar, 100);    // DB에 저장된 스키마를 따라서
                parmNames.Value = TxtTitle.Text;
                cmd.Parameters.Add(parmNames);

                SqlParameter parmReleaseDate = new SqlParameter("@ReleaseDate", SqlDbType.Date);    // DB에 저장된 스키마를 따라서
                parmReleaseDate.Value = dataTP.Text;
                cmd.Parameters.Add(parmReleaseDate);

                SqlParameter parmISBN = new SqlParameter("@ISBN", SqlDbType.VarChar, 200);    // DB에 저장된 스키마를 따라서
                parmISBN.Value = TxtIsbn.Text;
                cmd.Parameters.Add(parmISBN);

                SqlParameter parmPrice = new SqlParameter("@Price", SqlDbType.Decimal, 10);    // DB에 저장된 스키마를 따라서
                parmPrice.Value = TxtPrice.Text;
                cmd.Parameters.Add(parmPrice);


                cmd.ExecuteNonQuery();
            }
        }

        private void TxtNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                BtnSave_Click(sender, new EventArgs());
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtIdx.Text) || string.IsNullOrEmpty(TxtAuthor.Text))
            {
                MetroMessageBox.Show(this, "빈값은 삭제할 수없습니다.", "경고",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DeleteProcess();
            UpdateData();
            ClearTextControls();
        }

        private void DeleteProcess()
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM bookstbl WHERE Idx = @Idx";
                SqlParameter parmIdx = new SqlParameter("@Idx", SqlDbType.Int);
                parmIdx.Value = TxtIdx.Text;
                cmd.Parameters.Add(parmIdx);

                cmd.ExecuteNonQuery();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
