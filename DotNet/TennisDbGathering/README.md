# Tennis Database Gathering App

Contains code to connect to a web page, retrieve the html source, parse it to retrieve data from a table, store it in a class and store it in a database.

I then transformed this table so that it can have a relation with an existing country database. This resulted with the database featured in the [Tennis Database Client App](https://github.com/markojaa4/portfolio/tree/master/DotNet/TennisDbClient).

It also saves a backup page-source.html file. The version that is currently in the bin/Debug directory also serves as a snapshot demo of the website in case the live version loses compatibility with my parser.
