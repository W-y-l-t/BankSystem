# BankSystem

A console application that implements the basic functionality of the banking system.

The code is written according to the principles of onion architecture with a rich domain model

## Functionality

### Adding a user

When launching the application, two options are offered: log in to an existing account or add a new one.

There are two types of users:

1. **Customer** has access to basic functionality
2. **Admin** has access to advanced functionality: viewing the history of all banking transactions and the balance of all accounts \
In order to create an "Admin" type user, it is necessary that the entered password matches one of the system passwords (new system passwords are added using the migration mechanism)

Of course, all passwords are hashed.

### Adding a account

When a user is selected and authentication is successful, access is opened to create bank accounts and log in to the current user.

To create a bank account, you must enter a PIN code.

### Account actions

The following features are available when you log in to your account:

1. Viewing the current account's banking transaction history.
    - Transactions can either be completed or rejected.
2. Viewing the current account balance.
3. Creating a transaction of one of two types (deposit or withdraw).
4. Confirmation of the created transaction.
    - You will not be able to withdraw more funds than you have on your balance.

## Interaction with the database

- PostgreSQL is used as the database.
- Database migrations and SQL queries are implemented using Itmo.Dev.Platform.Postgres NuGet package.
- All database requests are made asynchronously.
- Data persistence is supported: restarting the application will not affect the integrity and safety of the data.

## Other technologies used

- **MediatR** NuGet package for processing requests and communication between the user and the application.
- **Spectre.Console** NuGet package for a better UI.
- **Dependency injection**.
- **Moq** to simulate the operation of repositories in tests.

## To get started

1. Place the **appsettings.json** file in your solution to the bin/debug/net folder.
2. Make sure that you have **Docker** installed and while on the solution path, write `docker compose up -d` in the terminal.