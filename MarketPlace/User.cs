using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace
{
    public class User
    {
       
        private static int unique = 0;
        [JsonPropertyAttribute]
        private int id;
        [JsonPropertyAttribute]
        private String name;
        [JsonPropertyAttribute]
        private String email;


            public int getId()
            {
                return id;
            }

            public int setId(int id)
            {
            return this.id = id;
            }



            public String getName()
            {
                return name;
            }

            public void setName(String name)
            {
                this.name = name;
            }


            public String getEmail()
            {
                return email;
            }

            public void setEmail(String email)
            {
                this.email = email;
            }



            public User(String name, String email)
            {

                this.id = setId(unique++);
                this.name = name;
                this.email = email;

            }
        
        }

    }

