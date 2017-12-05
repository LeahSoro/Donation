using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimchaDonors.Web.Models;
using SimchaDonors.Data;

namespace SimchaDonors.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SimchaViewModel viewmodel = new Models.SimchaViewModel();
            SimchaManager manager = new SimchaManager(Properties.Settings.Default.constr);
            viewmodel.simchas = manager.GetAllSimchas();
            viewmodel.ContributorCount = manager.AllContributorsCount();
            return View(viewmodel);
        }
        public ActionResult Contributors()
        {
            ContributorViewModel viewmodel = new ContributorViewModel();
            SimchaManager manager = new SimchaManager(Properties.Settings.Default.constr);
            viewmodel.contributors = manager.GetAllContributors();
           
            return View(viewmodel);

        }
        public ActionResult Filter(string Search)
        {
            ContributorViewModel viewmodel = new ContributorViewModel();
            SimchaManager manager = new SimchaManager(Properties.Settings.Default.constr);
            viewmodel.contributors = manager.GetAllContributors(Search);

            return View(viewmodel);
        }
        public ActionResult NewContributor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewContributor(Contributor contributor)
        {
            SimchaManager manager = new SimchaManager(Properties.Settings.Default.constr);
            manager.AddContributor(contributor);
            return Redirect("/");
        }
        public ActionResult Deposit(int id)
        {
            ContributionViewModel viewmodel = new ContributionViewModel();
            viewmodel.id = id;
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult NewDeposit(Contribution c)
        {
            SimchaManager manager = new SimchaManager(Properties.Settings.Default.constr);
            manager.AddContribution(c);
            return Redirect("/home/contributors");
            
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult ShowHistory(int id)
        {
            SimchaManager manager = new SimchaManager(Properties.Settings.Default.constr);
            HistoryViewModel viewmodel = new HistoryViewModel();
            viewmodel.contributor = manager.GetContributor(id);
            viewmodel.contributions = manager.GetContributionsForid(id);
            return View(viewmodel);
        }
        public ActionResult NewSimcha()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewSimcha(Simcha s)
        {
            SimchaManager manager = new SimchaManager(Properties.Settings.Default.constr);
            manager.AddSimcha(s);
            return Redirect("/");
        }
        public ActionResult Contributions( int simchaid)
        {
            SimchaManager manager = new SimchaManager(Properties.Settings.Default.constr);
            ContributorViewModel viewmodel = new ContributorViewModel();
            viewmodel.contributors = manager.GetAllContributors();
            viewmodel.simchaid = simchaid;
            viewmodel.count = manager.AllContributorsCount();
            viewmodel.Donated = manager.GetAllSimchaDonations(simchaid);
            
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult ContributeToSimcha(List<DonateToSimcha> simcha)
        {
            SimchaManager manager = new SimchaManager(Properties.Settings.Default.constr);
            List<DonateToSimcha> result = new List<DonateToSimcha>();
            ContributorViewModel viewmodel = new ContributorViewModel();
            viewmodel.Donated = result;
            foreach (DonateToSimcha d in simcha)
            {
                if(d.Include==true)
                {
                    result.Add(d);
                }
            }
            foreach (DonateToSimcha s in result)
            {
                manager.DonateToSimcha(s);
            }
            return Redirect("/");
        }
       
    }
}