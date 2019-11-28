# Az-IoT-Ramp-Up-Service-Fabric
This is Service Fabric project which listens to event hub and store messages in cosmos DB.

# Service-Fabric-Listener
This project is an Event Hub receiver that runs on MS Azure Service Fabric and allows you to

1. Use Service Fabric partitions to distribute the receiver on Service Fabric partitions.
2. Use Service Fabric reliable state to store Event Hub offsets and listens to the messages sent by devices and further stores those messages in cosmos DB. 


## Authors

* **Abhishek Gaurav** - *Full work*


## License

This project is licensed under the private License - see the [LICENSE.md](LICENSE.md) file for details
