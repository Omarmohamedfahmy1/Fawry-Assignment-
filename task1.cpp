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
const int MAX_USERS = 10;
User users[MAX_USERS] = {
    {"admin",     "admin123", ADMIN},
    {"customer1", "pass123",  CUSTOMER}
};
int userCount = 2;

// ── Simple login ─────────────────────────────────────────────────────────────
User* login() {
    string username, password;
    cout << "=== Shopping Cart Login ===\n";
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

// ── Product catalogue (shared state) ─────────────────────────────────────────
const int MAX_PRODUCTS  = 10;
const int MAX_CART_ITEMS = 10;

string productNames[MAX_PRODUCTS];
double productPrices[MAX_PRODUCTS];
int    productQuantities[MAX_PRODUCTS];
bool   isExpired[MAX_PRODUCTS];
bool   isShippable[MAX_PRODUCTS];
double productWeights[MAX_PRODUCTS];
int    productCount = 0;

// ── Admin: populate the catalogue ────────────────────────────────────────────
void runAdminFlow(const User& user) {
    if (user.role != ADMIN) {
        cout << "Access denied.\n";
        return;
    }
    cout << "How many products do you want to add? ";
    cin  >> productCount;
    for (int i = 0; i < productCount; i++) {
        cout << "Product " << (i + 1) << ":\n";
        cout << "Name: ";
        cin.ignore();
        getline(cin, productNames[i]);
        cout << "Price: ";
        cin  >> productPrices[i];
        cout << "Available quantity: ";
        cin  >> productQuantities[i];
        cout << "Is it expired? (1 = yes, 0 = no): ";
        cin  >> isExpired[i];
        cout << "Is it shippable? (1 = yes, 0 = no): ";
        cin  >> isShippable[i];
        if (isShippable[i]) {
            cout << "Weight (kg): ";
            cin  >> productWeights[i];
        } else {
            productWeights[i] = 0;
        }
    }
    cout << "Catalogue updated successfully.\n";
}

// ── Customer: browse and checkout ────────────────────────────────────────────
void runCustomerFlow(const User& user) {
    if (user.role != CUSTOMER) {
        cout << "Access denied.\n";
        return;
    }

    if (productCount == 0) {
        cout << "No products available yet.\n";
        return;
    }

    double customerBalance;
    cout << "Enter your balance: ";
    cin  >> customerBalance;

    // Show available products
    cout << "\nAvailable products:\n";
    for (int i = 0; i < productCount; i++) {
        cout << "[" << i << "] " << productNames[i]
             << " - $" << productPrices[i]
             << " (qty: " << productQuantities[i] << ")"
             << (isExpired[i] ? " [EXPIRED]" : "") << "\n";
    }

    int cartProductIndexes[MAX_CART_ITEMS];
    int cartQuantities[MAX_CART_ITEMS];
    int cartSize;

    cout << "\nHow many different items do you want to buy? ";
    cin  >> cartSize;

    for (int i = 0; i < cartSize; i++) {
        cout << "\nFor item " << (i + 1) << ":\n";
        cout << "Enter product index (0 to " << productCount - 1 << "): ";
        cin  >> cartProductIndexes[i];
        cout << "Enter quantity: ";
        cin  >> cartQuantities[i];
    }

    if (cartSize == 0) {
        cout << "Cart is empty!\n";
        return;
    }

    double subtotal    = 0;
    double shippingFees = 0;
    double totalWeight  = 0;

    cout << "\n** Shipment notice **\n";
    for (int i = 0; i < cartSize; i++) {
        int idx      = cartProductIndexes[i];
        int quantity = cartQuantities[i];

        if (isExpired[idx]) {
            cout << "Error: " << productNames[idx] << " is expired!\n";
            return;
        }
        if (quantity > productQuantities[idx]) {
            cout << "Error: Not enough " << productNames[idx] << " in stock.\n";
            return;
        }
        if (isShippable[idx]) {
            double weight = productWeights[idx] * quantity;
            int grams = (int)(weight * 1000);
            totalWeight += weight;
            cout << quantity << "x " << productNames[idx] << "   " << grams << "g\n";
            shippingFees += 15.0 * quantity;
        }
        subtotal += productPrices[idx] * quantity;
        productQuantities[idx] -= quantity;
    }

    int kg      = (int)totalWeight;
    int decimal = (int)((totalWeight - kg) * 10);
    cout << "Total package weight: " << kg << "." << decimal << "kg\n";

    double total = subtotal + shippingFees;
    if (customerBalance < total) {
        cout << "Error: Not enough balance.\n";
        return;
    }
    customerBalance -= total;

    cout << "\n** Checkout receipt **\n";
    for (int i = 0; i < cartSize; i++) {
        int idx      = cartProductIndexes[i];
        int quantity = cartQuantities[i];
        int cost     = (int)(productPrices[idx] * quantity);
        cout << quantity << "x " << productNames[idx] << "   " << cost << "\n";
    }
    cout << "---------------------\n";
    cout << "Subtotal      " << (int)subtotal      << "\n";
    cout << "Shipping      " << (int)shippingFees   << "\n";
    cout << "Amount        " << (int)total          << "\n";
    cout << "Remaining balance: " << customerBalance << "\n";
}

// ── Entry point ───────────────────────────────────────────────────────────────
int main() {
    User* currentUser = nullptr;
    while (currentUser == nullptr) {
        currentUser = login();
    }

    if (currentUser->role == ADMIN) {
        runAdminFlow(*currentUser);
    } else {
        runCustomerFlow(*currentUser);
    }

    return 0;
}
