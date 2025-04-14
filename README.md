# Objektorienterad programmering i C# (C1OJ1B) VT 2025

Denna reository är för kursen i Objektorienterad programmering i C# (VT 2025) på Högskolan i Borså.

Kursens schema finns på [Kronox](https://schema.hb.se/setup/jsp/Schema.jsp?startDatum=2025-03-31&intervallTyp=a&intervallAntal=1&sprak=SV&sokMedAND=true&forklaringar=true&resurser=k.C1OJ1B-20251-I17V5-) och kursmaterialet finns på [Canvas](https://hb.instructure.com/courses/9337).

## Mjukvara

Installera följande mjukvara på din egen dator (laptop):

- [Visual Studio Code](https://code.visualstudio.com)
- [.NET Sdk](https://dotnet.microsoft.com/en-us/download) (.NET 9.0)
- [Git](https://git-scm.com/downloads)
- [Miniconda](https://docs.anaconda.com/miniconda/install/#quick-command-line-install)
 
## Verifiera att mjukvaran istallerades korrekt

Verifiera att mjukvaran installerades korrekt genom att öppna en terminal och exekvera följande komamndon (varje kommando skall skriva ut en version):

- `code --version`
- `dotnet --version`
- `git --version`
- `conda --version`

Om du inte ser utskriften av en version för ett specifikt verktyg, se till att du har lagt till sökvägen till verktyget till din `PATH` miljövariabel.

## Kursens GitHub Repository

När du har installerat ovanstående mjukvara, öppna en terminal och klonda GitHub repositoryn [C1OJ1B_2025](https://github.com/paga-hb/C1OJ1B_2025) till din dator, och skapa en virtuell Python miljö:

- `git clone https://github.com/paga-hb/C1OJ1B_2025.git oopc`
- `cd oopc`
- `conda create -y -p ./.conda python=3.11`
- `conda activate ./.conda`
- `python -m pip install --upgrade pip`
- `pip install ipykernel jupyter`

## Öppna första Notebooken

Slutligen, se till att du är i `oopc` foldern i din terminal, och öppna första notebooken i Visual Studio Code (VSCode), genom att exekvera nedanstående kommando:

- `code -g workshop/environment.ipynb:0 .`

När notebooken öppnar i VSCode, klicka på texten `Select Kernel` (i övre-högra delen av notebooken), och välj `Python Environments... => conda (Python 3.11) .conda/bin/python`.

Nu kan du följa instruktioenrna i notebooken.
