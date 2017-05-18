using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BiraFit.Models;
using BiraFit.Controllers.Helpers;
using System.Net;
using System.Data.Entity;
using System.IO;

namespace BiraFit.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess
                    ? "Ihr Kennwort wurde geändert."
                    : message == ManageMessageId.SetPasswordSuccess
                        ? "Ihr Kennwort wurde festgelegt."
                        : message == ManageMessageId.SetTwoFactorSuccess
                            ? "Ihr Anbieter für zweistufige Authentifizierung wurde festgelegt."
                            : message == ManageMessageId.Error
                                ? "Fehler"
                                : message == ManageMessageId.AddPhoneSuccess
                                    ? "Ihre Telefonnummer wurde hinzugefügt."
                                    : message == ManageMessageId.RemovePhoneSuccess
                                        ? "Ihre Telefonnummer wurde entfernt."
                                        : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        // GET: /Manage/Delete
        public ActionResult Delete()
        {
            return View();
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result =
                await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new {Message = ManageMessageId.ChangePasswordSuccess});
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new {Message = ManageMessageId.SetPasswordSuccess});
                }
                AddErrors(result);
            }

            return View(model);
        }

        // GET: /Manage/Edit/34
        public ActionResult Edit()
        {
            string username = User.Identity.GetUserId();
            var user = Context.Users.Single(s => s.Id == username);
            var beschreibung = "";
            if (!IsSportler())
            {
                ViewBag.Type = "PersonalTrainer";
                var personalTrainer = Context.PersonalTrainer.Single(s => s.User_Id == user.Id);
                beschreibung = personalTrainer.Beschreibung;
            }
            return View(new EditViewModel
            {
                Vorname = user.Vorname,
                Name = user.Name,
                Email = user.Email,
                Adresse = user.Adresse,
                ProfilBild = user.ProfilBild,
                Beschreibung = beschreibung
            });
        }

        // POST: /Manage/Edit/34
        [HttpPost]
        public ActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.GetUserId();
                ApplicationUser user = Context.Users.Single(s => s.Id == username);
                PersonalTrainer personalTrainer = Context.PersonalTrainer.Single(s => s.User_Id == username);
                TryUpdateModel(user);
                TryUpdateModel(personalTrainer);
                Context.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }

            return View(model);
        }

        // GET: /Manage/UploadFile
        public ActionResult UploadFile()
        {
            return View();
        }

        //POST /Manage/UploadFile
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                string extension = Path.GetExtension(file.FileName);
                if (file.ContentLength > 0 && extension == ".jpg" || extension == ".png")
                {
                    string serverPath = "~/Resources/AccountPicture/";
                    string userId = User.Identity.GetUserId();
                    ApplicationUser user = Context.Users.Single(s => s.Id == userId);
                    if (user.ProfilBild != null)
                    {
                        string fullPath = Request.MapPath(serverPath + user.ProfilBild);
                        if ((System.IO.File.Exists(fullPath)))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    string fileName = System.Guid.NewGuid() + userId + file.FileName;
                    string path = Path.Combine(Server.MapPath(serverPath), fileName);
                    file.SaveAs(path);

                    user.ProfilBild = fileName;
                    Context.SaveChanges();
                }
                return RedirectToAction("Index", "Manage");
            }
            catch
            {
                return RedirectToAction("Index", "Manage");
            }
        }

        // GET: /Manage/Show/34
        public ActionResult Show(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ApplicationUser user = Context.Users.Single(s => s.Id == id);
            var personalTrainer = Context.PersonalTrainer.Single(k => k.User_Id == id);
            if (user == null || personalTrainer == null)
            {
                return HttpNotFound();
            }

            return View(new ShowViewModel
            {
                User_Id = user.Id,
                Picture_ID = user.ProfilBild,
                Vorname = user.Vorname,
                Name = user.Name,
                Adresse = user.Adresse,
                Mail = user.Email,
                Beschreibung = personalTrainer.Beschreibung,
                Bewertung = personalTrainer.Bewertung,
                AnzahlBew = personalTrainer.AnzahlBewertungen

            });
        }

        //POST /Manage/Show/34/3
        [HttpPost]
        public ActionResult RateTrainer(string id, float rating)
        {
            var pT = Context.PersonalTrainer.Single(k => k.User_Id == id);
            float cache = rating;
            Response.Cookies[id].Value = ""+rating;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                cache += pT.Bewertung * pT.AnzahlBewertungen;
                pT.AnzahlBewertungen = pT.AnzahlBewertungen + 1;
                pT.Bewertung = (float)System.Math.Round(cache / pT.AnzahlBewertungen, 1);
                TryUpdateModel(pT);
                Context.SaveChanges();
                return RedirectToAction("Show/"+pT.User_Id, "Manage");
            }
            catch
            {
                System.Console.WriteLine("Exit3");
                return RedirectToAction("Show/"+pT.User_Id, "Manage");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Hilfsprogramme

        // Wird für XSRF-Schutz beim Hinzufügen externer Anmeldungen verwendet.
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}