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
    public partial class DanhMucNhomHoaDonTheoNhanVien : Form
    {
        string strConnectionString = "Data Source=DESKTOP-3U4QMFJ;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daNhanVien = null;
        DataTable dtNhanVien = null;
        SqlDataAdapter daKhachHang = null;
        DataTable dtKhachHang = null;
        SqlDataAdapter daHoaDon = null;
        DataTable dtHoaDon = null;
        public DanhMucNhomHoaDonTheoNhanVien()
        {
            InitializeComponent();
        }
        void LoadData()
        {
            try
            {
                daHoaDon = new SqlDataAdapter("SELECT * FROM HOADON where MaNV = '"
                                + this.cbxNhanVien.SelectedValue + "'", conn);
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

        private void DanhMucNhomHoaDonTheoNhanVien_Load(object sender, EventArgs e)
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

                this.cbxNhanVien.DataSource = dtNhanVien;
                this.cbxNhanVien.DisplayMember = "Ten";
                this.cbxNhanVien.ValueMember = "MaNV";



                daKhachHang = new SqlDataAdapter("SELECT * FROM KHACHHANG", conn);
                dtKhachHang = new DataTable();
                dtKhachHang.Clear();
                daKhachHang.Fill(dtKhachHang);
                (dgvHOADON.Columns["MaKH"] as DataGridViewComboBoxColumn).DataSource = dtKhachHang;
                (dgvHOADON.Columns["MaKH"] as DataGridViewComboBoxColumn).DisplayMember = "TenCty";
                (dgvHOADON.Columns["MaKH"] as DataGridViewComboBoxColumn).ValueMember = "MaKH";


                
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung.Lỗi rồi!!!");
            }
        }
        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DanhMucNhomHoaDonTheoNhanVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtNhanVien.Dispose();
            dtNhanVien = null;
            dtHoaDon.Dispose();
            dtHoaDon = null;
            dtKhachHang.Dispose();
            dtKhachHang = null;
            conn = null;
        }

        private void cbxNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
