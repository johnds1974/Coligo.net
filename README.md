# Coligo.net
Small MVVM Framework for .Net WPF applications.

Coligo.Net was created after a small period of using Caliburn.Micro, and I give all credit to the Caliburn team 
for providing me with the inspiration and some ideas on how to implement my own MVVM Framework.

I take nothing away from Caliburn, but just were some places where I felt that it just did not provide me with
what I needed to get the job done with various WPF projects.

What I wanted to acheive with Coligo (latin for 'Connect/Bind') was a simple and easy way to Bind your ViewModels to 
your UI elements, in an easy manner and with a little more 'power' than what regular WPF binding allows.

The Caliburn 'conventions' were also an inspiration for my Coligo conventions, just implemented slightly differently, with 
it's own XAML markup mechanism.

Now, since Coligo is only in it's earliest stages, there will of course be many changes (hopefully improvements) to the way
it functions, but so far it has done a pretty good job at solving some of the issues I came across with Caliburn.

For now, the Coligo assemblies have been developed to be used in both a <b>Desktop WPF</b> environment and a <b>WinRT</b> environment, so 
in the case of a Windows Store application or a Windows Phone 8+ application, the libraries have beed developed to utilise
the WinRT BCL, which annoyingly, differs quite alot from the regular non-WinRT BCL!

Please keep tuned in formore updates, I am just writing this quickly to have some form of README in place.

<b>More documentation and samples to come soon!</b>
