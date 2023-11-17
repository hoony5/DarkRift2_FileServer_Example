# DarkRift2_FileServer_Example
The Darkfit2 is copyrighted by DarkRiftNetworking. This is made .Net8.0 up for test.

## File Server With DarkRift2
  -  Framework : .NET8.0
  -  Language : C# 12.0

### Server
  -  Use AES256 Encryptor ( key & iv are loaded local txt File )
  -  Divide Method SendEncryptedMessage and Send(Normal)Message
  -  Every party has a repository with its own party key. This repository contains files uploaded by that party member.
  -  Files are shared among people who share the same party key, including themselves.
  -  R/W
    -  ServerMessageReader / ServerEncryptedMessageReader
    -  ServerMessageWriter / ServerEncryptedMessageWriter
  -  Party Logics
    -  Creation
    -  Join
    -  Exit/Remove/Destroy
    -  Finder  
  -  File Logics
    -  Upload
    -  Download
    -  Finder
    -  Delete
    -  OnlyServer
       -  PdfConverter
       -  DirectoryCreator
       -  FileManagement

### Model
  - UseCase
    -   User : private info with public info
    -   UserHeader : only public Info
    -   Party : Users(Leader and Members) Header Info with Party Info
    -   FileSegmentsInfo : Total Bytes Information
    -   FileSegment : Partioned File Bytes , Index, FileExtension, FileNameWithoutExtension
  - Dto
    -   File
    -   Party
  - Tags
  - CommonValue


### Message Security
  -  party Logic => Normal Message
  -  File Logic => Encrypted Message

### Usage
  -  Look DarkRift2 [Github](https://github.com/DarkRiftNetworking/DarkRift)
