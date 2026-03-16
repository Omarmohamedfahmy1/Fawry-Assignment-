#include <iostream>
#include <string>
using namespace std;

// ── Role definitions (code-first approach) ──────────────────────────────────
enum Role { ADMIN, CUSTOMER };

struct User {
    string username;
    string password;
    Role   role;
};

// DEMO ONLY – hardcoded plain-text passwords are for illustration purposes.
// In a production system passwords must be hashed (e.g. bcrypt) and user
// records must be stored in a secure database, never in source code.
// The code-first model defines the schema; the database is generated from it.
const int MAX_USERS = 10;
User users[MAX_USERS] = {
    {"admin",     "admin123", ADMIN},
    {"customer1", "pass123",  CUSTOMER}
};
int userCount = 2;

// ── Returns the logged-in user, or nullptr on failure ────────────────────────
User* login() {
    string username, password;
    cout << "=== Book Store Login ===\n";
    cout << "Username: ";
    cin  >> username;
    cout << "Password: ";
    cin  >> password;

    for (int i = 0; i < userCount; i++) {
        if (users[i].username == username && users[i].password == password) {
            cout << "Login successful. Welcome, " << username << "!\n";
            return &users[i];
        }
    }
    cout << "Invalid credentials.\n";
    return nullptr;
}

// ── Book entity ──────────────────────────────────────────────────────────────
const int MAX_BOOKS = 100;

class Book {
public:
    string isbn;
    string title;
    int    year;
    double price;
    string type;     // "paper" | "ebook" | "showcase"
    int    stock;
    string fileType;

    Book() {}

    Book(string i, string t, int y, double p, string ty, int s = 0, string ft = "")
        : isbn(i), title(t), year(y), price(p), type(ty), stock(s), fileType(ft) {}
};

// ── BookStore with role-aware operations ─────────────────────────────────────
class BookStore {
private:
    Book books[MAX_BOOKS];
    int  bookCount;

public:
    BookStore() : bookCount(0) {}

    // ADMIN-only ──────────────────────────────────────────────────────────────
    void addBook(const User& user, Book b) {
        if (user.role != ADMIN) {
            cout << "Access denied. Only admins can add books.\n";
            return;
        }
        if (bookCount < MAX_BOOKS) {
            books[bookCount++] = b;
            cout << "Book added successfully.\n";
        } else {
            cout << "Inventory is full.\n";
        }
    }

    void removeOldBooks(const User& user, int currentYear, int maxAge) {
        if (user.role != ADMIN) {
            cout << "Access denied. Only admins can remove books.\n";
            return;
        }
        for (int i = 0; i < bookCount; i++) {
            if (currentYear - books[i].year > maxAge) {
                for (int j = i; j < bookCount - 1; j++) {
                    books[j] = books[j + 1];
                }
                bookCount--;
                i--;
            }
        }
        cout << "Old books removed.\n";
    }

    // CUSTOMER + ADMIN ────────────────────────────────────────────────────────
    void showInventory() {
        cout << "\nInventory:\n";
        for (int i = 0; i < bookCount; i++) {
            cout << "- " << books[i].title
                 << " (" << books[i].isbn << ")"
                 << " [" << books[i].type << "]\n";
        }
    }

    void buyBook(const User& user, string isbn, int quantity,
                 string email, string address) {
        if (user.role != CUSTOMER) {
            cout << "Access denied. Only customers can buy books.\n";
            return;
        }
        for (int i = 0; i < bookCount; i++) {
            if (books[i].isbn == isbn) {
                if (books[i].type == "showcase") {
                    cout << "This book is for display only. Not for sale.\n";
                    return;
                }
                if (books[i].type == "paper") {
                    if (books[i].stock < quantity) {
                        cout << "Not enough stock.\n";
                        return;
                    }
                    books[i].stock -= quantity;
                    cout << "Paid: $" << books[i].price * quantity << endl;
                    cout << "Shipping paper book to address: " << address << endl;
                } else if (books[i].type == "ebook") {
                    cout << "Paid: $" << books[i].price * quantity << endl;
                    cout << "Sending ebook to email: " << email << endl;
                }
                return;
            }
        }
        cout << "Book not found.\n";
    }
};

// ── Role-specific menus ───────────────────────────────────────────────────────
void runAdminMenu(BookStore& store, const User& user) {
    int choice;
    while (true) {
        cout << "\n--- Book Store Admin Menu ---\n";
        cout << "1. Add Book\n";
        cout << "2. Show Inventory\n";
        cout << "3. Remove Old Books\n";
        cout << "4. Exit\n";
        cout << "Enter your choice: ";
        cin  >> choice;

        if (choice == 1) {
            string type, isbn, title, fileType = "";
            int year, stock = 0;
            double price = 0;
            cout << "Enter book type (paper / ebook / showcase): ";
            cin  >> type;
            cout << "Enter ISBN: ";
            cin  >> isbn;
            cout << "Enter title: ";
            cin.ignore();
            getline(cin, title);
            cout << "Enter year: ";
            cin  >> year;
            if (type == "showcase") {
                store.addBook(user, Book(isbn, title, year, 0, type));
            } else {
                cout << "Enter price: ";
                cin  >> price;
                if (type == "paper") {
                    cout << "Enter stock quantity: ";
                    cin  >> stock;
                    store.addBook(user, Book(isbn, title, year, price, type, stock));
                } else if (type == "ebook") {
                    cout << "Enter file type (PDF, EPUB...): ";
                    cin  >> fileType;
                    store.addBook(user, Book(isbn, title, year, price, type, 0, fileType));
                } else {
                    cout << "Invalid type.\n";
                }
            }
        } else if (choice == 2) {
            store.showInventory();
        } else if (choice == 3) {
            int year, age;
            cout << "Enter current year: ";
            cin  >> year;
            cout << "Enter maximum allowed age of books: ";
            cin  >> age;
            store.removeOldBooks(user, year, age);
        } else if (choice == 4) {
            cout << "Goodbye, " << user.username << "!\n";
            break;
        } else {
            cout << "Invalid choice.\n";
        }
    }
}

void runCustomerMenu(BookStore& store, const User& user) {
    int choice;
    while (true) {
        cout << "\n--- Book Store Customer Menu ---\n";
        cout << "1. Show Inventory\n";
        cout << "2. Buy Book\n";
        cout << "3. Exit\n";
        cout << "Enter your choice: ";
        cin  >> choice;

        if (choice == 1) {
            store.showInventory();
        } else if (choice == 2) {
            string isbn, email, address;
            int quantity;
            cout << "Enter ISBN of book to buy: ";
            cin  >> isbn;
            cout << "Enter quantity: ";
            cin  >> quantity;
            cout << "Enter your email: ";
            cin  >> email;
            cout << "Enter your address: ";
            cin.ignore();
            getline(cin, address);
            store.buyBook(user, isbn, quantity, email, address);
        } else if (choice == 3) {
            cout << "Goodbye, " << user.username << "!\n";
            break;
        } else {
            cout << "Invalid choice.\n";
        }
    }
}

// ── Entry point ───────────────────────────────────────────────────────────────
int main() {
    BookStore store;

    User* currentUser = nullptr;
    while (currentUser == nullptr) {
        currentUser = login();
    }

    if (currentUser->role == ADMIN) {
        runAdminMenu(store, *currentUser);
    } else {
        runCustomerMenu(store, *currentUser);
    }

    return 0;
}
