# Fawry Assignment – Role-Based Access Control (RBAC)

## Architecture Approach: Code-First

For a role-based application, the recommended sequence is:

1. **Code first** – define your domain entities, roles, and permissions in code.
2. **Generate the database** from those definitions (e.g. migrations, ORM schema generation).

This keeps the source of truth in version-controlled code and lets you evolve the schema safely over time.

---

## Role Model

| Role       | Permissions                                  |
|------------|----------------------------------------------|
| `ADMIN`    | Add/remove products or books, view inventory |
| `CUSTOMER` | View inventory, purchase items               |

---

## Tasks

### Task 1 – Shopping Cart (`task1.cpp`)

A shopping-cart system with role-based flows:

- **Admin flow** – adds products to the shared catalogue (name, price, stock, expiry, shipping weight).
- **Customer flow** – enters balance, browses catalogue, builds a cart, and checks out with shipment notice and receipt.

**Build & run:**
```bash
g++ task1.cpp -o task1 && ./task1
```

**Demo users:**

| Username    | Password   | Role     |
|-------------|------------|----------|
| `admin`     | `admin123` | ADMIN    |
| `customer1` | `pass123`  | CUSTOMER |

---

### Task 2 – Book Store (`task2.cpp`)

A book-store inventory system with role-based menus:

- **Admin menu** – add books (paper / ebook / showcase), view inventory, remove old books.
- **Customer menu** – view inventory, buy books (paper books ship to address; ebooks are emailed).

**Build & run:**
```bash
g++ task2.cpp -o task2 && ./task2
```

**Demo users:** same as Task 1.

---

## Why Code-First for RBAC?

| Concern             | Code-First Advantage                                               |
|---------------------|--------------------------------------------------------------------|
| Role definition     | Roles and permissions live as enums/constants in source control    |
| Schema evolution    | Add a new role → update code → run migration; no manual SQL needed |
| Onboarding          | New developers read the code to understand the permission model     |
| Testing             | Business logic (access checks) is unit-testable without a database |
