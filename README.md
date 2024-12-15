# Chirp

## How to run playwright and intergration test
The tests are seperate to the unitest because they need to run on local host 
and therefore need to know the github authorisation secrets.
Playwright must be installed to run the tests and your computer need to know the secrets.

Step by step guide 
  1. Open a terminal. Navigate to the Chirp/src/Chirp.Web folder
  2. Run the command ```dotnet watch```
  3. Open a new terminal. Navigate to the Chirp/test
  4. Run the command ```dotnet test```


