package com.fawry.rbac.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;

import java.util.List;

@Data
@Builder
@AllArgsConstructor
public class LoginResponse {

    private String token;
    private String username;
    private String email;
    private List<String> roles;
}
