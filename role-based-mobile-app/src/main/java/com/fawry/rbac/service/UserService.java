package com.fawry.rbac.service;

import com.fawry.rbac.dto.RegisterRequest;
import com.fawry.rbac.entity.Role;
import com.fawry.rbac.entity.RoleName;
import com.fawry.rbac.entity.User;
import com.fawry.rbac.repository.RoleRepository;
import com.fawry.rbac.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.HashSet;
import java.util.List;
import java.util.Set;

@Service
@RequiredArgsConstructor
public class UserService {

    private final UserRepository userRepository;
    private final RoleRepository roleRepository;
    private final PasswordEncoder encoder;

    @Transactional
    public User register(RegisterRequest request) {
        if (userRepository.existsByUsername(request.getUsername())) {
            throw new IllegalArgumentException("Username is already taken.");
        }
        if (userRepository.existsByEmail(request.getEmail())) {
            throw new IllegalArgumentException("Email is already in use.");
        }

        Set<Role> roles = resolveRoles(request.getRoles());

        User user = User.builder()
                .username(request.getUsername())
                .email(request.getEmail())
                .password(encoder.encode(request.getPassword()))
                .roles(roles)
                .build();

        return userRepository.save(user);
    }

    @Transactional(readOnly = true)
    public List<User> findAll() {
        return userRepository.findAll();
    }

    @Transactional(readOnly = true)
    public User findById(Long id) {
        return userRepository.findById(id)
                .orElseThrow(() -> new IllegalArgumentException("User not found: " + id));
    }

    private Set<Role> resolveRoles(Set<String> roleNames) {
        Set<Role> roles = new HashSet<>();
        if (roleNames == null || roleNames.isEmpty()) {
            roles.add(findRole(RoleName.ROLE_USER));
        } else {
            for (String name : roleNames) {
                RoleName roleName = switch (name.toUpperCase()) {
                    case "ADMIN"     -> RoleName.ROLE_ADMIN;
                    case "MODERATOR" -> RoleName.ROLE_MODERATOR;
                    case "USER"      -> RoleName.ROLE_USER;
                    default -> throw new IllegalArgumentException(
                            "Unknown role: '" + name + "'. Valid values: ADMIN, MODERATOR, USER");
                };
                roles.add(findRole(roleName));
            }
        }
        return roles;
    }

    private Role findRole(RoleName name) {
        return roleRepository.findByName(name)
                .orElseThrow(() -> new IllegalStateException("Role not seeded: " + name));
    }
}
