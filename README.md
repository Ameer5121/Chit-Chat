# Chit-Chat
Social chatting application with MySQL database features
Login             |  Register
:-------------------------:|:-------------------------:
![1](https://user-images.githubusercontent.com/71935713/110955002-ca075580-8351-11eb-97ce-832d15ac0ad1.png)  |  ![2](https://user-images.githubusercontent.com/71935713/110955050-d68bae00-8351-11eb-9e12-406220e313a7.png)

Private Chat
![1](https://user-images.githubusercontent.com/71935713/115125572-bba3fd80-9fd1-11eb-9f73-d6f24e9c2e5e.png)

Public Chat
![2](https://user-images.githubusercontent.com/71935713/115125578-ca8ab000-9fd1-11eb-862c-4aaf45dee46f.png)




# Contains usages of:
* MultiThreading through **System.Threading**
* Socket Programming through **Microsoft.AspNetCore.SignalR.Client**
* JSON serialization through **Newtonsoft.Json**
* **Web API** project to handle HTTP & database requests. https://github.com/Sound932/WebAPI-ChatHub.git

# Core functionality:
* Login / Register options
* List of connected users, automatically updating when one joins/leaves
* Returning to home view automatically when server is down
* Private chatting
* Colored Emojis
* Logging out manually.

# Framework:
* Made in WPF
     * MVVM Design Pattern
 
 # Third-Party Libraries:
 * SignalR
 * Newtonsoft JSON
 * MaterialDesignThemes
 * Dapper

 # How were Colored Emojis achieved in WPF
WPF does not support unicode colored emojis. It only supports them in black and white. Question is, how do you go around this? and the answer would be Images.

By using a RichTextBox, you can insert images & rich content which internally uses FlowDocuments and Paragraphs. You can then take the FlowDocument, write it into a MemoryStream formatted as RTF, read the bytes out and send it to every socket that needs the data. Users who get the RTF data can now procced to use MemoryStream to Load back the data into a FlowDocument, which can then be inserted into a few controls:
* FlowDocumentPageViewer
* FlowDocumentScrollViewer
* FlowDocumentReader
