# Project 'application2'

This is a simple .NET based project used for interacting
with several infrastructure components, especially the database.

Credentials (database) can be optionally fetched from conjur.

## Disclaimer

My C# skills are a bit rusty (pun not intended), so I may have done some WTF here and there.
I just wanted to have a simple .NET project for my tests.

I decline all responsibility if you copy/paste anything into your
production code.

## How to run

You need to have .NET 9.0 installed. There should be no operating system restrictions (I coded on Linux FYI).

You need to have a postgresql database setup (see [this repository](https://github.com/deathdric/infrastructure-setup-scripts/postgresql), where I assume that you are using `application2` for the application name). Unlike for the java sample I didn't manage migrations in the application,
so you'll have to run the [schema creation script](migrations/create_tables.sql) explicitly (you can use liquibase if you prefer, or even entity framework migrations, but managing them with a secrets manager will be on your own) :

```
psql -v adm_role=application2_dev_adm -h postgresql -U svcapplication2admd -d application2 -f migrations/create_tables.sql
```

(Note : it's important to use the `adm_role` parameter, because it will grant the permissions to your readwrite account, otherwise you will have to grant them explictly).

Now you can decide whether you want to run with or without a scret manager.

### Without a secret manager

- Copy the content of `appsettings-nosecrets.json` to `appsettings.json`
- Update the host, database name, username and password of the connection string to the ones you will be using
- Compile and run

### With conjur

See [this repository](https://github.com/deathdric/infrastructure-setup-scripts/conjur) for howto setup conjur. The sample json configuration file expects that this is application2, so make sure that you have initialized the proper setup and that the account you are planning to use has read permissions to your variables.

Sample assumes that you are using the API Key to login. If you can to use another method of authentication you'll need up adapter the code accordingly
(better : make it configurable).

- Copy the content of `appsettings-conjur.json` to `appsettings.json`
- Update the host and databased name of your connection string to the ones you will be using
- In the `Secrets/Conjur/Config` section, update the `ApplianceUrl` value to the url of the conjur server you set up and update the `Account` value to the account (tenant) name you set up.
- Set the `CONJUR_AUTHN_LOGIN` environment variable to the conjur login you're planning to use
- Set the `CONJUR_AUTHN_API_KEY` environment variable to the conjur api key you're planning to use
- In the `Secrets/Conjur/Provider/ConnectionStrings/DefaultConnection` section, update the `UserNamePath` and ``PasswordPath` values to the full path names of the variables used to store your database username and password.
- Compile and run

**Warning** : as it is, the sample will call conjur each time it needs the database credentials, so basically at each service call. You should have some cache mechanism set up to avoid unnecessarty load on your conjur infrastructure (otherwise your admins will hate you ^^).


### Testing the application

Try to add an item :

```
curl -v -X POST -H 'Content-Type: application/json' -d '{"title": "Wake up !"}' http://localhost:8091/api/todos
```

Then get the list of items :

```
curl -v http://localhost:8091/api/todos
```
