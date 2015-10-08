// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Identity.EntityFramework6
{
    /// <summary>
    ///     EntityType that represents a user belonging to a role
    /// </summary>
    public class IdentityUserRole
    {
        /// <summary>
        ///     UserId for the user that is in the role
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        ///     RoleId for the role
        /// </summary>
        public virtual string RoleId { get; set; }
    }
}