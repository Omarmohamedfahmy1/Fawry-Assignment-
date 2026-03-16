package com.fawry.rbac.controller;

import com.fawry.rbac.entity.User;
import com.fawry.rbac.service.UserService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.util.List;

/**
 * User-management endpoints — role-gated with @PreAuthorize.
 *
 * GET  /api/users        → ROLE_ADMIN only
 * GET  /api/users/{id}   → ROLE_ADMIN or ROLE_MODERATOR
 * GET  /api/users/me     → any authenticated user
 */
@RestController
@RequestMapping("/api/users")
@RequiredArgsConstructor
public class UserController {

    private final UserService userService;

    @GetMapping
    @PreAuthorize("hasRole('ROLE_ADMIN')")
    public ResponseEntity<List<User>> getAllUsers() {
        return ResponseEntity.ok(userService.findAll());
    }

    @GetMapping("/{id}")
    @PreAuthorize("hasAnyRole('ROLE_ADMIN', 'ROLE_MODERATOR')")
    public ResponseEntity<User> getUserById(@PathVariable Long id) {
        return ResponseEntity.ok(userService.findById(id));
    }

    @GetMapping("/me")
    public ResponseEntity<String> getCurrentUser(
            org.springframework.security.core.Authentication auth) {
        return ResponseEntity.ok("Logged in as: " + auth.getName()
                + " | authorities: " + auth.getAuthorities());
    }
}
