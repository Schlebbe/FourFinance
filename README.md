# FourFinance

## Overview

The **FourFinance** project is a console-based banking system built with C#.  
It supports two user roles — **Admin** and **Customer** — and provides basic banking features such as account management, deposits, withdrawals, transfers, and loan handling along with interest.

---

## Application Flow

1. On launch, the application seeds dummy data and prompts the user to log in as either an **Admin** or **Customer**.  
2. **Admin** users can:
   - Add new customers.
   - Update currency exchange rates.  
   - View all customers.
3. **Customer** users can:
   - View their accounts.
   - Open new checking or savings accounts.
   - Request loans.
   - Deposit, withdraw, or transfer funds.
   - View transaction history.
---

## Key Classes and Methods

### `Program`
- Initializes the application and starts the transaction and interest schedulers.
- Seeds dummy data.
- Handles the login prompt.

### `LoginHelper`
- Manages the login process.
- Displays the welcome prompt.
- Validates credentials.
- Directs users to the appropriate menu based on role.

### `CustomerHelper`
- Contains the customer menu logic.
- Handles:
  - Account management.
  - Deposits, withdrawals, and transfers.
  - Loan requests.
  - Transaction listings.

### `AdminHelper`
- Provides admin functionality such as:
  - Adding new customers.
  - Updating exchange rates.
  - Viewing all customers.

### `Customer`
- Represents a customer entity.
- Handles account creation and total asset calculations.

### `BankHelper`
- Manages all user and account data.
- Handles currency exchange rates and account number generation.
- Keeps track of interest rate.

### `Loan`
- Manages loan creation and validation.
- Checks against customer assets.
- Associates loans with the selected account.

### `Account`, `SavingsAccount`, `TransactionLog`
- Represent different account types and transaction history.
- `SavingsAccount` includes interest calculation logic.

---

## UML - Class Diagram

<img width="813" height="755" alt="image" src="https://github.com/user-attachments/assets/49a63ce1-8e35-46ea-bd58-f2dc491af0b6" />

---