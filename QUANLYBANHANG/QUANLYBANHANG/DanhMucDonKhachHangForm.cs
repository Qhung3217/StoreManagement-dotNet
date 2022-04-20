using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QUANLYBANHANG
{
    public partial class DanhMucDonKhachHangForm : Form
    {
        string strConnectionString = "Data Source=DESKTOP-3U4QMFJ;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daThanhPho = null;
        DataTable dtThanhPho = null;
        SqlDataAdapter daKhachHang = null;
        DataTable dtKhachHang = null;
        bool Them;
        public DanhMucDonKhachHangForm()
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


                daThanhPho = new SqlDataAdapter("SELECT * FROM THANHPHO", conn);
                dtThanhPho = new DataTable();
                dtThanhPho.Clear();
                daThanhPho.Fill(dtThanhPho);
                (dgvKHACHHANG.Columns["ThanhPho"] as DataGridViewComboBoxColumn).DataSource = dtThanhPho;
                (dgvKHACHHANG.Columns["ThanhPho"] as DataGridViewComboBoxColumn).DisplayMember = "TenThanhPho";
                (dgvKHACHHANG.Columns["ThanhPho"] as DataGridViewComboBoxColumn).ValueMember = "ThanhPho";

                daKhachHang = new SqlDataAdapter("SELECT * FROM KHACHHANG", conn);
                dtKhachHang = new DataTable();
                dtKhachHang.Clear();
                daKhachHang.Fill(dtKhachHang);
                // Đưa dữ liệu lên DataGridView
                dgvKHACHHANG.DataSource = dtKhachHang;

                this.cbxThanhPho.DataSource = dtThanhPho;
                this.cbxThanhPho.DisplayMember = "TenThanhPho";
                this.cbxThanhPho.ValueMember = "ThanhPho";


                // Thay đổi độ rộng cột
                dgvKHACHHANG.AutoResizeColumns();
                // Xóa trống các đối tượng trong Panel
                this.txtMaKH.ResetText();
                this.txtTenCty.ResetText();
                this.txtDiaChi.ResetText();
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
                MessageBox.Show("Không lấy được nội dung trong table THANHPHO.Lỗi rồi!!!");
            }
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Them = true;
            this.txtMaKH.Enabled = true;
            // Xóa trống các đối tượng trong Panel
            this.txtMaKH.ResetText();
            this.txtTenCty.ResetText();
            this.txtDiaChi.ResetText();
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
            this.txtMaKH.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            Them = false;
            // Cho phép thao tác trên Panel
            this.panel.Enabled = true;
            // Thứ tự dòng hiện hành
            int r = dgvKHACHHANG.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel
            this.txtMaKH.Text =
            dgvKHACHHANG.Rows[r].Cells[0].Value.ToString();
            this.txtTenCty.Text =
            dgvKHACHHANG.Rows[r].Cells[1].Value.ToString();
            this.txtDiaChi.Text =
            dgvKHACHHANG.Rows[r].Cells[2].Value.ToString();
            this.cbxThanhPho.SelectedValue =
            dgvKHACHHANG.Rows[r].Cells[3].Value.ToString();
            this.txtDienThoai.Text =
            dgvKHACHHANG.Rows[r].Cells[4].Value.ToString();
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
            this.txtMaKH.Enabled = false;
            this.txtTenCty.Focus();
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
                int r = dgvKHACHHANG.CurrentCell.RowIndex;
                // Lấy MaKH của record hiện hành
                string strKHACHHANG =
                dgvKHACHHANG.Rows[r].Cells[0].Value.ToString();
                // Viết câu lệnh SQL
                cmd.CommandText = System.String.Concat("Delete From KHACHHANG Where MaKH = '" + strKHACHHANG + "'");
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
                    cmd.CommandText = System.String.Concat("Insert Into KhachHang Values(" +
                                "'" + this.txtMaKH.Text.ToString() +
                                "', N'" + this.txtTenCty.Text.ToString() +
                                "', N'" + this.txtDiaChi.Text.ToString() +
                                "', N'" + this.cbxThanhPho.SelectedValue.ToString() +
                                "', '" + this.txtDienThoai.Text.ToString() + "')");
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
                int r = dgvKHACHHANG.CurrentCell.RowIndex;
                // MaKH hiện hành
                string strKHACHHANG =
                dgvKHACHHANG.Rows[r].Cells[0].Value.ToString();
                // Câu lệnh SQL
                cmd.CommandText = System.String.Concat("Update KhachHang Set" +
                        " TenCty = N'" + this.txtTenCty.Text.ToString() + 
                        "', DiaChi = N'"+ this.txtDiaChi.Text.ToString() + 
                        "', ThanhPho = N'" + this.cbxThanhPho.SelectedValue.ToString() +
                        "', DienThoai = '" + this.txtDienThoai.Text.ToString() +
                        "' Where MaKH = '" + strKHACHHANG + "'");
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
            this.txtMaKH.ResetText();
            this.txtTenCty.ResetText();
            this.txtDiaChi.ResetText();
            this.txtDienThoai.ResetText();
            this.cbxThanhPho.SelectedIndex = 0;
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

        private void DanhMucDonKhachHangForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void DanhMucDonKhachHangForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtThanhPho.Dispose();
            dtThanhPho = null;
            dtKhachHang.Dispose();
            dtKhachHang = null;
            conn = null;
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
