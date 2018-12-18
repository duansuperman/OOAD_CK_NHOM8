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
using QuanLyCafe.DAO;
using QuanLyCafe.DTO;
using System.Globalization;

namespace QuanLyCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            LoadAll();
        }
        #region methods

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        void LoadAll()
        {
            dtgvFood.DataSource = foodList;
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            Report();
            LoadListFood();
            AddFoodBinding();
            LoadCategoryIntoCombobox(cbFoodCategory);
            LoadListCategory();
            LoadListTable();
            LoadListAccount();
        }
        void Report()
        {
            try
            {
                float total = 0;
                int rows = dtgvBill.RowCount;
                for (int i = 1; i < rows - 1; i++)
                {
                    total += float.Parse(dtgvBill.Rows[i].Cells[4].Value.ToString());
                }
                CultureInfo culture = new CultureInfo("vi-VN");
                txbTotal.Text = total.ToString("c", culture);
            }
            catch
            {
                MessageBox.Show("Không thể báo cáo tổng tiền !");
            }
        }
        void LoadDateTimePickerBill()
        {
            DateTime datenow = DateTime.Now;
            dtpkFromDate.Value = new DateTime(datenow.Year, datenow.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDate(checkIn, checkOut);
             

        }
        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        #endregion

        #region events

        #endregion
        private void tcAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void tpBill_Click(object sender, EventArgs e)
        {

        }

        private void tpFood_Click(object sender, EventArgs e)
        {

        }

        private void tpFoodCategory_Click(object sender, EventArgs e)
        {

        }

        private void tpTable_Click(object sender, EventArgs e)
        {

        }

        private void tpAccount_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtgvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtpkFromDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpkToDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            Report();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtgvFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            dtgvFood.DataSource = null;
            dtgvFood.DataSource = FoodDAO.Instance.GetListFood();
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            
        }

        private void txbSearchFoodName_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                    cbFoodCategory.SelectedItem = cateogory;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == cateogory.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch {
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txbFoodName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cbFoodCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void nmFoodPrice_ValueChanged(object sender, EventArgs e)
        {

        }

        private void fAdmin_Load(object sender, EventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btn_Click(object sender, EventArgs e)
        {

        }

        private void btnSearchFoodName_Click(object sender, EventArgs e)
        {
            dtgvFood.DataSource = null;
            dtgvFood.DataSource = SearchFoodByName(txtSearchFoodName.Text);
        }
        void LoadListCategory()
        {
            dtgrCategory.DataSource = null;
            dtgrCategory.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
           
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            if (txbCategoryName.Text == "")
            {
                MessageBox.Show("Không thể cập nhât !");
                txbCategoryName.Focus();
                return;
            }
            
            string strquery = "update FoodCategory set name = @Name where id = @ID";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(strquery, con);
                cmd.Parameters.AddWithValue("@ID", int.Parse(txbCategoryID.Text));
                cmd.Parameters.AddWithValue("@Name", txbCategoryName.Text.ToString());
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Không thể cập nhât !");
            }


            con.Close();
            LoadListCategory();
           
        }

        private void dtgrCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int numrow;
                numrow = e.RowIndex;
                txbCategoryID.Text = dtgrCategory.Rows[numrow].Cells[1].Value.ToString();
                txbCategoryName.Text = dtgrCategory.Rows[numrow].Cells[0].Value.ToString();
            }
            catch
            {
                MessageBox.Show("Không được chọn !");
            }
        }
        void UpdateTable()
        {
            fTableManager f = new fTableManager();
            f.LoadTable();

        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string strquery = "insert into FoodCategory values(@Name)";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(strquery, con);
                cmd.Parameters.AddWithValue("@Name", txbCategoryName.Text.ToString());
                cmd.ExecuteNonQuery();
            
            
            }
            catch
            {
                MessageBox.Show("Không thể thêm !");
            }
            con.Close();
            LoadListCategory();
          
        }
        
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            string strquery = "delete FoodCategory where id = @ID";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand(strquery, con);
            cmd.Parameters.AddWithValue("@ID", int.Parse(txbCategoryID.Text));
            cmd.ExecuteNonQuery();


            con.Close();
            LoadListCategory();
            
        }
        void LoadListTable()
        {
            dtgvTable.DataSource = null;
            dtgvTable.DataSource = TableDAO.Instance.LoadTableList();
        }
        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void dtgvTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int num = e.RowIndex;
                txbTableID.Text = dtgvTable.Rows[num].Cells[2].Value.ToString();
                txbTableName.Text = dtgvTable.Rows[num].Cells[1].Value.ToString();
                cbTableStatus.Text = dtgvTable.Rows[num].Cells[0].Value.ToString();
            }
            catch
            {
                MessageBox.Show("Không thể chọn !");
            }

        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string strquery = "insert into TableFood values( @TableName,@Status)";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(strquery, con);
                cmd.Parameters.AddWithValue("@TableName", txbTableName.Text.ToString());
                cmd.Parameters.AddWithValue("@Status", cbTableStatus.Text.ToString());
                cmd.ExecuteNonQuery();

            }
            catch
            {
                MessageBox.Show("Không thể thêm !");
            }
            con.Close();
            
            LoadListTable();
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            string strquery = "delete TableFood where id = @ID";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand(strquery, con);
            cmd.Parameters.AddWithValue("@ID", txbTableID.Text.ToString());

            cmd.ExecuteNonQuery();


            con.Close();
            LoadListTable();
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string strquery = "update TableFood set name = @TableName, status = @Status where id = @ID";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(strquery, con);
                cmd.Parameters.AddWithValue("@ID", int.Parse(txbTableID.Text.ToString()));
                cmd.Parameters.AddWithValue("@TableName", txbTableName.Text.ToString());
                cmd.Parameters.AddWithValue("@Status", cbTableStatus.Text.ToString());
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Không thể cập nhật !");
            }


            con.Close();
            LoadListTable();
        }
        void LoadListAccount()
        {
            dtgvAccount.DataSource = null;
            string strquery = "select * from Account";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(strquery, con);
               
                cmd.ExecuteNonQuery();
                DataTable table = new DataTable();
               SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);
                dtgvAccount.DataSource = table;


            }
            catch
            {
                MessageBox.Show("Không thể xem !");
            }
            con.Close();
            
        }
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadListAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string strquery = "insert into Account values(@username,@displayname,@password,@type)";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(strquery, con);
                cmd.Parameters.AddWithValue("@username", txbUserName.Text.ToString());
                cmd.Parameters.AddWithValue("@displayname", txbDisplayName.Text.ToString());
                cmd.Parameters.AddWithValue("@password", txbPassWord.Text.ToString());
                cmd.Parameters.AddWithValue("@type", int.Parse(cbAccountType.Text));
                cmd.ExecuteNonQuery();
                


            }
            catch
            {
                MessageBox.Show("Không thể thêm !");
            }
            con.Close();
            LoadListAccount();
        }

        private void dtgvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int num = e.RowIndex;
                txbUserName.Text = dtgvAccount.Rows[num].Cells[0].Value.ToString();
                txbDisplayName.Text = dtgvAccount.Rows[num].Cells[1].Value.ToString();
                cbAccountType.Text = dtgvAccount.Rows[num].Cells[3].Value.ToString();
                txbPassWord.Text = dtgvAccount.Rows[num].Cells[2].Value.ToString();
            }
            catch
            {
                MessageBox.Show("Không thể thêm !");
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string strquery = "delete Account where UserName = @username";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(strquery, con);
                cmd.Parameters.AddWithValue("@username", txbUserName.Text.ToString());
              
                cmd.ExecuteNonQuery();



            }
            catch
            {
                MessageBox.Show("Không thể xóa !");
            }
            con.Close();
            LoadListAccount();
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string strquery = "update  Account set DisplayName = @displayname,PassWord=@password,Type=@type where UserName=@username";
            SqlConnection con = new SqlConnection("Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True");
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(strquery, con);
                cmd.Parameters.AddWithValue("@username", txbUserName.Text.ToString());
                cmd.Parameters.AddWithValue("@displayname", txbDisplayName.Text.ToString());
                cmd.Parameters.AddWithValue("@password", txbPassWord.Text.ToString());
                cmd.Parameters.AddWithValue("@type", int.Parse(cbAccountType.Text));
                cmd.ExecuteNonQuery();



            }
            catch
            {
                MessageBox.Show("Không thể cập nhật !");
            }
            con.Close();
            LoadListAccount();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           UpdateTable();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
