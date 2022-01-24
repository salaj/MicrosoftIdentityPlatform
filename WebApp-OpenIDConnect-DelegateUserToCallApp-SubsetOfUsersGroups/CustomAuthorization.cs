using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace WebApp_OpenIDConnect_DelegateUserToCallApp_SubsetOfUsers_Groups
{
    /// <summary>
    /// GroupPolicyHandler deals with custom Policy-based authorization.
    /// GroupPolicyHandler evaluates the GroupPolicyRequirement against AuthorizationHandlerContext 
    /// by calling CheckUsersGroupMembership method to determine if authorization is allowed.
    /// </summary>
    public class GroupPolicyHandler : AuthorizationHandler<GroupPolicyRequirement>
    {
        public GroupPolicyHandler()
        {
        }

        /// <summary>
        /// Makes a decision if authorization is allowed based on GroupPolicyRequirement.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   GroupPolicyRequirement requirement)
        {
            // Calls method to check if requirement exists in user claims.
            if (context.User.Claims.Any(x => x.Type == "groups" && x.Value == requirement.GroupName))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// GroupPolicyRequirement contains data parameter that 
    /// GroupPolicyHandler uses to evaluate against the current user principal or session data.
    /// </summary>
    public class GroupPolicyRequirement : IAuthorizationRequirement
    {
        public string GroupName { get; }
        public GroupPolicyRequirement(string GroupName)
        {
            this.GroupName = GroupName;
        }
    }
}
