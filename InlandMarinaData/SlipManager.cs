using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlandMarinaData
{
    /// <summary>
    /// methods for working with Slip table in the InlandMarina database
    /// </summary>
    public static class SlipManager
    {

        /// <summary>
        /// retrieve  all slips
        /// </summary>
        /// <param name="db">context object</param>
        /// <returns>list of slips or null if none</returns>
        public static List<Slip> GetSlips(InlandMarinaContext db)
        {
            List<Slip> slips = null;
            try
            {
                slips = db.Slips.Include(s => s.Dock)
                                .Include(s => s.Leases)
                                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException("There was an error retrieving the slips from the database.");
            }
            return slips;
        }

        /// <summary>
        /// retrieves slips of given dock, ordered by name
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
                    .Include(s => s.ID).OrderBy(s => s.ID).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException("There was an error retrieving the slips from the database.");
            }
            return slips;
        }

        /// <summary>
        /// get all docks from slips
        /// </summary>
        /// <param name="db"></param>
        /// <returns>list of docks from slips</returns>
        public static List<Dock> GetAllDocksFromSlips(InlandMarinaContext db)
        {
            List<Dock> docks = null;
            try
            {
                docks = db.Slips.Select(s => s.Dock).Distinct().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException("There was an error retrieving the docks from the database.");
            }
            return docks;
        }

        /// <summary>
        /// get slip with given ID
        /// </summary>
        /// <param name="db">context object</param>
        /// <param name="id">ID of the  slip to find</param>
        /// <returns>slip or null if not found</returns>
        public static Slip GetSlipById(InlandMarinaContext db, int id)
        {
            Slip slip = db.Slips.Find(id);
            return slip;
        }
    }
}
