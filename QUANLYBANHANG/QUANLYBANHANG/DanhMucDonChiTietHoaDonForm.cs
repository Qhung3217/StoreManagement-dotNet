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
    public partial class DanhMucDonChiTietHoaDonForm : Form
    {
        string strConnectionString = "Data Source=DESKTOP-3U4QMFJ;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daCHITIETHD = null;
        DataTable dtCHITIETHD = null;
        SqlDataAdapter daHOADON = null;
        DataTable dtHOADON = null;
        SqlDataAdapter daSANPHAM = null;
        DataTable dtSANPHAM = null;
        bool Them;
        public DanhMucDonChiTietHoaDonForm()
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
                daSANPHAM = new SqlDataAdapter("select MaSP, TenSP from SanPham", conn);
                dtSANPHAM = new DataTable();
                dtSANPHAM.Clear();
                daSANPHAM.Fill(dtSANPHAM);

                (dgvCTHD.Columns["MaSP"] as DataGridViewComboBoxColumn).DataSource = dtSANPHAM;
                (dgvCTHD.Columns["MaSP"] as DataGridViewComboBoxColumn).DisplayMember = "TenSP";
                (dgvCTHD.Columns["MaSP"] as DataGridViewComboBoxColumn).ValueMember = "MaSP";

                daHOADON = new SqlDataAdapter("select MaHD from HoaDon", conn);
                dtHOADON = new DataTable();
                dtHOADON.Clear();
                daHOADON.Fill(dtHOADON);

                (dgvCTHD.Columns["MaHD"] as DataGridViewComboBoxColumn).DataSource = dtHOADON;
                (dgvCTHD.Columns["MaHD"] as DataGridViewComboBoxColumn).DisplayMember = "MaHD";
                (dgvCTHD.Columns["MaHD"] as DataGridViewComboBoxColumn).ValueMember = "MaHD";

                daCHITIETHD = new SqlDataAdapter("SELECT * FROM CHITIETHOADON", conn);
                dtCHITIETHD = new DataTable();
                dtCHITIETHD.Clear();
                daCHITIETHD.Fill(dtCHITIETHD);
                // Đưa dữ liệu lên DataGridView
                dgvCTHD.DataSource = dtCHITIETHD;

                this.cbxMaHD.DataSource = dtHOADON;
                this.cbxMaHD.DisplayMember = "MaHD";
                this.cbxMaHD.ValueMember = "MaHD";

                this.cbxMaSP.DataSource = dtSANPHAM;
                this.cbxMaSP.DisplayMember = "TenSP";
                this.cbxMaSP.ValueMember = "MaSP";

                // Thay đổi độ rộng cột
                dgvCTHD.AutoResizeColumns();
                // Xóa trống các đối tượng trong Panel
                this.txtSoLuong.ResetText();

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
            // Xóa trống các đối tượng trong Panel
            this.txtSoLuong.ResetText();

            // Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuyBo.Enabled = true;
            this.panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnTroVe.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            Them = false;
            // Cho phép thao tác trên Panel
            this.panel.Enabled = true;
            // Thứ tự dòng hiện hành
            int r = dgvCTHD.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel
            this.cbxMaHD.SelectedValue = dgvCTHD.Rows[r].Cells[0].Value.ToString();
            this.cbxMaSP.SelectedValue = dgvCTHD.Rows[r].Cells[1].Value.ToString();
            this.txtSoLuong.Text = dgvCTHD.Rows[r].Cells[2].Value.ToString();


            // Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuyBo.Enabled = true;
            this.panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnTroVe.Enabled = false;
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
                int r = dgvCTHD.CurrentCell.RowIndex;
                // Lấy MaKH của record hiện hành
                string strHOADON = dgvCTHD.Rows[r].Cells[0].Value.ToString();
                string strSANPHAM = dgvCTHD.Rows[r].Cells[1].Value.ToString();
                // Viết câu lệnh SQL
                cmd.CommandText = System.String.Concat("Delete From CHITIETHOADON Where MaHD = '" + 
                                strHOADON + "' And MaSP = '" + strSANPHAM + "'");
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
                    cmd.CommandText = System.String.Concat("Insert Into CHITIETHOADON Values(" +
                                "'" + this.cbxMaHD.SelectedValue.ToString() +
                                "', '" + this.cbxMaSP.SelectedValue.ToString() + 
                                "', '" + this.txtSoLuong.Text.ToString() + "')");
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
                int r = dgvCTHD.CurrentCell.RowIndex;
                // MaKH hiện hành
                string strHOADON = dgvCTHD.Rows[r].Cells[0].Value.ToString();
                string strSANPHAM = dgvCTHD.Rows[r].Cells[1].Value.ToString();

                // Câu lệnh SQL
                cmd.CommandText = System.String.Concat("Update CHITIETHOADON Set " +
                        "SoLuong = " + this.txtSoLuong.Text.ToString() +
                        " Where MaHD = '" + strHOADON + "' And MaSP = '" + strSANPHAM + "'");
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
            this.txtSoLuong.ResetText();
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


        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void DanhMucDonChiTietHoaDonForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void DanhMucDonChiTietHoaDonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtHOADON.Dispose();
            dtHOADON = null;
            dtSANPHAM.Dispose();
            dtSANPHAM = null;
            dtCHITIETHD.Dispose();
            dtCHITIETHD = null;
            conn = null;
        }
    }
}
