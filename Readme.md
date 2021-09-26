# Alarmy
The solution for displaying messages on computer networks.

## How does it work?
Client machines run a light-weight service (called Alarmy Service) which is connected to a server on 
client machines. The service waits for requests to display messages to the user. 
Once a minute, the service sends a "keep alive" message to the server.

When an operator decides to display a message to the users, all relevant client services
are informed and they display the messages. All messages are accessible via a web server as well,
to ensure legitimacy.

In addition, the server supports encrypted SSL communication.

## How to use the client?
After installation, ensure the service is running by looking for the Alarmy icon in the notification tray. It should have a *greenish color*. 

If it has a *red color*, that means that the client is running but is not connected to the server. 
Hover over the icon to see the reason for connection failure. The client usually attempts to reconnect on its own. If you suspect the client will not attempt automatic connection, right click the service and restart it.

If the icon does not appear in the notification tray, double click the Alarmy icon on the desktop or run Alarmy from the start menu. Don't worry from running two Alarmy instances - the application will inform you an instance is already running.

## How to collect diagnostic information?
The client and manager report to the Windows Event Log.
The manager reports to a console and a log file as well.
These settings can be customized by editing the file `nlog.config` in the binary directory.

The events are logged under `Windows Logs` > `Application`.
By default, the event sources are `Alarmy.exe` for the client or `AlarmyManager.exe` for the server.

To send the diagnostic information to the operator, in the Event Viewer, follow these steps:
1. Navigate to the `Application` logs.
2. On the `Actions` pane to the right, click `Filter Current Log...` and pick the source `Alarmy.exe` (for the client). Click `OK`.
3. On the `Actions` pane to the right, click `Save Fitered Log File As...`. Save the file in a known location.

To allow for an easier diagnostic process, include your IP address and hostname:
1. Press `WinKey` + `R`. Type `cmd` and press `Enter`.
2. Run the command `ipconfig` and press `Enter`. Collect the output.
3. Run the command `hostname` and press `Enter`. Collect the output.

Provide the data you have collected to the operator along with a experienced behavior.

## FAQs
### How to turn the service off?
You can disable the service by performing the following steps:
1. Right click the service icon in the notification tray.
2. Click `Stop`.

### What information about me does the service send the server?
Your username, computer name and IP address.

### Does the service allow code execution on my machine?
It requires user cooperation for that.

Alarmy works by sending JSON-serialized data which contains a message title and
styled text content (Rich Text Format). Rich Text Format information
can include objects such as text and binary files, some of which
may contain script or executable information. These cannot be activated
without user interaction with them. 

In the next version, the RTF data will be replaced with HTML,
the display object of which will be prohibited from running scripts.

### How can I validate the authenticity of a message?
You can rest assured - an SSL handshake is performed to ensure the authenticity of 
the server's identity. 

In addition, when the message pops on your client, 
you can click on the link below it to navigate to the web interface which indexes all messages.
Verify the connection is secure via the LOCK icon in the address bar of your browser.
