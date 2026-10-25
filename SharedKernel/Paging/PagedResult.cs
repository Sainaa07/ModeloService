using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Paging
{
        public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);
        public record PagedRequest(int Page = 1, int PageSize = 20);
}
