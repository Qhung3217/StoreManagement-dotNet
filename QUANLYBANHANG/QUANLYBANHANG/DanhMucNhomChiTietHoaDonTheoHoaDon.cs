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
    public partial class DanhMucNhomChiTietHoaDonTheoHoaDon : Form
    {
        string strConnectionString = "Data Source=DESKTOP-3U4QMFJ;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daSanPham = null;
        DataTable dtSanPham = null;
        SqlDataAdapter daCTHD = null;
        DataTable dtCTHD = null;
        SqlDataAdapter daHoaDon = null;
        DataTable dtHoaDon = null;
        public DanhMucNhomChiTietHoaDonTheoHoaDon()
        {
            InitializeComponent();
        }
        void LoadData()
        {
            try
            {
                daCTHD = new SqlDataAdapter("SELECT * FROM CHITIETHOADON where MaHD = '"
                                + this.cbxHD.SelectedValue + "'", conn);
                dtCTHD = new DataTable();
                dtCTHD.Clear();
                daCTHD.Fill(dtCTHD);
                // Đưa dữ liệu lên DataGridView
                dgvCTHD.DataSource = dtCTHD;
                // Thay đổi độ rộng cột
                dgvCTHD.AutoResizeColumns();
                this.txtTongSoCTHD.Text = (dgvCTHD.Rows.Count - 1).ToString();
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung.Lỗi rồi!!!");
            }
        }

        private void DanhMucNhomChiTietHoaDonTheoHoaDon_Load(object sender, EventArgs e)
        {
            try
            {
                // Khởi động connection
                conn = new SqlConnection(strConnectionString);
                // Vận chuyển dữ liệu lên DataTable dtSanPham


                daSanPham = new SqlDataAdapter("SELECT MaSP, TenSP FROM SANPHAM", conn);
                dtSanPham = new DataTable();
                dtSanPham.Clear();
                daSanPham.Fill(dtSanPham);
                (dgvCTHD.Columns["MaSP"] as DataGridViewComboBoxColumn).DataSource = dtSanPham;
                (dgvCTHD.Columns["MaSP"] as DataGridViewComboBoxColumn).DisplayMember = "TenSP";
                (dgvCTHD.Columns["MaSP"] as DataGridViewComboBoxColumn).ValueMember = "MaSP";

                



                daHoaDon = new SqlDataAdapter("SELECT * FROM HOADON", conn);
                dtHoaDon = new DataTable();
                dtHoaDon.Clear();
                daHoaDon.Fill(dtHoaDon);
                (dgvCTHD.Columns["MaHD"] as DataGridViewComboBoxColumn).DataSource = dtHoaDon;
                (dgvCTHD.Columns["MaHD"] as DataGridViewComboBoxColumn).DisplayMember = "MaHD";
                (dgvCTHD.Columns["MaHD"] as DataGridViewComboBoxColumn).ValueMember = "MaHD";

                this.cbxHD.DataSource = dtHoaDon;
                this.cbxHD.DisplayMember = "MaHD";
                this.cbxHD.ValueMember = "MaHD";

            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung.Lỗi rồi!!!");
            }
        }

        private void DanhMucNhomChiTietHoaDonTheoHoaDon_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtSanPham.Dispose();
            dtSanPham = null;
            dtCTHD.Dispose();
            dtCTHD = null;
            dtHoaDon.Dispose();
            dtHoaDon = null;
            conn = null;
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbxNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
