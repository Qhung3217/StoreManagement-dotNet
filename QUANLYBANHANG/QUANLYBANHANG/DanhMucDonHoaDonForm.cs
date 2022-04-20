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
    public partial class DanhMucDonHoaDonForm : Form
    {
        string strConnectionString = "Data Source=DESKTOP-3U4QMFJ;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daHOADON = null;
        DataTable dtHOADON = null;
        SqlDataAdapter daKhachHang = null;
        DataTable dtKhachHang = null;
        SqlDataAdapter daNhanVien = null;
        DataTable dtNhanVien = null;
        bool Them;
        public DanhMucDonHoaDonForm()
        {
            InitializeComponent();
        }
        void LoadData()
        {
            try
            {
                // Khởi động connection
                conn = new SqlConnection(strConnectionString);
                // Vận chuyển dữ liệu lên DataTable dtThanhPho
                daKhachHang = new SqlDataAdapter("select MaKH, TenCty from KhachHang", conn);
                dtKhachHang = new DataTable();
                dtKhachHang.Clear();
                daKhachHang.Fill(dtKhachHang);

                (dgvHOADON.Columns["MaKH"] as DataGridViewComboBoxColumn).DataSource = dtKhachHang;
                (dgvHOADON.Columns["MaKH"] as DataGridViewComboBoxColumn).DisplayMember = "TenCty";
                (dgvHOADON.Columns["MaKH"] as DataGridViewComboBoxColumn).ValueMember = "MaKH";

                daNhanVien = new SqlDataAdapter("select MaNV, Ho+' '+Ten as HoTen from NhanVien", conn);
                dtNhanVien = new DataTable();
                dtNhanVien.Clear();
                daNhanVien.Fill(dtNhanVien);

                (dgvHOADON.Columns["MaNV"] as DataGridViewComboBoxColumn).DataSource = dtNhanVien;
                (dgvHOADON.Columns["MaNV"] as DataGridViewComboBoxColumn).DisplayMember = "HoTen";
                (dgvHOADON.Columns["MaNV"] as DataGridViewComboBoxColumn).ValueMember = "MaNV";

                daHOADON = new SqlDataAdapter("SELECT * FROM HOADON", conn);
                dtHOADON = new DataTable();
                dtHOADON.Clear();
                daHOADON.Fill(dtHOADON);
                // Đưa dữ liệu lên DataGridView
                dgvHOADON.DataSource = dtHOADON;

                this.cbxMaKH.DataSource = dtKhachHang;
                this.cbxMaKH.DisplayMember = "TenCty";
                this.cbxMaKH.ValueMember = "MaKH";

                this.cbxMaNV.DataSource = dtNhanVien;
                this.cbxMaNV.DisplayMember = "HoTen";
                this.cbxMaNV.ValueMember = "MaNV";

                // Thay đổi độ rộng cột
                dgvHOADON.AutoResizeColumns();
                // Xóa trống các đối tượng trong Panel
                this.txtMaHD.ResetText();

                // Không cho thao tác trên các nút Lưu / Hủy
                this.btnLuu.Enabled = false;
                this.btnHuyBo.Enabled = false;
                this.panel.Enabled = false;
                // Cho thao tác trên các nút Thêm / Sửa / Xóa /Thoát
                this.btnThem.Enabled = true;
                this.btnSua.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnTroVe.Enabled = true;
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung trong table NHANVIEN.Lỗi rồi!!!");
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            Them = true;
            this.txtMaHD.Enabled = true;
            // Xóa trống các đối tượng trong Panel
            this.txtMaHD.ResetText();

            // Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuyBo.Enabled = true;
            this.panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnTroVe.Enabled = false;
            // Đưa con trỏ đến TextField txtThanhPho
            this.txtMaHD.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            Them = false;
            // Cho phép thao tác trên Panel
            this.panel.Enabled = true;
            // Thứ tự dòng hiện hành
            int r = dgvHOADON.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel
            this.txtMaHD.Text = dgvHOADON.Rows[r].Cells[0].Value.ToString();
            this.cbxMaKH.SelectedValue = dgvHOADON.Rows[r].Cells[1].Value.ToString();
            this.cbxMaNV.SelectedValue = dgvHOADON.Rows[r].Cells[2].Value.ToString();
            this.dtpNgayLapHD.Value = Convert.ToDateTime(dgvHOADON.Rows[r].Cells[3].Value.ToString());
            this.dtpNgayLapNhanHang.Value = Convert.ToDateTime(dgvHOADON.Rows[r].Cells[4].Value.ToString());


            // Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuyBo.Enabled = true;
            this.panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnTroVe.Enabled = false;
            // Đưa con trỏ đến TextField txtMaKH
            this.txtMaHD.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                // Thực hiện lệnh
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                // Lấy thứ tự record hiện hành
                int r = dgvHOADON.CurrentCell.RowIndex;
                // Lấy MaKH của record hiện hành
                string strHOADON = dgvHOADON.Rows[r].Cells[0].Value.ToString();
                // Viết câu lệnh SQL
                cmd.CommandText = System.String.Concat("Delete From HOADON Where MaHD = '" + strHOADON + "'");
                cmd.CommandType = CommandType.Text;
                // Thực hiện câu lệnh SQL
                cmd.ExecuteNonQuery();
                // Cập nhật lại DataGridView
                LoadData();
                // Thông báo
                MessageBox.Show("Đã xóa xong!");
            }
            catch (SqlException)
            {
                MessageBox.Show("Không xóa được. Lỗi rồi!");
            }
            // Đóng kết nối
            conn.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            conn.Open();
            String ngayLapHD = this.dtpNgayLapHD.Value.ToString("yyyy-MM-dd");
            String ngayNhanHang = this.dtpNgayLapNhanHang.Value.ToString("yyyy-MM-dd");

            // Thêm dữ liệu
            if (Them)
            {
                try
                {
                    // Thực hiện lệnh
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    // Lệnh Insert InTo
                    cmd.CommandText = System.String.Concat("Insert Into HOADON Values(" +
                                "'" + this.txtMaHD.Text.ToString() +
                                "', '" + this.cbxMaKH.SelectedValue.ToString() +
                                "', '" + this.cbxMaNV.SelectedValue.ToString() +
                                "', '" + ngayLapHD +
                                "', '" + ngayNhanHang + "')");
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    // Load lại dữ liệu trên DataGridView
                    LoadData();
                    // Thông báo
                    MessageBox.Show("Đã thêm xong!");
                }
                catch (SqlException)
                {
                    MessageBox.Show("Không thêm được. Lỗi rồi!");
                }
            }
            if (!Them)
            {
                // Thực hiện lệnh
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                // Thứ tự dòng hiện hành
                int r = dgvHOADON.CurrentCell.RowIndex;
                // MaKH hiện hành
                string strHOADON =
                dgvHOADON.Rows[r].Cells[0].Value.ToString();
                // Câu lệnh SQL
                cmd.CommandText = System.String.Concat("Update HOADON Set" +
                        " MaKH = N'" + this.cbxMaKH.SelectedValue.ToString() +
                        "', MaNV = N'" + this.cbxMaNV.SelectedValue.ToString() +
                        "', NgayLapHD = '" + ngayLapHD +
                        "', NgayNhanHang = '" + ngayNhanHang +
                        "' Where MaHD = '" + strHOADON + "'");
                // Cập nhật
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                // Load lại dữ liệu trên DataGridView
                LoadData();
                // Thông báo
                MessageBox.Show("Đã sửa xong!");
            }
            // Đóng kết nối
            conn.Close();
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            // Xóa trống các đối tượng trong Panel
            this.txtMaHD.ResetText();
            // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
            this.btnThem.Enabled = true;
            this.btnSua.Enabled = true;
            this.btnXoa.Enabled = true;
            this.btnTroVe.Enabled = true;
            // Không cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = false;
            this.btnHuyBo.Enabled = false;
            this.panel.Enabled = false;
        }
        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DanhMucDonHoaDonForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void DanhMucDonHoaDonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtHOADON.Dispose();
            dtHOADON = null;
            conn = null;
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
