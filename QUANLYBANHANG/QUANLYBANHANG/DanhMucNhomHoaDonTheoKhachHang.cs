using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QUANLYBANHANG
{
    public partial class DanhMucNhomHoaDonTheoKhachHang : Form
    {
        string strConnectionString = "Data Source=DESKTOP-3U4QMFJ;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daNhanVien = null;
        DataTable dtNhanVien = null;
        SqlDataAdapter daKhachHang = null;
        DataTable dtKhachHang = null;
        SqlDataAdapter daHoaDon = null;
        DataTable dtHoaDon = null;
        public DanhMucNhomHoaDonTheoKhachHang()
        {
            InitializeComponent();
        }

        void LoadData()
        {
            try
            {
                daHoaDon = new SqlDataAdapter("SELECT * FROM HOADON where MaKH = '"
                                + this.cbxKhachHang.SelectedValue + "'", conn);
                dtHoaDon = new DataTable();
                dtHoaDon.Clear();
                daHoaDon.Fill(dtHoaDon);
                // Đưa dữ liệu lên DataGridView
                dgvHOADON.DataSource = dtHoaDon;
                // Thay đổi độ rộng cột
                dgvHOADON.AutoResizeColumns();
                this.txtTongSoHD.Text = (dgvHOADON.Rows.Count - 1).ToString();
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung.Lỗi rồi!!!");
            }
        }

        private void DanhMucNhomHoaDonTheoKhachHang_Load(object sender, EventArgs e)
        {
            try
            {
                // Khởi động connection
                conn = new SqlConnection(strConnectionString);
                // Vận chuyển dữ liệu lên DataTable dtNhanVien


                daNhanVien = new SqlDataAdapter("SELECT MaNV, CONCAT(HO,' ',TEN) AS TEN FROM NHANVIEN", conn);
                dtNhanVien = new DataTable();
                dtNhanVien.Clear();
                daNhanVien.Fill(dtNhanVien);
                (dgvHOADON.Columns["MaNV"] as DataGridViewComboBoxColumn).DataSource = dtNhanVien;
                (dgvHOADON.Columns["MaNV"] as DataGridViewComboBoxColumn).DisplayMember = "Ten";
                (dgvHOADON.Columns["MaNV"] as DataGridViewComboBoxColumn).ValueMember = "MaNV";





                daKhachHang = new SqlDataAdapter("SELECT * FROM KHACHHANG", conn);
                dtKhachHang = new DataTable();
                dtKhachHang.Clear();
                daKhachHang.Fill(dtKhachHang);
                (dgvHOADON.Columns["MaKH"] as DataGridViewComboBoxColumn).DataSource = dtKhachHang;
                (dgvHOADON.Columns["MaKH"] as DataGridViewComboBoxColumn).DisplayMember = "TenCty";
                (dgvHOADON.Columns["MaKH"] as DataGridViewComboBoxColumn).ValueMember = "MaKH";


                this.cbxKhachHang.DataSource = dtKhachHang;
                this.cbxKhachHang.DisplayMember = "TenCty";
                this.cbxKhachHang.ValueMember = "MaKH";
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

        private void DanhMucNhomHoaDonTheoKhachHang_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtNhanVien.Dispose();
            dtNhanVien = null;
            dtHoaDon.Dispose();
            dtHoaDon = null;
            dtKhachHang.Dispose();
            dtKhachHang = null;
            conn = null;
        }
    }
}
