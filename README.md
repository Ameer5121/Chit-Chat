# Chit-Chat
Social chatting application with MySQL database features for windows.

Login  |  Register 
:-------------------------:|:-------------------------:
![1](https://user-images.githubusercontent.com/71935713/148786170-f16f5a46-5a10-4469-8f2c-bfd50c775b83.png)  |  ![2](https://user-images.githubusercontent.com/71935713/142070023-979d4f8e-7062-4a65-8bdb-df4afa38db54.png) 

Password Recovery 1 |  Password Recovery 2 
:-------------------------:|:-------------------------:
![2](https://user-images.githubusercontent.com/71935713/148786353-56141f35-cf02-47a6-b1cf-a18f6b563f48.png)  |  ![3](https://user-images.githubusercontent.com/71935713/154128894-df1caaf1-32f1-412a-be79-1dc2d4bcd918.png)
 
Public Chat
![142070066-2b4c091e-646f-496c-9576-d2236beba0b5](https://user-images.githubusercontent.com/71935713/147118569-4d61d3ba-74b2-4a50-a01b-db7316030b7e.png)

Private Chat
![142070100-2fc26403-dc05-482b-95e9-5cf083ae30e2](https://user-images.githubusercontent.com/71935713/147118538-e891a89c-c644-41fb-9e97-91277a0c8d98.png)

Light Theme
![142070127-7cd5d5f4-b1d5-4f21-903c-f17ccbd28ed3](https://user-images.githubusercontent.com/71935713/147118499-1c5878c0-c3fb-47a7-9b53-5bc2f68d4906.png)

Image Sending
![142070165-978f0dbe-ad02-4328-9d73-7a5d3ef57a65](https://user-images.githubusercontent.com/71935713/147118454-13bab06f-7b4f-4d20-b14e-65c633e4839d.png)


# Contains usages of:
* Makes use of Task based asynchronity through **System.Threading**
* Socket Programming through **Microsoft.AspNetCore.SignalR.Client**
* JSON serialization through **Newtonsoft.Json**
* **Web API** project to handle HTTP & database requests. https://github.com/Sound932/WebAPI-ChatHub.git

# Core functionality:
* Login / Register options
* Password Recovery
* List of connected users, automatically updating when one joins/leaves
* Returning to home view automatically when server is down
* Message control and loading old messages
* Private chatting
* Colored Emojis
* Profile Pictures
* Image Sending
* Light and Dark themes
* Character Limit
* Logging out manually.

# Memory Performance Features:
* Loading only up to 100 messages at a time.
   * Only 100 messages are shown at a time to preserve memory. This feature comes with the ability to load previous messages, however, this works differently for private chats. 
You are able to have multiple stacks of (up to 100) messages for each individual you're private chatting to. It doesn't count the messages globally.

# Framework:
* Made in WPF
     * MVVM Design Pattern
 
 # Third-Party Libraries:
 * SignalR
 * Newtonsoft JSON
 * MaterialDesignThemes

 # How were Colored Emojis achieved in WPF
WPF does not support unicode colored emojis. It only supports them in black and white. Question is, how do you go around this? and the answer would be Images.

By using a RichTextBox, you can insert images & rich content which internally uses FlowDocuments and Paragraphs. You can then take the FlowDocument, write it into a MemoryStream formatted as RTF, read the bytes out and send it to an API which then sends it to every socket that needs the data. Users who get the RTF data can now procced to use MemoryStream to Load back the data into a FlowDocument, which can then be inserted into a few controls:
* FlowDocumentPageViewer
* FlowDocumentScrollViewer
* FlowDocumentReader

For a more detailed overview, refer to these links:
* Converting FlowDocument to RTF bytes https://github.com/Sound932/Chit-Chat/blob/f57c50605586eed9eb14ca51218cbac9ffc8830f/Chit%20Chat/Helper/Extensions/DocumentExtensions.cs#L20
* Converting RTF bytes to FlowDocument https://github.com/Sound932/Chit-Chat/blob/46916d8ee7d1ac94174136b4b8e843dc99fd407f/Chit%20Chat/Helper/Extensions/MessageModelExtension.cs#L15

# Application State
This application is not finished, and there are a lot of things planned.

