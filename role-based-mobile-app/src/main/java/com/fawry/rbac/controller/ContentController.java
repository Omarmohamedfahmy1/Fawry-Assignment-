package com.fawry.rbac.controller;

import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/**
 * Sample content endpoints showing role-based access in practice.
 *
 * GET /api/content/public     → authenticated users (any role)
 * GET /api/content/moderated  → ROLE_ADMIN or ROLE_MODERATOR
 * GET /api/content/admin      → ROLE_ADMIN only
 */
@RestController
@RequestMapping("/api/content")
public class ContentController {

    @GetMapping("/public")
    @PreAuthorize("isAuthenticated()")
    public ResponseEntity<String> publicContent() {
        return ResponseEntity.ok("Public content — visible to all authenticated users.");
    }

    @GetMapping("/moderated")
    @PreAuthorize("hasAnyRole('ROLE_ADMIN', 'ROLE_MODERATOR')")
    public ResponseEntity<String> moderatedContent() {
        return ResponseEntity.ok("Moderated content — visible to admins and moderators.");
    }

    @GetMapping("/admin")
    @PreAuthorize("hasRole('ROLE_ADMIN')")
    public ResponseEntity<String> adminContent() {
        return ResponseEntity.ok("Admin-only content.");
    }
}
