using FinancialTracker.Domain;
using FinancialTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialTracker.Services
{
    public class TransactionService
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<Category> _categoryRepository;
        
    }
}
