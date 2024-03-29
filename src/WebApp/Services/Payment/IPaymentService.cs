﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models.Payment;

namespace WebApp.Services.Payment
{
    public interface IPaymentService
    {
        //BankAccounts
        public Task<IEnumerable<BankAccountModel>> GetBankAccounts();
        public Task<BankAccountModel> GetBankAccount(int userId);

        public Task DeleteBankAccount(int userId);

        public Task AddBankAccount(BankAccountModel newBankAccount);

        public Task UpdateBankAccount(BankAccountModel bankAccount);

        public Task WriteOffMoneyFromBankAccount(BankAccountModel newBankAccount, int money);
        
        //AdminPurse
        public Task TransferMoneyToAdminPurse(int money);
        
        //StorageOfMoney
        public Task TransferMoneyToBankAccount(BankAccountModel bankAccount);

        public Task AddMoneyToStorageOfMoney(int money);

        
        //Transfers
        public Task<IEnumerable<TransferModel>> GetTransfers();
        public Task<IEnumerable<TransferModel>> GetTransfersByUserFrom(int userId);
        public Task<IEnumerable<TransferModel>> GetTransfersByUserTo(int userId);

        public Task AddTransfer(TransferModel newTransfer);

        //VirtualPurses
        public Task<IEnumerable<VirtualPurseModel>> GetVirtualPurses();
        public Task<VirtualPurseModel> GetVirtualPurse(int userId);
        public Task UpdateVirtualPurse(int userId, int money);

        public Task DeleteVirtualPurse(int userId);
        public Task AddVirtualPurse(VirtualPurseModel newVirtualPurse);

        //Withdrawals
        public Task<IEnumerable<WithdrawalModel>> GetWithdrawals();
        public Task<IEnumerable<WithdrawalModel>> GetWithdrawalsByUserId(int userId);
        public Task AddWithdrawal(WithdrawalModel newWithdrawal);


    }
}
