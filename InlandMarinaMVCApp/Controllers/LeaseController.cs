using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InlandMarinaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace InlandMarinaMVCApp.Controllers
{
    public class LeaseController : Controller
    {
        private InlandMarinaContext _context { get; set; } // auto-implemented property

        // context get  injected to the constructor
        public LeaseController(InlandMarinaContext context)
        {
            _context = context;
        }


        // GET: LeaseController
        // list of leases when authorized
        [Authorize]
        public ActionResult Index()
        {
            // Retrieve ID of current user from session variable
            if (HttpContext.Session.TryGetValue("CurrentCustomer", out byte[] currentUserId))
            {
                // Retrieve Customer object from database
                int customerId = int.Parse(Encoding.UTF8.GetString(currentUserId));
                Customer customer = _context.Customers.Find(customerId);

                // Retrieve first name of current user
                string firstName = customer.FirstName;

                // Pass first name to view
                HttpContext.Session.SetString("FirstName", firstName);
            }
            List<Lease> leases = null;
            try
            {
                leases = LeaseManager.GetLeases(_context);
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
            }

            return View(leases);
        }

        // filter slips by docks when authorized
        [Authorize]
        public ActionResult FilteredList(string id = "1")
        {
            List<Slip> slips = null;
            try
            {
                // get all docks from slips
                List<Dock> docks = DockManager.GetDocks(_context);

                // select dock with id
                var list = new SelectList(docks, "ID", "Name", id);
                ViewBag.Docks = list;

                slips = DockManager.GetSlipsByDock(_context, id)
                    .Where(s => s.Leases.Count == 0)
                    .ToList();
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
            }
            return View("FilteredList", slips);
        }

        [HttpPost]
        public ActionResult FilteredList(IFormCollection form)
        {
            string id = form["id"];
            return RedirectToAction("FilteredList", new { id = id });
        }

        public IActionResult MySlips()
        {
            //get list of slips the current user has leased
            List<Slip> slips = new List<Slip>();
            string customerID = HttpContext.Session.GetString("CurrentCustomer");
            var id = int.Parse(customerID);
            using (InlandMarinaContext db = new InlandMarinaContext())
            {
                //get list of leases for the current customer
                List<Lease> leases = LeaseManager.GetLeasesByCustomer(db, id);
                //get list of slips for the leases
                return View(leases);
            }

        }

        // GET: LeaseController/Details/5 when authorized
        [Authorize]
        public ActionResult Details(int id)
        {
            try
            {
                Lease lease = LeaseManager.GetLeaseById(_context, id);
                return View(lease);
            }
            catch
            {
                TempData["Message"] = "Database connection error. Try again later.";
                TempData["IsError"] = true;
                return View(null);
            }
        }

        // GET: LeaseController/Create
        //when authorized
        [Authorize]
        public ActionResult Create()
        {
            Lease newLease = new Lease(); // empty Lease object
            return View(newLease);
        }

        // POST: LeaseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int[] slipId) // data collected from the form
        {
            try
            {
                // Retrieve ID of current user from session variable
                if (HttpContext.Session.TryGetValue("CurrentCustomer", out byte[] currentUserId))
                {
                    // Create new Lease object for each selected SlipID
                    foreach (var id in slipId)
                    {
                        var newLease = new Lease
                        {
                            CustomerID = int.Parse(Encoding.UTF8.GetString(currentUserId)),
                            SlipID = id
                        };
                        LeaseManager.AddLease(_context, newLease);
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle case where CurrentCustomer session variable is not found
                    ModelState.AddModelError("", "Current user not found");
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Display exception message
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }


        // GET: LeaseController/Delete/5 when authorized
        [Authorize]
        public ActionResult Delete(int id)
        {
            Lease lease = LeaseManager.GetLeaseById(_context, id);
            return View(lease);
        }

        // POST: LeaseController/Delete/5 when authorized
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Lease Lease)
        {
            try
            {
                LeaseManager.DeleteLease(_context, id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
