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
    public partial class DanhMucForm : Form
    {
        string strConnectionString = "Data Source=DESKTOP-3U4QMFJ;Initial Catalog=QuanLyBanHang;Integrated Security=True";
        SqlConnection conn = null;
        SqlDataAdapter daTable = null;
        DataTable dtTable = null;
        public DanhMucForm()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                conn = new SqlConnection(strConnectionString);
                // Xử lý danh mục
                int intDM = Convert.ToInt32(this.Text);
                switch (intDM)
                {
                    case 1:
                        lblDM.Text = "Danh Mục Thành Phố";
                        daTable = new SqlDataAdapter("SELECT ThanhPho, TenThanhPho FROM THANHPHO",
                        conn);
                        break;
                    case 2:
                        lblDM.Text = "Danh Mục Khách Hàng";
                        daTable = new SqlDataAdapter("SELECT MaKH, TenCTy FROM KHACHHANG", conn);
                        break;
                    case 3:
                        lblDM.Text = "Danh Mục Nhân Viên";
                        daTable = new SqlDataAdapter("SELECT MaNV, Ho, Ten FROM NHANVIEN", conn);
                        break;
                    case 4:
                        lblDM.Text = "Danh Mục Sản Phẩm";
                        daTable = new SqlDataAdapter("SELECT MaSP, TenSP, DonViTinh, DonGia FROM SANPHAM",
                        conn);
                        break;
                    case 5:
                        lblDM.Text = "Danh Mục Hóa Đơn";
                        daTable = new SqlDataAdapter("SELECT MaHD, MaKH, MaNV FROM HOADON", conn);
                        break;
                    case 6:
                        lblDM.Text = "Danh Mục Chi Tiết Hóa Đơn";
                        daTable = new SqlDataAdapter("SELECT * FROM CHITIETHOADON", conn);
                        break;
                    default:
                        break;

                }
                dtTable = new DataTable();
                dtTable.Clear();
                daTable.Fill(dtTable);
                dgvDANHMUC.DataSource = dtTable;
                dgvDANHMUC.AutoResizeColumns();
            }
            catch(SqlException)
            {
                MessageBox.Show("Không lấy được nội dung trong table.Lỗi rồi!!!");
            }
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
