using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using YAFF.Core.Common;
using YAFF.Core.Entities;

namespace YAFF.Business.Specifications
{
    public class ChatHasUsersSpecification : Specification<Chat>
    {
        public override Expression<Func<Chat, bool>> Expression { get; }

        public ChatHasUsersSpecification(IEnumerable<int> users)
        {
            var pb = PredicateBuilder.New<Chat>();

            foreach (var userId in users)
            {
                pb = pb.And(c => c.Users.Select(u => u.UserId).Contains(userId));
            }

            Expression = pb;
        }
    }
}