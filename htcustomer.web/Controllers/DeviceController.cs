﻿using htcustomer.entity.Enums;
using htcustomer.service.Helper;
using htcustomer.service.Interfaces;
using htcustomer.service.ViewModel.Device;
using htcustomer.service.ViewModel.Transaction;
using System;
using System.Web.Mvc;

namespace htcustomer.web.Controllers
{
    public class DeviceController : Controller
    {
        private readonly ITransactionService transactionService;

        public DeviceController(ITransactionService _transactionService)
        {
            this.transactionService = _transactionService;
        }


        // GET: Device
        public ActionResult Index(int statusId = (int)TransactionStatus.NotFix)
        {
            var transactions = transactionService.GetListTransaction((TransactionStatus)statusId);
            return View("Index", transactions);
        }

        public ActionResult GetListTransactionHome(int? month, int? year)
        {
            if(month == null)
            {
                month = DateTime.Now.Month;
            }
            if(year == null)
            {
                year = DateTime.Now.Year;
            }
            var transactionList = transactionService.GetListTransactionHome(month.Value, year.Value);
            return Json(transactionList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterTransaction(int statusId = (int)TransactionStatus.NotFix, int? month = null, int? year = null, int? categoryId = null) {

            var viewModel = transactionService.GetListTransaction((TransactionStatus)statusId, month, year, categoryId);

            if (statusId == (int)TransactionStatus.NotFix) return PartialView("_NotFixDevice", viewModel.Transactions);
            if (statusId == (int)TransactionStatus.Fixed) return PartialView("_FixedDevice", viewModel.Transactions);
            if (statusId == (int)TransactionStatus.CannotFix) return PartialView("_CannotFixDevice", viewModel.Transactions);

            return PartialView("_DeliveredDevice", viewModel.Transactions);                
        }
                
        // Render Partial View for Device Tabs
        public ActionResult PartialTabs()
        {
            return PartialView("_DeviceTab");
        }

        // Render Partial View for Filters
        public ActionResult PartialFilter()
        {
            return PartialView("_DeviceFilter");
        }

        public ActionResult GetBillForm(int transactionId)
        {
            var result = transactionService.GetTransactionToAddPrice(transactionId);
            return PartialView("_AddBillForm", result);
        }
        [HttpPost]
        public ActionResult MakeAsFixDevice(PriceTransactionViewModel model, bool? callFromHome = false)
        {

            bool result;
            try
            {
                result = transactionService.FixedTransaction(model.TransactionID, model.PriceList);
                if (result == false)
                    return Json(new JsonMessage() { Status = JsonResultStatus.Fail, Message = "Fix action is unsuccessful" });
            }
            catch (Exception ex)
            {
                return Json(new JsonMessage() { Status = JsonResultStatus.Fail, Message = "Fix action is unsuccessful" });
            }

            return Json(new JsonMessage() { Status = JsonResultStatus.Success, Message = "Fix action is successful" });
        }
        public ActionResult GetReasonForm(int transactionId)
        {
            return PartialView("_AddReason", transactionId);
        }
        [HttpPost] 
        public ActionResult AddReasonForCannotFix(int transactionId, string reason)
        {
            bool result = transactionService.CannotFixTransaction(transactionId, reason);
            if (result == false)
            {
                return Json(new JsonMessage { Status = JsonResultStatus.Fail, Message = "Something went wrong with adding description"});
            }
            return Json(new JsonMessage { Status = JsonResultStatus.Success, Message = "Add reason succesfully"});
        }
      
        public ActionResult DeliverTransaction(int transactionId, bool getTransactionRow = false)
        {
            TransactionViewModel result = transactionService.DeliverTransaction(transactionId);
            if (result != null)
            {
                if (!getTransactionRow)
                {
                    return Json(new JsonMessage() { Status = JsonResultStatus.Success, Message = "Deliver Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return PartialView("~/Views/Contact/_DeliveredTransactionRow.cshtml", result);
                }
            }
            return Json(new JsonMessage() { Status = JsonResultStatus.Fail, Message = "Deliver Fail!" }, JsonRequestBehavior.AllowGet);
        }        

        [HttpPost]
        public ActionResult AddTransactions(TransactionCreateViewModel model)
        {
            try
            {
                transactionService.Add(model);
            }catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new JsonMessage() { Status = JsonResultStatus.Fail, Message = "Adding failure" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonMessage() { Status = JsonResultStatus.Success, Message = "Adding successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}