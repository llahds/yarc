# YARC (Yet Another Reddit Clone)

YARC is a reddit clone built using C#, .NET 6, EF, SQL Server, and Angular 14. Most of my code is under NDA or privately owned so I put this together 
to demonstrate my preferred tech stack. By the way, if you're hiring and like what you see then please contact me at llah.d.nevets@gmail.com.

It currently supports user created forums, posts, comments, full text search, basic moderation, a toxicity filter, and a basic spam filter. 

![YARC](https://user-images.githubusercontent.com/49455496/180092812-a84b4a3a-d3f6-45ff-9e80-d7c551ae87a5.gif)

# Getting Started

The quickest way to get started is using docker. Clone the repo and run `docker-compose up` from the solution root. Open a browser and goto to localhost:8000. 
You can sign in using admin/password. 

If you want to step through the code then you'll need to run the REST api and Angular front end. I use VS2022, VSCode, and SQL Server on my dev box.

## Start the REST api

Open ~/YARC.sln is VS2022 and make sure "Api" is set as the startup project. Verify the connection string (connectionStrings:db) in appsettings.json is correct. 
You'll also need to map a folder for the lucene index (connectionStrings:fts). Run the solution. 

You should be able to access http://localhost:5262/swagger if things went as expected.

## Start the web app

Open ~/UI/public-ui is VSCode then open a terminal. Type `npm install --force` and then `ng serve` to start the Angular dev server. You should be able to view the 
app at localhost:4200 once the build process is finished. 

# Architecture

The app consists of two projects - a frontend built using Angular 14, Typescript, and Bootstrap and a REST api built using C#. The two data stores are SQL Server and 
a Lucene full text index. The data is accessed using EF. There's a stored procedure used to calculate forum post scores that gets ran as a background process using 
Hangfire. There's a machine learning model included to classify toxicity (/Api/Services/Text/Toxicty/ToxicityModel.cs). The model is built using source from https://github.com/andrewfry/SharpML-Recurrent and lives in the YARC.DL project.

The REST api (/Api) is built as a monolith to simplify building and deployment. The data folder (/Api/Data) contains the EF context and entity definitions. DB migrations 
are handled using EF Code First. The services (/Api/Services) contain the meat and potatoes. Services contracts are injected into the controllers using .NET's DI 
framework. Most of the service code is straight forward linq queries with the exception of the machine learning bits (see /Api/Services/Text/Toxicity/ToxicityModel.cs). 

The frontend is built using Bootstrap 5, Angular 14, and Typescript. There are some npm package version issues I need to sort (note the `--force` on the `npm install --force` from above) 
but is should still transpile and render as expected. It follows the standard Angular CLI file structure for components (separate files for scss, spec, markup, and typescript). 
The REST api is access through services that get injected into the components (see /app/services/posts.service.ts).

# Known Issues

* The toxicity classifier service isn't threadsafe
* There is some repeated code used in LINQ statements
* The sp used to calculate post scores is painfully slow once you get into the millions of rows
* The toxicity classifier was built on training data from a Kaggle competition that used wikipedia comments. YMMV
