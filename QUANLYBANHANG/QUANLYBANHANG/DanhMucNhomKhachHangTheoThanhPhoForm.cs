using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QUANLYBANHANG
{
    public partial class DanhMucNhomKhachHangTheoThanhPhoForm : Form
    {
        string strConnectionString = "Data Source=DESKTOP-3U4QMFJ;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daThanhPho = null;
        DataTable dtThanhPho = null;
        SqlDataAdapter daKhachHang = null;
        DataTable dtKhachHang = null;
        public DanhMucNhomKhachHangTheoThanhPhoForm()
        {
            InitializeComponent();
        }
        void LoadData()
        {
            try
            {
                daKhachHang = new SqlDataAdapter("SELECT * FROM KHACHHANG where ThanhPho = '"
                                + this.cbxThanhPho.SelectedValue + "'", conn);
                dtKhachHang = new DataTable();
                dtKhachHang.Clear();
                daKhachHang.Fill(dtKhachHang);
                // Đưa dữ liệu lên DataGridView
                dgvKHACHHANG.DataSource = dtKhachHang;
                // Thay đổi độ rộng cột
                dgvKHACHHANG.AutoResizeColumns();
                this.txtTongSoKH.Text = (dgvKHACHHANG.Rows.Count - 1).ToString();
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung.Lỗi rồi!!!");
            }
        }

        private void DanhMucNhomKhachHangTheoThanhPhoForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Khởi động connection
                conn = new SqlConnection(strConnectionString);
                // Vận chuyển dữ liệu lên DataTable dtThanhPho


                daThanhPho = new SqlDataAdapter("SELECT * FROM THANHPHO", conn);
                dtThanhPho = new DataTable();
                dtThanhPho.Clear();
                daThanhPho.Fill(dtThanhPho);
                (dgvKHACHHANG.Columns["ThanhPho"] as DataGridViewComboBoxColumn).DataSource = dtThanhPho;
                (dgvKHACHHANG.Columns["ThanhPho"] as DataGridViewComboBoxColumn).DisplayMember = "TenThanhPho";
                (dgvKHACHHANG.Columns["ThanhPho"] as DataGridViewComboBoxColumn).ValueMember = "ThanhPho";


                this.cbxThanhPho.DataSource = dtThanhPho;
                this.cbxThanhPho.DisplayMember = "TenThanhPho";
                this.cbxThanhPho.ValueMember = "ThanhPho";
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung.Lỗi rồi!!!");
            }
        }

        private void cbxThanhPho_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DanhMucNhomKhachHangTheoThanhPhoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtThanhPho.Dispose();
            dtThanhPho = null;
            dtKhachHang.Dispose();
            dtKhachHang = null;
            conn = null;
        }
    }
}
