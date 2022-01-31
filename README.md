# OrbitalWitness

Hi guys, thanks for taking the time to look at my project! 

I've created a small postman collection to make it easier to test the project, although there are only 2 calls so it's still fairly simple. 
https://www.postman.com/collections/4e9a21d8552df7a4bc71
The end of the test data seemed to be cut out so I just cropped the invalid bits out to make it run.


I've also Dockerised the code, if you want to run it through docker you can do so with the following commands:

docker build -t orbitalwitnesstest .
docker run -d -p 8080:80 --name orbital orbitalwitnesstest

A couple of extra things to note: 
The data is fairly inconsistent (different numbers of spaces between column elements etc) so it's really difficult to be exact when we split the data into the columns. I think the method I have has worked for all the test data, but I'm aware that if more inconsistent data comes through then this would probably have to be tinkered with a bit, and as far as I can tell there isn't really a way to sort this 100% correctly. I've attempted to check that the data is valid, and in doing so have made some assumptions that the format will always be the same. If one of the entries has a column which doesn't look valid, at the moment it still processes it as normal, but logs it in a txt file so that we can assess it manually. 

I'm also saving the data once we've processed it in a sqlite db, to save us from processing it each time, it'll save it as soon as it's sorted. Obviously in a real scenario we'd use a proper db, but the sqlite is good for demo purposes. 

Thanks again for taking the time to look over the project, and feedback - good or bad - would be appreciated!

Gavin
