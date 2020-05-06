# Setup
* Install redis and make sure its running on the default port (6379)
* clone the repository 
* Refresh the dotnet packages
* dotnet run


## Changes
* IDataProvider interface and implementation has been modified to return a PlayerRequestResult object which contains a player field, a request status field and message
* No url errors
* DataProvider interface and implementation uses async/await
* Additional attributes have been added to the player model for the front end
* all entries (kicks, pushes, rushes, receives ... ) about the player id are saved, entries containing other player ids are discarded 
* Duplicates have been removed using linq queries before persistence
* LatestPlayers method finds the last entry of the desired skills and uses a view model to return the desired structure
* All responses are performant, taking no more than a few miliseconds. .net core response was combined with redis caching. responses are cached for 24 hours. we call out to the api only if the data doesnt exist in the cache
* In a real world scenario this data would also be stored in a relational or nosql data store

