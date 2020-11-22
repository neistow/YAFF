using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YAFF.Api.Helpers
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            };

            using (var transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions,
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var ctx = await next();
                if (ctx.Exception == null)
                {
                    transaction.Complete();
                }
            }
        }
    }

    public class EnableTransactionAttribute : TypeFilterAttribute
    {
        public EnableTransactionAttribute() : base(typeof(UnitOfWorkFilter))
        {
        }
    }
}