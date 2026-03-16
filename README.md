# Fawry Assignment

## Tasks

| File   | Description |
|--------|-------------|
| task 1 | C++ shopping-cart / checkout console app |
| task 2 | C++ bookstore management console app |
| task 3 | C++ role-based bookstore — demonstrates RBAC using a **code-first** approach |

---

## Task 3 — Role-Based App Design: Code-First vs Database-First

### The Question

> *"What should be done — code first or database first — to make a role-based mobile app?"*

### Short Answer: **Code-First**

For a role-based (RBAC) mobile app, the **code-first approach is recommended**. Define your
domain model and permission logic in code; let the data layer be derived from those definitions.

---

### What is Code-First?

You write the **entity classes and business rules first**, then use them to generate (or inform)
the database schema.

```
[Roles in code] → [Permission logic in code] → [DB schema / persistence layer]
```

### What is Database-First?

You design the **database schema first**, then generate entity classes from it.

```
[DB schema] → [Generated entity classes] → [Business logic added later]
```

---

### Why Code-First is Better for RBAC Mobile Apps

| Concern | Code-First ✅ | Database-First ❌ |
|---------|--------------|-----------------|
| Roles & permissions | Defined once in code, single source of truth | Scattered between DB tables and code |
| Rapid iteration | Change a class → migration auto-generated | Change DB → regenerate code → update logic |
| Testability | Unit-test permission functions without a DB | Requires DB setup for every test |
| Version control | Models travel with the code in the same PR | Schema changes need separate migration files |
| Onboarding | New developer reads one class to understand roles | New developer must inspect both DB and code |

---

### RBAC Design Used in Task 3

#### Roles

| Role     | Description |
|----------|-------------|
| ADMIN    | Full access: manage users, add/remove books, view inventory |
| STAFF    | Limited write access: add books, view inventory |
| CUSTOMER | End-user: view inventory, buy books |

#### Permissions Matrix

| Action           | ADMIN | STAFF | CUSTOMER |
|------------------|:-----:|:-----:|:--------:|
| View Inventory   |  ✅   |  ✅   |    ✅    |
| Add Book         |  ✅   |  ✅   |    ❌    |
| Buy Book         |  ❌   |  ❌   |    ✅    |
| Remove Old Books |  ✅   |  ❌   |    ❌    |
| Manage Users     |  ✅   |  ❌   |    ❌    |

#### Code-First Implementation Pattern

```cpp
// 1. Define roles as an enum (single source of truth)
enum Role { ADMIN, STAFF, CUSTOMER };

// 2. Express permissions as simple boolean functions
bool canAddBook(Role r)        { return r == ADMIN || r == STAFF; }
bool canBuyBook(Role r)        { return r == CUSTOMER; }
bool canRemoveOldBooks(Role r) { return r == ADMIN; }
bool canManageUsers(Role r)    { return r == ADMIN; }

// 3. Enforce at every operation
void BookStore::addBook(Role callerRole, Book b) {
    if (!canAddBook(callerRole)) {
        cout << "Access denied: your role cannot add books.\n";
        return;
    }
    // ... proceed
}
```

Any persistence layer (SQLite, Firebase, REST API) would store `users.role` as a value derived
from this enum — the code definition comes first.

---

### How to Compile and Run Task 3

```bash
g++ -o bookstore "task 3" && ./bookstore
```

Demo credentials pre-loaded at startup:

| Username   | Password    | Role     |
|------------|-------------|----------|
| admin      | admin123    | ADMIN    |
| staff1     | staff123    | STAFF    |
| customer1  | customer123 | CUSTOMER |
