# Chirp!
Chirp! is an ASP.NET Razor web application which is a Twitter clone. This app was developed as a part of the course "Analysis, Design and Software Architecture (Autumn 2024)" at the IT-University of Copenhagen. 

## How to run the Chirp! application locally
Before attempting to run Chirp!, ensure that dotnet 8 is installed on your computer.
For the localhost version of the Chirp!, there are no other install requirements.

The program does however need two secrets present on your computer for it to run.
To add the secrets to your "secrets-storage" navigate your terminal to **"chirp/src/chirp.web/"** folder and 
use the following commands from the terminal:

``` dotnet user-secrets set "authentication:github:clientId" "SECRET-A" ```

``` dotnet user-secrets set "authentication:github:clientSecret" "SECRET-B ```

**Initializing the secret store should not be necessary. However, just in case the command is "dotnet user-secrets init"**

These are our localhost secrets and will enable GitHub login on Chirp!. This is required, as an exception is triggered if the GitHub secrets are missing.

Now Chirp! should be ready to run.

The Program must be run from the **chirp/src/chirp.web/** folder.
Either ``` dotnet run ```  or ``` dotnet Watch ```  will work.  

## How to run playwright and intergration test
The tests are seperate to the unitest because they need to run on local host 
and therefore need to know the github authorisation secrets.
Playwright must be installed to run the tests and your computer need to know the secrets.

Step by step guide 
  1. Open a terminal. Navigate to the Chirp/src/Chirp.Web folder
  2. Run the command ```dotnet run```
  3. Open a new terminal. Navigate to the Chirp/test
  4. Run the command ```dotnet test```


