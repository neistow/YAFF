using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using YAFF.Core.Common;
using YAFF.Core.Entities;

namespace YAFF.Business.Specifications
{
    public class PostHasTagsSpecification : Specification<Post>
    {
        public PostHasTagsSpecification(IEnumerable<int> tagsToInclude, FilterMode inclusionMode,
            IEnumerable<int> tagsToExclude, FilterMode exclusionMode)
        {
            var includeExpression = PredicateBuilder.New<Post>(true);

            foreach (var tagId in tagsToInclude)
            {
                includeExpression = inclusionMode == FilterMode.Or
                    ? includeExpression.Or(p => p.PostTags.Select(pt => pt.TagId).Contains(tagId))
                    : includeExpression.And(p => p.PostTags.Select(pt => pt.TagId).Contains(tagId));
            }

            var excludeExpression = PredicateBuilder.New<Post>(true);
            foreach (var tagId in tagsToExclude)
            {
                excludeExpression = exclusionMode == FilterMode.Or
                    ? excludeExpression.Or(p => !p.PostTags.Select(pt => pt.TagId).Contains(tagId))
                    : excludeExpression.And(p => !p.PostTags.Select(pt => pt.TagId).Contains(tagId));
            }

            Expression = includeExpression.And(excludeExpression);
        }

        public override Expression<Func<Post, bool>> Expression { get; }
    }
}