package com.fawry.rbac.config;

import com.fawry.rbac.entity.Permission;
import com.fawry.rbac.entity.Role;
import com.fawry.rbac.entity.RoleName;
import com.fawry.rbac.entity.User;
import com.fawry.rbac.repository.PermissionRepository;
import com.fawry.rbac.repository.RoleRepository;
import com.fawry.rbac.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.crypto.password.PasswordEncoder;

import java.util.Set;

/**
 * Bootstraps the database with default roles, permissions, and sample users.
 *
 * CODE-FIRST in action:
 *   1. Spring Boot starts → Hibernate reads the entity classes (User, Role, Permission).
 *   2. Hibernate auto-creates the tables (ddl-auto=update).
 *   3. This runner inserts the seed data into those freshly-created tables.
 *
 * In a production setup replace ddl-auto with Flyway/Liquibase migration scripts.
 */
@Configuration
@RequiredArgsConstructor
public class DataInitializer {

    private final PermissionRepository permissionRepo;
    private final RoleRepository roleRepo;
    private final UserRepository userRepo;
    private final PasswordEncoder encoder;

    @Bean
    CommandLineRunner initDatabase() {
        return args -> {
            if (roleRepo.count() > 0) {
                return; // already seeded
            }

            // --- Permissions ---
            Permission readUsers   = save("READ_USERS");
            Permission writeUsers  = save("WRITE_USERS");
            Permission deleteUsers = save("DELETE_USERS");
            Permission readContent = save("READ_CONTENT");
            Permission writeContent = save("WRITE_CONTENT");

            // --- Roles ---
            Role adminRole = roleRepo.save(Role.builder()
                    .name(RoleName.ROLE_ADMIN)
                    .permissions(Set.of(readUsers, writeUsers, deleteUsers,
                                        readContent, writeContent))
                    .build());

            Role modRole = roleRepo.save(Role.builder()
                    .name(RoleName.ROLE_MODERATOR)
                    .permissions(Set.of(readUsers, readContent, writeContent))
                    .build());

            Role userRole = roleRepo.save(Role.builder()
                    .name(RoleName.ROLE_USER)
                    .permissions(Set.of(readContent))
                    .build());

            // --- Sample Users ---
            userRepo.save(User.builder()
                    .username("admin")
                    .email("admin@fawry.com")
                    .password(encoder.encode("admin123"))
                    .roles(Set.of(adminRole))
                    .build());

            userRepo.save(User.builder()
                    .username("moderator")
                    .email("mod@fawry.com")
                    .password(encoder.encode("mod123"))
                    .roles(Set.of(modRole))
                    .build());

            userRepo.save(User.builder()
                    .username("john")
                    .email("john@example.com")
                    .password(encoder.encode("user123"))
                    .roles(Set.of(userRole))
                    .build());

            System.out.println("✅ Database seeded: roles, permissions and sample users created.");
        };
    }

    private Permission save(String name) {
        return permissionRepo.save(Permission.builder().name(name).build());
    }
}
