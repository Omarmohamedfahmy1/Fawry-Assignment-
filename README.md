# Fawry-Assignment-

## Tasks

- **Task 1** – Shopping cart console application (C++)
- **Task 2** – Bookstore management console application (C++)
- **Task 3** – Role-Based Access Control (RBAC) system — Code-First approach (C++)

---

## Role-Based Mobile App: Code-First vs Database-First

### What is Code-First?

In the **Code-First** approach, you define your domain model (classes, roles, and relationships) entirely in code.
The database schema is then generated **from** that code.

### What is Database-First?

In the **Database-First** approach you design the database schema first (tables, columns, foreign keys) and then generate or write the corresponding model classes from that schema.

### Which should you choose for a role-based mobile app?

**Use Code-First** when:
- You are starting a new project from scratch and want full control over the domain model.
- You want roles and permissions defined alongside the rest of your business logic, making them easy to version-control and review.
- You prefer iterating quickly: add a new role or permission in code, run a migration, and the database catches up automatically.

**Use Database-First** when:
- A legacy database already exists and you need to build a mobile app on top of it.
- A dedicated DBA owns the schema and the application code must conform to it.

### Recommended approach for a new role-based mobile app

1. **Define entities in code first** – `User`, `Role`, `Permission`, and their relationships.
2. **Generate the database migration** from those entities (e.g. with Entity Framework Core, Room, or a similar ORM).
3. **Seed initial roles and permissions** through a code-based seeder so the authoritative source of truth stays in version control.
4. **Apply migrations on every deployment** to keep the database in sync with the code.

Task 3 in this repository demonstrates the Code-First design by implementing a self-contained RBAC system in C++ with no hard-coded schema separate from the code.