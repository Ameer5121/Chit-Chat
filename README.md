# Chit-Chat
Social chatting application with MySQL database features for windows.
Login             |  Register
:-------------------------:|:-------------------------:
![1](https://user-images.githubusercontent.com/71935713/110955002-ca075580-8351-11eb-97ce-832d15ac0ad1.png)  |  ![2](https://user-images.githubusercontent.com/71935713/110955050-d68bae00-8351-11eb-9e12-406220e313a7.png)

Public Chat
![publichat](https://user-images.githubusercontent.com/71935713/130807833-144d3ea8-fb8c-4b05-b6ce-df612f1219eb.png)

Private Chat
![private chat](https://user-images.githubusercontent.com/71935713/130813934-48f1e417-ea15-4afd-a1f2-8f85c3bb12e2.png)

Light Theme
![lightmode](https://user-images.githubusercontent.com/71935713/130816718-9c4eb442-5d8c-4c8e-81b6-d5e0c7928235.png)

Image Sending
![imagesending](https://user-images.githubusercontent.com/71935713/130817184-83d9cec1-db45-4707-a180-eada765ffc65.png)


# Contains usages of:
* Makes use of Task based asynchronity through **System.Threading**
* Socket Programming through **Microsoft.AspNetCore.SignalR.Client**
* JSON serialization through **Newtonsoft.Json**
* **Web API** project to handle HTTP & database requests. https://github.com/Sound932/WebAPI-ChatHub.git

# Core functionality:
* Login / Register options
* List of connected users, automatically updating when one joins/leaves
* Returning to home view automatically when server is down
* Private chatting
* Colored Emojis
* Profile Pictures
* Image Sending
* Light and Dark themes
* Character Limit
* Logging out manually.

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


# More Images:

Character Limit
![messagetoolong](https://user-images.githubusercontent.com/71935713/130817450-e3d8d093-f245-455b-9e18-4cea70eb71ad.png)
