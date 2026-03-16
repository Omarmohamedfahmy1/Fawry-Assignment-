package com.fawry.rbac.entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * Permission — a fine-grained action a user may perform (e.g. "READ_USERS").
 *
 * Code-first: this class defines the `permissions` table.
 * The table is auto-created by Hibernate on startup (spring.jpa.hibernate.ddl-auto=update).
 */
@Entity
@Table(name = "permissions")
@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class Permission {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(nullable = false, unique = true, length = 60)
    private String name;
}
