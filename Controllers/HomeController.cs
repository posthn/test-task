using Microsoft.AspNetCore.Mvc;
using Project01.Models;

namespace Project01.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;

        public HomeController(IRepository repository) => _repository = repository;

        public ViewResult Create() {
            ViewBag.Teams = _repository.Teams;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Person person) {
            if (!ModelState.IsValid) {
                ViewBag.Teams = _repository.Teams;
                return View(person);
            }

            if (_repository.People.Select(p => p.Id).Contains(person.Id))
                _repository.UpdatePerson(person);
            else
                _repository.AddPerson(person);

            return RedirectToAction(nameof(List));
        }

        public ViewResult Edit(int personId) {
            ViewBag.Teams = _repository.Teams;
            return View(nameof(Create), _repository.People.FirstOrDefault(p => p.Id == personId));
        }

        public ViewResult List() => View(_repository.People);
    }
}