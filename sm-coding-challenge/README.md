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

Notes
* In a real world scenario this data would also be stored in a relational or nosql data store
* We would do alot more with the data returned from the API for instance we would
    want to create a new player record for every new player found
    that means we would have a cache of player_ids that we would reference to determine the existence of new players
    we would check if the skills (rushes, passes...) returned already exist and if not we would add them to the data store
    we would introduce pagination when fetching a player object graph meaning that we could return say only 50 skills per skill type

