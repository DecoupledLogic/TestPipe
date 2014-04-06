#This build assumes the following directory structure
#
#  \               - This is where the project build code lives
#  \build          - This folder is created if it is missing and contains output of the build
#  \src            - This folder contains the source code or solutions you want to build
#
Properties {
    $build_dir = Split-Path $psake.build_script_file    
    $build_artifacts_dir = "$build_dir\build\"
    $solution_file = "$build_dir\src\NPoco\NPoco.csproj"
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends Build35

Task Build35 -Depends Build40 { 
	Write-Host "Building 3.5 $solution_file" -ForegroundColor Green
	Exec { msbuild "$solution_file" /t:Clean /p:Configuration=Release } 
	Exec { c:\windows\microsoft.net\framework\v3.5\msbuild.exe "$solution_file" /t:Build /p:Configuration=Release /v:quiet /p:DefineConstants="POCO_NO_DYNAMIC" /p:OutDir="$build_artifacts_dir\35\" }
}

Task Build40 -Depends Clean { 
    Write-Host "Building 4.0 $solution_file" -ForegroundColor Green
	Exec { msbuild "$solution_file" /t:Build /p:Configuration=Release /v:quiet /p:OutDir="$build_artifacts_dir\40\" } 
}

Task Clean {
    Write-Host "Creating BuildArtifacts directory" -ForegroundColor Green
    if (Test-Path $build_artifacts_dir) 
    {   
        rd $build_artifacts_dir -rec -force | out-null
    }
    
    mkdir $build_artifacts_dir | out-null
    
    Write-Host "Cleaning $solution_file" -ForegroundColor Green
    Exec { msbuild "$solution_file" /t:Clean /p:Configuration=Release /v:quiet } 
}