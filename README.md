# Chit-Chat
Social chatting application with MySQL database features for windows.

Login             |  Register
:-------------------------:|:-------------------------:
![1](https://user-images.githubusercontent.com/71935713/110955002-ca075580-8351-11eb-97ce-832d15ac0ad1.png)  |  ![2](https://user-images.githubusercontent.com/71935713/110955050-d68bae00-8351-11eb-9e12-406220e313a7.png)

Public Chat
![publicchat](https://user-images.githubusercontent.com/71935713/134582068-833dd3fe-ec5a-4cf3-bf68-4c949a98f5e9.png)

Private Chat
![privatechat](https://user-images.githubusercontent.com/71935713/133803099-8c3ee5b4-3464-4491-982f-7d3faaadaa49.png)

Light Theme
![lighttheme](https://user-images.githubusercontent.com/71935713/133803203-20900b4a-5a98-4426-b7dd-33d7b8b234e5.png)

Image Sending
![imagesending](https://user-images.githubusercontent.com/71935713/133803716-5eed0ac1-b604-42e4-b2b7-310a5cbcf07d.png)


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
