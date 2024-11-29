# Chirp
The chirp.cli application has two commands:

Read:
read is the command to read cheeps from the database, this has an option of defining
How many cheeps you want to read. not including an int wil give you all cheeps.

Usage:
dotnet run -- read <int>
Where <int> is optional
and
Cheep:
Cheeps is the command to add a message to our database.

Usage:
dotnet run -- chirp "message"