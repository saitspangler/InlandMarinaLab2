using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlandMarinaData
{
    /// <summary>
    /// methods for working with Dock table in the InlandMarina database
    /// </summary>
    public static class DockManager
    {
        /// <summary>
        /// retrieve all docks
        /// </summary>
        /// <param name="db">context object</param>
        /// <returns>list of docks or null if none</returns>
        public static List<Dock> GetDocks(InlandMarinaContext db)
        {
            List<Dock> docks = null;
            try
            {
                docks = db.Docks.Include(d => d.Slips)
                                 .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException("There was an error retrieving the docks from the database.");
            }
            return docks;
        }

        /// <summary>
        /// retrieves all slips at a given dock, ordered by ID
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="dockId">ID of the dock</param>
        /// <returns>list of slips at that dock or null</returns>
        public static List<Slip> GetSlipsByDock(InlandMarinaContext db, string dockId)
        {
            List<Slip> slips = null;
            try
            {
                int dockIdInt = int.Parse(dockId);
                slips = db.Slips.Where(s => s.DockID == dockIdInt)
                                .Include(s => s.Dock)
                                .Include(s => s.Leases)
                                .OrderBy(s => s.ID)
                                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException("There was an error retrieving the slips from the database.");
            }
            return slips;
        }
    }
}