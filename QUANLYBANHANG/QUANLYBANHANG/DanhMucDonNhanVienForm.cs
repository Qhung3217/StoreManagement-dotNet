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
    public partial class DanhMucDonNhanVienForm : Form
    {
        string strConnectionString = "Data Source=DESKTOP-3U4QMFJ;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daNhanVien = null;
        DataTable dtNhanVien = null;
        bool Them;
        public DanhMucDonNhanVienForm()
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

                daNhanVien = new SqlDataAdapter("SELECT * FROM NHANVIEN", conn);
                dtNhanVien = new DataTable();
                dtNhanVien.Clear();
                daNhanVien.Fill(dtNhanVien);
                // Đưa dữ liệu lên DataGridView
                dgvNHANVIEN.DataSource = dtNhanVien;



                // Thay đổi độ rộng cột
                dgvNHANVIEN.AutoResizeColumns();
                // Xóa trống các đối tượng trong Panel
                this.txtMaNV.ResetText();
                this.txtHo.ResetText();
                this.txtTen.ResetText();
                this.txtDiaChi.ResetText();
                this.chbxNu.Checked = false;
                this.txtDienThoai.ResetText();

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
        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Them = true;
            this.txtMaNV.Enabled = true;
            // Xóa trống các đối tượng trong Panel
            this.txtMaNV.ResetText();
            this.txtHo.ResetText();
            this.txtTen.ResetText();
            this.txtDiaChi.ResetText();
            this.chbxNu.Checked = false;
            this.txtDienThoai.ResetText();

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
            this.txtMaNV.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            Them = false;
            // Cho phép thao tác trên Panel
            this.panel.Enabled = true;
            // Thứ tự dòng hiện hành
            int r = dgvNHANVIEN.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel
            this.txtMaNV.Text = dgvNHANVIEN.Rows[r].Cells[0].Value.ToString();
            this.txtHo.Text = dgvNHANVIEN.Rows[r].Cells[1].Value.ToString();
            this.txtTen.Text = dgvNHANVIEN.Rows[r].Cells[2].Value.ToString();
            this.chbxNu.Checked = Convert.ToBoolean(dgvNHANVIEN.Rows[r].Cells[3].Value);
            this.dtpNgayNV.Value = Convert.ToDateTime(dgvNHANVIEN.Rows[r].Cells[4].Value.ToString());
            this.txtDiaChi.Text = dgvNHANVIEN.Rows[r].Cells[5].Value.ToString();
            this.txtDienThoai.Text = dgvNHANVIEN.Rows[r].Cells[6].Value.ToString();

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
            this.txtMaNV.Enabled = false;
            this.txtHo.Focus();
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
                int r = dgvNHANVIEN.CurrentCell.RowIndex;
                // Lấy MaKH của record hiện hành
                string strNHANVIEN = dgvNHANVIEN.Rows[r].Cells[0].Value.ToString();
                // Viết câu lệnh SQL
                cmd.CommandText = System.String.Concat("Delete From NHANVIEN Where MaNV = '" + strNHANVIEN + "'");
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
            int nu = this.chbxNu.Checked ? 1 : 0;
            String ngayNV = this.dtpNgayNV.Value.ToString("yyyy-MM-dd");
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
                    cmd.CommandText = System.String.Concat("Insert Into NHANVIEN Values(" +
                                "'" + this.txtMaNV.Text.ToString() +
                                "', N'" + this.txtHo.Text.ToString() +
                                "', N'" + this.txtTen.Text.ToString() +
                                "', " + nu +
                                ", '" + ngayNV +
                                "', N'" + this.txtDiaChi.Text.ToString() +
                                "', '" + this.txtDienThoai.Text.ToString() + "', NULL)");
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
                int r = dgvNHANVIEN.CurrentCell.RowIndex;
                // MaKH hiện hành
                string strNHANVIEN =
                dgvNHANVIEN.Rows[r].Cells[0].Value.ToString();
                // Câu lệnh SQL
                cmd.CommandText = System.String.Concat("Update NHANVIEN Set" +
                        " Ho = N'" + this.txtHo.Text.ToString() +
                        "', Ten = N'" + this.txtTen.Text.ToString() +
                        "', Nu = '" + nu +
                        "', NgayNv = '" + ngayNV +
                        "', DiaChi = N'" + this.txtDiaChi.Text.ToString() +
                        "', DienThoai = '" + this.txtDienThoai.Text.ToString() +
                        "' Where MaNV = '" + strNHANVIEN + "'");
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
            this.txtMaNV.ResetText();
            this.txtHo.ResetText();
            this.txtTen.ResetText();
            this.txtDienThoai.ResetText();
            this.txtDiaChi.ResetText();
            this.chbxNu.Checked = false;
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

        private void DanhMucDonNhanVienForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void DanhMucDonNhanVienForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtNhanVien.Dispose();
            dtNhanVien = null;
            conn = null;
        }
    }
}
