Club Management System
============

Club Management System helps users manage martial art clubs. It tracks member activity,  groups and payments. It has a notification service which send SMS messages to remind club members for expiring membership. It also provides some statistics.

![CMS Preview](https://github.com/TheDayIsMyEnemy/ClubManagementSystem/blob/main/screenshots/dashboard.png)

## Technologies
- Blazor Server
- Microsoft SQL Server
- ASP.NET Core Identity
- MudBlazor 

## Features
- Manage memberships
- Member groups
- Upload members CSV file
- User Management Module
- Role based Authorization
- Localization
- Mobile Responsiveness
- Dark Theme
- Other awesome features yet to be implemented

## Setup
1. Ensure the tool EF was already installed. You can find some help [here](https://docs.microsoft.com/ef/core/miscellaneous/cli/dotnet)

    ```
    dotnet tool update --global dotnet-ef
    ```

2. Open a command prompt in the project folder and execute the following commands:

  - This command will create the initial migration.
    ```
    dotnet ef migrations add Initial --output-dir ./Data/Migrations
    ```

  - This command will update/create the database to the last migrations.
    ```
    dotnet ef database update 
    ```  

3. Run the application.
    ```
    dotnet run
    ```

## License
>You can check out the full license [here](https://github.com/TheDayIsMyEnemy/ClubManagementSystem/blob/main/LICENSE)

This project is licensed under the terms of the **MIT** license.
