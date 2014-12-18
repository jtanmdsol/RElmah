RElmah - Reactive ELMAH
======

If you already used [ElmahR] in the past, you already know what **RElmah** is about. We want to monitor applications and receive real time notifications about unhandled exceptions. But the way things are done behind the scenes are totally redesigned to offer a true **reactive** experience to the developer. Quickly:

- we have a clean separation between server and client portions
- we have true and independent client libraries for both JavaScript and .NET
- [Rx] is at the core on both the server side the clients side libraries
- Errors, applications, basically anything that can go to a client will be managed as a push stream of information

Do you want a sneak preview? What about being able to do something like this in Linqpad?

```c#
var c = new Connection("http://localhost:50360/");
var q = 
	from error in c.Errors
	where error.Detail.Message.StartsWith("B")
	select error.Detail;
	
q.DumpLive();
```
You get the idea :)

Current version is 0.2, which means some foundations are there, but it's not really ready to be used yet. You might want to have a look and see if you like what's happening, and maybe contribute with ideas or coding.



[ElmahR]:http://elmahr.apphb.com/
[Rx]:http://msdn.microsoft.com/en-us/data/gg577609.aspx
