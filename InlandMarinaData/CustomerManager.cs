using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlandMarinaData
{
    /// <summary>
    /// methods for working with Customer table in the InlandMarina database
    /// </summary>
    public class CustomerManager
    {
        ///<summary>
        ///Create Customer from Register Page
        /// </summary>
        ///<param name="Customer"></param>
        ///<returns> a new customer object</returns>
        public static Customer AddCustomer(Customer customer)
        {
            using (InlandMarinaContext db = new InlandMarinaContext())
            {
                db.Customers.Add(customer);
                db.SaveChanges();
            }
            return customer;
        }

        /// <summary>
        /// Customer is authenticated based on credentials and a customer returned if exists or null if not.
        /// </summary>
        /// <param name="username">Username as string</param>
        /// <param name="password">Password as string</param>
        /// <returns>A customer object or null.</returns>
        public static Customer Authenticate(string username, string password)
        {
            Customer customer = null;
            using (InlandMarinaContext db = new InlandMarinaContext())
            {
                customer = db.Customers.SingleOrDefault(cust => cust.Username == username
                                                    && cust.Password == password);
            }

            return customer; //this will either be null or an object
        }

        /// <summary>
        /// get customer with given ID
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="id">ID of the  customer to find</param>
        /// <returns>lease or null if not found</returns>
        public static Customer GetCustomerById(InlandMarinaContext db, int id)
        {
            Customer customer = db.Customers.Find(id);
            return customer;
        }
    }
}
