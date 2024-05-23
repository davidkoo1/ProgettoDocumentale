using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Extensions
{
    public static class PrincipalExtensions
    {
        public static int GetUserId(this IPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new InvalidOperationException("The principal identity is not a ClaimsIdentity.");
            }

            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new InvalidOperationException("The NameIdentifier claim is not present.");
            }

            return int.Parse(claim.Value);
        }
    }

}
