::test in unity2019.2+win10+rider

cd %~dp0
set tableml=%~dp0TableML\TableML\bin\Release\TableML.dll
set tableml_compiler=%~dp0TableML\TableMLCompiler\bin\Release\TableMLCompiler.dll

set mono_path="D:\Program Files\Unity\Editor\Data\MonoBleedingEdge\bin\mono.exe"
set pdb2mdb_path="D:\Program Files\Unity\Editor\Data\MonoBleedingEdge\lib\mono\4.5\pdb2mdb.exe"

%mono_path% %pdb2mdb_path% %tableml%
%mono_path% %pdb2mdb_path% %tableml_compiler%


::copy to ksframework
set tableml_path=%~dp0TableML\TableML\bin\Release\*
set tableml_compiler_path=%~dp0TableML\TableMLCompiler\bin\Release\*

set dst_tableml=E:\Code\KSFramework_master_2018\KSFramework\Assets\KSFramework\KEngine\KEngine.Lib\TableML\
set dst_tableml_compiler=E:\Code\KSFramework_master_2018\KSFramework\Assets\KSFramework\KEngine\KEngine.Lib\TableMLCompiler\

xcopy %tableml_path% %dst_tableml% /S/Y/R/I
xcopy %tableml_compiler_path% %dst_tableml_compiler% /S/Y/R/I


pause