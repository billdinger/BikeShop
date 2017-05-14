# BikeShop



This code is only for demo purposes of unit testing and is necessarily neglecting many features that would be considered 
critical for any sort of production use. It's also built in such a way to explicitly be easy to follow for beginners to unit
testing, including some delibrate design mistakes to highlight ways of unit testing.

The solution presents a hypothetical problem in which you have to build a REST API that various clients can consume.
Your REST Api needs to be able to retrieve product data from a third party SOAP api, and it needs to talk to 
a backend database for cart functions. In addition, a coworker decided to helpfully implement a cache for you as a static
class.
