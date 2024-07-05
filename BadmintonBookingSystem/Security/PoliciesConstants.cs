using BadmintonBookingSystem.BusinessObject.Constants;
using Microsoft.AspNetCore.Authorization;

namespace BadmintonBookingSystem.Security
{
    public static class PoliciesConstants
    {
        public static readonly AuthorizationPolicy PolicyAdmin = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().RequireRole(RoleConstants.ADMIN).Build();

        public static readonly AuthorizationPolicy PolicyManager = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().RequireRole(RoleConstants.MANAGER).Build();

        public static readonly AuthorizationPolicy PolicyStaff = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().RequireRole(RoleConstants.STAFF).Build();

        public static readonly AuthorizationPolicy PolicyCustomer = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().RequireRole(RoleConstants.CUSTOMER).Build();
    }
}
