# Async Word Count

This is a sample project I built to get hands on experience with Azure Functions. 

User and logic flow is as follows:
Note: The user is referred as User and the frontend is referred as the Client.

1. User visits the website
2. User selects a text file
3. Client setups a websockt connection. This uses SignalR service provided by Azure. Client also stores the connection id in the local storage.
4. Client calls an Azure function to get a Shared Access Signature (SAS) token for the blob storage. 
5. A PUT request is made to the blob storage with the file along with the connection id as metadata.
6. An Azure function is triggered when the blob is uploaded. This function reads the file and computes the top 5 most frequent words. It then communicates the result to SignalR service passing the computed result and the connection id.
7. Client receives the result and displays it to the user.


## Project Structure
Codebase consists of two projects:
Web and Worker. This was done since when WorkerDefaults() and WebApplication() are used together, the GetFileUploadUrl function as per logs executes but the body of the function is not executed.
