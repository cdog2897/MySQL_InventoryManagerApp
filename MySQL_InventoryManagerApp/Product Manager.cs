using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySQL_InventoryManagerApp
{
    public partial class Form1 : Form
    {
        BindingSource categoriesBindingSource = new BindingSource();
        BindingSource productsBindingSource = new BindingSource();
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void btnShowAllCategories_Click(object sender, EventArgs e)
        {
            DAO dao = new DAO();

            categoriesBindingSource.DataSource = dao.showCategories();
            dgvCategories.DataSource = categoriesBindingSource;
        }
        private void btnShowAllProducts_Click(object sender, EventArgs e)
        {
            DAO dao = new DAO();

            productsBindingSource.DataSource = dao.showProducts();
            dgvProducts.DataSource = productsBindingSource;
        }
        private void dgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DAO dao = new DAO();
            List<Category> list = (List<Category>) categoriesBindingSource.DataSource;
            int rowClicked = list[dataGridView.CurrentRow.Index].ID;

            dgvProducts.DataSource = dao.getProductsUsingJoin(rowClicked);
            dao.getProductsUsingJoin(rowClicked);
        }
        private void btnSearchCategories_Click(object sender, EventArgs e)
        {
            DAO dao = new DAO();
            categoriesBindingSource.DataSource = dao.searchCategories(txtSearchCategories.Text);
            dgvCategories.DataSource = categoriesBindingSource;
        }
        private void btnSearchProducts_Click(object sender, EventArgs e)
        {
            DAO dao = new DAO();
            productsBindingSource.DataSource = dao.searchProducts(txtSearchProducts.Text);
            dgvProducts.DataSource = productsBindingSource;
        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            DAO dao = new DAO();
            Category category = new Category
            {
                Name = txtCategoryName.Text,
                Description = txtCategoryDescription.Text
            };

            int rowsInserted = dao.addNewCategory(category);
            categoriesBindingSource.DataSource = dao.showCategories();
            dgvCategories.DataSource = categoriesBindingSource;
        }
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            DAO dao = new DAO();
            Product product = new Product
            {
                Name = txtProductName.Text,
                Description = txtProductDescription.Text,
                Price = Int32.Parse(txtProductWholesalePrice.Text),
                Wholesale = Int32.Parse(txtProductRetailPrice.Text),
                Quantity = Int32.Parse(txtProductQuantity.Text),
                Category_Id = cbProductCategoryId.SelectedIndex + 1,
            };
            int rowsInserted = dao.addNewProduct(product);
            productsBindingSource.DataSource = dao.getProductsUsingJoin(product.Category_Id);
            dgvProducts.DataSource = productsBindingSource;
    
        }
        private void cbProductCategoryId_Click(object sender, EventArgs e)
        {
            // update the category options
            cbProductCategoryId.Items.Clear();
            DAO dao = new DAO();
            List<string> list = dao.getCategories();
            foreach(string str in list)
            {
                cbProductCategoryId.Items.Add(str);
            }
        }
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            DAO dao = new DAO();
            int rowClicked = dgvCategories.CurrentRow.Index;
            string IDstring = dgvCategories.Rows[rowClicked].Cells[0].Value.ToString();
            int ID = Int32.Parse(IDstring);
            int rowsDeleted = dao.deleteCategory(ID);
            categoriesBindingSource.DataSource = dao.showCategories();
            dgvCategories.DataSource = categoriesBindingSource;
        }
        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            DAO dao = new DAO();
            int rowClicked = dgvProducts.CurrentRow.Index;
            string IDstring = dgvProducts.Rows[rowClicked].Cells[0].Value.ToString();
            int ID = Int32.Parse(IDstring);
            int rowsDeleted = dao.deleteProduct(ID);
            int categoryId = Int32.Parse(dgvProducts.Rows[rowClicked].Cells[6].Value.ToString());
            productsBindingSource.DataSource = dao.getProductsUsingJoin(categoryId);
            dgvProducts.DataSource = productsBindingSource;
        }
    }
}
