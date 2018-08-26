using htcustomer.entity;
using htcustomer.service.Interface;
using htcustomer.service.ViewModel;
using htcustomer.web.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace htcustomer.web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IAuthService authService;
        public AccountController(IAuthService _authService)
        {
            authService = _authService;
        }
        // GET: Account  
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView, string ReturnUrl = "")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                    {
                        var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                        if (user != null)
                        {
                            CustomSerializeModel userModel = new CustomSerializeModel()
                            {
                                UserId = user.UserId,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                RoleName = user.Roles.Select(r => r.RoleName).ToList()
                            };

                            string userData = JsonConvert.SerializeObject(userModel);
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                                (
                                1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                                );

                            string enTicket = FormsAuthentication.Encrypt(authTicket);
                            HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                            faCookie.Expires = DateTime.Now.AddMinutes(2);
                            Response.Cookies.Add(faCookie);
                        }

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Something Wrong : Username or Password invalid ^_^ ");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return View(loginView);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationView registrationView)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    // Email Verification  
                    string userName = Membership.GetUserNameByEmail(registrationView.Email);
                    if (!string.IsNullOrEmpty(userName))
                    {
                        ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
                        return View(registrationView);
                    }
                    string activeCode = authService.RegisterAccount(registrationView);

                    //Verification Email  
                    VerificationEmail(registrationView.Email, activeCode);
                    messageRegistration = "Your account has been created successfully. ^_^";
                    statusRegistration = true;
                }
                else
                {
                    messageRegistration = "Something Wrong!";
                }
                ViewBag.Message = messageRegistration;
                ViewBag.Status = statusRegistration;
            }
            catch (Exception ex)
            {
                // Log Elmah
            }
            return View(registrationView);
        }

        [HttpGet]
        public ActionResult ActivationAccount(string id)
        {
            bool statusAccount = authService.ActivateAccount(id);
            if (!statusAccount)
            {
                ViewBag.Message = "Something Wrong !!";
            }

            ViewBag.Status = statusAccount;
            return View();
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        [NonAction]
        public void VerificationEmail(string email, string activationCode)
        {
            var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

            var fromEmail = new MailAddress("tiendatdeveloper1997@gmail.com", "Activation Account - AKKAHHHA");
            var toEmail = new MailAddress(email);

            var fromEmailPassword = "tiendatprogrammer711221";
            string subject = "Activation Account !";

            string body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Activation Account ! </a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })
                smtp.Send(message);
        }
    }
}