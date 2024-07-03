using BadmintonBookingSystem.BusinessObject.Constants;
using Microsoft.AspNetCore.Authorization;

namespace BadmintonBookingSystem.Security
{
    public static class PoliciesConstants
    {
        public static readonly AuthorizationPolicy PolicyAdmin = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().RequireRole(RoleConstants.ADMIN).Build();

        public static readonly AuthorizationPolicy PolicyUser = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().RequireRole(RoleConstants.USER).Build();
    }
}
