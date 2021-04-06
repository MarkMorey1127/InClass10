using Microsoft.EntityFrameworkCore;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenKattis10
{

    
    class Product
    {
        public int ID { get; set; }
        public string Product_Name { get; set; }

        public List<Purchase> PurchasedOrders { get; set; }

        
    }


    class Order
    {
        public int ID { get; set; }
        public DateTime Order_Date { get; set; }

        public List<Purchase> PurchasedProducts { get; set; }

    }

    class Purchase //need associative entity to bring the 2 tables together 
    {
        public int Id { get; set; }
        public Order PurchasedOrder {get; set;}
        public Product PurchasedProduct { get; set; }

        public int QuantityPurchased { get; set; }


    }

    class OrderContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Purchase> Purchases { get; set; }


        string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ProuctsNOrders;Trusted_Connection=True;MultipleActiveResultSets=true";

        //creates connection link to database and collections for tables
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }



    class Program
    {
        static void Main(string[] args) {

            using(OrderContext context = new OrderContext())
            {
                context.Database.EnsureCreated();

                //creating variables in a table 
                Order Order1 = new Order { ID = 1, Order_Date = DateTime.Now };
                Product Product1 = new Product { ID = 1, Product_Name = "Fork" };
                Product Product2 = new Product { ID = 2, Product_Name = "Spoon" };
                Product Product3 = new Product { ID = 3, Product_Name = "Basketball" };
                Order Order2 = new Order {ID = 2, Order_Date = DateTime.Now };

                //create the purchases
                Purchase FirstPurchase = new Purchase
                {
                    Id = 1,
                    PurchasedOrder = Order1,
                    PurchasedProduct = Product1,
                    QuantityPurchased = 5

                };

                ////dont use
                ////Purchase SecondPurchase = new Purchase
                ////{
                ////    Id = 1,
                ////    PurchasedOrder = Order1,
                ////    PurchasedProduct = Product2,
                ////    QuantityPurchased = 7

                ////};

                //create the purchases
                Purchase ThirdPurchase = new Purchase
                {
                    Id = 2,
                    PurchasedOrder = Order2,
                    PurchasedProduct = Product3,
                    QuantityPurchased = 10

                };

                //Add in order objects above
                context.Orders.Add(Order1);
                context.Orders.Add(Order2);

                //add in product objects above
                context.Products.Add(Product1);
                context.Products.Add(Product2);
                context.Products.Add(Product3);

                //add in the purchases
                context.Purchases.Add(FirstPurchase);
                ////context.Purchases.Add(SecondPurchase); dont use
                context.Purchases.Add(ThirdPurchase);

                //Save the changes
                context.SaveChanges();

                //list all orders where an item is sold 

                IQueryable<Purchase> queryOrders = context.Purchases
                                                .Include(p => p.PurchasedOrder)
                                                .Where(p => p.QuantityPurchased > 0);

                foreach( var p in queryOrders)
                {
                    Console.WriteLine("{0} {1}", p.PurchasedOrder, p.QuantityPurchased);
                }

                //List the max order for a product

                int maxQuant = context.Purchases.Max(p => p.QuantityPurchased);
                IQueryable<Purchase> maxOrderAmt = context.Purchases
                                                   .Where(p => p.QuantityPurchased == maxQuant);



            }


            

    }




    }
    
 }
