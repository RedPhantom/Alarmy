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
After installation, ensure the service is running. You can do so by following these steps:
1. Press `WinKey` + `R`. Type `services.msc` and press `Enter`.
2. Look for `Alarmy Service`. Make sure its status is `Running`. If it isn't, right click
	it and press `Start`.
That's it!

## FAQs
### How to turn the service off?
You can disable the service by performing the following steps:
1. Press `WinKey` + `R`. Type `services.msc` and press `Enter`.
2. Look for `Alarmy Service`. If it's state is `Running`, click `Stop`.
You will not receive messages until the service is started again.

### What information about me does the service send the server?
Your username, computer name and IP address.

### Does the service allow code execution on my machine?
No. Alarmy works by sending JSON-serialized data which contains a message title and
styled text content (Right Text Format). RTF does not allow code execution.

### How can I validate the authenticity of a message?
You can rest assured - an SSL handshake is performed to ensure the authenticity of 
the server's identity. 

In addition, when the message pops on your client, 
you can click on the link below it to navigate to the web interface which indexes all messages.
Verify the connection is secure via the LOCK icon in the address bar of your browser.
