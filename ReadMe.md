# Intro
Hello. I've gone for an API on the front with N-Tier architecture that I believe meets the requirements. There is room for improvement and it's a bit messier than I would have liked but I had to draw a line under it somewhere. Was having trouble deserializing the JSON so went with XML and used a xsd.exe to generate the classes.

# To Run
Launch the API project using IIS Express and you should be directed to Swagger.

# Potential TODOs
- Consider moving mapping out of repo into service and using automapper
- Better error handling
- DI assembly scanning
- Consider use of fluent validator is not meant for simple types
- Get error mapping logic out of controller action method
- Consider validator is called twice (API and service layer)
- Tidy up XML class for used for deserialization
- Unit test controllers, Bud.Utils
- Inject FlurlClient in repo