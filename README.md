# Async Word Count

## Demo Video
https://youtu.be/1_YNMmNs1T8

This is a sample project I built to get hands on experience with Azure Functions. 

User and logic flow is as follows:
Note: The user is referred as User and the frontend is referred as the Client.

1. User selects a text file
2. Client generates a unique identifier for itself
3. Client setups a websocket connection. This uses SignalR service provided by Azure. The unique identifier is used as the user id.
4. Client calls an Azure function to get a Shared Access Signature (SAS) token for the blob storage. The user id is passed to include it in the file name.
5. A PUT request is made to the blob storage with the file along with the user id as metadata through headers.
6. An Azure function is triggered when the blob is uploaded. This function reads the file and computes the top 5 most frequent words. It then communicates the result to SignalR service passing the computed result and the user id which it can fetch from both the file name and the metadata as per requirement.
7. Client receives the result and displays it to the user.


## Project Structure
Codebase consists of two projects:
Web and Worker. This was done since when WorkerDefaults() and WebApplication() are used together, the GetFileUploadUrl function as per logs executes but the body of the function is not executed. mostly due package versioning issues of dependencies.
