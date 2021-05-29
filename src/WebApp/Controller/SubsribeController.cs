﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApp.Models;
using WebApp.Models.Developer;
using WebApp.Models.Payment;
using WebApp.Models.Subscription;
using WebApp.Services;
using WebApp.Services.Developer;
using WebApp.Services.Payment;
using WebApp.Services.Subscription;

namespace WebApp.Controller
{
    public class SubsribeController : Microsoft.AspNetCore.Mvc.Controller
    {
        // GET

        private ISubscriptionService _subscriptionService;
        private IPaymentService _paymentService;
        private IDeveloperService _developerService;

        SubsribeController(ISubscriptionService _subscriptionService,IPaymentService _paymentService, IDeveloperService _developerService )
        {
            this._subscriptionService = _subscriptionService;
            this._developerService = _developerService;
            this._paymentService = _paymentService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        
        public bool HasBankAccount(int userId)
        {
            return _paymentService.GetBankAccount(userId) != null;
        }

        public async Task Follow(int userId, int developerId)
        {
            var newPaidSubscription = new PaidSubscriptionModel()
            {
                SubscribedToId = developerId,
                Tariff = new TariffModel()
                {
                    PriceType = PriceType.Free,
                    TypeOfSubscription = TypeOfSubscription.User
                },
                UserId = userId
            };
            await _subscriptionService.AddPaidSubscription(newPaidSubscription);
        }

        //TODO: процент где фиксировать?
        public double PercentForAdmin = 0.1;

        //1. ---Если впервые, то должен ввести данные карты (BankAccount)
        //2. ---Берем номер карты, списываем деньги, переводя их в хранилище, фиксируем списывание
        //3. ----Перекидываем в виртуальный кошелек девелопера, вычитая процент
        //4. ----Добавляем в список подписок PaidSubscription
       
        private async Task SendMoneyToVirtualPursesOfDevs(TariffModel tariff, IEnumerable<UserModel> allDevelopers)
        {
            var adminMoney = (int) (tariff.PricePerMonth * PercentForAdmin);
            await _paymentService.TransferMoneyToAdminPurse(adminMoney);
            foreach (var developer in allDevelopers)
            {
                var developerPurse = await _paymentService.GetVirtualPurse(developer.Id);//  
                await _paymentService.UpdateVirtualPurse(developer.Id,
                    developerPurse.Money + (tariff.PricePerMonth - adminMoney)/allDevelopers.Count()); 
            }

        }
        public async Task SubscribeToUser(int userId, int developerId, bool isBasic, bool isImproved, bool isMax)
        {
            var typeOfSubscription = TypeOfSubscription.User;
            var priceType = await WriteOffMoneyFromUserAndGetPriceType(userId, typeOfSubscription, isBasic, isImproved, isMax);
            var tariff =
                await _subscriptionService.GetTariffByPriceTypeAndSubscriptionType(typeOfSubscription, priceType);
            //Перекидываем в виртуальный кошелек девелопера, вычитая процент
            var developerPurse = await _paymentService.GetVirtualPurse(developerId);
            var adminMoney = (int) (tariff.PricePerMonth * PercentForAdmin);
            await _paymentService.UpdateVirtualPurse(developerId,
                developerPurse.Money + tariff.PricePerMonth - adminMoney);
            await _paymentService.TransferMoneyToAdminPurse(adminMoney);
            //Добавляем в список подписок 
            await _subscriptionService.AddPaidSubscription(new PaidSubscriptionModel()
            {
                SubscribedToId = developerId,
                Tariff = new TariffModel()
                {
                    PriceType = priceType,
                    TypeOfSubscription = typeOfSubscription
                },
                UserId = userId
            });
        }

        public async Task SubscribeToProject(int userId, int projectId, bool isBasic, bool isImproved, bool isMax)
        {
            //Берем номер карты, списываем деньги, переводя их в хранилище, фиксируем списывание
            var typeOfSubscription = TypeOfSubscription.Project;
            var priceType = await WriteOffMoneyFromUserAndGetPriceType(userId, typeOfSubscription, isBasic, isImproved, isMax);
            var tariff =
                await _subscriptionService.GetTariffByPriceTypeAndSubscriptionType(typeOfSubscription, priceType);
            // Перекидываем в виртуальный кошелек девелоперОВ, вычитая процент
            var allDevelopers = await _developerService.GetProjectUsers(projectId);
            SendMoneyToVirtualPursesOfDevs(tariff, allDevelopers);

            //4. ----Добавляем в список подписок PaidSubscription
            await _subscriptionService.AddPaidSubscription(new PaidSubscriptionModel()
            {
                SubscribedToId = projectId,
                Tariff = new TariffModel()
                {
                    PriceType = priceType,
                    TypeOfSubscription = typeOfSubscription
                },
                UserId = userId
            });
        }

        public async Task SubscribeToCompany(int userId, int companyId, bool isBasic, bool isImproved, bool isMax)
        {
            
            //2. ---Берем номер карты, списываем деньги, переводя их в хранилище, фиксируем списывание
            var typeOfSubscription = TypeOfSubscription.Team;
            var priceType = await WriteOffMoneyFromUserAndGetPriceType(userId, typeOfSubscription, isBasic, isImproved, isMax);
            var tariff =
                await _subscriptionService.GetTariffByPriceTypeAndSubscriptionType(typeOfSubscription, priceType);
            //3. ----Перекидываем в виртуальный кошелек девелоперОВ, вычитая процент
            var allDevelopers = await _developerService.GetCompanyUsers(companyId);
            SendMoneyToVirtualPursesOfDevs(tariff, allDevelopers);
            //4. ----Добавляем в список подписок PaidSubscription
            await _subscriptionService.AddPaidSubscription(new PaidSubscriptionModel()
            {
                SubscribedToId = companyId,
                Tariff = new TariffModel()
                {
                    PriceType = priceType,
                    TypeOfSubscription = typeOfSubscription
                },
                UserId = userId
            });
        }
        private async Task<PriceType> WriteOffMoneyFromUserAndGetPriceType(int userId, TypeOfSubscription typeOfSubscription, bool isBasic, bool isImproved, bool isMax)
        {
            //Берем номер карты, списываем деньги, переводя их в хранилище, фиксируем списывание
            var userAccount = await _paymentService.GetBankAccount(userId);
            var priceType = PriceType.Max;
            if (isBasic)
                priceType = PriceType.Basic;
            if (isImproved)
                priceType = PriceType.Improved;
            if (isMax)
                priceType = PriceType.Max;
            var tariff =
                await _subscriptionService.GetTariffByPriceTypeAndSubscriptionType(typeOfSubscription, priceType);
            await _paymentService.WriteOffMoneyFromBankAccount(userAccount, tariff.PricePerMonth);
            await _paymentService.AddWithdrawal(new WithdrawalModel()
            {
                Sum = tariff.PricePerMonth,
                DateTime = DateTime.Now,
                UserID = userId,
                ViewOfBankNumber = ViewOfBankNumber.Real
            });
            await _paymentService.AddMoneyToStorageOfMoney(tariff.PricePerMonth);
            return priceType;
        }
    }
}