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
        /// retrieve  all leases
        /// </summary>
        /// <param name="db">context object</param>
        /// <returns>list of leases or null if none</returns>
        public static List<Lease> GetLeases(InlandMarinaContext db) // dependency injection
        {
            List<Lease> leases = null;
            //using(InlandMarinaContext db = new InlandMarinaContext())
            //{
            leases = db.Leases.Include(l => l.Customer).Include(l => l.Slip).OrderBy(l => l.SlipID).ToList();
            //}
            return leases;
        }

        /// <summary>
        /// retrieve lease customers
        /// </summary>
        /// <param name="db">context object</param>
        /// <returns>list of lease customers</returns>
        public static List<Customer> GetCustomers(InlandMarinaContext db)
        {
            List<Customer> customers = db.Customers.OrderBy(c => c.LastName).ToList();
            return customers;
        }

        /// <summary>
        /// retrieve docks
        /// </summary>
        /// <param name="db">context object</param>
        /// <returns>list of docks</returns>
        public static List<Dock> GetDocks(InlandMarinaContext db)
        {
            List<Dock> docks = db.Docks.OrderBy(d => d.Name).ToList();
            return docks;
        }

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
        /// retrieves leases of given dock, ordered by name
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="dockId">ID of the dock</param>
        /// <returns>list of leases at that dock or null</returns>
        public static List<Lease> GetLeasesByDock(InlandMarinaContext db, string dockId)
        {
            List<Lease> leases = db.Leases.Where(l => Convert.ToString(l.Slip.DockID) == dockId).
                Include(l => l.Customer).Include(l => l.Slip).OrderBy(l => l.SlipID).ToList();
            return leases;
        }


        /// <summary>
        /// retrieves leases slips, ordered by name
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="slipId">ID of the slip</param>
        /// <returns>list of slips that are leased or null</returns>
        public static List<Lease> GetLeasesBySlip(InlandMarinaContext db, string slipId)
        {
            List<Lease> leases = db.Leases.Where(l => Convert.ToString(l.SlipID) == slipId).
                Include(l => l.Customer).Include(l => l.Slip).OrderBy(l => l.SlipID).ToList();
            return leases;
        }

        public static List<Slip> GetSlipsFromCustomer(InlandMarinaContext db, int customerId)
        {
            List<Slip> slips = db.Leases.Where(l => l.CustomerID == customerId).Select(l => l.Slip).ToList();
            return slips;
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
        /// update lease with given id using new lease data
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="id">id for the lease to update</param>
        /// <param name="newLease">new lease data</param>
        public static void UpdateLease(InlandMarinaContext db, int id, Lease newLease)
        {
            Lease lease = db.Leases.Find(id);
            if (lease != null)
            {
                // copy over new lease data
                lease.SlipID = newLease.SlipID;
                lease.CustomerID = newLease.CustomerID;
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
