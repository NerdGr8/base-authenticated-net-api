CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200809153759_JKRepositoryContext') THEN
    CREATE TABLE "Branches" (
        "Id" uuid NOT NULL,
        "Name" text NOT NULL,
        "Location" text NOT NULL,
        "Created" timestamp without time zone NULL,
        "ModifiedDate" timestamp without time zone NULL,
        CONSTRAINT "PK_Branches" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200809153759_JKRepositoryContext') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL,
        "Name" text NOT NULL,
        "Surname" text NOT NULL,
        "Email" text NOT NULL,
        "Usertype" text NULL,
        "Role" text NOT NULL,
        "BranchId" text NOT NULL,
        "BranchId1" uuid NULL,
        "Password" text NOT NULL,
        "Active" boolean NOT NULL,
        "Created" timestamp without time zone NULL,
        "ModifiedDate" timestamp without time zone NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Users_Branches_BranchId1" FOREIGN KEY ("BranchId1") REFERENCES "Branches" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200809153759_JKRepositoryContext') THEN
    CREATE INDEX "IX_Users_BranchId1" ON "Users" ("BranchId1");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200809153759_JKRepositoryContext') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200809153759_JKRepositoryContext', '3.1.4');
    END IF;
END $$;
