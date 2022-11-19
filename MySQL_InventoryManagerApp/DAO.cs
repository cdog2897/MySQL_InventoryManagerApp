using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace MySQL_InventoryManagerApp
{
    internal class DAO
    {

        string connectionString = "datasource=172.24.108.38;port=8889;username=root;password=root;database=milestone";

        public List<Product> showProducts()
        {
            List<Product> returnThese = new List<Product>();

            // connect to mySQL server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // fetch items from database
            MySqlCommand command = new MySqlCommand("SELECT id, product_name, description, price, wholesale, quantity, Categories_id FROM Products;", connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product p = new Product
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDouble(3),
                        Wholesale = reader.GetDouble(4),
                        Quantity = reader.GetInt32(5),
                        Category_Id = reader.GetInt32(6)
                    };
                    returnThese.Add(p);
                }
            }
            connection.Close();
            return returnThese;

        }

        public List<Category> showCategories()
        {
            List<Category> returnThese = new List<Category>();

            // connect to mySQL server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // fetch items from database
            MySqlCommand command = new MySqlCommand("SELECT ID, category_name, category_description FROM Categories;", connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Category c = new Category
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                    };
                    returnThese.Add(c);
                }
            }
            connection.Close();
            return returnThese;
        }

        public List<Product> getProductsUsingJoin(int categoryId)
        {
            List<Product> returnThese = new List<Product>();

            // connect to mySQL server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // fetch items from database
            MySqlCommand command = new MySqlCommand();
            command.CommandText = "SELECT Products.id, product_name, description, price, wholesale, quantity, Categories_id FROM PRODUCTS JOIN Categories ON products.Categories_id = Categories.id WHERE Categories_id = @categoryId;";
            command.Parameters.AddWithValue("@categoryId", categoryId);
            command.Connection = connection;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product p = new Product
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDouble(3),
                        Wholesale = reader.GetDouble(4),
                        Quantity = reader.GetInt32(5),
                        Category_Id = reader.GetInt32(6)
                    };
                    returnThese.Add(p);
                }
            }
            connection.Close();
            return returnThese;
        }

        public List<Category> searchCategories(string searchTerm)
        {
            // start with empty list
            List<Category> returnThese = new List<Category>();

            // connect to mysql
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            String searchWildPhrase = "%" + searchTerm + "%";

            MySqlCommand command = new MySqlCommand("SELECT ID, category_name, category_description FROM Categories WHERE " +
                "category_name LIKE @search", connection);
            command.Parameters.AddWithValue("@search", searchWildPhrase);
            command.Connection = connection;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Category c = new Category
                    {
                        ID = reader.GetInt32(0),
                        Name= reader.GetString(1),
                        Description= reader.GetString(2)
                    };
                    returnThese.Add(c);
                }
            }
            connection.Close();
            return returnThese;
        }
        public List<Product> searchProducts(string searchTerm)
        {
            // start with empty list
            List<Product> returnThese = new List<Product>();

            // connect to mysql
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            String searchWildPhrase = "%" + searchTerm + "%";

            MySqlCommand command = new MySqlCommand("SELECT Products.id, product_name, description, price, wholesale, quantity, Categories_id FROM Products WHERE " +
                "product_name LIKE @search", connection);
            command.Parameters.AddWithValue("@search", searchWildPhrase);
            command.Connection = connection;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product p = new Product
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDouble(3),
                        Wholesale = reader.GetDouble(4),
                        Quantity = reader.GetInt32(5),
                        Category_Id = reader.GetInt32(6)
                    };
                    returnThese.Add(p);
                }
            }
            connection.Close();
            return returnThese;
        }

        public int addNewCategory(Category c)
        {
            // connect to mySQL server 
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // define the sql statement to add album
            MySqlCommand command = new MySqlCommand("INSERT INTO `Categories`(`category_name`, `category_description`) VALUES (@category_name, @category_description)", connection);

            command.Parameters.AddWithValue("@category_name", c.Name);
            command.Parameters.AddWithValue("@category_description", c.Description);

            int newRows = command.ExecuteNonQuery();
            connection.Close();

            return newRows;
        }
        public int addNewProduct(Product p)
        {
            // connect to mySQL server 
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // define the sql statement to add album
            MySqlCommand command = new MySqlCommand("INSERT INTO `Products`(`product_name`, `description`, `quantity`, `price`, `wholesale`, `Categories_id`) VALUES (@product_name, @description, @quantity, @price, @wholesale, @Categories_id)", connection);

            command.Parameters.AddWithValue("@product_name", p.Name);
            command.Parameters.AddWithValue("@description", p.Description);
            command.Parameters.AddWithValue("@quantity", p.Quantity);
            command.Parameters.AddWithValue("@price", p.Price);
            command.Parameters.AddWithValue("@wholesale", p.Wholesale);
            command.Parameters.AddWithValue("@Categories_id", p.Category_Id);

            int newRows = command.ExecuteNonQuery();
            connection.Close();

            return newRows;
        }
        public List<string> getCategories()
        {
            List<string> list = new List<string>();

            // connect to mySQL server 
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // define the sql statement to add album
            MySqlCommand command = new MySqlCommand("SELECT category_name from Categories;", connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }
            connection.Close();
            return list;
        }
        public int deleteCategory(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = new MySqlCommand("DELETE FROM `Categories` WHERE id = @ID;", connection);

            command.Parameters.AddWithValue("@ID", id);

            int result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }
        public int deleteProduct(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            MySqlCommand command = new MySqlCommand("DELETE FROM `Products` WHERE id = @ID;", connection);

            command.Parameters.AddWithValue("@ID", id);

            int result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }
    }
}
