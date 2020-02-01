<#
    The main repository of MicroCloud is https://github.com/stho32/MicroCloud .
    And this main repository essentially owns the database scripts. 

    Since we need copies of the database for development of the apis, we need those scripts 
    in here, too.

    This script downloads the latest version of the sql scripts from the main repository into this
    one, so both are in sync.
#>
Set-Location $PSScriptRoot

Invoke-WebRequest -Uri "https://raw.githubusercontent.com/stho32/MicroCloud/master/Database/database.sql" -OutFile "$PSScriptRoot\Database\Database.sql"
Invoke-WebRequest -Uri "https://raw.githubusercontent.com/stho32/MicroCloud/master/Database/example-database-entries.sql" -OutFile "$PSScriptRoot\Database\example-database-entries.sql"
