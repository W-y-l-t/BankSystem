using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;
using BankSystem.Application.Application.Services;

namespace Workshop5.Infrastructure.DataAccess.Migrations;

#pragma warning disable SA1649
[Migration(1, "Initial")]
public class Initial : SqlMigration
{
    private readonly string _systemPassword1 = PasswordHasherService.HashPassword("admin");

    protected override string GetUpSql(IServiceProvider serviceProvider)
    {
        return $"""
                    create type user_role as enum
                    (
                        'admin' ,
                        'customer'
                    );

                    create type transaction_type as enum
                    (
                        'deposit' ,
                        'withdraw'
                    );
                    
                    create type transaction_status as enum
                    (
                        'created' ,
                        'completed' ,
                        'rejected' ,
                        'undefined'
                    );

                    create table users
                    (
                        user_id uuid not null primary key ,
                        user_name text not null ,
                        user_role user_role not null ,
                        user_password text not null
                    );

                    create table accounts
                    (
                        account_id uuid not null primary key ,
                        account_pin_code text not null ,
                        account_balance decimal not null ,
                        account_owner_id uuid not null references users(user_id)
                    );

                    create table transactions
                    (
                        transaction_id uuid not null primary key ,
                        transaction_type transaction_type not null ,
                        transaction_amount decimal not null ,
                        transaction_account_id uuid not null references accounts(account_id) ,
                        transaction_datetime text not null ,
                        transaction_status transaction_status not null
                    );

                    create table system_passwords
                    (
                        system_password text not null
                    );

                    insert into system_passwords (system_password)
                    values ('{_systemPassword1}')
               """;
    }

    protected override string GetDownSql(IServiceProvider serviceProvider)
    {
        return $"""
                    drop table users;
                    drop table accounts;
                    drop table transactions;

                    drop type user_role;
                    drop type transaction_type;
                    drop type transaction_status;
                """;
    }
}