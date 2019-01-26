using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCAppConsumingAPI.Models;
using System.Net.Http;



namespace MVCAppConsumingAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<EmpModel> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57510/");
                //HTTP GET
                var responseTask = client.GetAsync("api/employee");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EmpModel>>();
                    readTask.Wait();

                    students = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    students = Enumerable.Empty<EmpModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(students);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmpModel emp)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57510/");
                //HTTP GET
                var postTask = client.PostAsJsonAsync<EmpModel>("api/employee", emp);
                postTask.Wait();



                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(emp);
        }


        public ActionResult Edit(int id)
        {
            EmpModel emp = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57510/api/");
                //HTTP GET
                var responseTask = client.GetAsync("employee?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<EmpModel>();
                    readTask.Wait();
                    // var response = client.PutAsJsonAsync("api/person",emp).Result;

                    emp = readTask.Result;
                }
            }
            return View(emp);
        }

        [HttpPost]
        public ActionResult Edit(EmpModel emp)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57510/api/");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<EmpModel>("employee", emp);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(emp);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
