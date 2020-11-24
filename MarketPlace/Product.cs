using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace
{
    public class Product
    {
        [JsonPropertyAttribute]
        public int id;
        [JsonPropertyAttribute]
        private String name;
        [JsonPropertyAttribute]
        private String description;
        [JsonPropertyAttribute]
        private double price;
        [JsonPropertyAttribute]
        private DateTime offerDate;
        [JsonPropertyAttribute]
        private Nullable<DateTime> sellDate;
        [JsonPropertyAttribute]
        private Boolean available;
        [JsonPropertyAttribute]
        private User seller;
        [JsonPropertyAttribute]
        private User buyer;

        public Product(int id,String name, String description, double price, User seller)
        {

            this.id = setId(id);
            this.name = name;
            this.description = description;
            this.price = price;
            this.offerDate = DateTime.Now;
            this.sellDate  = null;
            this.available = true;
            this.buyer = null;
            this.seller = setSeller(seller);

        }

        

        public int setId(int id)
        {
            return this.id = id;
        }


        public int getId()
        {
            return id;
        }

       

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }


        public String getDescription()
        {
            return description;
        }

        public void setDescription(String description)
        {
            this.description = description;
        }


        public double getPrice()
        {
            return price;
        }

        public void setPrice(double price)
        {
            this.price = price;
        }


        public DateTime getOfferDate()
        {
            return offerDate;
        }

        public void setOfferDate(DateTime offerDate)
        {
            this.offerDate = offerDate;
        }


        public Nullable<DateTime> getSellDate()
        {
            return sellDate;
        }

        public void setSellDate(Nullable<DateTime> sellDate)
        {
            this.sellDate = sellDate;
        }


        public Boolean getAvailable()
        {
            return available;
        }

        public void setAvailable(Boolean available)
        {
            this.available = available;
        }

        public User getSeller()
        {
            return seller;
        }

        public User setSeller(User s)
        {
            this.seller = s;
            return seller;
        }


    }
}
