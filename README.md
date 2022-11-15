# Disaster Alleviation Foundation
This web app allows users to register and keep track of current disasters around the world. 
Users can also make monetary and goods donations, which can then be allocated to specific disasters. 

This web app was developed using Visual Studio and C# (ASP.NET Core 6.0). 
To store the app’s data Entity Framework Core along with dependency injection was used to communicate with an SQL Database hosted on Azure.

## Features
- Users can log in securely to the web application and edit information.
- Users can make monetary and goods donations.
- Users can choose whether to donate publicly or anonymously.
- When donating goods, users can select from pre-configured categories or make their own.
- Users can load new disasters on the system.
- Once donations are received and disasters are loaded on the system. Users can allocate donations to specific disasters.
- Non registered users, can view a public page, displaying:
    - Total monetary and goods donations received.
    - Currently active disasters, with all donations allocated to them.
    
[Current Version Live Demo](https://appr-poe.azurewebsites.net)

# Changelog
All notable changes to this project will be documented in this file.

## [Final Release] - 2022-11-11
### Added
- 'Home' Public Page
    - Can view Total Goods Donations Received.
    - Can view Total Money Donations Received.
    - Can view All Disasters.
    - Can view Allocations Made to each Disaster.
- Unit Testing
    - Testing all Models with the Database.
    - Testing main features of Database, outside of reading and writing to and from the database.
- Auto-Deployment Pipeline
    - When merges are made to the Master Branch, the pipeline auto-deploys, and updates the live website.

### Changed
- Updated more pages to modals instead of full pages for one form.
- Modularized some pages, for better testing, and test cases.
- Further improved validation, to include more test cases.
- Readme.md:
    - Updated to reflect notable changes to the application.

### Removed
- Pages that are no longer in need because of modal changes.

## [Part 2] - 2022-10-17
### Added
- 'Disaster Details' page:
    - Allocate money.
    - Allocate goods.
    - View allocation history.
- 'Goods Inventory' page:
    - Acquire new goods using money received from donations.
    - View all goods (Donated or Acquired), and the stock/quantity.

### Changed
- Now using modals instead of full pages that contain only one form for one purpose.
- Incorrect button labels.
- Improved Form Validation to check for more cases.
- Readme.md:
    - Updated to reflect notable changes to the application.

### Removed
- Pages that are no longer in need because of modal changes.

## [Part 1] - 2022-09-14
### Added
- Login and Registration.
- Logout.
- User roles.
- 'My Account' page:
    - Personal details (Name, Surname, Email, Phone Number).
    - Edit personal details.
    - Add a money donation.
    - Add a goods donation.
    - Pre-defined categories for new goods donations.
    - Add New Category when donating goods.
    - View past monetary donations.
    - View past goods donations.
- 'Donations' page:
    - View all past monetary and goods donations.
- 'Disasters' page:
    - Add new disaster.
    - View added disasters.
    - Specify aid-types in need for disasters.
    - View aid-types in need for each disaster.
- 'Disaster Details' page:
    - View disaster details:
        - Start & end dates.
        - Location.
        - Description.
        - Created by.
        - Required aids.
    - Edit disaster details.
- 'Users' page:
    - Accessible by users that have role of 'admin'.
    - View new registration requests.
    - Approve or deny new registration requests.
    - View approved users.
    - Revoke access for approved users.
    - View denied users.
    - Grant access to denied users.
