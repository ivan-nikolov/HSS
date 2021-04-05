# HSS























# :pencil2: Home Services and Solutions
This is my defense project for **ASP.NET Core MVC** course at [SoftUni](https://softuni.bg). 
# :memo: Overview
&nbsp;&nbsp;&nbsp;&nbsp;**Home services and solutions** is a platform for service delivering. The main idea is to gather client, administration and team features in one place. The services are categorized into recursive categories. Visitation frequency of a service can be once, daily, weekly or monthly.

&nbsp;&nbsp;&nbsp;&nbsp;Clients can book services, check their active, pending and done jobs. Invoice is generated for every done job. For recurrent services invoices are generated monthly. When a client is booking a service he receives a list of free hours for the selected date according to the chosen visitation frequency and the schedule of teams in that location. The job is assigned to the team with less working hours for the month. Clients can cancel booked services.
  
&nbsp;&nbsp;&nbsp;&nbsp;Administrators can create categories, services and teams. They assign users to a team as team members. Administrators confirm the pending jobs but before that they can change the team selecting from the free teams in the location.
  
&nbsp;&nbsp;&nbsp;&nbsp;Team members can check their team jobs and complete them. Upon completing if the visitation frequency of the booking is different than once a new job is generated for the next visitation date and hour and it is assigned to the team.

&nbsp;&nbsp;&nbsp;&nbsp;URL: <https://homeservicesandsolutions.azurewebsites.net/>

&nbsp;&nbsp;&nbsp;&nbsp;**Test Accounts**:

&nbsp;&nbsp;&nbsp;&nbsp;Username: Admin  
&nbsp;&nbsp;&nbsp;&nbsp;Password: 123456  

&nbsp;&nbsp;&nbsp;&nbsp;Username: Client  
&nbsp;&nbsp;&nbsp;&nbsp;Password: 123456  

&nbsp;&nbsp;&nbsp;&nbsp;Username: TeamMember  
&nbsp;&nbsp;&nbsp;&nbsp;Password: 123456
# :hammer: Built With:
* ASP.NET Core 3.1 MVC
* Blazor Server
* ASP.NET Core view components
* ASP.NET Core areas
* MSSQL Server
* Entity Framwork Core 3.1
* Real-time Notifications bases on Blazor Server
* Hangfire
* Twilio SendGrid
* Cloudinary
* TinyMCE
* Bootstrap
* Moq
* xUnit
