AccessRecoveryPasswordBrute
=============

Basic application to retrieve the password from a database using brute force.

To use a class that implements the victim must be created. This will work as wrapper between the database and the program.

Possible features to improve:
- Speed: It does not test the different permutations in the best order, some variables are constantly being created and discarded during the execution, and so on.
- Interface: The database can be selected only at design time.