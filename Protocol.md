# Alarmy Communication Protocol
* Alarmy communication data is passed over JSON-serialized objects.
* Communications sent from the server to the client are called `Messages`. 
  Communications sent from the client to the server are called `Responses`.

## Models
### `Instance`
A strcture describing the unique identity of an Alarmy client.
The model can be found in `AlarmyLib\Instance`.

* **`Username`** (`string`) - name of the user running the Alarmy client.
* **`ComputerName`** (`string`) - name of the computer the user is logged onto.

**Note**: the `Username` field may sometimes contain both the NT username and the computer name, e.g. `JOHNPC\JohnDoe`.

### `Group`
Contains information allowing to separate alarms into different channels to which users are a part of.
The model can be found in `AlarmyLib\Group`.

* **`ID`** (`Guid`) - unique identifier of the group. `Guid.Empty` is the Global Group - all users are members of that group.
The use of a `UUID` for a group ID allows to anonymize group names and have a duplicate `Name`, although not recommended.
* **`Name`** (`string`) - display name of the group.

### `Alarm`
A structure containg the information required to display an Alarm to the user.
The model can be found in `AlarmyLib\Alarm`.

* **`ID`** (`string`) - unique identifier of the Alarm.
* **`IsRtl`** (`bool`) - whether the alarm content should be displayed as right to left.
* **`Title`** (`string`) - title of the alarm. It will appear at the top of the window.
* **`Content`** (`string`) - content of the alarm. Rich text format.
* **`GroupID`** (`Guid`) - ID of the Group this message was sent to.
Defaults to `Guid.Empty` which means Global Group - all users are members of that group.

## Protocol Messages
The messages can be found in `AlarmyLib\Messages`.

### `MessageWrapper`
A structure wrapping all communications between servers and clients.

* **`MessageType`** (`string`) - namespaced type of the contained message.
* **`Message`** (`object`) - message of the specified type.

### `BaseMessage`
A base message object containing fields every inheriting message should contain.

### `BaseResponse`
Same as `BaseMessage`, but for `Responses`.

* **`Instance`** (`Instance`) - identity of the client sending a message to the server.

### `ShowAlarmMessage`
Display an alarm for the client.

* **`Alarm`** (`Alarm`) - the alarm to display to the user.

### `PingMessage`
Send a *ping* to the client.

### `ErrorMessage`
Inform the client a server error as occurred.

* **`Code`** (`ErrorCode`) - enum of server error codes, currntly only containing `MaxConnectionsExceeded`.
* **`Text`** (`string`) - informative message.

### `ServiceStartedResponse`
Client has started.

* **`GroupIDs`** (`List<Guid>`) - all groups the client is subscribed to.

### `PingResponse`
A client's `Pong`.

## Communication Protocol

### Normal Operation
0. Assuming the server is operating normally.
1. Client starts. It sends a `ServiceStartedResponse` with its `Instance` and groups.
2. Server adds the client to the client pool.
3. An alarm is triggered. The server sends `ShowAlarmMessage` to the relevant clients (and only the relevant ones - 
clients will display every message they receive regardless of the message's `GroupID` and whether they are a part of it or not).
4. Server wants to check which clients are online. The server sends `PingMessage`.
5. Clients reply with a `PingResponse`.

### Edge Cases
These are the most common edge cases of the server-client operation.
When implementing a server and/or client, the developer should consider the following are the expected behaviors.

It's recommended to go over the implementation before attempting to implement the server and/or client.

* **The clients attempts to connect to a non-responsive or offline server**. The client will retry every 15 seconds by default.
* **The client loses communication with the server**. The client will retry every 15 seconds by default.
* **The server is stopped by the operator**. The connection will be gracefully closed (at the TCP level). The client will retry every 15 seconds by default.
* **The server loses communication with the client**. The server will drop the client from its client list when sending an alarm *or* at the next refresh.