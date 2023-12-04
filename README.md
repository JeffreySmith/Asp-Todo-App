This is my Final Project for PROG8550, a course on ASP.NET Core, and the code for the page hosted 
<a href="http://jsmith7-001-site1.atempurl.com">here</a>.
You shouldn't need to edit the ConnectionString since it is connected directly to the hosted SQL Server, well, server.

If you do want to try it locally, you'll need to edit a few things:
* Change which connection string is selected in Program.cs (change it to "DefaultConnection")
* Set Urls in appsettings.json to "http://localhost:5247;http://your.local.ip.here:5247"
* Then set myUrl to "http://your.local.ip.here:5247"

Easiest is just to leave it running against the hosted site. If you don't change 'Urls' and 'myUrl' when hosted locally,
you won't be able to try out the QR code links (they won't work).
