using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace Lab5.Infrastucture.Migrations;

[Migration(1, "Initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider)
    {
        return """
                   CREATE TABLE bank_accounts
                   (
                       account_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                       account_number TEXT NOT NULL UNIQUE,
                       pin_code INT NOT NULL CHECK(pin_code BETWEEN 1000 AND 9999),
                       balance DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
                       currency TEXT NOT NULL
                   );

               CREATE TABLE users
                          (
                              user_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                                account_id uuid NOT NULL Unique DEFAULT gen_random_uuid(),
                                name TEXT NOT NULL UNIQUE
                          );
               CREATE TABLE admins
               (
                   admin_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                     system_password TEXT NOT NULL UNIQUE
               );
               CREATE TABLE transactions
               (
                   transactions_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                   account_id UUID DEFAULT gen_random_uuid(),
                   timestamp DATE NOT NULL,
                   amount DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
                   currency TEXT NOT NULL,
                   description TEXT NOT NULL
               );
               """;
    }

    protected override string GetDownSql(IServiceProvider serviceProvider)
    {
        return """
                   -- Удаляем индексы и таблицы
                   DROP INDEX IF EXISTS idx_transaction_account_id;
                   DROP INDEX IF EXISTS idx_account_number;
                   DROP TABLE IF EXISTS transactions;
                   DROP TABLE IF EXISTS bank_accounts;
                   DROP TABLE IF EXISTS users;
               """;
    }
}