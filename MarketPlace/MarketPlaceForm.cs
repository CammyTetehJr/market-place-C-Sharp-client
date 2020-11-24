using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections;



namespace MarketPlace
{
    public partial class MarketPlaceForm : Form
    {
        static HttpClient httpClient;
        static List<Product> products;
      

        public MarketPlaceForm()
        {
            InitializeComponent();
            httpClient = new HttpClient();
            products = new List<Product>();
            

            httpClient.BaseAddress = new Uri("http://localhost:8080/marketplace/rest/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }




        private void printMessage(string message)

        {
            lbProducts.Items.Add(Environment.NewLine);
            lbProducts.Items.Add(message);

        }
        private void printWelcomeMessage()
        {
            printMessage("Welcome to the marketplace for used phones");

        }

        private void ShowProducts()
        {
            
            foreach (Product p in products)
            {
                lbProducts.Items.Add("id: " + " " + p.getId() + ", Name: " + p.getName() + ", Description: " + p.getDescription() + ", Price: " +
                        p.getPrice() + ", Offer date: " + p.getOfferDate() + " , Seller: "+ p.getSeller().getName());
            }
        }



        private void BtnBuy_Click(object sender, EventArgs e)
        {
            if (lbProducts.SelectedIndex == -1)
            {
                MessageBox.Show("No Selected Item to buy");
            }
            else
            {
                Delete();
                GetAllproducts();
            }
            
        }

        public string GetHello(string url)
        {

            var response = httpClient.GetStringAsync(new Uri(url)).Result;
            lbProducts.Items.Add(response);

            return response;

        }

        

        public async void GetProduct(string path)
        {

            httpClient.BaseAddress = new Uri("http://localhost:8080/marketplace/rest/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // GET Method
            HttpResponseMessage response = await httpClient.GetAsync("products/" + path);
            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<List<Product>>(res);
               
                products = content;

                lbProducts.Items.Clear();

                foreach (Product product in products)
                {
                    lbProducts.Items.Add("id: " + " " + product.getId() + ", Name: " + product.getName() + ", Description: " + product.getDescription() + ", Price: " +
                    product.getPrice() + ", Offer date: " + product.getOfferDate());
                }
                
            }
            else
            {
                lbProducts.Items.Add("Error getting resource");
            }

        }

        public async void GetAllproducts()
        {
            // GET Method
            HttpResponseMessage response = await httpClient.GetAsync("products/all");
            
            if (response.IsSuccessStatusCode)
            {
                string res = response.Content.ReadAsStringAsync().Result;

                var content = JsonConvert.DeserializeObject<List<Product>>(res);                            
                products = content;
                lbProducts.Items.Clear();

                foreach (Product product in products)
                {
                    if(products.Count != 0)
                    {
                        lbProducts.Items.Add("id: " + " " + product.getId() + ", Name: " + product.getName() + ", Description: " + product.getDescription() + ", Price: " +
                        product.getPrice() + ", Offer date: " + product.getOfferDate() + " " + "seller: " + product.getSeller().getName());
                    }
                }
                
            }
            else
            {
                lbProducts.Items.Add("Error getting resource");
            }
        }

        private async void Delete()
        {
            int index = lbProducts.SelectedIndex;
            Product selected = GetProduct(index);
            if (selected != null)
            {
                string productId = selected.getId().ToString();

                var response = await httpClient.DeleteAsync("http://localhost:8080/marketplace/rest/products/" + productId);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("yes!");
                    lbProducts.Items.Clear();
                    products.RemoveAt(index);
                    ShowProducts();
                }
                else { MessageBox.Show("null!"); }

            }
        }

        private bool ValidateDelete()
        {
            if (tbCustomerName.Text == string.Empty || tbCustomerEmail.Text == string.Empty || tbProductId.Text == string.Empty)

            {
                return false;
            }
            return true;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ValidateDelete())
            {
                VerifyDelete();
            }
            else
            {
                MessageBox.Show("No product specified to be deleted");
            }
            
        }

        private Product GetProduct(int index)
        {
            
            for (int i = 0; i < products.Count; i++)
            {
              if(i == index)
                {
                    return products[i];
                }
            }
            return null;
        }

        private bool ValidateSell()
        {
            if(tbPhoneBrand.Text == string.Empty || tbDescription.Text == string.Empty|| tbPrice.Text == string.Empty
                || tbSellerName.Text == string.Empty || tbSellerEmail.Text == string.Empty)
            {
                return false;
            }
            return true;
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            if(ValidateSell())
            {
                string name = tbPhoneBrand.Text;
                string description = tbDescription.Text;
                double price = Convert.ToDouble(tbPrice.Text);
                string uname = tbSellerName.Text;
                string uemail = tbSellerEmail.Text;
                Random r = new Random();
                int id = r.Next(200, 210);

                var content = new FormUrlEncodedContent(new[]
            {
             new KeyValuePair<string, string>("name",name),
             new KeyValuePair<string, string>("description",description),
             new KeyValuePair<string, string>("price",price.ToString()),
             new KeyValuePair<string, string>("uname",uname),
             new KeyValuePair<string, string>("uemail",uemail),
             new KeyValuePair<string, string>("id",id.ToString()),

        });

                var result = httpClient.PostAsync("http://localhost:8080/marketplace/rest/products", content).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    MessageBox.Show("Success!");
                    GetAllproducts();
                }
                else
                {
                    MessageBox.Show("Error creating product");
                }
            }

            else
            {
                MessageBox.Show("Fill Product details for sale");
            }
            


        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            GetAllproducts();
        }

        private async void VerifyDelete()
        {
            string customerName = tbCustomerName.Text;
            string customerEmail = tbCustomerEmail.Text;
            int productId = Convert.ToInt32(tbProductId.Text);
            
            Product selected = GetProductById(productId);
            bool verify = VerifyUserForDelete(selected, customerName, customerEmail);
            
            if(selected != null && customerName != string.Empty && customerEmail != string.Empty &&tbProductId != null)
            {
                if (verify)
                {

                    var response = await httpClient.DeleteAsync("http://localhost:8080/marketplace/rest/products/" + productId);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("yes!");
                        lbProducts.Items.Clear();
                        GetAllproducts();

                    }
                   

                }
                else { MessageBox.Show("You do not have permission to delete product"); }
            }

            else { MessageBox.Show("Error Deleting Product"); }
            


      
        }

        private bool VerifyUserForDelete(Product product, String CustomerName, String CustomerEmail)
        {
            User seller = product.getSeller();
            
            if(seller.getName() == CustomerName && seller.getEmail() == CustomerEmail )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool VerifyUserForUpdate(Product product, String CustomerName, String CustomerEmail)
        {
            User seller = product.getSeller();

            if (seller.getName() == CustomerName && seller.getEmail() == CustomerEmail)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Product GetProductById(int id)
        {
            foreach (Product p in products)
            {
                if(p.getId() == id)
                {
                    return p;
                }
            }
            return null;
        }

       private bool ValidateUpdate()
        {
            if (tbPhoneBrandUpdate.Text == string.Empty || tbDescriptionUpdate.Text == string.Empty || tbPriceUpdate.Text == string.Empty
                || tbSellerNameUpdate.Text == string.Empty || tbSellerEmailUpdate.Text == string.Empty || tbProductIdUpdate.Text == string.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {

            if (ValidateUpdate())
            {
                string name = tbPhoneBrandUpdate.Text;
                string description = tbDescriptionUpdate.Text;
                double price = Convert.ToDouble(tbPriceUpdate.Text);
                string uname = tbSellerNameUpdate.Text;
                string uemail = tbSellerEmailUpdate.Text;
                int id = Convert.ToInt32(tbProductIdUpdate.Text);

                Product p = GetProductById(id);
                bool verify = VerifyUserForUpdate(p, uname, uemail);

                if (verify)
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("name",name),
                         new KeyValuePair<string, string>("description",description),
                         new KeyValuePair<string, string>("price",price.ToString()),
                         new KeyValuePair<string, string>("uname",uname),
                         new KeyValuePair<string, string>("uemail",uemail),
                         new KeyValuePair<string, string>("id",id.ToString()),

                    });

                    var result = httpClient.PutAsync("http://localhost:8080/marketplace/rest/products/update", content).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        MessageBox.Show("Success!");
                        GetAllproducts();
                    }
                    else
                    {
                        MessageBox.Show("Error creating product");
                    }
                }
                else { MessageBox.Show("you do not have permission to update this product!"); }

            }

            else
            {
                MessageBox.Show("No Product details to update!");
            }
        }
            
    }
}


