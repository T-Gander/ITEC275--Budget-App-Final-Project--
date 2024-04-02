using BlazorBootstrap;
using ITEC275__Budget_App_Final_Project__.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using ITEC275__Budget_App_Final_Project__.Data;
using ITEC275__Budget_App_Final_Project__.Pages;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using System.CodeDom;

namespace ITEC275__Budget_App_Final_Project__
{
    public class DataManager
    {
        public Dictionary<Budget, Dictionary<Account, List<Transaction>>>? BudgetAccountsTransactionsDictionary { get; set; } = new();

        public Dictionary<Budget, Dictionary<Section, List<Category>>>? BudgetSectionCategoriesDictionary { get; set; } = new();

        public Dictionary<Budget, decimal>? TotalAssetsPerBudget { get; set; } = new();

        public Dictionary<Budget, decimal>? TotalUnassignedAssetsPerBudget { get; set; } = new();

        public Dictionary<Section, Collapse>? SectionCollapseDictionary { get; set; } = new();

        public Dictionary<Category, Collapse>? CategoryCollapseDictionary { get; set; } = new();

        public AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        public User? LoggedInUser { get; set; } = new();

        public Budget? CurrentBudget { get; set; }

        public void GenerateCollapseComponents(Budget budget)
        {
            foreach (Section Section in BudgetSectionCategoriesDictionary![budget].Keys)
            {
                Collapse newCollapse = default!;
                SectionCollapseDictionary![Section] = newCollapse;
            }

            foreach (List<Category> categorys in BudgetSectionCategoriesDictionary[budget].Values)
            {
                foreach(Category category in categorys)
                {
                    Collapse newCollapse2 = default!;
                    CategoryCollapseDictionary![category] = newCollapse2;
                }
            }
        }

        private async void LoadUser()
        {
            ClaimsPrincipal user;

            var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();
            user = authState.User;

            if (user != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    LoggedInUser = context.Users.FirstOrDefault(x => x.Email == user.Identity!.Name)!;
                }
            }
        }

        private bool CheckAuthentication()
        {
            LoadUser();

            if(LoggedInUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public decimal CalculateAssets(Budget budget)
        {
            Dictionary<Account, List<Transaction>> budgetAccounts = BudgetAccountsTransactionsDictionary![budget];

            decimal totalAssets = 0;

            foreach (List<Transaction> transactions in budgetAccounts.Values)
            {
                totalAssets += transactions.Sum(x =>
                {
                    if (!x.IsCredit)
                    {
                        return x.TransactionValue * -1;
                    }
                    else
                    {
                        return x.TransactionValue;
                    }
                });
            }

            if (TotalAssetsPerBudget!.ContainsKey(budget))
            {
                TotalAssetsPerBudget[budget] = totalAssets;
            }
            else
            {
                TotalAssetsPerBudget.Add(budget, totalAssets);
            }

            return TotalAssetsPerBudget[budget];
        }

        public decimal CalculateCategoryLeftToTransact(Budget budget, Category category)
        {
            decimal totalTransacted = 0;

            foreach(List<Transaction> transactions in BudgetAccountsTransactionsDictionary![budget].Values)
            {
                totalTransacted += transactions.Where(x => x.CategoryId == category.CategoryId).Sum(x => {
                    if (!x.IsCredit)
                    {
                        return x.TransactionValue * -1;
                    }
                    else
                    {
                        return x.TransactionValue;
                    }
                });
            }

            return category.AssignedBudget - totalTransacted;
        }

        public decimal CalculateAllAssignedAssets(Budget budget)
        {
            Dictionary<Section, List<Category>> budgetSections = BudgetSectionCategoriesDictionary![budget];

            decimal totalAssets = 0;

            foreach (List<Category> categories in budgetSections.Values)
            {
                totalAssets += categories.Sum(x =>
                {
                    return x.AssignedBudget;
                });
            }

            if (TotalUnassignedAssetsPerBudget!.ContainsKey(budget))
            {
                TotalUnassignedAssetsPerBudget[budget] = totalAssets;
            }
            else
            {
                TotalUnassignedAssetsPerBudget.Add(budget, totalAssets);
            }

            return TotalUnassignedAssetsPerBudget[budget];
        }

        public async void LoadAllData()
        {
            bool authenticated = CheckAuthentication();

            if (authenticated)
            {
                ClearData();
                LoadUser();

                using (var context = new ApplicationDbContext())
                {
                    //Add all associated budgets attached to the logged in user.
                    context.Budgets
                        .Where(x => x.UserId == LoggedInUser!.Id).ToList()
                        .ForEach(x =>
                        {
                            BudgetAccountsTransactionsDictionary!.Add(x, new Dictionary<Account, List<Transaction>>());
                            BudgetSectionCategoriesDictionary!.Add(x, new Dictionary<Section, List<Category>>());
                        });

                    //Populate each budget with its associated accounts.
                    foreach(Budget b in BudgetAccountsTransactionsDictionary!.Keys)
                    {
                        await context.Accounts
                            .Where(x => x.BudgetId == b.BudgetId)
                            .ForEachAsync(x => BudgetAccountsTransactionsDictionary[b].Add(x, new()));

                        //Populate all of the transactions for each account.
                        foreach (Account a in BudgetAccountsTransactionsDictionary[b].Keys)
                        {
                            BudgetAccountsTransactionsDictionary[b][a]
                                .AddRange(context.Transactions.Where(x => x.AccountId == a.AccountId));
                        }

                        //Also populate the budget-section-category whilst we're here.
                        await context.Sections
                           .Where(x => x.BudgetId == b.BudgetId)
                           .ForEachAsync(y => BudgetSectionCategoriesDictionary![b].Add(y, new()));

                        //Populate the categories for each section
                        foreach (Section s in BudgetSectionCategoriesDictionary![b].Keys)
                        {
                            BudgetSectionCategoriesDictionary[b][s]
                                .AddRange(context.Categories.Where(x => x.SectionId == s.SectionId));
                        }
                    }

                    ////Start populating the sections
                    //foreach(Budget b in BudgetSectionCategoriesDictionary!.Keys)
                    //{
                    //    await context.Sections
                    //        .Where(x => x.BudgetId == b.BudgetId)
                    //        .ForEachAsync(y => BudgetSectionCategoriesDictionary[b].Add(y, new()));

                    //    //Populate the categories for each section
                    //    foreach (Section s in BudgetSectionCategoriesDictionary[b].Keys)
                    //    {
                    //        BudgetSectionCategoriesDictionary[b][s]
                    //            .AddRange(context.Categories.Where(x => x.SectionId == s.SectionId));
                    //    }
                    //}
                };
                    
                await _OrderLists();
            }
            else
            {
                ClearData();
            }
        }

        public void ClearData()
        {
            BudgetAccountsTransactionsDictionary = new();
            BudgetSectionCategoriesDictionary = new();
            SectionCollapseDictionary = new();
            CategoryCollapseDictionary = new();
            LoggedInUser = new();
        }

        private async Task _OrderLists()
        {
            //I hope I never need to debug this...

            //Order the budgets accounts and transactions
            var budgetAccountsSort = Task.Run(() =>
            {
                BudgetAccountsTransactionsDictionary =
                    BudgetAccountsTransactionsDictionary!
                        .OrderBy(x => x.Key.BudgetName)
                        .ToDictionary(kv => kv.Key, kv => kv.Value
                            .OrderBy(innerKv => innerKv.Key.AccountName) //Order the inner dictionary by AccountName
                            .ToDictionary(innerKv => innerKv.Key, innerKv => innerKv.Value
                                .OrderBy(dateVal => dateVal.TransactionDate)    //Order the transaction list by date
                                .ToList()));
            });

            //Order the budgets sections and categories.
            var budgetSectionsSort = Task.Run(() =>
            {
                BudgetSectionCategoriesDictionary =
                BudgetSectionCategoriesDictionary!
                    .OrderBy(x => x.Key.BudgetName)
                    .ToDictionary(kv => kv.Key, kv => kv.Value
                        .OrderBy(innerKv => innerKv.Key.SectionName)    //Order by Section name
                        .ToDictionary(innerKv => innerKv.Key, innerKv => innerKv.Value
                            .OrderBy(catVal => catVal.CategoryName)     //Order by Category name
                            .ToList()));
            });

            await Task.WhenAll(budgetAccountsSort, budgetSectionsSort);
        }

        public async void OrderLists()
        {
            await _OrderLists();
        }
    }
}
