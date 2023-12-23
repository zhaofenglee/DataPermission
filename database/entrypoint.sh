#!/bin/bash

until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U SA -P $SA_PASSWORD -Q 'SELECT name FROM master.sys.databases'; do
>&2 echo "SQL Server is starting up"
sleep 1
done

/opt/mssql-tools/bin/sqlcmd -S sqlserver -U SA -P $SA_PASSWORD -Q "CREATE DATABASE [$AuthServer_DB]"
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U SA -P $SA_PASSWORD -Q "CREATE DATABASE [$DataPermission_DB]"

/opt/mssql-tools/bin/sqlcmd -d $AuthServer_DB -S sqlserver -U sa -P $SA_PASSWORD -i migrations-AuthServerHost.sql
/opt/mssql-tools/bin/sqlcmd -d $DataPermission_DB -S sqlserver -U sa -P $SA_PASSWORD -i migrations-DataPermission.sql