using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlandMarinaData
{
    /// <summary>
    /// methods for working with Lease table in the InlandMarina database
    /// </summary>
    public static class LeaseManager
    {

        /// <summary>
        /// retrieves leases of given customer, ordered by name
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>list of movies or null</returns>
        public static List<Lease> GetLeasesByCustomer(InlandMarinaContext db, int customerId)
        {
            List<Lease> leases = db.Leases.Where(l => l.CustomerID == customerId).
                Include(l => l.Customer).Include(l => l.Slip).ThenInclude(slip => slip.Dock).OrderBy(l => l.SlipID).ToList();
            return leases;
        }

        /// <summary>
        /// get lease with given ID
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="id">ID of the  lease to find</param>
        /// <returns>lease or null if not found</returns>
        public static Lease GetLeaseById(InlandMarinaContext db, int id)
        {
            Lease lease = db.Leases.Find(id);
            return lease;
        }


        /// <summary>
        /// add another lease to the table
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="newlease">new lease to add</param>
        public static void AddLease(InlandMarinaContext db, Lease newlease)
        {
            if (newlease != null)
            {
                db.Leases.Add(newlease);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// delete lease with given id
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="id">ID of the lease to delete</param>
        public static void DeleteLease(InlandMarinaContext db, int id)
        {
            Lease lease = db.Leases.Find(id);
            if (lease != null)
            {
                db.Leases.Remove(lease);
                db.SaveChanges();
            }
        }
    }
}
