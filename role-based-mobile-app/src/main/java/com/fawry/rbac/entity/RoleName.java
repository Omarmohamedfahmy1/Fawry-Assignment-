package com.fawry.rbac.entity;

/**
 * Available roles in the system.
 * Code-first: changing this enum automatically propagates to the DB via Hibernate.
 */
public enum RoleName {
    ROLE_ADMIN,
    ROLE_MODERATOR,
    ROLE_USER
}
