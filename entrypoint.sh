#!/bin/bash
set -e

# start SQL Server in background
/opt/mssql/bin/sqlservr &

sqlservr_pid=$!

# wait for SQL Server to be ready
for i in {1..60}; do
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "$SA_PASSWORD" -Q "SELECT 1" >/dev/null 2>&1 && break
  echo "Waiting for SQL Server to be ready..."
  sleep 1
done

# run init script if present
if [ -f /init.sql ]; then
  echo "Running init.sql..."
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "$SA_PASSWORD" -i /init.sql
fi

# wait on sqlservr process so container stays alive
wait "$sqlservr_pid"