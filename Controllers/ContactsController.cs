using Microsoft.AspNetCore.Mvc;
using ContactManagerB.Data;
using ContactManagerB.Models;
using System.Linq;

namespace ContactManagerB.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var contacts = _context.Contacts.ToList();
            return View(contacts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        public IActionResult Edit(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Update(contact);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            _context.SaveChanges(); // 确保保存更改

            return Ok(); // 返回200状态码表示成功
        }

        [HttpGet]
        public IActionResult Search(string name)
        {
            // 查找联系人，进行模糊查询
            var contacts = _context.Contacts
                .Where(c => c.Name.Contains(name)) // 使用 Contains 进行模糊匹配
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Address,
                    c.Phone
                }).ToList();

            return Json(contacts); // 返回 JSON 格式的联系人列表
        }
    }
}
