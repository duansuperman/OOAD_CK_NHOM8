using QuanLyCafe.DAO;
using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing.Printing;

namespace QuanLyCafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }

        }
        public fTableManager(Account acc)
        {
            
            InitializeComponent();
            this.LoginAccount = acc;
            LoadCategory();
            LoadTable();
            LoadComboboxTable(cbSwitchTable);
            
        }

        public fTableManager()
        {
            
            // TODO: Complete member initialization
        }

        #region Method
        void ChangeAccount(int type)
        {
           // MessageBox.Show(type.ToString());
            if (type == 1)
            {
                adminToolStripMenuItem.Enabled = true;
            }
            else
            {
                adminToolStripMenuItem.Enabled = false;
            }
           
            thôngTinToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        public void LoadTable()
        {
            
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
         

        }

        

        void ShowBill(int id)
        {
            List<QuanLyCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            lsvBill.Items.Clear();
            float TotalPrice = 0;
            foreach (QuanLyCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                TotalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
                //MessageBox.Show(item.FoodID.ToString() + item.Count.ToString());
            }
            CultureInfo culture = new CultureInfo("vi-VN");

            txtTotalPrice.Text = TotalPrice.ToString("c",culture);
            

        }
        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        #endregion

        #region Events

        private void bt_Click(object sender, EventArgs e)
        {
            
        }
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void thôngTinToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.ShowDialog();

        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog();
        }

        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }
        private void flpTable_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fTableManager_Load(object sender, EventArgs e)
        {

        }
       

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
            {
                return;
            }
                

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }
            //MessageBox.Show(lsvBill.Tag.ToString());
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                Table table = lsvBill.Tag as Table;
                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                int discount = (int)nmDisCount.Value;
                //MessageBox.Show(discount.ToString());
                float totalPrice;
                string []Str_total = txtTotalPrice.Text.ToString().Split('.');
                if (Str_total.Length == 2)
                {
                    totalPrice = float.Parse(Str_total[0]);
                }
                else
                {
                    totalPrice = float.Parse(Str_total[0])*1000+float.Parse(Str_total[1]);
                }
                //MessageBox.Show(totalPrice.ToString());
                float finalTotalPrice = (1 - (float)discount / 100) * totalPrice * 1000;
                if (idBill != -1)
                {
                    CultureInfo culture = new CultureInfo("vi-VN");
                    if (MessageBox.Show("Bạn muốn thanh toán hóa đơn " + table.Name + "\n" + "Tổng tiền = " + finalTotalPrice.ToString("c",culture), "Thông báo !", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                        PrintReciept();
                        ShowBill(table.ID);
                        
                    }
                }
                
                LoadTable();
            }
            catch
            {
                MessageBox.Show("Mời bạn chọn bàn để thanh toán ! ");
            }
        }
        #endregion

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;

            int id2 = (cbSwitchTable.SelectedItem as Table).ID;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }
        }

        private void lsvBill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {

        }

        private void loadTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flpTable.Controls.Clear();
            LoadTable();
        }

        public void PrintReciept()
        {
            PrintDialog printDialog = new PrintDialog();

            PrintDocument printDocument = new PrintDocument();

            printDialog.Document = printDocument; //add the document to the dialog box...        

            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing

            //on a till you will not want to ask the user where to print but this is fine for the test envoironment.

            DialogResult result = printDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                printDocument.Print();

            } 
        }
        private void btnPrintReciept_Click(object sender, EventArgs e)
        {
            


        }

        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

           
            //this prints the reciept

            Graphics graphic = e.Graphics;

            Font font = new Font("Courier New", 12); //must use a mono spaced font as the spaces need to line up

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 40;

            graphic.DrawString("Nhà hàng Thiên Bút Restaurant", new Font("Courier New", 18), new SolidBrush(Color.Black), startX, startY);
            string top = "Tên món".PadRight(20) + "SL".PadRight(10) +"Đ.Giá".PadRight(10)+ "Thành tiền";
            graphic.DrawString(top, font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("--------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent

            
            

            foreach (ListViewItem item in lsvBill.Items)
            {
                string t =item.SubItems[0].Text.ToString();
                if(t.Length>=13){
                    t=t.Substring(0,13);
                    t += "...";
                }
                graphic.DrawString(t.PadRight(20) + item.SubItems[1].Text.ToString().PadRight(10) + item.SubItems[2].Text.ToString().PadRight(10) + item.SubItems[3].Text.ToString(), new Font("Courier New", 12, FontStyle.Italic), new SolidBrush(Color.Red), startX, startY + offset);

                offset = offset + (int)fontHeight + 5; //make the spacing consistent

            }


            graphic.DrawString("--------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent
            //when we have drawn all of the items add the total

            offset = offset + 20; //make some room so that the total stands out.

            graphic.DrawString("Total  ".PadRight(40) + txtTotalPrice.Text.ToString(), new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            int discount = (int)nmDisCount.Value;
            //MessageBox.Show(discount.ToString());
            float totalPrice;
            string[] Str_total = txtTotalPrice.Text.ToString().Split('.');
            if (Str_total.Length == 2)
            {
                totalPrice = float.Parse(Str_total[0]);
            }
            else
            {
                totalPrice = float.Parse(Str_total[0]) * 1000 + float.Parse(Str_total[1]);
            }
            //MessageBox.Show(totalPrice.ToString());
            float finalTotalPrice = (1 - (float)discount / 100) * totalPrice * 1000;

            offset = offset + 20; //make some room so that the total stands out.

            graphic.DrawString("Sales ".PadRight(40) + discount.ToString()+"%", new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            CultureInfo culture = new CultureInfo("vi-VN");
            offset = offset + 40; //make some room so that the total stands out.

            graphic.DrawString("Total to pay ".PadRight(40) + finalTotalPrice.ToString("c",culture), new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            
        }
    }
}
