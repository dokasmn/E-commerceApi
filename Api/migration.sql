CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "Products" (
    "ProductId" int NOT NULL,
    "ProductTitle" longtext NOT NULL,
    "ProductDescription" longtext,
    "ProductPrice" double NOT NULL,
    "ProductThumbnail" longtext,
    "ProductIsFeatured" tinyint(1),
    CONSTRAINT "PK_Products" PRIMARY KEY ("ProductId")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240930125921_InitialCreate', '9.0.1');

ALTER TABLE "Products" ALTER COLUMN "ProductPrice" DROP NOT NULL;

CREATE TABLE "Users" (
    "Id" varchar(255) NOT NULL,
    "UserName" longtext NOT NULL,
    "NormalizedUserName" longtext,
    "Email" longtext,
    "NormalizedEmail" longtext,
    "EmailConfirmed" tinyint(1) NOT NULL,
    "PasswordHash" longtext,
    "SecurityStamp" longtext,
    "ConcurrencyStamp" longtext,
    "PhoneNumber" longtext,
    "PhoneNumberConfirmed" tinyint(1) NOT NULL,
    "TwoFactorEnabled" tinyint(1) NOT NULL,
    "LockoutEnd" datetime(6),
    "LockoutEnabled" tinyint(1) NOT NULL,
    "AccessFailedCount" int NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Carts" (
    "CartId" int NOT NULL,
    "CartUserId" varchar(255) NOT NULL,
    CONSTRAINT "PK_Carts" PRIMARY KEY ("CartId"),
    CONSTRAINT "FK_Carts_Users_CartUserId" FOREIGN KEY ("CartUserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "CartItems" (
    "CartItemId" int NOT NULL,
    "CartItemCartCartId" int NOT NULL,
    "CartItemProductProductId" int NOT NULL,
    CONSTRAINT "PK_CartItems" PRIMARY KEY ("CartItemId"),
    CONSTRAINT "FK_CartItems_Carts_CartItemCartCartId" FOREIGN KEY ("CartItemCartCartId") REFERENCES "Carts" ("CartId") ON DELETE CASCADE,
    CONSTRAINT "FK_CartItems_Products_CartItemProductProductId" FOREIGN KEY ("CartItemProductProductId") REFERENCES "Products" ("ProductId") ON DELETE CASCADE
);

CREATE INDEX "IX_CartItems_CartItemCartCartId" ON "CartItems" ("CartItemCartCartId");

CREATE INDEX "IX_CartItems_CartItemProductProductId" ON "CartItems" ("CartItemProductProductId");

CREATE UNIQUE INDEX "IX_Carts_CartUserId" ON "Carts" ("CartUserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241007194645_UpdateUserCartRelationship', '9.0.1');

COMMIT;

