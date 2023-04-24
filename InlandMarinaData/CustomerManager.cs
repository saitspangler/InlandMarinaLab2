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
    }
}
