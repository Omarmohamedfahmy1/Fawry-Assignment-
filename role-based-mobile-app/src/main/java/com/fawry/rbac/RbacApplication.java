package com.fawry.rbac;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

/**
 * Role-Based Mobile App Backend — Code-First Approach
 *
 * CODE-FIRST vs DATABASE-FIRST:
 *   Code-First  : Define Java entity classes → Hibernate auto-generates the DB schema.
 *                 Best when the team is developer-centric and the schema evolves with features.
 *   Database-First: Design the SQL schema first → generate/write entity classes from it.
 *                 Best when a dedicated DBA owns the schema or integrating with an existing DB.
 *
 * WHY CODE-FIRST FOR A MOBILE APP RBAC BACKEND?
 *   - Rapid iteration: add/rename roles or permissions just by editing an enum/entity.
 *   - Single source of truth: the entity model IS the documentation.
 *   - Works seamlessly with Flyway/Liquibase for controlled migrations in production.
 */
@SpringBootApplication
public class RbacApplication {

    public static void main(String[] args) {
        SpringApplication.run(RbacApplication.class, args);
    }
}
