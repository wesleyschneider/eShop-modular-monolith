#!/usr/bin/env bash
set -euo pipefail

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
    CREATE DATABASE catalogdb;
    CREATE DATABASE identitydb;
    CREATE DATABASE orderingdb;
EOSQL
