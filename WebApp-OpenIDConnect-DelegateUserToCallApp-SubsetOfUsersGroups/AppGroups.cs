namespace WebApp_OpenIDConnect_DelegateUserToCallApp_SubsetOfUsers_Groups
{
    /// <summary>
    /// Contains a list of all the Azure AD app roles this app depends on and works with.
    /// </summary>
    public static class AppGroups
    {
        /// <summary>
        /// Teams messages writers
        /// </summary>
        public const string GroupPresence = "GroupPresence";
    }

    /// <summary>
    /// Wrapper class the contain all the authorization policies available in this application.
    /// </summary>
    public static class AuthorizationPolicies
    {
        public const string AssignmentToGroupPresenceRequired = "AssignmentToGroupPresenceRequired";
    }
}