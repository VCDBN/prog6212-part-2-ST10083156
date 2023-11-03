##Time Management Application##
The reason i have created this application is to aid college students with the management of their study time. It will prompt them for their module and semester details, and then provide them with a recommended weekly amount self-study hours in order to do well in the given modules.

##App funciontality##
The application is coded in the C# programming language and can be run using the Visual Studio application.
Once run, the application works with users. This means that each person who uses the application will be requiredto sign up with an original username and password. The username and hash of the password will be stored in the database and the database will be used to check user credentials upon sign in request. Once the user has logged in succesfully for the first time, they wil be sent to a window where they can enter the details of ttheir modules. They can add multiple modules on this window. Once they have entered all the details for all the modules theywould like to add, they can click the done button and proceedto a window where they will enter the details of the semester for the modules. Once entered, they click the confirm button and will proceed to a window that displays all the modules and the hours of self study required per week, with each week as an option in a combobox. Whenever they have studied during a week, they are able to update the hours remaining when they click the update button on the display screen. Once on the update screen they can select the week and module and enter the number of hours they have studied. This will then return them to the display screen where they can continue to loop through the update process or proceed to sign out when they are done. The user is only able to see their own data and not that of different users. The app stores all user details, moduledetails and semester details in an SQL database made in SQL Server Management Studio.

##App running##
In order to run it, one must first download Visual Studio and pull the application code into their IDE and click on the green run button located in the tools bar above the open window. Once clicked, the user will just need to follow the prompts on the screen and enter the details in order to calculate their recommended self-study hours.

##Video Link##
https://www.youtube.com/watch?v=rkOIgTtfiTc 
